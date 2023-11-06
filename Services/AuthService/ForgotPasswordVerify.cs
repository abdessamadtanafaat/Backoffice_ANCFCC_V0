namespace Backoffice_ANCFCC.Services.AuthService
{
    public class ForgotPasswordVerify
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }


        public ForgotPasswordVerify(bool isSuccess, string errorMessage, string successMessge)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            SuccessMessage = successMessge;

        }
    }
}
