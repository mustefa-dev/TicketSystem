using TicketSystem.Entities;

namespace TicketSystem.DATA.DTOs
{

    public class TicketFilter : BaseFilter 
    {
        public string? Title { get; set; }
        public string? Ddescription { get; set; }
        public TicketStatus? Status { get; set; }
        public TicketPriority? Priority { get; set; }
        
        public string? UserName { get; set; }
        public Guid? UserId { get; set; }
        
        public long? TicketNumber { get; set; }
        public Guid? TicketId { get; set; }
    }
}
