using TicketSystem.Entities;

namespace TicketSystem.Interface;

public interface IPermissionRepository : IGenericRepository<Permission, Guid>
{
}