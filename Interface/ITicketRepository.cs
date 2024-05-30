using TicketSystem.Entities;

namespace TicketSystem.Interface
{
    public interface ITicketRepository : IGenericRepository<Ticket , Guid>
    {
         
    }
}
