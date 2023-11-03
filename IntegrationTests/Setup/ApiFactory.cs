using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Setup
{
    public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IContainer _postgresContainer = new ContainerBuilder()
            .WithImage("postgres:latest")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithPortBinding(5433, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextOptionsBlogContext = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BlogContext>));

                var dbContextOptionsAppIdentityDbContext = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppIdentityDbContext>));

                services.Remove(dbContextOptionsBlogContext!);
                services.Remove(dbContextOptionsAppIdentityDbContext!);

                services.AddDbContext<BlogContext>(c =>
                            c.UseNpgsql("Host=localhost; Port=5433; Database=dbTechChallenge2; Username=postgres; Password=postgres"));

                services.AddDbContext<AppIdentityDbContext>(c =>
                            c.UseNpgsql("Host=localhost; Port=5433; Database=dbTechChallenge2; Username=postgres; Password=postgres"));

            });
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _postgresContainer.StopAsync();
        }
    }
}
