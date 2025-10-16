using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class PlataformaPorJuego : IEntidad
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Genero))]
        public int IdPlataforma { get; set; }
        [ForeignKey(nameof(Juego))]
        public int IdJuego { get; set; }
        public virtual Plataforma Plataforma { get; set; }
        public virtual Juego Juego { get; set; }
    }
}
