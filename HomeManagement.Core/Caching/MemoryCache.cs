using System;
using System.Collections.Generic;

namespace HomeManagement.Core.Caching
{
    public class MemoryCache : ICachingService
    {
        Dictionary<string, object> cachedVaues = new Dictionary<string, object>();

        public object Get(string key)
        {
            if (!cachedVaues.ContainsKey(key)) throw new ArgumentException($"No cached value found for: {key}");

            return cachedVaues[key];
        }

        public T Get<T>(string key)
        {
            if (!cachedVaues.ContainsKey(key)) throw new ArgumentException($"No cached value found for: {key}");

            var cachedValue = cachedVaues[key];

            if (cachedValue.GetType() == typeof(T)) return (T)cachedValue;

            throw new ArgumentException($"Cannot cast type {cachedValue.GetType().Name} to type {typeof(T).Name}");
        }

        public void Remove(string key)
        {
            if (!cachedVaues.ContainsKey(key)) throw new ArgumentException($"No cached value found for: {key}");

            cachedVaues.Remove(key);
        }

        public void Store(string key, object value)
        {
            cachedVaues.Add(key, value);
        }
    }
}
