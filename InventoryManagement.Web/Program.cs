using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InventoryManagement.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load API base URL from configuration (appsettings.json or environment)
            var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5041/";

            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();
            // Register a named HttpClient preconfigured with the API base address
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });
            
            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();

            app.MapRazorPages();

            // Redirect root path to Login page
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Login");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}
