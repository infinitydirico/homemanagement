using System.IO;
using System.Linq;

namespace HomeManagement.Models
{
    public class FileModel
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;

                SetExtension();

                SetContentType();
            }
        }

        public string Extension { get; private set; }

        public string ContentType { get; private set; }

        public byte[] Contents { get; set; }

        public Stream Stream { get; set; }

        private void SetExtension()
        {
            if (string.IsNullOrEmpty(name)) return;

            var values = name.Split('.');

            if (values.Count() < 1) return;

            var extension = values.LastOrDefault();

            Extension = extension;
        }

        private void SetContentType()
        {
            switch (Extension)
            {
                case "img":
                    ContentType = "image/jpeg";
                    break;
                case "png":
                    ContentType = "image/png";
                    break;
                case "zip":
                    ContentType = "application/zip";
                    break;
                case "csv":
                    ContentType = "text/csv";
                    break;
                case "doc":
                    ContentType = "application/msword";
                    break;
                case "pdf":
                    ContentType = "application/pdf";
                    break;
                default:
                    break;
            }
        }
    }
}
