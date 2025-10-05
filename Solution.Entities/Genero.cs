using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class Genero : IEntidad
    {
        public Genero()
        {
            GenerosPorJuegos = new HashSet<GeneroPorJuego>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public virtual ICollection<GeneroPorJuego> GenerosPorJuegos { get; set; }
    }
}
