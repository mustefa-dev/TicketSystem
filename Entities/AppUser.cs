using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Entities
{
    public class AppUser : BaseEntity<Guid>
    {
        public string? Email { get; set; }
        
        public string? FullName { get; set; }
        
        public string? Password { get; set; }
        
        public Guid? RoleId { get; set; }
        public UserRole? Role { get; set; }

        public bool? IsActive { get; set; }

        public Guid? GarageId { get; set; }
        public Garage? Garage { get; set; }
        
        
    
    }
    public enum UserRole
    {
       
        Admin = 0,
        SolvingCenter = 1,
        issuer = 2,
    }
    
}