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
    public interface IExportableCharge : IExportable<Charge>
    {
    }

    public class ExportableCharge : Exportable<Charge>, IExportableCharge
    {
        ICategoryRepository categoryRepository;

        public ExportableCharge(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        protected override Charge CreateInstanceOf(List<string> exportableEntity)
        {
            var charge = new Charge();

            charge.Name = exportableEntity[0];
            charge.Price = Convert.ToInt32(exportableEntity[1]);

            DateTime date;
            if (!DateTime.TryParse(exportableEntity[2], out date))
            {
                date = ForceParsingDateTime(exportableEntity[2]);
            }
            charge.Date = date;

            charge.ChargeType = exportableEntity[3].ToEnum<ChargeType>();

            if (exportableEntity.Count > 4 && !string.IsNullOrEmpty(exportableEntity[4]))
            {
                var categoryName = exportableEntity[4];

                categoryName = categoryName.ToLower();

                var categories = categoryRepository.Where(c => c.Name.ToLower().Equals(categoryName));

                if (categories.Count() > default(int))
                {
                    var category = categories.FirstOrDefault();
                    charge.CategoryId = category.Id;
                }
            }
            else
            {
                var category = categoryRepository.FirstOrDefault(x => x.IsDefault);

                charge.CategoryId = category.Id;
            }

            return charge;
        }

        private DateTime ForceParsingDateTime(string v)
        {
            var splitted = v.Split("/");

            var yearAndTime = splitted[2].Split(" ");
            var year = Convert.ToInt32(yearAndTime[0]);
            var month = Convert.ToInt32(splitted[0]);
            var day = Convert.ToInt32(splitted[1]);

            return new DateTime(year, month, day);
        }

        protected override string GetValues(Charge exportableEntity)
        {
            var headers = GetProperties();

            StringBuilder sb = new StringBuilder();

            foreach (var header in headers)
            {
                switch (header)
                {
                    case nameof(Charge.Name):
                        sb.Append(exportableEntity.Name + divider);
                        break;
                    case nameof(Charge.Price):
                        sb.Append(exportableEntity.Price.ToString() + divider);
                        break;
                    case nameof(Charge.Date):
                        sb.Append(exportableEntity.Date.ToString() + divider);
                        break;
                    case nameof(Charge.ChargeType):
                        sb.Append(exportableEntity.ChargeType.ToString() + divider);
                        break;
                    case nameof(Charge.CategoryName):
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
