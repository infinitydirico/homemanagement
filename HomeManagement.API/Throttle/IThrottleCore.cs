namespace HomeManagement.API.Throttle
{
    public interface IThrottleCore
    {
        bool CanRequest(string ip);
    }
}
