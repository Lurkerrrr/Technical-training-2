using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Entities
{
    public class Seance : common.entities.BaseEntity
    {
        protected Seance() { }

        public Seance(DateTime date, long movieId) 
        { 
        
        }

        public DateTime Date { get; protected set; }
        public long Id { get; protected set; }
        public long MovieId { get; protected set; }
        public Movie Movie { get; protected set; }

        public virtual ICollection<Ticket> Tickets { get; protected set; } = new HashSet<Ticket>();

        public List<Ticket> GetTicketByEmail(string email) 
        { 
            return Tickets.Where(x => x.Email == email).OrderBy(x => x.CreatedOn).ToList();
        }

        public List<Ticket> GetAllSeanceTicket() 
        { 
            return Tickets == null ? new List<Ticket>() : Tickets.ToList();
        }

        public void Add(Ticket ticket) 
        { 
            Tickets.Add(ticket);
        }
    }
}
