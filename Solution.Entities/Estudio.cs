using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    //Distribuidor
    public class Estudio : IEntidad
    {
        public Estudio()
        {
            EstudiosPorJuegos = new HashSet<EditorPorJuego>();
        }
        public int Id { get; set; }
        [StringLength(30)]
        public string Nombre { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFundacion { get; set; }
        public virtual ICollection<EditorPorJuego> EstudiosPorJuegos { get; set; }
    }
}
