using HomeManagement.Contracts;
using HomeManagement.Core.Extensions;
using HomeManagement.Core;
using HomeManagement.Data;
using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeManagement.API.Business;
using System.Globalization;

namespace HomeManagement.API.Exportation
{
    public interface IExportableTransaction : IExportable<Transaction>
    {
    }

    public class ExportableTransaction : Exportable<Transaction>, IExportableTransaction
    {
        ICategoryRepository categoryRepository;
        IPreferenceService preferenceService;
        IUserSessionService userService;

        public ExportableTransaction(ICategoryRepository categoryRepository,
            IPreferenceService preferenceService,
            IUserSessionService userService)
        {
            this.categoryRepository = categoryRepository;
            this.preferenceService = preferenceService;
            this.userService = userService;
        }

        protected override Transaction CreateInstanceOf(List<string> exportableEntity)
        {
            var transaction = new Transaction();

            transaction.Name = exportableEntity[0];
            transaction.Price = double.Parse(exportableEntity[1]);

            var user = userService.GetAuthenticatedUser();
            var language = preferenceService.GetUserLanguage(user.Id);
            var culture = new CultureInfo(language);

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
            if(transaction.CategoryId == 0)
            {
                var category = categoryRepository.FirstOrDefault(x => x.IsDefault);

                transaction.CategoryId = category.Id;
            }

            return transaction;
        }

        protected override string GetValues(Transaction exportableEntity)
        {
            var headers = GetProperties();

            StringBuilder sb = new StringBuilder();

            var user = userService.GetAuthenticatedUser();
            var language = preferenceService.GetUserLanguage(user.Id);
            var culture = new CultureInfo(language);

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
    }
}
