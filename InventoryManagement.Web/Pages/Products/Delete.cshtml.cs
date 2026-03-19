using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.API.Models;

namespace InventoryManagement.Web.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _http;
        public DeleteModel(IHttpClientFactory http) => _http = http;

        [BindProperty]
        public Product? Product { get; set; }

        public async Task OnGetAsync(int id)
        {
            var client = _http.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("jwttoken");
            if (!string.IsNullOrEmpty(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Product = await client.GetFromJsonAsync<Product>($"api/products/{id}");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Product == null) return RedirectToPage("/Products");
            var client = _http.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("jwttoken");
            if (!string.IsNullOrEmpty(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await client.DeleteAsync($"api/products/{Product.Id}");
            return RedirectToPage("/Products");
        }
    }
}
