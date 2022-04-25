using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
