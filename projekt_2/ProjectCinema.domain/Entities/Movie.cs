using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Entities
{
    public class Movie : common.entities.BaseEntity
    {
        protected Movie() { }

        public Movie(string name, int year, int seanceTime, long categoryId) 
        { 
            Name = name;
            Year = year;
            SeanceTime = seanceTime;
            CategoryId = categoryId;
        }

        public long CategoryId { get; protected set; }
        public Category Category { get; protected set; }

        public long Id { get; protected set; }
        
        [MaxLength(256)]
        [Required]
        public string Name { get; protected set; }
        
        [Range(1888, 2100)]
        public int Year { get; protected set; }
        
        [Range(1, 600)]
        public int SeanceTime { get; protected set; }

        public virtual ICollection<Seance> Seances { get; protected set; }

        public Seance GetSeanceByDateAndRoomId(DateTime date) 
        {
            return Seances.SingleOrDefault(x => DateTime.Compare(x.Date, date) == 1);
        }
        public void SetName(string name) 
        { 
            Name = name;
        }
        public void SetYear(int year) 
        { 
            Year = year;
        }
        public void SetSeanceTime(int seancetime) 
        { 
            SeanceTime = seancetime;
        }
    }
}
