using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository
{

    public class CommentRepository : GenericRepository<Comment , Guid> , ICommentRepository
    {
        public CommentRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
