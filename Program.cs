using Backoffice_ANCFCC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Serilog;
using Microsoft.AspNetCore.HttpLogging;
using AspNetCoreRateLimit;
using Backoffice_ANCFCC.Services.EmailService;
using Backoffice_ANCFCC.Services.AuthService;
using Backoffice_ANCFCC.Services.ClaimsHelper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Hangfire;
using Hangfire.SqlServer; 


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


builder.Logging.AddSerilog(); 
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "POST:/Authentication/login",
                Period = "10s",
                Limit = 3
            }
        };
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();




builder.Services.AddDbContext<DbAncfccContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB_ANCFCC")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Autorization header using Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,

    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Back office_ANCFCC",
        Version = "v1",
        Description = "An API to perform operations of the back-office",
        Contact = new OpenApiContact
        {
            Name = "Abdessamad Tanafaat",
            Email = "tanafaat.wac.49@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/abdessamad-tanafaat-924534222"),
        }


    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHangfire(options => options
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()

       .UseSqlServerStorage(builder.Configuration.GetConnectionString("DB_ANCFCC"), new SqlServerStorageOptions
       {
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
           QueuePollInterval = TimeSpan.Zero,
           UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true
       })) ;

builder.Services.AddHangfireServer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// to log fields given by the user : dak request body li kaydkhel yla biti te3ref ach ga3d taydkhel .. ... 

builder.Services.AddHttpLogging(httpLogging =>
{
    httpLogging.LoggingFields = HttpLoggingFields.All;
}
);
builder.Services.AddMemoryCache();
// important if you create a service (interface + classe) and you want it to be worked 
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClaimsHelper, ClaimsHelper>();

builder.Services.AddApiVersioning(Options =>
{
    Options.DefaultApiVersion = new ApiVersion(1,0);
    Options.AssumeDefaultVersionWhenUnspecified = true;
    Options.ReportApiVersions = true;

});
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });

}); 
var app = builder.Build();
 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enablend  HSTS
}

app.UseHttpsRedirection();   //redirect http to https to secure the requests from hacking and attacking, that's why we must add it in the begining of all the middlwares (before acting and interacting... )

app.UseHttpLogging(); 

// create your first middlware , you  can remove it ... 
app.Use(async (ctx, next) =>
{
    var start = DateTime.UtcNow;
    await next.Invoke(ctx);
    app.Logger.LogInformation($"RequestInexecuteTime{ctx.Request.Path}: {(start).Millisecond}ms");
    app.Logger.LogInformation($"Request{ctx.Request.Path}: { (DateTime.UtcNow - start).TotalMilliseconds}ms");
});


app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyPolicy");

// Add authentication middleware before UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseIpRateLimiting();
app.UseHangfireDashboard("/dashboard"); 
app.Run();
Log.CloseAndFlush();

