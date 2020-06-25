using HomeManagement.App.Services.Rest;
using System;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.BackgroundWorker
{
    public class SecurityCodesWorker : BaseWorker
    {
        private readonly AuthServiceClient authServiceClient = new AuthServiceClient();
        private bool initialized = false;

        public override int GetTimerPeriod() => 30;

        public int Code { get; private set; }

        protected override async Task Process()
        {
            var securityCode = await authServiceClient.GetCode();

            if (!initialized)
            {
                timer.Change(securityCode.Expiration.TimeOfDay, TimeSpan.FromSeconds(GetTimerPeriod()));
            }

            Code = securityCode.Code;
        }
    }
}
