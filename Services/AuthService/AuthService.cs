using Backoffice_ANCFCC.Models;
using Backoffice_ANCFCC.Services.EmailService;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;


namespace Backoffice_ANCFCC.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DbAncfccContext _DB_ANCFCC;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration configuration, DbAncfccContext dbAncfccContext, IEmailService emailService, ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _DB_ANCFCC = dbAncfccContext;
            _emailService = emailService;
            _logger = logger;
        }

        public RegistrationResult RegisterUser(UserModel userModel)
        {
            try
            {
                if (!IsvalidModelState(userModel).SuccessRegistration)
                {
                    return new RegistrationResult(false, $"{IsvalidModelState(userModel).ErrorRegistration}");
                }

                CreatePasswordHash(userModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User
                {

                    Username = userModel.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = userModel.Email,
                    Nom = userModel.Nom,
                    Prenom = userModel.Prenom,
                    Role = userModel.Role,
                    VerificationToken = CreateRandomToken(),
                    
                };


                _DB_ANCFCC.Users.Add(user);
                user.LockoutTimestamp = DateTime.Now.AddMinutes(60);
                _DB_ANCFCC.SaveChanges();

               BackgroundJob.Enqueue(() =>  _emailService.SendEmail(user.Email, "Registration Confirmation", $"Hello your registration is successful.Your token : {user.VerificationToken} and your unique code service : {user.UserUniqueCode}"));


                return new RegistrationResult(true, "Registration successful. Please check your email for confirmation.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _logger.LogError(ex, "An error occurred while registering the user.");
                return new RegistrationResult(false, "An error occurred while registering the user.");
            }
        }
        public VerifyRegistrationResult VerifyTokenRegistrtion(string token)
        {
            var user = _DB_ANCFCC.Users.FirstOrDefault(a => a.VerificationToken == token);

            if (user == null)
            {
                return new VerifyRegistrationResult(false, "Invalid Token !", string.Empty);
            }
            if (user.LockoutTimestamp < DateTime.Now)
            {
                return new VerifyRegistrationResult(false, $"Sorry {user.Nom} {user.Prenom} ,Token Expires", string.Empty);
            }

            if (user.VerifiedAt != null)
            {
                return new VerifyRegistrationResult(false, $"Account already verified on {user.VerifiedAt.Value}", string.Empty);
            }

            user.VerifiedAt = DateTime.Now;
            user.LockoutTimestamp = null;
            _DB_ANCFCC.SaveChanges();
            return new VerifyRegistrationResult(true, "Verification Ok.", $"Verified succssefully...Welcome back {user.Nom} {user.Prenom}");

        }
        public ForgotPasswordVerify ForgotPassword(string email)
        {

            var user = _DB_ANCFCC.Users.FirstOrDefault(a => a.Email == email);

            if (user == null)
            {
                return new ForgotPasswordVerify (false,"Sorry! this email is not in our system,Please check your email if it's correct", string.Empty);
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddMinutes(60);
            _DB_ANCFCC.SaveChangesAsync();
            BackgroundJob.Enqueue(()=>_emailService.SendEmail(email, "Reset your password", $"Hello , Your reset password token: {user.PasswordResetToken}"));



            return new ForgotPasswordVerify (true , string.Empty, $"Hello {user.Nom} {user.Prenom} ,You may reset your password,Please check your email");

        }
        public ResetPasswordVerify ResetPassword(ResetPasswordRequest request)
        {
                 if (!IsPasswordStrong(request.Password))
                 {
                return new ResetPasswordVerify(false, "Password must meet complexity requirements. you need to enter :  one lower charactere, one Upper charactere and oneDigit", string.Empty);
                 }

                var user = _DB_ANCFCC.Users.FirstOrDefault(a => a.PasswordResetToken == request.Token);

                if (user == null)
                {
                return new ResetPasswordVerify(false, "Invalid Token", string.Empty); 
                }

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                if (user.ResetTokenExpires < DateTime.Now)
                {
                return new ResetPasswordVerify(false, $"Sorry {user.Nom} {user.Prenom} , Token Expires.", string.Empty); 
                 
                }

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;
                _DB_ANCFCC.SaveChangesAsync();

                return new ResetPasswordVerify(true, string.Empty, $"Hello {user.Nom} {user.Prenom}, the passsword successfully reset."); 
            }
        public string CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Role),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("name",user.Nom),
                    new Claim("prenom",user.Prenom),
                    new Claim("role",user.Role),
                    new Claim("uniqueUserCode", user.UserUniqueCode),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            //claims.Add(new Claim(ClaimTypes.Role, role)); // Add the role claim




            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                             _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(

                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
        public User GetUser(string username)
        {
            return _DB_ANCFCC.Users.FirstOrDefault(a => a.Username == username);
        }
        public LoginResult ValidateLogin(User user, string password)
        {
            if (user == null)
            {
                return new LoginResult(false, "User not found.");
            }


            // Check the password based on the user type (Admin or Agent)

            if (user.IsLockedOut == true && user.LockoutTimestamp.HasValue)
            {
                var lockoutDuration = TimeSpan.FromMinutes(2);
                if (DateTime.Now - user.LockoutTimestamp.Value > lockoutDuration)
                {
                    user.AccessFailedCount = null;
                    user.LockoutTimestamp = null;
                    user.IsLockedOut = false;
                    _DB_ANCFCC.SaveChanges();
                    return new LoginResult(false, "Incorrect password");


                }
                return new LoginResult(false, "Account is locked due to too many failed login attempts. Try again later.");
            }

            user.LastLoginAttemptDate = DateTime.Now;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {

                // acces the field if it's null (?) set it to 0 . 
                user.AccessFailedCount ??= 0;
                var lockoutDuration = TimeSpan.FromMinutes(2);


                if (DateTime.Now - user.LastLoginAttemptDate <= lockoutDuration)
                {
                    user.AccessFailedCount++;
                    user.LastLoginAttemptDate = DateTime.Now;

                    if (user.AccessFailedCount == 4)
                    {

                        user.IsLockedOut = true;
                        user.LockoutTimestamp = DateTime.Now;
                        BackgroundJob.Enqueue(()=> _emailService.SendEmail(user.Email, "Something wrong", "Someone is trying to access you account"));
                    }


                    _DB_ANCFCC.SaveChanges();
                }
                else
                {
                    user.AccessFailedCount = null;
                    user.LockoutTimestamp = null;
                    user.IsLockedOut = false;
                    _DB_ANCFCC.SaveChanges();
                }

                return new LoginResult(false, "Incorrect password.");
            }

            if (user.VerifiedAt == null)
            {
                return new LoginResult(false, "Not Verified !");
            }
            return new LoginResult(true, string.Empty);


        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {

                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }

        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool IsPasswordStrong(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit);
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        }

        public RegistrationResult IsvalidModelState(UserModel userModel)
        {

            if (string.IsNullOrWhiteSpace(userModel.Username) || string.IsNullOrWhiteSpace(userModel.Password) || string.IsNullOrWhiteSpace(userModel.Email))
            {
                return new RegistrationResult(false, "All the fields are required !");
            }

            if (!IsValidEmail(userModel.Email))
            {
                return new RegistrationResult(false, "Email not valid");
            }


            if (userModel.Role != "Admin" && userModel.Role != "Agent")
            {
                return new RegistrationResult(false, "The role is not valide! Chose : Admin or Agent");
            }

            if (!IsPasswordStrong(userModel.Password))
            {
                return new RegistrationResult(false, "The password is not strong enough!");
            }

            if (userModel.Email.ToLower().Equals(userModel.Password.ToLower()))
            {
                return new RegistrationResult(false, "The email must be different to the password");
            }
            if (UserExists(userModel.Email))
            {
                return new RegistrationResult(false, "The email is already exist !");
            }

            return new RegistrationResult(true, "The verification is set up");
        }

        private bool UserExists(string email)
        {
            return _DB_ANCFCC.Users.Any(u => u.Email == email);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }

        }

    }
}
