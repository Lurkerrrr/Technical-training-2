namespace ProjectCinema.domain.Query.Dtos { 
    public sealed record MovieDetailsDto(
        long Id,
        string Name,
        int Year,
        int SeanceTime,
        List<SeanceDto> Seances
    );
}