using System.ComponentModel.DataAnnotations;

namespace BookStore.API.DTOs
{
    public class UserForLoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
