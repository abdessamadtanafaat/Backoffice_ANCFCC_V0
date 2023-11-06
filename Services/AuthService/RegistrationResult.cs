namespace Backoffice_ANCFCC.Services.AuthService
{
    public class RegistrationResult
    {
        public bool SuccessRegistration { get; set; } 
        public string ErrorRegistration { get; set; }
        public RegistrationResult(bool successRegistration, string errorRegistration)
        {
            SuccessRegistration = successRegistration;
            ErrorRegistration = errorRegistration; 
        }
    }
}
