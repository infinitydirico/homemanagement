using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HomeManagement.API.WebApp.Services
{
    public class Json
    {        
        public static string Serialize(object jsonModel)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(jsonModel, options);
        }

        public static StringContent CreateJsonContent(object data)
        {
            var json = Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        public static TTarget Deserialize<TTarget>(string value)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var result = JsonSerializer.Deserialize<TTarget>(value, options);
            return result;
        }
    }
}
