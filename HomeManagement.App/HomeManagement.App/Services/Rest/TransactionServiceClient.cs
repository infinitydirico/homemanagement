using HomeManagement.App.Common;
using HomeManagement.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public interface ITransactionServiceClient
    {
        Task<TransactionPageModel> Page(TransactionPageModel dto);

        Task Delete(int id);

        Task Post(TransactionModel transaction);

        Task Put(TransactionModel transaction);

        Task<TransactionModel> PostPicture(Stream stream);

        Task<IEnumerable<TransactionModel>> GetAutoComplete();
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

        public async Task<IEnumerable<TransactionModel>> GetAutoComplete()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync($"{Constants.Endpoints.Transaction.TRANSACTION}GetAutoComplete")
                .ReadContent< IEnumerable<TransactionModel>>();
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

        public async Task<TransactionModel> PostPicture(Stream stream)
        {
            var streamContent = new System.Net.Http.StreamContent(stream);
            var response = await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Images.Image, streamContent);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<TransactionModel>(content);
            return objectResult;
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
