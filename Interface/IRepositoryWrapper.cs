using TicketSystem.Interface;

namespace TicketSystem.Repository;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }
    IArticleRespository Article { get; }
    IPermissionRepository Permission { get; }

    IRoleRepository Role { get; }

    // here to add
ISolverDataRepository SolverData{get;}
INotificationRepository Notification{get;}
ITicketRepository Ticket{get;}
IGarageRepository Garage{get;}
ICommentRepository Comment{get;}
    IGovernorateRepository Governorate { get; }
}
