using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class PaisPorEstudio
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Pais))]

        public int IdPais { get; set; }

        public virtual Pais Pais { get; set; }
        [ForeignKey(nameof(Estudio))]

        public int IdEstudio { get; set; }
        public virtual Estudio Estudio { get; set; }
    }
}
