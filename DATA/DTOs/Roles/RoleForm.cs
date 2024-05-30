using System.ComponentModel.DataAnnotations;

namespace TicketSystem.DATA.DTOs.roles;

public class RoleForm
{
    [Required] public string Name { get; set; }
}