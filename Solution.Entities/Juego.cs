using GameStore.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Entities
{
    public class Juego : IEntidad
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }
        [ForeignKey(nameof(Estudio))]
        public int IdEstudio { get; set; }
        public virtual Estudio Estudio { get; set; }
        public virtual ICollection<EditorPorJuego> EditoresPorJuegos { get; set; }
        public virtual ICollection<GeneroPorJuego> GenerosPorJuegos { get; set; }
    }
}

