using HomeManagement.AI.Vision.Analysis;
using HomeManagement.AI.Vision.Analysis.Criterias;
using HomeManagement.AI.Vision.Entities;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HomeManagement.API.Business
{
    public class ImageService : IImageService
    {
        private readonly IConfigurationSettingsService configurationSettingsService;
        private readonly ITransactionRepository transactionRepository;
        private readonly ITransactionMapper transactionMapper;
        private readonly IPreferenceService preferenceService;
        private readonly IUserSessionService userSessionService;

        public ImageService(IConfigurationSettingsService configurationSettingsService,
            ITransactionRepository transactionRepository,
            ITransactionMapper transactionMapper,
            IPreferenceService preferenceService,
            IUserSessionService userSessionService)
        {
            this.configurationSettingsService = configurationSettingsService;
            this.transactionRepository = transactionRepository;
            this.transactionMapper = transactionMapper;
            this.preferenceService = preferenceService;
            this.userSessionService = userSessionService;
        }

        public async Task<TransactionModel> CreateTransactionFromImage(byte[] image)
        {
            try
            {
                var user = userSessionService.GetAuthenticatedUser();
                var subscriptionKey = configurationSettingsService.GetConfig("VisionApiKey");
                var apiEndpoint = configurationSettingsService.GetConfig("VisionApiEndpoint");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey.Value);
                    string requestParameters = "language=unk&detectOrientation=false";

                    string endpoint = apiEndpoint.Value;
                    string uriBase = endpoint + "vision/v2.1/ocr";
                    string uri = uriBase + "?" + requestParameters;

                    HttpResponseMessage response;

                    using (ByteArrayContent content = new ByteArrayContent(image))
                    {
                        content.Headers.ContentType =
                            new MediaTypeHeaderValue("application/octet-stream");

                        response = await client.PostAsync(uri, content);
                    }

                    string contentString = await response.Content.ReadAsStringAsync();

                    var visionResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<VisionResponseV2>(contentString);

                    var langVaule = preferenceService.GetUserLanguage(user.Id);
                    var userCountry = preferenceService.GetUserCountryCode();

                    var language = new CultureInfo($"{langVaule}-{userCountry}");

                    var trans = GetMatchingTransaction(visionResponse) ?? new Transaction();
                    var price = GetPrice(visionResponse, language);
                    var date = GetDate(visionResponse, language);

                    var entity = new Transaction
                    {
                        Name = trans.Name,
                        Price = price,
                        Date = date,
                        TransactionType = trans.TransactionType,
                        CategoryId = trans.CategoryId
                    };

                    return transactionMapper.ToModel(entity);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Transaction GetMatchingTransaction(VisionResponseV2 visionResponse)
        {
            var engine = new Engine
            {
                Criterias = new List<IMatch>()
                {
                    new TextLookUpCriteria()
                }
            };

            var matches = engine.GetAllMatches(visionResponse.RecognitionResult);

            var transactions = transactionRepository
                .Where(t => matches.Any(m => m.Contains(t.Name)))
                .ToList();

            return transactions.FirstOrDefault();
        }

        private double GetPrice(VisionResponseV2 visionResponse, CultureInfo culture)
        {
            var engine = new Engine
            {
                Criterias = new List<IMatch>()
                {
                    new MoneyLookUpCriteria()
                }
            };

            var matches = engine.GetAllMatches(visionResponse.RecognitionResult);
            var price = matches.FirstOrDefault() ?? "0";

            price = price.Replace(" ", "");
            var styles = NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            var parsed = double.Parse(price, styles, culture);
            return parsed;
        }

        private DateTime GetDate(VisionResponseV2 visionResponse, CultureInfo culture)
        {
            var engine = new Engine
            {
                Criterias = new List<IMatch>()
                {
                    new DateLookUpCriteria()
                }
            };

            var matches = engine.GetAllMatches(visionResponse.RecognitionResult);

            var d = matches.FirstOrDefault() ?? DateTime.Now.ToString(culture);

            var date = DateTime.Parse(d, culture);
            return date;
        }

        public bool IsConfigured()
        {
            var subscriptionKey = configurationSettingsService.GetConfig("VisionApiKey");
            var apiEndpoint = configurationSettingsService.GetConfig("VisionApiEndpoint");

            return subscriptionKey != null && !string.IsNullOrEmpty(subscriptionKey.Value) && 
                apiEndpoint != null && !string.IsNullOrEmpty(apiEndpoint.Value);
        }
    }

    public interface IImageService
    {
        Task<TransactionModel> CreateTransactionFromImage(byte[] image);

        bool IsConfigured();
    }
}
