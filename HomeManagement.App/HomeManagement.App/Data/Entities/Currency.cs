using System;

namespace HomeManagement.App.Data.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public DateTime ChangeStamp { get; set; }
    }
}
