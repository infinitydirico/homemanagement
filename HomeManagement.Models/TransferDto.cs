using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManagement.Models
{
    public class TransferDto
    {
        public string OperationName { get; set; }

        public int SourceAccountId { get; set; }

        public int TargetAccountId { get; set; }

        public int Price { get; set; }

        public int CategoryId { get; set; }
    }
}
