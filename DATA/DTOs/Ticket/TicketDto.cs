using TicketSystem.Entities;

namespace TicketSystem.DATA.DTOs
{

    public class TicketDto:BaseDto<Guid>
    {
        public string? Title { get; set; }
        public string? Ddescription { get; set; }
        public List<string> Images { get; set; }
        public TicketStatus? Status { get; set; }
        public TicketPriority? Priority { get; set; }
        public long? TicketNumber { get; set; }

        public string UserName { get; set; }
    }
}
