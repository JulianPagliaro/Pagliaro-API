using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Entities
{
    public class Genero : IEntidad, IClassMethods
    {
        public Genero()
        {
            GenerosPorJuegos = new HashSet<GeneroPorJuego>();
        }
        #region propiedades
        public int Id { get; set; }
        public string Nombre { get; private set; }
        #endregion
        #region propiedades virtuales
        public virtual ICollection<GeneroPorJuego> GenerosPorJuegos { get; set; }

        #endregion
        #region getters y setters
        public void SetNombre(string nombre)
        {
            if(string.IsNullOrWhiteSpace(nombre))
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
