using ArtClub.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtClub.DataAccess
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 1. Cheia Primară
            builder.HasKey(u => u.Id);

            // 2. Validări de câmpuri (pentru a nu avea surprize în DB)
            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            // 3. Configurarea TPH (Strategia de Moștenire)
            builder.HasDiscriminator<string>("UserType")
                   .HasValue<Member>("Member")
                   .HasValue<Admin>("Admin");

            // 4. Index Unic (Recomandat pentru Email)
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}