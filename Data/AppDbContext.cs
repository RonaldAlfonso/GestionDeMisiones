using GestionDeMisiones.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionDeMisiones.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Aquí defines tus tablas como DbSet
        public DbSet<Hechicero> Hechiceros { get; set; }

        public DbSet<Ubicacion> Ubicaciones { get; set; }

        public DbSet<Maldicion> Maldiciones { get; set; }
        public DbSet<Traslado> Traslados { get; set; }

        public DbSet<TecnicaMaldita> TecnicasMalditas { get; set; }
        public DbSet<PersonalDeApoyo> PersonalDeApoyo { get; set; } 
        public DbSet<Solicitud> Solicitud{ get; set; }
        public DbSet<Mision> Misiones { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<UsoDeRecurso> UsosDeRecurso { get; set; } 
        public DbSet<HechiceroEncargado>HechiceroEncargado{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mision>()
                .HasOne(m => m.Ubicacion)
                .WithMany(u => u.Misiones)
                .HasForeignKey(m => m.UbicacionId);

            modelBuilder.Entity<UsoDeRecurso>()
                .HasOne(ur => ur.Mision)
                .WithMany(m => m.UsosDeRecurso)
                .HasForeignKey(ur => ur.MisionId);

            modelBuilder.Entity<UsoDeRecurso>()
                .HasOne(ur => ur.Recurso)
                .WithMany(r => r.UsosDeRecurso)
                .HasForeignKey(ur => ur.RecursoId);

            modelBuilder.Entity<Traslado>()
                .HasOne(tr => tr.Mision)
                .WithMany(m => m.Traslados)
                .HasForeignKey(tr => tr.MisionId);

            modelBuilder.Entity<Traslado>()
                .HasMany(tr => tr.Hechiceros)
                .WithMany(h => h.Traslados)
                .UsingEntity(t => t.ToTable("TrasladoDeHechicero"));
        }
    }
}
