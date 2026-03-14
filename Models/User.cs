using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        // Stored as a SHA256 hash (hex)
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? Role { get; set; }
    }
}