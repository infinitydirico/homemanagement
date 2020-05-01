using HomeManagement.Business.Contracts;
using HomeManagement.Core;
using HomeManagement.Domain;
using System.Collections.Generic;
using System.Text;

namespace HomeManagement.Business.Exportation
{
    public class ExportableCategory : Exportable<Category>, IExportableCategory
    {
        protected override Category CreateInstanceOf(List<string> exportableEntity)
        {
            var category = new Category();

            category.Name = exportableEntity[0];
            category.IsActive = bool.Parse(exportableEntity[1]);
            category.Icon = exportableEntity[2];
            category.Measurable = bool.Parse(exportableEntity[3]);

            return category;
        }

        protected override string GetValues(Category exportableEntity)
        {
            var headers = GetProperties();

            StringBuilder sb = new StringBuilder();

            foreach (var header in headers)
            {
                switch (header)
                {
                    case nameof(Category.Name):
                        sb.Append(exportableEntity.Name + divider);
                        break;
                    case nameof(Category.IsActive):
                        sb.Append(exportableEntity.IsActive.ToString() + divider);
                        break;
                    case nameof(Category.Icon):
                        sb.Append(exportableEntity.Icon + divider);
                        break;
                    case nameof(Category.Measurable):
                        sb.Append(exportableEntity.Measurable.ToString() + divider);
                        break;
                    default:
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
