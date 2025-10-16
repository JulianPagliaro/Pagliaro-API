using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Application.Dtos.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }


        public int Calificacion { get; set; }

        public string Comentario { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int JuegoId { get; set; }

        public int UsuarioId { get; set; }
    }
}
