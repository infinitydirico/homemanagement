namespace HomeManagement.Core.Caching
{
    public interface ICachingService
    {
        object Get(string key);

        T Get<T>(string key);

        void Store(string key, object value);

        void Remove(string key);
    }
}
