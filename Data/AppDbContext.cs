using Microsoft.EntityFrameworkCore;
using NutriPlan.Models;

namespace NutriPlan.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Definición de las 6 tablas principales
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Objetivo> Objetivos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<PlanSemanal> PlanesSemanales { get; set; }
        public DbSet<PlanComida> PlanesComida { get; set; }
        public DbSet<ListaCompra> ListasCompra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación: Usuario tiene un Objetivo
            modelBuilder.Entity<Usuario>()
                .HasOne<Objetivo>()
                .WithMany()
                .HasForeignKey(u => u.IdObjetivo);

            // Relación: PlanSemanal pertenece a un Usuario
            modelBuilder.Entity<PlanSemanal>()
                .HasOne(ps => ps.Usuario)
                .WithMany()
                .HasForeignKey(ps => ps.IdUsuario);
        }
    }
}