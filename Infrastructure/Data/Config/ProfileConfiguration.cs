using ApplicationCore.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(cb => cb.ApplicationUserId)
                .IsRequired();

            builder.Property(cb => cb.UserName)
                .IsRequired(false)
                .HasMaxLength(128);

            builder.Property(cb => cb.Biography)
                .IsRequired(false)
                .HasColumnType("text");

            builder.Property(cb => cb.PictureUri)
                .IsRequired()
                .IsRequired(false)
                .HasColumnType("text");

            builder.Property(e => e.Gender)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(b => b.DateCreate)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            builder.Property(b => b.DateUpdate)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : v,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
        }
    }
}
