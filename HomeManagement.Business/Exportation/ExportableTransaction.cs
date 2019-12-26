using HomeManagement.Business.Contracts;
using HomeManagement.Core;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HomeManagement.Business.Exportation
{
    public class ExportableTransaction : Exportable<Transaction>, IExportableTransaction
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IPreferencesRepository preferencesRepository;
        private readonly IUserSessionService userService;

        public ExportableTransaction(IRepositoryFactory repositoryFactory,
            IUserSessionService userService)
        {
            this.userService = userService;

            categoryRepository = repositoryFactory.CreateCategoryRepository();
            preferencesRepository = repositoryFactory.CreatePreferencesRepository();
        }

        protected override Transaction CreateInstanceOf(List<string> exportableEntity)
        {
            var transaction = new Transaction();

            var user = userService.GetAuthenticatedUser();

            transaction.Name = exportableEntity[0];
            transaction.Price = double.Parse(exportableEntity[1]);

            var culture = GetUserLanguage();

            transaction.Date = DateTime.Parse(exportableEntity[2], culture);

            transaction.TransactionType = exportableEntity[3].ToEnum<TransactionType>();

            if (exportableEntity.Count > 4 && !string.IsNullOrEmpty(exportableEntity[4]))
            {
                var categoryName = exportableEntity[4];

                categoryName = categoryName.ToLower();

                var categories = categoryRepository.Where(c => c.Name.ToLower().Equals(categoryName));

                if (categories.Count() > default(int))
                {
                    var category = categories.FirstOrDefault();
                    transaction.CategoryId = category.Id;
                }
            }

            //default fallback
            if (transaction.CategoryId == 0)
            {
                var category = categoryRepository.FirstOrDefault(x => x.UserId.Equals(user.Id));

                transaction.CategoryId = category.Id;
            }

            return transaction;
        }

        protected override string GetValues(Transaction exportableEntity)
        {
            var headers = GetProperties();

            StringBuilder sb = new StringBuilder();

            var culture = GetUserLanguage();

            foreach (var header in headers)
            {
                switch (header)
                {
                    case nameof(Transaction.Name):
                        sb.Append(exportableEntity.Name + divider);
                        break;
                    case nameof(Transaction.Price):
                        sb.Append(exportableEntity.Price.ToString() + divider);
                        break;
                    case nameof(Transaction.Date):
                        sb.Append(exportableEntity.Date.ToString(culture) + divider);
                        break;
                    case nameof(Transaction.TransactionType):
                        sb.Append(exportableEntity.TransactionType.ToString() + divider);
                        break;
                    case nameof(Transaction.CategoryName):
                        var category = categoryRepository.GetById(exportableEntity.CategoryId);
                        sb.Append(category?.Name);
                        break;
                    default:
                        break;
                }
            }

            return sb.ToString();
        }

        private CultureInfo GetUserLanguage()
        {
            var user = userService.GetAuthenticatedUser();

            var languagePreference = preferencesRepository.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.Key.Equals("Language"));

            var lang = languagePreference?.Value ?? "en";

            return new CultureInfo(lang);
        }
    }
}
