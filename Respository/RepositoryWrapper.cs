using AutoMapper;
using TicketSystem.DATA;
using TicketSystem.Interface;

namespace TicketSystem.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    private IArticleRespository _articles;

    // here to add
private ISolverDataRepository _SolverData;

public ISolverDataRepository SolverData {
    get {
        if(_SolverData == null) {
            _SolverData = new SolverDataRepository(_context, _mapper);
        }
        return _SolverData;
    }
}
private INotificationRepository _Notification;

public INotificationRepository Notification {
    get {
        if(_Notification == null) {
            _Notification = new NotificationRepository(_context, _mapper);
        }
        return _Notification;
    }
}
private ITicketRepository _Ticket;

public ITicketRepository Ticket {
    get {
        if(_Ticket == null) {
            _Ticket = new TicketRepository(_context, _mapper);
        }
        return _Ticket;
    }
}
private IGarageRepository _Garage;

public IGarageRepository Garage {
    get {
        if(_Garage == null) {
            _Garage = new GarageRepository(_context, _mapper);
        }
        return _Garage;
    }
}
private ICommentRepository _Comment;

public ICommentRepository Comment {
    get {
        if(_Comment == null) {
            _Comment = new CommentRepository(_context, _mapper);
        }
        return _Comment;
    }
}

    private IGovernorateRepository _Governorate;
    private IPermissionRepository _permission;
    private IRoleRepository _role;

    private IUserRepository _user;


    public RepositoryWrapper(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IRoleRepository Role
    {
        get
        {
            if (_role == null) _role = new RoleRepository(_context, _mapper);
            return _role;
        }
    }

    public IPermissionRepository Permission
    {
        get
        {
            if (_permission == null) _permission = new PermissionRepository(_context, _mapper);
            return _permission;
        }
    }


    public IArticleRespository Article
    {
        get
        {
            if (_articles == null) _articles = new ArticleRepository(_context, _mapper);
            return _articles;
        }
    }


    public IUserRepository User
    {
        get
        {
            if (_user == null) _user = new UserRepository(_context, _mapper);
            return _user;
        }
    }

    public IGovernorateRepository Governorate
    {
        get
        {
            if (_Governorate == null) _Governorate = new GovernorateRepository(_context, _mapper);
            return _Governorate;
        }
    }
}
