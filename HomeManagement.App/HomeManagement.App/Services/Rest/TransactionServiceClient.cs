using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.Models;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public interface ITransactionServiceClient
    {
        Task<TransactionPageModel> Page(TransactionPageModel dto);

        Task Delete(int id);

        Task Post(TransactionModel charge);

        Task Put(TransactionModel charge);
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

        public async Task<TransactionPageModel> Page(TransactionPageModel dto)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Charge.PAGE, dto.SerializeToJson())
                .ReadContent<TransactionPageModel>();
        }

        public async Task Post(TransactionModel charge)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Charge.CHARGE, charge.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }

        public async Task Put(TransactionModel charge)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PutAsync(Constants.Endpoints.Charge.CHARGE, charge.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }
    }
}
