using HomeManagement.Core.Cryptography;
using HomeManagement.Models;

namespace HomeManagement.API.Tests.Builders
{
    public class TestContext
    {
        public readonly AesCryptographyService aesCryptographyService = new AesCryptographyService();

        public TestContext()
        {
            User = new UserModel();
        }

        public UserModel User { get; set; }
    }
}
