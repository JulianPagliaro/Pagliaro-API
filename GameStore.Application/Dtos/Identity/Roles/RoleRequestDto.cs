using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.Dtos.Identity.Roles
{
    public class RoleRequestDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
