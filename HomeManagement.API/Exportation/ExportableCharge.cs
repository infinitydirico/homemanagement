using HomeManagement.Contracts;
using HomeManagement.Core.Extensions;
using HomeManagement.Core;
using HomeManagement.Data;
using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeManagement.API.Exportation
{
    public interface IExportableCharge : IExportable<Transaction>
    {
    }

    public class ExportableCharge : Exportable<Transaction>, IExportableCharge
    {
        ICategoryRepository categoryRepository;

        public ExportableCharge(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        protected override Transaction CreateInstanceOf(List<string> exportableEntity)
        {
            var transaction = new Transaction();

            transaction.Name = exportableEntity[0];
            transaction.Price = Convert.ToInt32(exportableEntity[1]);

            //TODO use user settings (table settings) to fetch user default language
            DateTime date;
            if (!DateTime.TryParse(exportableEntity[2], out date))
            {                
                date = ForceParsingDateTime(exportableEntity[2]);
            }
            transaction.Date = date;

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

        private DateTime ForceParsingDateTime(string v)
        {
            try
            {
                return DateTime.Parse(v, new System.Globalization.CultureInfo("es-ar"));
            }
            catch
            {
                return LastResort(v);
            }
        }

        private DateTime LastResort(string v)
        {
            var splitted = v.Split("/");

            var yearAndTime = splitted[2].Split(" ");
            var year = Convert.ToInt32(yearAndTime[0]);
            var month = Convert.ToInt32(splitted[0]);
            var day = Convert.ToInt32(splitted[1]);

            if (month > 12)
            {
                month = day;
                day = Convert.ToInt32(splitted[0]);
            }

            return new DateTime(year, month, day);
        }

        protected override string GetValues(Transaction exportableEntity)
        {
            var headers = GetProperties();

            StringBuilder sb = new StringBuilder();

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
                        sb.Append(exportableEntity.Date.ToString() + divider);
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
