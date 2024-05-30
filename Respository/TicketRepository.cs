using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository
{

    public class TicketRepository : GenericRepository<Ticket , Guid> , ITicketRepository
    {
        public TicketRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
