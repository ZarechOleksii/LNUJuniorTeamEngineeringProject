using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("BaseEntities")]
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
