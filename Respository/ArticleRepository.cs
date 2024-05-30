using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Entities;
using TicketSystem.Interface;

namespace TicketSystem.Repository;

public class ArticleRepository : GenericRepository<Article, int>, IArticleRespository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ArticleRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
        _context = context;
    }
}