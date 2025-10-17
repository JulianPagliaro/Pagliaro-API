using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Application.Dtos.Juego
{
    public class JuegoRequestDto
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Titulo { get; set; }
        [DataType(DataType.Date)]

        public DateTime FechaPublicacion { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }
        public int IdEstudio { get; set; }
        public int IdGenero { get; set; }

    }
}
