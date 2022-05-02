using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("MovieRating")]
    public class MovieRate : BaseEntity
    {
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public Movie Movie { get; set; }
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }

        [Required]
        public byte Rate { get; set; }
    }
}
