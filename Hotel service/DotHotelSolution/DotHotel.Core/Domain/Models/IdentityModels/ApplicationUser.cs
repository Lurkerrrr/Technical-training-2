using DotHotel.Core.Domain.Models.HotelModels;
using Microsoft.AspNetCore.Identity;

namespace DotHotel.Core.Domain.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
