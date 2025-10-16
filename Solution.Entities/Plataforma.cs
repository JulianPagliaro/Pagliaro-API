using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class Plataforma :IEntidad
    {
        public Plataforma()
        {
            PlataformasPorJuegos = new HashSet<PlataformaPorJuego>();

        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        public virtual ICollection<PlataformaPorJuego> PlataformasPorJuegos { get; set; }


    }
}
