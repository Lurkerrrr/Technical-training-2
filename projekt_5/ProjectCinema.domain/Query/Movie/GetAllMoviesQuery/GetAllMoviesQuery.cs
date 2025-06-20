using MediatR;
using ProjectCinema.domain.Query.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Query.Movie.GetAllMoviesQuery
{
    public sealed record GetAllMoviesQuery : IRequest<List<MovieDto>> 
    {
    }
}
