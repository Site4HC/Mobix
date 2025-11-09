using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mobix.Api.Data;
using Mobix.Api.DTOs;
using Mobix.Api.Models; 
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace Mobix.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    [Authorize] 
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return Guid.Empty;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserIdFromToken();
            if (userId == Guid.Empty)
            {
                return Unauthorized(new { message = "Токен недійсний або відсутній ID." });
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(new { message = "Профіль не знайдено." });
            }
            
            var favorites = await _context.UserFavorites
                .Where(uf => uf.UserId == userId)
                .Select(uf => new UserFavoriteDto
                {
                    Id = uf.SmartphoneId, 
                    Name = uf.Smartphone.Name,
                    ImageUrl = uf.Smartphone.ImageUrl
                })
                .ToListAsync();


            var userProfile = new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Favorites = favorites 
            };

            return Ok(userProfile);
        }

        [HttpPost("favorites/{smartphoneId:int}")]
        public async Task<IActionResult> AddFavorite(int smartphoneId)
        {
            var userId = GetUserIdFromToken();
            if (userId == Guid.Empty) return Unauthorized();

            if (!await _context.Smartphones.AnyAsync(s => s.Id == smartphoneId))
            {
                return NotFound(new { message = "Смартфон не знайдено" });
            }

            var favorite = new UserFavorite
            {
                UserId = userId,
                SmartphoneId = smartphoneId
            };

            try
            {
                _context.UserFavorites.Add(favorite);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Додано до вибраного" });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Вже у вибраному" });
            }
        }


        [HttpDelete("favorites/{smartphoneId:int}")]
        public async Task<IActionResult> RemoveFavorite(int smartphoneId)
        {
            var userId = GetUserIdFromToken();
            if (userId == Guid.Empty) return Unauthorized();

            var favorite = await _context.UserFavorites
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.SmartphoneId == smartphoneId);

            if (favorite == null)
            {
                return NotFound(new { message = "У вибраному не знайдено" });
            }

            _context.UserFavorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Видалено з вибраного" });
        }
    }
}