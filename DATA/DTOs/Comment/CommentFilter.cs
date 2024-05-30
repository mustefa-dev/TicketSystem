namespace TicketSystem.DATA.DTOs
{

    public class CommentFilter : BaseFilter 
    {
        public string? Content { get; set; }
        public Guid? TicketId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public Guid? CommentId { get; set; }
        
    }
}
