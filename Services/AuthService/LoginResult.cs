namespace Backoffice_ANCFCC.Services.AuthService
{
    public class LoginResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set;  }

        public LoginResult (bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }
    }
}
// it's common practice in software development and has several benefits.
// this class is used to encapsulate the result of a login attempt. to make sure if the login was 
// successful and if not what went wrong by returning a boolean and error messasge. 
