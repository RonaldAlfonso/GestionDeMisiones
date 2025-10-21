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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mision>()
                .HasOne(m => m.Ubicacion)
                .WithMany(u => u.Misiones)
                .HasForeignKey(m => m.UbicacionId);
        }
    }
}
