using System.Runtime.Serialization;

namespace HomeManagement.Models
{
    public class MetricValueDto
    {
        [DataMember(Name = "percentage")]
        public int Percentage { get; set; }

        [DataMember(Name = "total")]
        public int Total { get; set; }
    }
}
