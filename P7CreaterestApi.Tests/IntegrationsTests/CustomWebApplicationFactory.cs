using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P7CreateRestApi.Domain.DTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Dot.Net.WebApi.Domain;
using System.Text.Json;

namespace P7CreateRestApi.Tests.IntegrationsTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<LocalDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<LocalDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"InMemoryTestDatabase");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<LocalDbContext>();
                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                    var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                    db.Database.EnsureCreated();

                    CreateTestUsers(userManager, roleManager).GetAwaiter().GetResult();
                }
            });

        }

        private async Task CreateTestUsers(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
           
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

           
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new User { UserName = "admin", Email = "admin@test.com" };
                await userManager.CreateAsync(adminUser, "Admin123.");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            
            var basicUser = await userManager.FindByNameAsync("user");
            if (basicUser == null)
            {
                basicUser = new User { UserName = "user", Email = "user@test.com" };
                await userManager.CreateAsync(basicUser, "User123.");
                await userManager.AddToRoleAsync(basicUser, "User");
            }
        }

        public async Task AuthenticateAdminAsync(HttpClient _client)
        {
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAdminAsync(_client));
        }

        public async Task AuthenticateUserAsync(HttpClient _client)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtUserAsync(_client));
        }

        private async Task<string> GetJwtAdminAsync(HttpClient _client)
        {
            var loginDTO = new LoginDTO
            {
                Username = "admin",
                Password = "Admin123."
            };

            var response = await _client.PostAsJsonAsync("/login", loginDTO);
            response.EnsureSuccessStatusCode();

            var registrationResponse = await response.Content.ReadAsStringAsync();
            using (var document = JsonDocument.Parse(registrationResponse))
            {
                var json = document.RootElement.GetProperty("token").GetString();
                return json;
            }
        }

        private async Task<string> GetJwtUserAsync(HttpClient _client)
        {
            var loginDTO = new LoginDTO
            {
                Username = "user",
                Password = "User123."
            };

            var response = await _client.PostAsJsonAsync("/login", loginDTO);
            response.EnsureSuccessStatusCode();

            var registrationResponse = await response.Content.ReadAsStringAsync();
            using (var document = JsonDocument.Parse(registrationResponse))
            {
                var json = document.RootElement.GetProperty("token").GetString();
                return json;
            }
        }
    }
}
