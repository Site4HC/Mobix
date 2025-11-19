using System.ComponentModel.DataAnnotations;

namespace Mobix.Api.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "User"; 

        [StringLength(500)]
        public string AvatarUrl { get; set; }
    }
}