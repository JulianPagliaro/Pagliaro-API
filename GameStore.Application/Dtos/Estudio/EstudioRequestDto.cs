using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Application.Dtos.Estudio
{
    public class EstudioRequestDto
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Nombre { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFundacion { get; set; }
        public int IdPais { get; set; }

    }
}
