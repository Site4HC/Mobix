using System.ComponentModel.DataAnnotations;

namespace Mobix.Api.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Некоректний формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        public string Password { get; set; }
    }
}