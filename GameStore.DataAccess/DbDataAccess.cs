using GameStore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DataAccess
{
    public class DbDataAccess : IdentityDbContext
    {
        public virtual DbSet<Juego> Juegos { get; set; }
        public virtual DbSet<Estudio> Estudios { get; set; }    
        public virtual DbSet<Genero> Generos { get; set; }
        public virtual DbSet<Editor> Editores { get; set; }
        public virtual DbSet<GeneroPorJuego> GenerosPorJuegos { get; set; }
        public virtual DbSet<EditorPorJuego> EditoresPorJuegos { get; set; }
        public DbDataAccess(DbContextOptions<DbDataAccess> options) : base(options)  { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine).EnableDetailedErrors();

    }
}
