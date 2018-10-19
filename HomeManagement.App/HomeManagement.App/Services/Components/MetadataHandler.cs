using System.Collections.Generic;

namespace HomeManagement.App.Services.Components
{
    public class ApplicationValues : IApplicationValues
    {
        Dictionary<string, string> values = new Dictionary<string, string>();

        public string Get(string key)
        {
            return values.ContainsKey(key) ? values[key] : string.Empty;
        }

        public void Remove(string key)
        {
            values.Remove(key);
        }

        public void Store(string key, string value)
        {
            values.Add(key, value);
        }
    }

    public interface IApplicationValues
    {
        string Get(string key);

        void Store(string key, string value);

        void Remove(string key);
    }
}
