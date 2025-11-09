using Microsoft.AspNetCore.Mvc;
using Mobix.Api.Data;
using Mobix.Api.DTOs;
using Mobix.Api.Models;
using Mobix.Api.Services;
using Microsoft.EntityFrameworkCore; 

namespace Mobix.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenGenerator _tokenGenerator;

        public AuthController(ApplicationDbContext context, TokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == registerRequest.Email.ToLower());

            if (existingUser != null)
            {
                return BadRequest(new { message = "Користувач з таким email вже існує" });
            }

            string passwordHash = PasswordHasher.HashPassword(registerRequest.Password);

            var newUser = new User
            {
                Email = registerRequest.Email,
                PasswordHash = passwordHash,
                Role = "User"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = newUser.Id }, new { newUser.Id, newUser.Email, newUser.Role });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower());

            if (user == null)
            {
                return Unauthorized(new { message = "Невірний email або пароль" });
            }

            bool isPasswordValid = PasswordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Невірний email або пароль" });
            }

            string token = _tokenGenerator.GenerateToken(user);

            return Ok(new 
            { 
                message = "Вхід успішний",
                Token = token,
                User = new { user.Id, user.Email, user.Role }
            });
        }
    }
}