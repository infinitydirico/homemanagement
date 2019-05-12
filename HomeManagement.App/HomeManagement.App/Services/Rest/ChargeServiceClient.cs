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

    public class ChargeServiceClient : IChargeServiceClient
    {
        public async Task Delete(int id)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .DeleteAsync($"{Constants.Endpoints.Charge.CHARGE}/?id={id.ToString()}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<ChargePageModel> Page(ChargePageModel dto)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Charge.PAGE, dto.SerializeToJson())
                .ReadContent<ChargePageModel>();
        }

        public async Task Post(Charge charge)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Charge.CHARGE, charge.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }

        public async Task Put(Charge charge)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PutAsync(Constants.Endpoints.Charge.CHARGE, charge.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }
    }
}
