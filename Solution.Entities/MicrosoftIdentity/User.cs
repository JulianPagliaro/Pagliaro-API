using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Entities.MicrosoftIdentity
{
    public class User : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "{0} Required")]
        [StringLength(100)]
        [PersonalData]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "{0} Required")]
        [StringLength(100)]
        [PersonalData]
        public string Apellidos { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }
    }
}
