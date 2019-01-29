using HomeManagement.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HomeManagement.API.Tests.Builders
{
    public class TestContext<TContext> : TestContext
     where TContext : TestContext
    {
        public TestContext(TestServerFixture fixture)
            : base(fixture)
        {
        }

        public TContext Serialize(object value)
        {
            RequestBody = JsonConvert.SerializeObject(value);
            return this as TContext;
        }

        public TContext AssertCondition(Action action)
        {
            action();
            return this as TContext;
        }

        public TContext ContainsToken()
        {
            Assert.NotNull(modelObject);
            Assert.IsType<UserModel>(modelObject);
            Assert.NotNull((modelObject as UserModel).Token);

            return this as TContext;
        }

        public TContext ProvideAuthorizationToken(string token)
        {
            if (string.IsNullOrEmpty(Token))
            {
                Token = token;
            }

            if (!fixture.Client.DefaultRequestHeaders.Contains("Authorization"))
            {
                fixture
                    .Client
                    .DefaultRequestHeaders.Add("Authorization", Token);
            }

            return this as TContext;
        }

        public string GetAuthorizationToken() => (modelObject as UserModel).Token;

        public TContext EnsureSuccessResponse()
        {
            Response.EnsureSuccessStatusCode();
            return this as TContext;
        }

        public TContext GetAsync(string uri)
        {
            Response =
                fixture
                .Client
                .GetAsync(uri)
                .Result;

            return this as TContext;
        }

        public TContext PostAsync(string uri)
        {
            Response = fixture
                .Client
                .PostAsync(uri, new StringContent(RequestBody, Encoding.UTF8, "application/json"))
                .Result;

            return this as TContext;
        }

        public TContext PutAsync(string uri)
        {
            Response = fixture
                .Client
                .PutAsync(uri, new StringContent(RequestBody, Encoding.UTF8, "application/json"))
                .Result;

            return this as TContext;
        }

        public TContext DeleteAsync(string uri)
        {
            Response = fixture
                .Client
                .DeleteAsync(uri)
                .Result;

            return this as TContext;
        }        

        public TEntity GetResponseValues<TEntity>() => (TEntity)modelObject;

        protected void ReadResponse<TEntity>()
        {
            modelObject = JsonConvert.DeserializeObject<TEntity>(Response.Content.ReadAsStringAsync().Result);
        }
    }
}
