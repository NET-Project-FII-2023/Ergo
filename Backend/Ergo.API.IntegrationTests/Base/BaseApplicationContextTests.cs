using Ergo.Application.Persistence;
using Ergo.Identity;
using Ergo.Identity.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Ergo.API.IntegrationTests.Base
{
    public class BaseApplicationContextTests
    {
        protected readonly WebApplicationFactory<Program> Application;
        protected readonly HttpClient Client;
        protected BaseApplicationContextTests()
        {
            Application = new WebApplicationFactory<Program>();
            Application = Application.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<ErgoContext>));
                    services.AddDbContext<ErgoContext>(options =>
                    {
                        options.UseInMemoryDatabase("ErgoDbForTesting");
                    });

                    services.Configure<JwtBearerOptions>(
                        JwtBearerDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.Configuration = new OpenIdConnectConfiguration
                            {
                                Issuer = JwtTokenProvider.Issuer,
                            };
                            options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                            options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                            options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                        }
                    );
                    services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("ErgoUserDbForTesting");
                    });
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ErgoContext>();

                        db.Database.EnsureCreated();

                        Seed.InitializeDbForTests(db);
                        var ergoUserDb = scopedServices.GetRequiredService<ApplicationDbContext>();
                        ergoUserDb.Database.EnsureCreated();
                        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
                        var userRepository = scopedServices.GetRequiredService<IUserRepository>();
                        var badgeRepository = scopedServices.GetRequiredService<IBadgeRepository>();
                        var passwordResetCode = scopedServices.GetRequiredService<IPasswordResetCode>();
                        
                        Seed.InitializeUserDbForTests(userManager, roleManager, userRepository,passwordResetCode,badgeRepository);

                    }
                });
            });
            Client = Application.CreateClient();
        }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await Application.DisposeAsync();
        }
    }
}
