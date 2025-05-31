using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.domain.Query.Dtos
{
    public sealed record MovieSeanceDetailsDto(
        long MovieId,
        long SeanceId, 
        string MovieName,
        DateTime SeanceDate
    );
}
