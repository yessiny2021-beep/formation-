using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class MvcMovieContext : IdentityDbContext<ApplicationUser>
    {
        public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        // ✅ DbSet pour tes entités métier
        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Formation> Formations { get; set; } = default!;
        public DbSet<Employe> Employes { get; set; } = default!;
        public DbSet<Inscription> Inscriptions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Fix global MySQL : éviter nvarchar(max)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(256); // impose varchar(256) par défaut
                    }
                }
            }

            // ✅ Contraintes spécifiques pour tes entités
            modelBuilder.Entity<Inscription>()
                .HasIndex(i => new { i.FormationId, i.EmployeId })
                .IsUnique();

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
