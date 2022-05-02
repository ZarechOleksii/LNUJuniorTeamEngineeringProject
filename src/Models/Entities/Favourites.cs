using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Favourites")]
    public class Favourites : BaseEntity
    {
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public Movie Movie { get; set; }
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
    }
}
