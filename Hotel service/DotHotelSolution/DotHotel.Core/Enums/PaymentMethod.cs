using System.ComponentModel.DataAnnotations;

namespace DotHotel.Core.Enums
{
    public enum PaymentMethod
    {
        None,
        Cash,
        [Display(Name = "Google Pay")]
        GooglePay,
        BLIK
    }
}
