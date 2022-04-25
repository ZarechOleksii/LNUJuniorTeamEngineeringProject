using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    [Table("Movies")]
    public class Movie : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        public List<Favourites> Favourites { get; set; }
        public List<Comment> Comments { get; set; }

        public Movie()
        {
            Favourites = new List<Favourites>();
            Comments = new List<Comment>();
        }
    }
}
