using System.Collections.Generic;

namespace HomeManagement.Models.Storage
{
    public class FolderModel
    {
        public FolderModel()
        {
            Folders = new List<FolderModel>();
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public List<FolderModel> Folders { get; set; }
    }
}
