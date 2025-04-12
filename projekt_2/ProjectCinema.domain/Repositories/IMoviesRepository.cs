using ProjectCinema.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Repositories
{
    public interface IMoviesReposytory
    {
        Task<Movie> GetByIdAsync(long id);
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<bool> IsMovieExistAsync(string name, int year);
        Task<bool> IsSeanceExistAsync(DateTime seanceDate);
        Task AddAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task<Movie> GetSeanceDetailsAsync(long movieId);
        Task<List<Seance>> GetSeanceByMovieIdAsync(long movieId);
        Task RemoveAsync(Movie movie);
    }
}
