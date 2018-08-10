using HomeManagement.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManagement.Data
{
    public class AccountRepository : IAccountRepository
    {
        ILayerContext layerContext;

        public AccountRepository(ILayerContext layerContext)
        {
            this.layerContext = layerContext ?? throw new ArgumentNullException($"{nameof(layerContext)} is null");
        }

        public void Add(object value)
        {
            using (layerContext)
            {
                layerContext.Add(value);
            }
        }

        public object GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
