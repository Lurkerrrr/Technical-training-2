using Microsoft.EntityFrameworkCore;
using ProjectCinema.domain.Entities;
using ProjectCinema.domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.infrastructure.Repository
{
    public class MoviesRepository : IMovieRepository
    {
        private readonly CinemaTicketDbContext _context;

        public MoviesRepository(CinemaTicketDbContext context) 
        { 
            _context = context;
        }

        public Movie GetById(long id) 
        {
            return _context.Movies.Include(c => c.Seances).ThenInclude(c => c.Tickets).SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Movie> GetAll() 
        { 
            return _context.Movies.ToList();
        }

        public bool IsMovieExist(string name, int year) 
        {
            return _context.Movies.Any(x => x.Name == name && x.Year == year);
        }

        public bool IsSeanceExist(DateTime seanceDate) 
        {
            return _context.Seances.Any(x => x.Date == seanceDate);
        }

        public List<Seance> GetSeancesByMovieId(long movieId) 
        { 
            return _context.Seances.Where(x => x.MovieId == movieId).ToList();
        }
    }
}
