using Backoffice_ANCFCC.Models;
using Microsoft.AspNetCore.Mvc;
using Backoffice_ANCFCC.Services.EmailService;
using Backoffice_ANCFCC.Services.AuthService;
using Hangfire;

namespace Backoffice_ANCFCC.Controllers
{
    [Route("/authentication")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {

        public static LoginModel loginmodel = new LoginModel();
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly DbAncfccContext _DB_ANCFCC;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;


        public AuthenticationController(DbAncfccContext DB_ANCFCC, IConfiguration configuration, ILogger<AuthenticationController> logger, IEmailService emailService, IAuthService authService)
        {
            _DB_ANCFCC = DB_ANCFCC;
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to register for the application.
        /// </remarks>
        /// <param name="request">User registration data.</param>
        /// <returns>
        /// A response indicating the registration status.
        /// </returns>
        /// <response code="200">Registration successful.</response>
        /// <response code="400">Invalid user registration data or registration failed.</response>
        /// <response code="500">An error occurred during the verification process.</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        public IActionResult Register(UserModel request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var registrationResult = _authService.RegisterUser(request);

                if (registrationResult.SuccessRegistration)
                {


                    _logger.LogInformation("User registered : sending the Registration Confirmation .");
                    return Ok(new { Message = "Registration successful. Please check your email for confirmation." });
                }
                _logger.LogError("User registration failed.");
                return BadRequest(new { Message = registrationResult.ErrorRegistration });
            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while enqueuing the registration process.");
                return StatusCode(500, "An error occurred while enqueuing the registration process.");
            }

        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to log in to the application.
        /// </remarks>
        /// <param name="request">User login data.</param>
        /// <returns>
        /// A response indicating the login status and, if successful, a token for authentication.
        /// </returns>
        /// <response code="200">Login successful.</response>
        /// <response code="400">Invalid user login data or login failed.</response>
        /// <response code="500">An error occurred during the verification process.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        public  ActionResult<string> Login(LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                 
                var user = _authService.GetUser(request.Username);

                if (user == null)
                {
                    return BadRequest("User not found."); 
                }

                var LoginResult = _authService.ValidateLogin(user, request.Password);

                if (LoginResult.IsSuccessful)
                {
                    user.AccessFailedCount = null;
                    user.LastLoginDate = DateTime.Now;
                    _DB_ANCFCC.SaveChanges();

                    string token = _authService.CreateToken(user);

                    _logger.LogInformation("User login successfully. Token: {Token}", token);
                    return Ok(token);

            }
            return BadRequest(LoginResult.ErrorMessage); 

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while reset user password. ");
                return StatusCode(500, "An error occured while reset user password. ");
            }

        }

        /// <summary>
        /// Verify user registration using a verification token.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to verify their registration by providing a verification token sent to their email.
        /// </remarks>
        /// <param name="token">The verification token received via email.</param>
        /// <returns>
        /// A response indicating the verification status.
        /// </returns>
        /// <response code="200">Verification successful.</response>
        /// <response code="400">Verification failed due to an invalid token.</response>
        /// <response code="500">An error occurred during the verification process.</response>
        [HttpPost("verify")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        public IActionResult Verify(string token)
        {
            try
            {
                var verifyRegistration = _authService.VerifyTokenRegistrtion(token);

                if (verifyRegistration.SuccessVerification)

                {
                    _logger.LogError($"{verifyRegistration.SuccessMessge}");
                    return Ok($"{verifyRegistration.SuccessMessge}");
                }

                _logger.LogInformation($"Registration Verification failed: {verifyRegistration.ErrorMessage}");
                return BadRequest(new { Message = verifyRegistration.ErrorMessage });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while verifying user. ");
                return StatusCode(500, "An error occured while verifying user. ");

            }
        }

        /// <summary>
        /// Send a password reset email to the user.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to request a password reset by providing their email address.
        /// An email with instructions for resetting the password will be sent to the provided email address.
        /// </remarks>
        /// <param name="email">The email address of the user requesting the password reset.</param>
        /// <returns>
        /// A response indicating the result of the password reset request.
        /// </returns>
        /// <response code="200">Password reset email sent successfully.</response>
        /// <response code="400">Invalid email address or other request-related issues.</response>
        /// <response code="500">An error occurred during the password reset process.</response>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var user = _authService.ForgotPassword(email);

                if (user.IsSuccess)
                { 
                    _logger.LogError($"{user.SuccessMessage}");
                    return Ok($"{user.SuccessMessage}"); 
                }

                return BadRequest($"{user.ErrorMessage}"); 

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while reset user password. ");
                return StatusCode(500, "An error occured while reset user password. ");

            }

        }

        /// <summary>
        /// Reset the user's password using a token and a new password.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to reset their password by providing a valid reset token and their new password.
        /// Users must have received a reset token via email as part of the password recovery process.
        /// </remarks>
        /// <param name="request">The reset password request, including the reset token and the new password.</param>
        /// <returns>
        /// A response indicating the result of the password reset operation.
        /// </returns>
        /// <response code="200">Password reset successfully.</response>
        /// <response code="400">Invalid reset token, password format, or other request-related issues.</response>
        /// <response code="500">An error occurred during the password reset process.</response>
        
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(string), 200)] // Response type for 200 OK
        [ProducesResponseType(typeof(string), 400)] // Response type for 400 Bad Request
        [ProducesResponseType(typeof(string), 500)] // Response type for 500 Internal Server Error
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {

            try
            {
                var user = _authService.ResetPassword(request);

                if (user.IsSuccess)
                {
                    _logger.LogError($"{user.SuccessMessage}");
                    return Ok($"{user.SuccessMessage}");  
                }

                _logger.LogError($"{user.ErrorMessage}");
                return BadRequest($"{user.ErrorMessage}"); 

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while reset the user password. ");
                return StatusCode(500, "An error occured while reset the user password. ");

            }

        }
    }
}
