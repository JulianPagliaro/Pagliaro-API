using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class Plataforma :IEntidad, IClassMethods
    {
        public Plataforma()
        {
            PlataformasPorJuegos = new HashSet<PlataformaPorJuego>();

        }

        public int Id { get; set; }
        #region propiedades
        [StringLength(100)]
        public string Nombre { get; set; }
        public DateTime FechaLanzamiento { get; private set; }
        #endregion
        #region propiedades virtuales
        public virtual ICollection<PlataformaPorJuego> PlataformasPorJuegos { get; set; }

        #endregion
        #region getters y setters
        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre no puede estar vacío.");
            }
            Nombre = nombre;
        }
        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, Nombre);
        }
        #endregion
    }
}
