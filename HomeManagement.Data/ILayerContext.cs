using System;

namespace HomeManagement.Data
{
    public interface ILayerContext : IDisposable
    {
        void Add(object value);
    }
}
