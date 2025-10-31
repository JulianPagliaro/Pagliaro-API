using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class Pais : IEntidad
    {
        public int Id { get; set; }
        public string NombrePais { get; set; }
        public virtual ICollection<PaisPorEditor> PaisesPorEditores { get; set; }
        public virtual ICollection<PaisPorEstudio> PaisesPorEstudios { get; set; }
    }
}
