namespace HomeManagement.Api.Identity.Authentication
{
    public class AuthenticationResult
    {
        public bool Succeed { get; set; }

        public string Error { get; set; }

        public static AuthenticationResult Succeeded() => new AuthenticationResult { Succeed = true };
        public static AuthenticationResult NotSucceeded(string error) => new AuthenticationResult { Succeed = false, Error = error };
    }
}
