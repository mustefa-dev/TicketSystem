namespace TicketSystem.DATA.DTOs.roles;

public class AllPermissionsDto
{
    public List<PermissionDto> Data { get; set; } = new();
}

public class PermissionDto
{
    public string Subject { get; set; }
    public List<ActionDetailDto> Actions { get; set; } = new();
}

public class ActionDetailDto
{
    public Guid Id { get; set; }
    public string Action { get; set; }
}