using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository
{

    public class NotificationRepository : GenericRepository<Notifications , Guid> , INotificationRepository
    {
        public NotificationRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
