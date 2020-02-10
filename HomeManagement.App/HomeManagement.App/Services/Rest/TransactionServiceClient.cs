using HomeManagement.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class TransactionServiceClient
    {
        BaseRestClient restClient;

        public TransactionServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task Delete(int id)
        {
            await restClient.Delete($"{Endpoints.Transaction.TRANSACTION}/?id={id.ToString()}");
        }

        public async Task<IEnumerable<TransactionModel>> GetAutoComplete()
        {
            var result = await restClient.Get<IEnumerable<TransactionModel>>($"{Endpoints.Transaction.TRANSACTION}GetAutoComplete");
            return result;
        }

        public async Task<TransactionPageModel> Page(TransactionPageModel dto)
        {
            var result = await restClient.Post<TransactionPageModel>(Endpoints.Transaction.PAGE, dto);
            return result;
        }

        public async Task Post(TransactionModel transaction)
        {
            await restClient.Post(Endpoints.Transaction.TRANSACTION, transaction);
        }

        public async Task<TransactionModel> PostPicture(Stream stream)
        {
            using (var client = await restClient.CreateAuthenticatedClient())
            {
                var streamContent = new System.Net.Http.StreamContent(stream);
                var result = await client.PostAsync(Endpoints.Images.Image, streamContent);
                result.EnsureSuccessStatusCode();

                var content = await result.Content.ReadAsStringAsync();
                var objectResult = JsonConvert.DeserializeObject<TransactionModel>(content);
                return objectResult;
            }
        }

        public async Task Put(TransactionModel transaction)
        {
            await restClient.Put(Endpoints.Transaction.TRANSACTION, transaction);
        }
    }
}
