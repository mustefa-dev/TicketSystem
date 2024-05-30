namespace TicketSystem.Entities;

public class Ticket : BaseEntity<Guid>
{
    public string? Title { get; set; }
    public string? Ddescription { get; set; }
    public TicketStatus? Status { get; set; }
    public string[]? Images { get; set; }
    public Guid AssigneeId { get; set; } 
    public TicketPriority? Priority { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public long? TicketNumber { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public AppUser? Assignee { get; set; }  
    public ICollection<Comment> Comments { get; set; } 
}

public enum TicketStatus
{
    Open,
    Pending,
    Solved,
    Closed
}

public enum TicketPriority
{
    Low,
    Medium,
    High,
}
