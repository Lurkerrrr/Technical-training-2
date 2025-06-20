using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Entities
{
    [Table("Categories", Schema = "Cinema")]
    public class Category : common.entities.BaseEntity
    {
        protected Category() { }

        public Category(string name) 
        { 
            Name = name;
        }

        [MaxLength(256)]
        [Required]
        public string Name { get; protected set; }

        public virtual ICollection<Movie> Movies { get; protected set; } = new List<Movie>();

        public void SetName(string name)
        {
            Name = name;
        }
    }
}
