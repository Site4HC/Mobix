using System.ComponentModel.DataAnnotations;

namespace Mobix.Api.DTOs
{
    public class UpdateProfileRequest
    {
        [StringLength(500)]
        public string NewAvatarUrl { get; set; }
    }
}