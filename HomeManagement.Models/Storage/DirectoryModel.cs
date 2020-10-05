using System.Collections.Generic;

namespace HomeManagement.Models.Storage
{
    public class DirectoryModel
    {
        public DirectoryModel()
        {
            Children = new List<DirectoryModel>();
        }

        public string Name { get; set; }

        public DirectoryModel Parent { get; set; }

        public string Path { get; set; }

        public bool IsDirectory { get; set; }

        public List<DirectoryModel> Children { get; }
    }
}
