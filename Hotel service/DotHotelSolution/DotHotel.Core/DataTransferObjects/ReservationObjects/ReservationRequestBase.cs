using System.ComponentModel.DataAnnotations;
using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public abstract class ReservationRequestBase
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateNotInPast]
        [Remote(action: "ValidateReservationDates", controller: "Reservation",
            AdditionalFields = nameof(CheckOutDate) + "," + nameof(ApartmentId),
            ErrorMessage = "These dates are not available for reservation")]
        public DateTime? CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [MinimumDaysAfter(nameof(CheckInDate), 1)]
        [DateNotInPast]
        [Remote(action: "ValidateReservationDates", controller: "Reservation",
            AdditionalFields = nameof(CheckInDate) + "," + nameof(ApartmentId),
            ErrorMessage = "These dates are not available for reservation")]
        public DateTime? CheckOutDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Number of Guests")]
        [Remote(action: "ValidateGuestCount", controller: "Apartment",
        AdditionalFields = nameof(ApartmentId),
        ErrorMessage = "This apartment cannot accommodate this amount of guests")]
        public int GuestCount { get; set; }

        [Required]
        public Guid ReservationHolderId { get; set; }

        [Required]
        public Guid ApartmentId { get; set; }

        public ApartmentResponse? Apartment { get; set; }

        public abstract Reservation ToReservation(decimal totalPriceUsd);
    }
}
