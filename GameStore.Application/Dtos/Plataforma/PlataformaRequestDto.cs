using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Application.Dtos.Plataforma
{
    public class PlataformaRequestDto
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        [DataType(DataType.Date)]

        public DateTime FechaLanzamiento { get; set; }
    }
}
