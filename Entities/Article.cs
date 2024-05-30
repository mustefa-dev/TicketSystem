namespace TicketSystem.Entities;

public class Article : BaseEntity<int>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}