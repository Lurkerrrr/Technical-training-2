using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Command.Movie.Update
{
    internal class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
    {
        public UpdateMovieCommandValidator() 
        {
            RuleFor(x => x.Id)
                .InclusiveBetween(1, 1000)
                .WithMessage("Id musi być pomiędzy 1 a 1000");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Nazwa filmu wymagana")
                .MaximumLength(256)
                .WithMessage("Maksymalna liczba znaków 256");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, 2080)
                .WithMessage("Rok musi być między 1900 a 2080");

            RuleFor(x => x.SeanceTime)
                .InclusiveBetween(30, 300)
                .WithMessage("Seans musi byc pomiędzy 30 a 300 minutami");
        }
    }
}
