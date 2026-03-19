using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace InventoryManagement.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _http;
        public LoginModel(IHttpClientFactory http) => _http = http;

        [BindProperty]
        public string Username { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _http.CreateClient("ApiClient");
            var hash = ComputeSha256Hash(Password);
            var user = new { Username, PasswordHash = hash };
            var res = await client.PostAsJsonAsync("api/auth/login", user);
            if (res.IsSuccessStatusCode)
            {
                var payload = await res.Content.ReadFromJsonAsync<JsonElement>();
                if (payload.TryGetProperty("token", out var tokenEl))
                {
                    var token = tokenEl.GetString();
                    HttpContext.Session.SetString("jwttoken", token ?? string.Empty);
                    return RedirectToPage("Products");
                }
            }

            ModelState.AddModelError(string.Empty, "Login failed");
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
