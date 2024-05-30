using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository;

public class GovernorateRepository : GenericRepository<Governorate, Guid>, IGovernorateRepository
{
    public GovernorateRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
    }
}