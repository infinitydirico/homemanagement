namespace HomeManagement.Models
{
    public class StorageItemModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsFolder { get; set; }

        public ulong Size { get; set; }

        public int ChargeId { get; set; }
    }
}
