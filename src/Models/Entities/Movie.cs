using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Movies")]
    public class Movie : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        public List<Favourites> Favourites { get; set; }
        public List<Comment> Comments { get; set; }

        public List<MovieRate> Rating { get; set; }

        public Movie()
        {
            Favourites = new List<Favourites>();
            Comments = new List<Comment>();
            Rating = new List<MovieRate>();
        }
    }
}
