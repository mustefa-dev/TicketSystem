namespace TicketSystem.DATA.DTOs;

public class CommentForm
{
    public string? Content { get; set; }
    public Guid? TicketId { get; set; }
}