using GameStore.Abstractions;
using GameStore.Entities.MicrosoftIdentity;
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
        #region propiedades
        [Required(ErrorMessage = "La calificación es obligatoria.")]
        [Range(1, 10, ErrorMessage = "La calificación debe estar entre 1 y 10.")]

        public int Calificacion { get; private set; }

        [StringLength(1500, ErrorMessage = "El comentario no puede exceder los 1500 caracteres.")]
        public string Comentario { get; private set; }

        public DateTime FechaCreacion { get; set; }

        public int JuegoId { get; set; }
        public Guid UserId { get; set; }
        public virtual Juego Juego { get; set; }
        public virtual User User { get; set; }
        #endregion
        #region getters y setters
        public void SetCalificacion(int calificacion)
        {
            if (calificacion < 1 || calificacion > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(calificacion), "La calificación debe estar entre 1 y 10.");
            }
            Calificacion = calificacion;
        }
        public void SetComentario(string comentario)
        {
            if (comentario != null && comentario.Length > 1500)
            {
                throw new ArgumentException("El comentario no puede exceder los 1500 caracteres.", nameof(comentario));
            }
            Comentario = comentario;
        }
        #endregion
    }
}