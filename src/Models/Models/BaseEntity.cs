using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

    }
}