namespace HomeManagement.Api.Core.Throttle
{
    public interface IThrottleCore
    {
        bool CanRequest(string ip);
    }
}
