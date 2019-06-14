using System;
using System.Collections.Generic;

namespace HomeManagement.Core.Caching
{
    public class MemoryCache : ICachingService
    {
        Dictionary<string, object> cachedVaues = new Dictionary<string, object>();

        public bool Exists(string key) => cachedVaues.ContainsKey(key);

        public object Get(string key)
        {
            if (!cachedVaues.ContainsKey(key)) throw new ArgumentException($"No cached value found for: {key}");

            return cachedVaues[key];
        }

        public T Get<T>(string key)
        {
            if (!cachedVaues.ContainsKey(key)) throw new ArgumentException($"No cached value found for: {key}");

            var cachedValue = cachedVaues[key];

            var cachedValueType = cachedValue.GetType();

            if (cachedValueType.GetType() == typeof(T) || typeof(T).IsAssignableFrom(cachedValueType)) return (T)cachedValue;

            throw new ArgumentException($"Cannot cast type {cachedValue.GetType().Name} to type {typeof(T).Name}");
        }

        public void Remove(string key)
        {
            if (!cachedVaues.ContainsKey(key)) throw new ArgumentException($"No cached value found for: {key}");

            cachedVaues.Remove(key);
        }

        public void Store(string key, object value)
        {
            if (Exists(key)) throw new ArgumentNullException($"The {key} is already cached.");

            cachedVaues.Add(key, value);
        }

        public void Update(string key, object value)
        {
            if (!Exists(key)) throw new ArgumentNullException($"No cached value found for: {key}");

            cachedVaues[key] = value;
        }

        public void StoreOrUpdate(string key, object value)
        {
            if (Exists(key))
            {
                Update(key, value);
            }
            else
            {
                Store(key, value);
            }
        }
    }
}
