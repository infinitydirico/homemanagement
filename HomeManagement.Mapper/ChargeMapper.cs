using HomeManagement.Contracts.Mapper;
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
                ChargeType = model.ChargeType == ChargeTypeModel.Incoming ? ChargeType.Incoming : ChargeType.Outgoing,
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
                ChargeType = entity.ChargeType == ChargeType.Incoming ? ChargeTypeModel.Incoming : ChargeTypeModel.Outgoing,
                Date = entity.Date,
                Name = entity.Name,
                Price = entity.Price
            };
        }
    }
}
