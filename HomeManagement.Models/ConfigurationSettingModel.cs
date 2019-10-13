using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class ConfigurationSettingModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Value { get; set; }
    }
}
