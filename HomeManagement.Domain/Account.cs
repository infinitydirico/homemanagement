using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManagement.Domain
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public int Balance { get; set; }

        public bool ExcludeFromStatistics { get; set; }

        public bool IsCash { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public virtual List<Charge> Charges { get; set; }

        public virtual List<Tax> Taxes { get; set; }
    }
}
