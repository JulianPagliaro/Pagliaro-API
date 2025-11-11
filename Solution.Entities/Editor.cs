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
        #region propiedades
        public int Id { get; set; }
        public string Nombre { get; private set; }

        [DataType(DataType.Date)]
        public DateTime FechaFundacion { get; private set; }
        public int IdPais { get;  set; }
        #endregion
        #region propiedades virtuales
        public virtual Pais Pais { get; set; }
        public virtual ICollection<Juego> Juegos { get; set; }
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
