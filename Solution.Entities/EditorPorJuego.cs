using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class EditorPorJuego : IEntidad
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Editor))]
        public int IdAutor { get; set; }
        [ForeignKey(nameof(Juego))]
        public int IdLibro { get; set; }
        public virtual Editor Editor { get; set; }
        public virtual Juego Juego { get; set; }
    }
}
