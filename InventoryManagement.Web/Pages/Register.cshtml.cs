using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManagement.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _http;
        public RegisterModel(IHttpClientFactory http) => _http = http;

        [BindProperty]
        public string Username { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _http.CreateClient("ApiClient");
            var hash = ComputeSha256Hash(Password);
            var user = new { Username, PasswordHash = hash };
            var res = await client.PostAsJsonAsync("api/auth/register", user);
            if (res.IsSuccessStatusCode) return RedirectToPage("Login");
            ModelState.AddModelError(string.Empty, "Registration failed");
            return Page();
        }

        private static string ComputeSha256Hash(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
