using CarRental.Services;
using e_parliament.Interface;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DATA;
using TicketSystem.Helpers;
using TicketSystem.Repository;
using TicketSystem.Services;

namespace TicketSystem.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(
            options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<AuthorizeActionFilter>();
        services.AddScoped<PermissionSeeder>();
        // here to add
services.AddScoped<ISolverDataServices, SolverDataServices>();
services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ITicketServices, TicketServices>();
        services.AddScoped<IGarageServices, GarageServices>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ICommentServices, CommentServices>();
        services.AddScoped<IGovernorateServices, GovernorateServices>();


        // seed data from permission seeder service

        var serviceProvider = services.BuildServiceProvider();
        var permissionSeeder = serviceProvider.GetService<PermissionSeeder>();
        permissionSeeder.SeedPermissions();

        return services;
    }
}
