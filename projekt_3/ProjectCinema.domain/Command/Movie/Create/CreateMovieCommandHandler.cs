using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

using ProjectCinema.domain.Repositories;

namespace ProjectCinema.domain.Command.Movie.Create
{
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result>
    {
        private readonly IMoviesReposytory _moviesRepository;

        public CreateMovieCommandHandler(IMoviesReposytory repository) 
        { 
            _moviesRepository = repository;
        }

        public async Task<Result> Handle(CreateMovieCommand request, CancellationToken cancellationToken) 
        { 
            var validator = new CreateMovieCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) 
            {
                return Result.Fail(validationResult);
            }

            if (await _moviesRepository.IsMovieExistAsync(request.Name, request.Year)) 
            {
                return Result.Fail("This movie already exists");
            }

            var movie = new ProjectCinema.domain.Entities.Movie(
                request.Name,
                request.Year,
                request.SeanceTime,
                request.MovieCategoryId
                );

            await _moviesRepository.AddAsync(movie);

            return Result.Ok();
        }
    }
}
