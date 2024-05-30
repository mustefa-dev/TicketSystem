using TicketSystem.DATA.DTOs;

namespace TicketSystem.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public string? Content { get; set; }
        
        public Guid? UserId { get; set; }
        
        public AppUser? User { get; set; }
        public Guid? TicketId { get; set; }
        
        public Ticket? Ticket { get; set; }
        
        public Guid? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public List<Comment> Replies { get; set; } 

    }
}