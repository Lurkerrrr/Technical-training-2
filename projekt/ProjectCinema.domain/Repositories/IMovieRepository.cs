using ProjectCinema.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Repositories
{
    public interface IMovieRepository
    {
        Movie GetById(long id);
        IEnumerable<Movie> GetAll();
        IEnumerable<Category> GetCategories();
        bool IsMovieExist(string name, int year);
        bool IsSeanceExist(DateTime seanceDate);
        void Add(Movie movie);
        void Update(Movie movie);
        Movie GetSeanceDetails(long movieId);
        List<Seance> GetSeanceByMovieId(long movieId);
        void Remove(Movie movie);
    }
}
