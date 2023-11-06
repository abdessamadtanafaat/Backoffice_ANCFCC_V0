namespace Backoffice_ANCFCC.Services.AuthService
{
    public class VerifyRegistrationResult
    {
        public bool SuccessVerification { get; set; }
        public string ErrorMessage { get; set; }

        public string SuccessMessge { get; set; }

        public VerifyRegistrationResult(bool successVerification, string errorMessage, string successMessge)
        {
            SuccessVerification = successVerification;
            ErrorMessage = errorMessage;
            SuccessMessge = successMessge;

        }
    }
}
