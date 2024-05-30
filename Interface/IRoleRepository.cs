using Entities_Role = TicketSystem.Entities.Role;
using Role = TicketSystem.Entities.Role;

namespace TicketSystem.Interface;

public interface IRoleRepository : IGenericRepository<Entities_Role, Guid>
{
}