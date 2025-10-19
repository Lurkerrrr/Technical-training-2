using System.ComponentModel.DataAnnotations;

namespace DotHotel.Core.DataTransferObjects.IdentityObjects
{
    public class LoginObject
    {
        [Required(ErrorMessage = "Username can't be blank.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password can't be blank.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
