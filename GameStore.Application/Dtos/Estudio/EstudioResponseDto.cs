using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Application.Dtos.Estudio
{
    public class EstudioResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaFundacion { get; set; }
        public int IdPais { get; set; }
    }
}
