using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ArtClub.Models.Entities;

namespace ArtClub.DataAccess
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // 1. Setăm Cheia Primară
            builder.HasKey(e => e.Id);

            // 2. Configurare Proprietăți (Validări la nivel de bază de date)
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .HasMaxLength(1000);

            // 3. Relația cu Organizatorul (Member)
            // Un membru poate organiza mai multe evenimente
            builder.HasOne(e => e.Organizer)
                .WithMany(m => m.OrganizedEvents)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);
            // Restrict: Dacă ștergem un membru, nu vrem să dispară 
            // automat evenimentele organizate de el din istoric.

            // 4. Relația cu Resursa (Sala/Echipamentul)
            builder.HasOne(e => e.Resource)
                .WithMany(r => r.Events)
                .HasForeignKey(e => e.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);

            // 5. Relația 1:1 cu Reservation (Inima sistemului de buffer)
            // Fiecare Eveniment are exact o Rezervare în calendar
            builder.HasOne(e => e.Reservation)
                .WithOne(r => r.Event)
                .HasForeignKey<Reservation>(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            // Cascade: Dacă ștergem evenimentul, ștergem și 
            // intervalul ocupat în calendar (rezervarea).
        }
    }
}