namespace Backoffice_ANCFCC.Services.AuthService
{
    public class ResetPasswordVerify
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public ResetPasswordVerify(bool isSuccess, string errorMessage, string successMessage)
        {
               IsSuccess = isSuccess;
               ErrorMessage = errorMessage;
               SuccessMessage = successMessage;
        }
    }
}
