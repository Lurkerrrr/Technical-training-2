using ProjectCinema.domain.Command.Movie.Create;
using ProjectCinema.domain.Command.Movie.Update;
using ProjectCinema.domain.Query.Movie.GetAllMoviesQuery;
using ProjectCinema.domain.Repositories;
using ProjectCinema.infrastructure;
using ProjectCinema.infrastructure.Repository;

namespace ProjectCinema.Extentions
{
    public static class CinemaDependancyInjection
    {
        public static IServiceCollection CinemaAddApplication(this IServiceCollection services) {
            services.AddScoped<IMoviesReposytory, MoviesRepository>();
            services.AddDbContext<CinemaTicketDbContext, CinemaTicketDbContext>();
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(CreateMovieCommand).Assembly);
                //cfg.RegisterServicesFromAssembly(typeof(DeleteMovieCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdateMovieCommand).Assembly);
                //cfg.RegisterServicesFromAssembly(typeof(RegisterSeanceCommand).Assembly);
                //cfg.RegisterServicesFromAssembly(typeof(BuyTicketCommand).Assembly);
                
                cfg.RegisterServicesFromAssembly(typeof(GetAllMoviesQuery).Assembly);
                //cfg.RegisterServicesFromAssembly(typeof(GetMovieQuery).Assembly);
                //cfg.RegisterServicesFromAssembly(typeof(GetMovieCategoriesQueryHandler).Assembly);
            });
            return services;
        }
    }
}
