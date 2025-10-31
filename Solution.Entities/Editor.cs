using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    //Desarrollador
    public class Editor : IEntidad
    {
        public Editor()
        {
            Juegos = new HashSet<Juego>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFundacion { get; set; }
        public int IdPais { get; set; }
        public virtual Pais Pais { get; set; }
        public virtual ICollection<Juego> Juegos { get; set; }
    }
}
