using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Application.Dtos.Pais
{
    public class PaisResponseDto
    {
        public int Id { get; set; }
        public string NombrePais { get; set; }
    }
}
