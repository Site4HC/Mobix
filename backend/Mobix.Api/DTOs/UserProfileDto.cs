namespace Mobix.Api.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AvatarUrl { get; set; }
        public List<UserFavoriteDto> Favorites { get; set; } = new List<UserFavoriteDto>();
    }
}