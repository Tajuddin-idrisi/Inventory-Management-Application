using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.API.Models;

namespace InventoryManagement.Web.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientFactory _http;
        public ProductsModel(IHttpClientFactory http) => _http = http;

        public List<Product> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _http.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("jwttoken");
            if (!string.IsNullOrEmpty(token)) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Products = await client.GetFromJsonAsync<List<Product>>("api/products") ?? new List<Product>();
        }
    }
}
