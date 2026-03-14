using Microsoft.AspNetCore.Mvc;
using InventoryManagement.API.Models;
using InventoryManagement.API.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.API.Services;

namespace InventoryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _db.Users.AnyAsync(u => u.Username == user.Username))
                return BadRequest("Username already exists");

            user.PasswordHash = ComputeSha256Hash(user.PasswordHash);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var hash = ComputeSha256Hash(user.PasswordHash);
            var existing = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.PasswordHash == hash);
            if (existing == null)
                return Unauthorized();

            var token = _tokenService.GenerateToken(existing.Username, existing.Role);
            return Ok(new { token });
        }

        private static string ComputeSha256Hash(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }
    }
}