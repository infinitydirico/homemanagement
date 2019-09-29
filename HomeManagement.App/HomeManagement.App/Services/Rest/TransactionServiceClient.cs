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

        Task Post(TransactionModel transaction);

        Task Put(TransactionModel transaction);
    }

    public class TransactionServiceClient : ITransactionServiceClient
    {
        public async Task Delete(int id)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .DeleteAsync($"{Constants.Endpoints.Transaction.TRANSACTION}/?id={id.ToString()}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<TransactionPageModel> Page(TransactionPageModel dto)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Transaction.PAGE, dto.SerializeToJson())
                .ReadContent<TransactionPageModel>();
        }

        public async Task Post(TransactionModel transaction)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Transaction.TRANSACTION, transaction.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }

        public async Task Put(TransactionModel transaction)
        {
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PutAsync(Constants.Endpoints.Transaction.TRANSACTION, transaction.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }
    }
}
