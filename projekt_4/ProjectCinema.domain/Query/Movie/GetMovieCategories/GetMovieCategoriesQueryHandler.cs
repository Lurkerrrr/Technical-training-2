using MediatR;
using ProjectCinema.domain.Query.Dtos;
using ProjectCinema.domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Query.Movie.GetMovieCategories
{
    internal class GetMovieCategoriesQueryHandler : IRequestHandler<GetMovieCategoriesQuery, List<MovieCategoryDto>>
    {
        private readonly IMoviesReposytory _repository;

        public GetMovieCategoriesQueryHandler(IMoviesReposytory repository)
        { 
            _repository = repository;
        }

        public async Task<List<MovieCategoryDto>> Handle(GetMovieCategoriesQuery query, CancellationToken cancellationToken) 
        {
            var movies = await _repository.GetMovieCategoriesAsync();

            return movies
                .Select(movie => new MovieCategoryDto(movie.Id, movie.Name))
                .ToList();
        }
    }
}
