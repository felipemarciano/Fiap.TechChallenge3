using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Dependencies
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            var dmpConnectionString = configuration.GetConnectionString("BlogConnection") ?? throw new InvalidOperationException("Connection string 'BlogConnection' not found.");
            services.AddDbContext<BlogContext>(c =>
                c.UseNpgsql(dmpConnectionString));

            // Add Identity
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseNpgsql(connectionString));


        }
    }
}
