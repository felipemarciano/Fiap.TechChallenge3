using ApplicationCore.Aggregates;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Comment> Comment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlogPost>()
                .Property(p => p.Timestamp)
                .IsRowVersion();

            builder.Entity<Comment>()
                .Property(b => b.DateCreate)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties().Where(p => p.IsPrimaryKey()))
                {
                    property.ValueGenerated = ValueGenerated.Never;
                }
            }

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
