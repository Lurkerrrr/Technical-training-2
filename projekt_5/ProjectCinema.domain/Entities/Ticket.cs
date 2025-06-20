using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Entities
{
    public class Ticket : common.entities.BaseEntity
    {
        protected Ticket() { }

        public Ticket(string email, int peopleCount)
        { 
            Email = email;
            PeopleCount = peopleCount;
            CreatedOn = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(256)]
        public string Email { get; protected set; }
        public long Id { get; protected set; }
        [Range(1, 100)]
        public int PeopleCount { get; protected set; }
        public long SeanceId { get; protected set; }
        public Seance Seance { get; protected set; }
    }
}
