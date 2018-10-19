using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Services.Components;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public abstract class BaseService
    {
        private const string AuthorizationHeader = "Authorization";

        public virtual async Task<T> Get<T>(string api, bool requiresAuthentication = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.Endpoints.BASEURL);
                if (requiresAuthentication)
                {
                    client.DefaultRequestHeaders.Add(AuthorizationHeader, GetToken());
                }
                var response = await client.GetAsync(api);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var objectResult = JsonConvert.DeserializeObject<T>(content);
                    return objectResult;
                }

                throw new Exception(response.StatusCode.ToString());
            }
        }

        public virtual async Task<T> Post<T>(T data, string api, bool requiresAuthentication = false)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                client.BaseAddress = new Uri(Constants.Endpoints.BASEURL);
                if (requiresAuthentication)
                {
                    client.DefaultRequestHeaders.Add(AuthorizationHeader, GetToken());
                }
                var response = await client.PostAsync(api, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var objectResult = JsonConvert.DeserializeObject<T>(content);
                    return objectResult;
                }

                throw new Exception(response.StatusCode.ToString());
            }
        }

        public virtual async Task<TResult> Post<TPost, TResult>(TPost data, string api, bool requiresAuthentication = false)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                client.BaseAddress = new Uri(Constants.Endpoints.BASEURL);
                if (requiresAuthentication)
                {
                    client.DefaultRequestHeaders.Add(AuthorizationHeader, GetToken());
                }
                var response = await client.PostAsync(api, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var objectResult = JsonConvert.DeserializeObject<TResult>(content);
                    return objectResult;
                }

                throw new Exception(response.StatusCode.ToString());
            }
        }

        public virtual async Task<T> Put<T>(T data, string api, bool requiresAuthentication = false)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                client.BaseAddress = new Uri(Constants.Endpoints.BASEURL);
                if (requiresAuthentication)
                {
                    client.DefaultRequestHeaders.Add(AuthorizationHeader, GetToken());
                }
                var response = await client.PutAsync(api, new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var objectResult = JsonConvert.DeserializeObject<T>(content);
                    return objectResult;
                }

                throw new Exception(response.StatusCode.ToString());
            }
        }

        public virtual async Task Delete(int id, string api, bool requiresAuthentication = false)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(id);
                client.BaseAddress = new Uri(Constants.Endpoints.BASEURL);
                if (requiresAuthentication)
                {
                    client.DefaultRequestHeaders.Add(AuthorizationHeader, GetToken());
                }
                var response = await client.DeleteAsync(api + "/?id=" + id.ToString());
            }
        }

        protected string GetToken()
        {
            return App._container.Resolve<IApplicationValues>().Get("header");
        }
    }
}
