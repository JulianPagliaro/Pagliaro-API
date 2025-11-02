using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.Dtos.Identity.User
{
    public class UserRegistroResponseDto
    {
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
