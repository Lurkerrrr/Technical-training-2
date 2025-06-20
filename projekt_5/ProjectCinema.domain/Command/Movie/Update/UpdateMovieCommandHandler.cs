using MediatR;
using ProjectCinema.domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Command.Movie.Update
{
    internal class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Result>
    {
        private readonly IMoviesReposytory _moviesRepository;

        public UpdateMovieCommandHandler(IMoviesReposytory repository) 
        { 
            _moviesRepository = repository;
        }

        public async Task<Result> Handle(UpdateMovieCommand request, CancellationToken cancellationToken) 
        { 
            var validator = new UpdateMovieCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult);
            }

            var movie = await _moviesRepository.GetByIdAsync(request.Id);
            if (movie is null)
            {
                return Result.Fail("This movie does not exist");
            }
            else 
            {
                movie.SetName(request.Name);
                movie.SetYear(request.Year);
                movie.SetSeanceTime(request.SeanceTime);

                await _moviesRepository.UpdateAsync(movie);

                return Result.Ok();
            }
        }
    }
}