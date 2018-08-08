using System.ComponentModel.DataAnnotations.Schema;

namespace HomeManagement.Domain
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}