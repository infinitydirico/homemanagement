using System;
using System.Linq;

namespace HomeManagement.Models
{
    public class StorageFileModel
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public string Tag { get; set; }

        public long Size { get; set; }

        public DateTime LastModified { get; set; }

        public string GetFilename() => Key.Split('/').Last();
    }
}
