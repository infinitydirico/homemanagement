namespace HomeManagement.API.Tests.Builders.Data
{
    public static class UserModelBuilder
    {
        public static TestContext SetAuthenticationUser(this TestContext context, string email, string password)
        {
            context.User.Email = email;
            context.User.Password = context.aesCryptographyService.Encrypt(password);
            return context;
        }
    }
}
