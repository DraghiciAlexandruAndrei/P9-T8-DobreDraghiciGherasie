using Microsoft.EntityFrameworkCore;
using ArtClub.Models.Entities;

namespace ArtClub.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Tabelele (DbSets)
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ArtPiece> ArtPieces { get; set; }
        public DbSet<EventArtPiece> EventArtPieces { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Înregistrăm configurațiile din noul namespace
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());

            // Cheia compusă pentru tabela de legătură
            modelBuilder.Entity<EventArtPiece>()
                .HasKey(eap => new { eap.EventId, eap.ArtPieceId });

            //avem grija cu zecimalele
            modelBuilder.Entity<Event>()
                .Property(e => e.Budget)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Resource>()
                .Property(r => r.BasePrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}