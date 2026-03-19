using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.API.Models;

namespace InventoryManagement.Web.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _http;
        public EditModel(IHttpClientFactory http) => _http = http;

        [BindProperty]
        public Product Product { get; set; } = new();

        public bool IsNew => Product.Id == 0;

        public async Task OnGetAsync(int? id)
        {
            var client = _http.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("jwttoken");
            if (!string.IsNullOrEmpty(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (id.HasValue)
            {
                Product = await client.GetFromJsonAsync<Product>($"api/products/{id.Value}") ?? new Product();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _http.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("jwttoken");
            if (!string.IsNullOrEmpty(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (IsNew)
            {
                var res = await client.PostAsJsonAsync("api/products", Product);
                if (res.IsSuccessStatusCode) return RedirectToPage("/Products");
            }
            else
            {
                var res = await client.PutAsJsonAsync($"api/products/{Product.Id}", Product);
                if (res.IsSuccessStatusCode) return RedirectToPage("/Products");
            }

            ModelState.AddModelError(string.Empty, "Save failed");
            return Page();
        }
    }
}
