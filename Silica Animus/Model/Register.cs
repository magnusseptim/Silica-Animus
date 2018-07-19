using System.ComponentModel.DataAnnotations;

namespace Silica_Animus.Model
{
    public class Register
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Pass. length should be between 6 and 20", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
