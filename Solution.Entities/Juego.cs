using GameStore.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Entities
{
    public class Juego : IEntidad, IClassMethods
    {
        public int Id { get; set; }
        #region propiedades 
        [StringLength(100)]
        public string Titulo { get; private set; }
        public DateTime FechaPublicacion { get; private set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; private set; }
        [ForeignKey(nameof(Estudio))]
        public int IdEstudio { get; set; }
        public int IdGenero { get; set; }
        public int IdPlataforma { get; set; }



        #endregion
        #region propiedades virtuales
        public virtual Estudio Estudio { get; set; }
        public virtual Genero Genero { get; set; }
        public virtual Plataforma Plataforma { get; set; }
        public virtual ICollection<EditorPorJuego> EditoresPorJuegos { get; set; }
        public virtual ICollection<GeneroPorJuego> GenerosPorJuegos { get; set; }
        public virtual ICollection<PlataformaPorJuego> PlataformasPorJuegos { get; set; }
        #endregion

        #region getters y setters
        public void SetTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El título no puede estar vacío.");
            }
            Titulo = titulo;
        }
        public void SetFechaPublicacion(DateTime fechaPublicacion)
        {
            if (fechaPublicacion > DateTime.Now)
            {
                throw new ArgumentException("La fecha de publicación no puede ser futura.");
            }
            FechaPublicacion = fechaPublicacion;
        }
        public void SetPrecio(decimal precio)
        {
            if (precio <= 0)
            {
                throw new ArgumentException("El precio no puede ser negativo o 0.");
            }
            Precio = precio;
        }


        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, Titulo);
        }
        #endregion
    }
}

