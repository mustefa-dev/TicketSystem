using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository
{

    public class GarageRepository : GenericRepository<Garage , Guid> , IGarageRepository
    {
        public GarageRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
