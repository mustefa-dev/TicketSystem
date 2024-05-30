using TicketSystem.Entities;

namespace TicketSystem.DATA.DTOs
{

    public class TicketForm 
    {
        public string? Title { get; set; }
        public string Ddescription { get; set; }
        public List<IFormFile>? Images { get; set; }
        public TicketPriority? Priority { get; set; }
    }
}
