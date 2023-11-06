using Backoffice_ANCFCC.Models;

namespace Backoffice_ANCFCC.Services.AuthService
{
    public interface IAuthService
    {
        User GetUser(string username);
        LoginResult ValidateLogin(User user, string password);
        string CreateToken(User user);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        RegistrationResult RegisterUser(UserModel userModel);
        VerifyRegistrationResult VerifyTokenRegistrtion(string token);
        ForgotPasswordVerify ForgotPassword(string email); 
        ResetPasswordVerify ResetPassword(ResetPasswordRequest request);
        RegistrationResult IsvalidModelState(UserModel userModel); 

    }
}
