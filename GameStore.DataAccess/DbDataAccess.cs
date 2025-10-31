using Biblioteca.Entities.MicrosoftIdentity;
using GameStore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace GameStore.DataAccess
{
    public class DbDataAccess : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public virtual DbSet<Juego> Juegos { get; set; }
        public virtual DbSet<Estudio> Estudios { get; set; }
        public virtual DbSet<Genero> Generos { get; set; }
        public virtual DbSet<Editor> Editores { get; set; }
        public virtual DbSet<Plataforma> Plataformas { get; set; }
        public virtual DbSet<PlataformaPorJuego> PlataformasPorJuegos { get; set; }
        public virtual DbSet<GeneroPorJuego> GenerosPorJuegos { get; set; }
        public virtual DbSet<EditorPorJuego> EditoresPorJuegos { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Pais> Paises { get; set; }
        public virtual DbSet<PaisPorEditor> PaisesPorEditores { get; set; }
        public virtual DbSet<PaisPorEstudio> PaisesPorEstudios { get; set; }

        public DbDataAccess(DbContextOptions<DbDataAccess> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine).EnableDetailedErrors();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
