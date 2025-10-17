using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class Review :IEntidad
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La calificación es obligatoria.")]
        [Range(1, 10, ErrorMessage = "La calificación debe estar entre 1 y 10.")]

        public int Calificacion { get; set; }

        [StringLength(1500, ErrorMessage = "El comentario no puede exceder los 1500 caracteres.")]
        public string Comentario { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int JuegoId { get; set; }
        public virtual Juego Juego { get; set; }

        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}