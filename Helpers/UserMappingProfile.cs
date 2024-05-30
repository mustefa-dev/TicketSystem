using AutoMapper;
using OneSignalApi.Model;
using TicketSystem.DATA.DTOs;
using TicketSystem.DATA.DTOs.ArticleDto;
using TicketSystem.DATA.DTOs.roles;
using TicketSystem.DATA.DTOs.User;
using TicketSystem.Entities;

namespace TicketSystem.Helpers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        var baseUrl = "http://localhost:5051/";

        CreateMap<ArticleForm, Article>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ArticleUpdate, Article>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<AppUser, UserDto>();
        CreateMap<UpdateUserForm, AppUser>();
        CreateMap<AppUser, TokenDTO>();

        CreateMap<RegisterForm, App>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Role, RoleDto>();
        CreateMap<Article, ArticleDto>();
        CreateMap<AppUser, AppUser>();

        CreateMap<Permission, PermissionDto>();

        // here to add
        CreateMap<SolverData, SolverData>();

CreateMap<SolverData, SolverDataDto>();
CreateMap<SolverDataForm,SolverData>();
CreateMap<SolverDataUpdate,SolverData>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
CreateMap<Notifications, NotificationDto>()
    .ForMember(dest => dest.TicketId, opt => opt.MapFrom(src => src.TicketId));
CreateMap<NotificationForm,Notifications>();
CreateMap<NotificationUpdate,Notifications>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.FullName,
                opt
                    => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.UserId,
                opt
                    => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.TicketId,
                opt =>
                    opt.MapFrom(src => src.TicketId));
        
        CreateMap<Comment, RepliesDto>()
            .ForMember(dest => dest.UserName, 
                opt 
                    => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User!.GarageId))
            .ForMember(dest => dest.ParentCommentId, opt => opt.MapFrom(src => src.ParentCommentId));
        CreateMap<CommentForm,Comment>();
        CreateMap<CommentUpdate,Comment>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Ticket, TicketDto>()
            
            
            .ForMember(dist => dist.UserName,
                opt => opt.MapFrom(
                    x => x.User.FullName))

            .ForMember(dist => dist.UserName,
                opt => opt.MapFrom(
                    x => x.User.FullName
                ));
        CreateMap<TicketForm,Ticket>();
        CreateMap<TicketUpdate,Ticket>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Garage, GarageDto>();
        CreateMap<GarageForm, Garage>();
        CreateMap<GarageUpdate, Garage>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
   
}
