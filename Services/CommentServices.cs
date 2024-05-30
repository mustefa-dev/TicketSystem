using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DATA.DTOs;
using TicketSystem.Entities;
using TicketSystem.Helpers.OneSignal;
using TicketSystem.Repository;

namespace TicketSystem.Services;

public interface ICommentServices
{
    Task<(CommentDto? comment, string? error)> Create(Guid UserId, CommentForm commentForm);
    Task<(List<CommentDto> comments, int? totalCount, string? error)> GetAll(CommentFilter filter);
    Task<(CommentDto?commentDto,string? error)>GetById(Guid id);
    Task<(Comment? comment, string? error)> Update(Guid id, CommentUpdate commentUpdate);
    Task<(Comment? comment, string? error)> Delete(Guid id);
    Task<(RepliesDto? comment, string? error)> ReplyToComment(Guid userId, Guid parentCommentId, CommentForm replyForm);

}

public class CommentServices : ICommentServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CommentServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper
    )
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(CommentDto? comment, string? error)> Create(Guid UserId, CommentForm commentForm)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == UserId);
        if (user == null) return (null, "User not found");
        var comment = _mapper.Map<Comment>(commentForm);
        comment.UserId = UserId;
        var response = await _repositoryWrapper.Comment.Add(comment);
        var responseDto = _mapper.Map<CommentDto>(response);
        if (response == null) return (null, "Comment Couldn't add");

        var ticket = await _repositoryWrapper.Ticket.Get(x => x.Id == comment.TicketId);
        var notification = new Notifications
        {
            NotifyId = Guid.NewGuid(),
            Title = "تمت إضافة تعليق جديد",
            Description = $"المستخدم {user.FullName} أضاف تعليقًا جديدًا على تذكرتك.",
            UserId = user.Role == UserRole.issuer ? ticket.AssigneeId : ticket.UserId,
            Date = DateTime.Now,
            TicketId = response.TicketId
        };
        await _repositoryWrapper.Notification.Add(notification);
        if (comment.User != null)
        {
            OneSignal.SendNoitications(notification, comment.User.ToString());
        }

        return (responseDto, null);
    }

    public async Task<(List<CommentDto> comments, int? totalCount, string? error)> GetAll(CommentFilter filter)
    {

        var (comment,totalCount) = await _repositoryWrapper.Comment.GetAll(
            x =>
                (filter.Content == null || x.Content.Contains(filter.Content)) &&
                (filter.ParentCommentId == null || x.ParentCommentId== filter.ParentCommentId) &&
                (filter.CommentId == null || x.Id== filter.CommentId) &&
                (filter.TicketId == null || x.TicketId == filter.TicketId) &&
                !x.Deleted,
            i => i.Include(x => x.User)
                .Include(x => x.Ticket)
            .Include(x => x.ParentComment)
                .Include(x => x.Replies),
                
            filter.PageNumber, filter.PageSize);
        
        var commentDto = _mapper.Map<List<CommentDto>>(comment);
        return (commentDto, totalCount, null);
    }

    public async Task<(Comment? comment, string? error)> Update(Guid id, CommentUpdate commentUpdate)
    {
        var comment = await _repositoryWrapper.Comment.GetById(id);
        if (comment == null) return (null, "Comment not found");
        comment.Content = commentUpdate.Content;
        var response = await _repositoryWrapper.Comment.Update(comment);
        return response == null ? (null, "Comment couldn't be updated") : (response, null);
    }
    
    public async Task<(CommentDto? commentDto, string? error)> GetById(Guid id)
    {
        var comment = await _repositoryWrapper.Comment.Get(x => x.Id == id && !x.Deleted);
        if (comment == null) return (null, "Comment not found");
        var responseDto = _mapper.Map<CommentDto>(comment);
        return (responseDto, null);
    }

    public async Task<(Comment? comment, string? error)> Delete(Guid id)
    {
        var comment = await _repositoryWrapper.Comment.Get(x => x.Id == id && !x.Deleted);
        if (comment == null) return (null, "Comment not found");
        comment.Deleted = true;
        var response = await _repositoryWrapper.Comment.Update(comment);
        return response == null ? (null, "Comment couldn't be deleted") : (response, null);
    }
    
    public async Task<(RepliesDto? comment, string? error)> ReplyToComment(Guid userId, Guid parentCommentId, CommentForm replyForm)
    {
        var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
        if (user == null) return (null, "User not found");
        var parentComment = await _repositoryWrapper.Comment.Get(x => x.Id == parentCommentId && !x.Deleted);
        if (parentComment == null) return (null, "Comment not found");
        var reply = _mapper.Map<Comment>(replyForm);
        reply.UserId = userId;
        reply.ParentCommentId = parentCommentId;
        var response = await _repositoryWrapper.Comment.Add(reply);
        var responseDto = new RepliesDto()
        {
            Content = response.Content,
            ParentCommentId = response.ParentCommentId,
            UserId = response.UserId,
            UserName = response.User?.FullName,
            Replies = new List<RepliesDto>()
        };
        return response == null ? (null, "Comment couldn't be added") : (responseDto, null);
    }
}