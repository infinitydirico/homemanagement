using System.Collections.Generic;
using System.Reflection;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public class ChargeMapper : BaseMapper<Charge, ChargeModel>, IChargeMapper
    {
        public override Charge ToEntity(ChargeModel model)
        {
            return new Charge
            {
                Id = model.Id,
                AccountId = model.AccountId,
                CategoryId = model.CategoryId,
                ChargeType = model.ChargeType == ChargeTypeModel.Income ? ChargeType.Income : ChargeType.Expense,
                Date = model.Date,
                Name = model.Name,
                Price = model.Price
            };
        }

        public override ChargeModel ToModel(Charge entity)
        {
            return new ChargeModel
            {
                Id = entity.Id,
                AccountId = entity.AccountId,
                CategoryId = entity.CategoryId,
                ChargeType = entity.ChargeType == ChargeType.Income ? ChargeTypeModel.Income : ChargeTypeModel.Expense,
                Date = entity.Date,
                Name = entity.Name,
                Price = entity.Price
            };
        }

        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Charge).GetProperty(nameof(Charge.Id));
            yield return typeof(Charge).GetProperty(nameof(Charge.Name));
            yield return typeof(Charge).GetProperty(nameof(Charge.Price));
            yield return typeof(Charge).GetProperty(nameof(Charge.Date));
            yield return typeof(Charge).GetProperty(nameof(Charge.ChargeType));
            yield return typeof(Charge).GetProperty(nameof(Charge.AccountId));
            yield return typeof(Charge).GetProperty(nameof(Charge.CategoryId));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(ChargeModel).GetProperty(nameof(ChargeModel.Id));
            yield return typeof(ChargeModel).GetProperty(nameof(ChargeModel.Name));
            yield return typeof(ChargeModel).GetProperty(nameof(ChargeModel.Price));
            yield return typeof(ChargeModel).GetProperty(nameof(ChargeModel.Date));
            yield return typeof(ChargeModel).GetProperty(nameof(ChargeModel.ChargeType));
            yield return typeof(ChargeModel).GetProperty(nameof(ChargeModel.AccountId));
            yield return typeof(ChargeModel).GetProperty(nameof(ChargeModel.CategoryId));
        }
    }
}
