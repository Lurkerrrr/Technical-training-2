using System.ComponentModel.DataAnnotations;

namespace ProjectCinema.Components.Models
{
    public class CreateMovieFormModel
    {
        [Required]
        public string Name { get; set; }
        public int Year { get; set; }
        public int SeanceTime { get; set; }
        public long MovieCategoryId { get; set; }
    }
}
