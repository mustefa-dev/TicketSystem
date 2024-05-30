using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository
{

    public class SolverDataRepository : GenericRepository<SolverData , Guid> , ISolverDataRepository
    {
        public SolverDataRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
