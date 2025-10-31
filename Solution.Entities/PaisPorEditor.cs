using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class PaisPorEditor
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Pais))]

        public int IdPais { get; set; }
        public virtual Pais Pais { get; set; }
        [ForeignKey(nameof(Editor))]

        public int IdEditor { get; set; }
        public virtual Editor Editor { get; set; }

    }
}
