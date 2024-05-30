using TicketSystem.Entities;

namespace TicketSystem.DATA.DTOs
{

    public class TicketUpdate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus? Status { get; set; }
        public TicketPriority? Priority { get; set; }
        public DateTime? UpdatedAt { get; set; } =DateTime.UtcNow;
        public bool EscalatePriority { get; set; }
    }
}

