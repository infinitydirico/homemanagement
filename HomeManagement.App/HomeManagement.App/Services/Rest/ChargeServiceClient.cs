using HomeManagement.App.Common;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public interface IChargeServiceClient
    {
        Task<ChargePageModel> Page(ChargePageModel dto);

        Task Delete(int id);

        Task Post(Charge charge);

        Task Put(Charge charge);
    }

    public class ChargeServiceClient : BaseService, IChargeServiceClient
    {
        public Task Delete(int id)
        {
            return this.Delete(id, Constants.Endpoints.Charge.CHARGE);
        }

        public Task<ChargePageModel> Page(ChargePageModel dto)
        {
            return this.Post(dto, Constants.Endpoints.Charge.PAGE);
        }

        public Task Post(Charge charge)
        {
            return this.Post(charge, Constants.Endpoints.Charge.CHARGE);
        }

        public Task Put(Charge charge)
        {
            return this.Put(charge, Constants.Endpoints.Charge.CHARGE);
        }
    }
}
