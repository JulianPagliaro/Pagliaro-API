using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.Dtos.Identity.User
{
    public class UserRegistroRequestDto
    {
        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }
    }
}
