using HomeManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeManagement.Core
{
    public abstract class Exportable<TExport> : IExportable<TExport>
     where TExport : IExportable
    {
        TExport template = Activator.CreateInstance<TExport>();

        protected const string divider = ";";

        public virtual IList<string> GetProperties() => template.GetProperties();

        protected abstract string GetValues(TExport exportableEntity);

        protected abstract TExport CreateInstanceOf(List<string> exportableEntity);

        protected virtual string GetCsvHeaders()
        {
            var sb = new StringBuilder();

            var headers = GetProperties();

            for (int i = 0; i < headers.Count; i++)
            {
                if ((headers.Count - 1).Equals(i))
                {
                    sb.Append(headers[i]);
                }
                else
                {
                    sb.Append(headers[i] + divider);
                }
            }

            return sb.ToString();
        }

        public byte[] ToCsv(List<TExport> collection)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetCsvHeaders());

            foreach (var item in collection)
            {
                sb.AppendLine(GetValues(item));
            }

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());

            return bytes;
        }

        public virtual List<TExport> ToEntities(byte[] rawData)
        {
            var text = Encoding.ASCII.GetString(rawData);
            var rows = text.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None).ToList();

            var header = rows.FirstOrDefault();
            var headerValues = header.Split(new string[] { divider }, StringSplitOptions.None).ToList();

            var list = new List<TExport>();

            foreach (var row in rows)
            {
                if (rows.IndexOf(row).Equals(default(int)) || string.IsNullOrEmpty(row)) continue;

                var rowValues = row.Split(new string[] { divider }, StringSplitOptions.None).ToList();

                var entity = CreateInstanceOf(rowValues);

                list.Add(entity);
            }

            return list;
        }
    }
}
