using System.ComponentModel.DataAnnotations;

namespace Mobix.Api.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Некоректний формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [MinLength(6, ErrorMessage = "Пароль має бути щонайменше 6 символів")]
        public string Password { get; set; }
    }
}