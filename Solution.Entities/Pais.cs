using GameStore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameStore.Entities
{
    public class Pais : IEntidad, IClassMethods
    {
        public int Id { get; set; }
        #region propiedades
        public string NombrePais { get; private set; }
        #endregion

        #region propiedades virtuales
        public virtual ICollection<PaisPorEditor> PaisesPorEditores { get; set; }
        public virtual ICollection<PaisPorEstudio> PaisesPorEstudios { get; set; }
        #endregion
        #region getters y setters
        public void SetNombrePais(string nombrePais)
        {
            if (string.IsNullOrWhiteSpace(nombrePais))
            {
                throw new ArgumentException("El nombre del país no puede estar vacío.");
            }
            NombrePais = nombrePais;
        }
        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, NombrePais);
        }
        #endregion
    }
}
