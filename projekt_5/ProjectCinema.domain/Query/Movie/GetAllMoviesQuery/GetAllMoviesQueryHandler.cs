using MediatR;
using ProjectCinema.domain.Query.Dtos;
using ProjectCinema.domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Query.Movie.GetAllMoviesQuery
{
    internal sealed class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, List<MovieDto>>
    {
        private readonly IMoviesReposytory _repository;

        public GetAllMoviesQueryHandler(IMoviesReposytory repository)
        {
            _repository = repository;
        }

        public async Task<List<MovieDto>> Handle(GetAllMoviesQuery query, CancellationToken cancellationToken) 
        {
            var movies = await _repository.GetAllAsync();

            return movies
                .Select(movie => new MovieDto(movie.Id, movie.Name))
                .ToList();
        }
    }
}
