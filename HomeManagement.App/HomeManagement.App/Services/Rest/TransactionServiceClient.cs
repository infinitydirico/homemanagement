using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.Models;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public interface ITransactionServiceClient
    {
        Task<ChargePageModel> Page(ChargePageModel dto);

        Task Delete(int id);

        Task Post(ChargeModel charge);

        Task Put(ChargeModel charge);
    }

    public class TransactionServiceClient : ITransactionServiceClient
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

        public async Task Post(ChargeModel charge)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Charge.CHARGE, charge.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }

        public async Task Put(ChargeModel charge)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PutAsync(Constants.Endpoints.Charge.CHARGE, charge.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }
    }
}
