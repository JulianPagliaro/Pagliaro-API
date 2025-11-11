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
    public class Estudio : IEntidad, IClassMethods
    {
        public Estudio()
        {
            EstudiosPorJuegos = new HashSet<EditorPorJuego>();
        }
        #region propiedades
        public int Id { get; set; }
        [StringLength(30)]
        public string Nombre { get; private set; }

        [DataType(DataType.Date)]
        public DateTime FechaFundacion { get; private set; }
        public int IdPais { get; set; }
        #endregion
        #region propiedades virtuales
        public virtual Pais Pais { get; set; }

        public virtual ICollection<EditorPorJuego> EstudiosPorJuegos { get; set; }
        #endregion
        #region getters y setters
        public string GetClassName()
        {
            return string.Join(": ", this.GetType().BaseType.Name, Nombre);
        }
        public void SetNombre(string nombre)
        {
            if(string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre no puede estar vacío.");
            }
            Nombre = nombre;
        }
        public void SetFechaFundacion(DateTime fechaFundacion)
        {
            if(fechaFundacion > DateTime.Now)
            {
                throw new ArgumentException("La fecha de fundación no puede ser en el futuro.");
            }
            FechaFundacion = fechaFundacion;
        }
        #endregion
    }
}
