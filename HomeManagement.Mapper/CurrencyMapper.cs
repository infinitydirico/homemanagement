using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public interface ICurrencyMapper : IMapper<Currency, CurrencyModel>
    {

    }

    public class CurrencyMapper : BaseMapper<Currency, CurrencyModel>, ICurrencyMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Currency).GetProperty(nameof(Currency.Id));
            yield return typeof(Currency).GetProperty(nameof(Currency.Name));
            yield return typeof(Currency).GetProperty(nameof(Currency.Value));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(CurrencyModel).GetProperty(nameof(CurrencyModel.Id));
            yield return typeof(CurrencyModel).GetProperty(nameof(CurrencyModel.Name));
            yield return typeof(CurrencyModel).GetProperty(nameof(CurrencyModel.Value));
        }
    }
}
