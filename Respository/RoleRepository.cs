using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository;

public class RoleRepository : GenericRepository<Role, Guid>, IRoleRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public RoleRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }
}