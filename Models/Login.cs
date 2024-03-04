using System.ComponentModel.DataAnnotations;

namespace BicycleStore.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
