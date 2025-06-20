using ProjectCinema.domain.Query.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;
using ProjectCinema.Components.Models;
using ProjectCinema.domain.Command;
using ProjectCinema.domain.Command.Movie.Create;
using ProjectCinema.domain.Query.Movie.GetMovieCategories;

namespace ProjectCinema.Components.Pages
{
    public partial class MovieCreate : ComponentBase
    {
        [Inject]
        public IMediator MediatorService { get; set; } = default!;

        public CreateMovieFormModel Model { get; set; } = new();
        private List<MovieCategoryDto> Categories = new();
        private List<Result.Error> ValidationErrors = new();
        private string? SuccessMessage;

        protected override async Task OnInitializedAsync() 
        {
            Categories = await MediatorService.Send(new GetMovieCategoriesQuery());
        }

        private async Task HandleValidSubmit() 
        { 
            ValidationErrors.Clear();
            SuccessMessage = null;

            var command = new CreateMovieCommand(Model.Name, Model.Year, Model.SeanceTime, Model.MovieCategoryId);
            var result = await MediatorService.Send(command);

            if (result.IsSuccess)
            {
                SuccessMessage = "Movie created succesfully!";
                Model = new();
            }
            else 
            { 
                ValidationErrors = result.Errors.ToList();
            }
        }
    }
}
