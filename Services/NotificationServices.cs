using AutoMapper;
using TicketSystem.DATA.DTOs;
using TicketSystem.Entities;
using TicketSystem.Repository;

namespace CarRental.Services;

public interface INotificationService{
    Task<(List<NotificationDto> notification, int? totalCount, string? error)> GetAll(NotificationFilter stationFilter,Guid userId);
    Task<(NotificationDto? notification, string? error)> GetById(Guid id);
    Task<(NotificationDto? notification, string?)> Delete(Guid id);
    Task<(NotificationDto? notification, string?)> add(NotificationForm stationForm);
    Task<(List<NotificationDto> notification, int? totalCount, string? error)> GetUnreadNotifications(BaseFilter stationFilter);
    

    
}

public class NotificationService : INotificationService{
    
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public NotificationService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(List<NotificationDto> notification, int? totalCount, string? error)> GetAll(NotificationFilter stationFilter
    ,Guid userId
    ) {
        var (notification, totalCount) = await _repositoryWrapper.Notification.GetAll(
            (x => x.UserId == userId)
            ,
            
       stationFilter.PageNumber, stationFilter.PageSize);
        
        
        var NotificationDto = _mapper.Map<List<NotificationDto>>(notification);
        return (NotificationDto, totalCount, null);
        
            
    }

    public async Task<(NotificationDto? notification, string? error)> GetById(Guid id) {
        var notification = await _repositoryWrapper.Notification.Get(d => d.Id == id );
        if (notification == null) return (null, "الاشعار غير متوفره");
        var NotificationDto = _mapper.Map<NotificationDto>(notification);
        
            notification.IsRead = true;
            await _repositoryWrapper.Notification.Update(notification);
        
        return (NotificationDto, null);
    }
    
    
    public async Task<(NotificationDto? notification, string?)> Delete(Guid id) {
        var notification = await _repositoryWrapper.Notification.GetById(id);
        if (notification == null) {
            return (null, "الاشعار غير متوفره");
        }
    
        notification.Deleted = true;
        var response = await _repositoryWrapper.Notification.Update(notification);
        var NotificationDto = _mapper.Map<NotificationDto>(response);
        return response == null ? (null, "الاشعار غير متوفره") : (NotificationDto, null);
        
    }
        
    public async Task<(NotificationDto? notification, string?)> add(NotificationForm notificationForm) {
        var notification = _mapper.Map<Notifications>(notificationForm);
        var result = await _repositoryWrapper.Notification.Add(notification);
        var NotificationDto = _mapper.Map<NotificationDto>(result);
        return result == null ? (null, "لا يمكن اضافة اشعار") : (NotificationDto, null);
    }
    
    //GET UNREAD NOTIFICATIONS
    
    public async Task<(List<NotificationDto> notification, int? totalCount, string? error)> GetUnreadNotifications(BaseFilter stationFilter) {
        var (notification, totalCount) = await _repositoryWrapper.Notification.GetAll(
            (x => x.IsRead == false)
            , stationFilter.PageNumber, stationFilter.PageSize);
        var NotificationDto = _mapper.Map<List<NotificationDto>>(notification);
        return (NotificationDto, totalCount, null);
    }
}