using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MvcMovie.Data
{
    public class MvcMovieContext : IdentityDbContext<ApplicationUser>
    {
        public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        // Définition des DbSet pour toutes les entités
        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Formation> Formations { get; set; } = default!;
        public DbSet<Employe> Employes { get; set; } = default!;
        public DbSet<Inscription> Inscriptions { get; set; } = default!;

        // Configuration des relations et des contraintes
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Index unique pour éviter double inscription d'un employé à la même formation
            modelBuilder.Entity<Inscription>()
                .HasIndex(i => new { i.FormationId, i.EmployeId })
                .IsUnique();

            // Relations explicites
            modelBuilder.Entity<Inscription>()
                .HasOne(i => i.Formation)
                .WithMany(f => f.Inscriptions)
                .HasForeignKey(i => i.FormationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inscription>()
                .HasOne(i => i.Employe)
                .WithMany(e => e.Inscriptions)
                .HasForeignKey(i => i.EmployeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
