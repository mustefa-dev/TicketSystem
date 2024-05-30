namespace TicketSystem.DATA.DTOs;

public class CommentDto:BaseDto<Guid>
{
    public string? Content { get; set; }
    public Guid? TicketId { get; set; }
    public Guid? UserId { get; set; }
    public string? FullName { get; set; }

    // public Guid? ParentCommentId { get; set; }
    //
    // public List<RepliesDto> Replies { get; set; }
}

public class RepliesDto:BaseDto<Guid>
{
    public string? Content { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
        
    public Guid? ParentCommentId { get; set; }

    public List<RepliesDto> Replies { get; set; }

}