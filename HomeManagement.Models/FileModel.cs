using System.IO;

namespace HomeManagement.Models
{
    public class FileModel
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public Stream FileStream { get; set; }
    }
}
