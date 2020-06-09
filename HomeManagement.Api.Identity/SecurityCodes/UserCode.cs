using System;
using System.Threading.Tasks;

namespace HomeManagement.Api.Identity.SecurityCodes
{
    public class UserCode
    {
        private readonly Random random;

        public UserCode(Random random)
        {
            this.random = random;
            ScheduleCodeGeneration();
        }

        public string Email { get; set; }

        public int Code { get; private set; }

        public DateTime CodeExpirationStamp { get; private set; }

        public void GenerateNewCode()
        {
            Code = random.Next(111111, 999999);
            CodeExpirationStamp = DateTime.Now.AddSeconds(30);
            ScheduleCodeGeneration();
        }

        public void ScheduleCodeGeneration()
        {
            Task.Run(async () =>
            {
                await Task.Delay(30 * 1000);
                GenerateNewCode();
            });
        }
    }
}