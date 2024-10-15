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
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        public HttpClient _client;

        public CustomWebApplicationFactory()
        {
            _client = CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            CreateAdminUserAsync().GetAwaiter().GetResult();
        }

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
                    options.UseInMemoryDatabase("InMemoryTestDatabase");
                });

            });
        }

        public async Task CreateAdminUserAsync()
        {
            using (var scope = Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }

                var userAdmin = new User { UserName = "TestUser", Email = "testuser@test.com" };
                var resultAdmin = await userManager.CreateAsync(userAdmin, "Admin123.");

                if (resultAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(userAdmin, "Admin");
                }

                var user = new User { UserName = "TestBasicUser", Email = "testbasicuser@test.com" };
                var result = await userManager.CreateAsync(user, "User123.");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }

                
            }
        }

        public async Task AuthenticateAdminAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAdminAsync());
        }

        public async Task AuthenticateUserAsync()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtUserAsync());
        }

        private async Task<string> GetJwtAdminAsync()
        {
            var loginDTO = new LoginDTO
            {
                Username = "TestUser",
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

        private async Task<string> GetJwtUserAsync()
        {
            var loginDTO = new LoginDTO
            {
                Username = "TestBasicUser",
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

        public async Task ClearDatabase()
        {
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();

                dbContext.Bids.RemoveRange(dbContext.Bids); 
                dbContext.CurvePoints.RemoveRange(dbContext.CurvePoints);
                dbContext.RuleNames.RemoveRange(dbContext.RuleNames);
                dbContext.Ratings.RemoveRange(dbContext.Ratings);
                dbContext.Trades.RemoveRange(dbContext.Trades);

                await dbContext.SaveChangesAsync(); 
            }
        } 
    }
}
