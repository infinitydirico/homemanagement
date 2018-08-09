using System.Collections.Generic;

namespace HomeManagement.App.Services.Components
{
    public class MetadataHandler : IMetadataHandler
    {
        Dictionary<string, string> values = new Dictionary<string, string>();

        public string GetValue(string key)
        {
            return values.ContainsKey(key) ? values[key] : string.Empty;
        }

        public void Remove(string key)
        {
            values.Remove(key);
        }

        public void StoreValue(string key, string value)
        {
            values.Add(key, value);
        }
    }

    public interface IMetadataHandler
    {
        string GetValue(string key);

        void StoreValue(string key, string value);

        void Remove(string key);
    }
}
