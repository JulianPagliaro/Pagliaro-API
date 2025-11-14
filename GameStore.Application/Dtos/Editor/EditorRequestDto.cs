using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Application.Dtos.Editor
{
    public class EditorRequestDto
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(20)]
        public int IdPais { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaFundacion { get; set; }

    }
}
