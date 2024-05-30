using Microsoft.AspNetCore.Mvc;
using TicketSystem.DATA.DTOs;
using TicketSystem.Services;
using TicketSystem.Utils;

namespace TicketSystem.Controllers;

public class CommentsController : BaseController
{
    private readonly ICommentServices _commentServices;


    public CommentsController(ICommentServices commentServices)
    {
        _commentServices = commentServices;
    }


    [HttpGet]
    public async Task<ActionResult<Respons<CommentDto>>> GetAll([FromQuery] CommentFilter filter)
    {
        var result = await _commentServices.GetAll(filter);
        return Ok(result, filter.PageNumber);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetById(Guid id)
    {
        return Ok(await _commentServices.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<CommentForm>> Create([FromBody] CommentForm commentForm)
    {
        return Ok(await _commentServices.Create(Id, commentForm));
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<CommentUpdate>> Update([FromBody] CommentUpdate commentUpdate, Guid id)
    {
        return Ok(await _commentServices.Update(id, commentUpdate));
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<CommentDto>> Delete(Guid id)
    {
        return Ok(await _commentServices.Delete(id));
    }

    // [HttpPost("ReplyToComment/{parentCommentId}")]
    // public async Task<ActionResult<CommentForm>> ReplyToComment(Guid parentCommentId, [FromBody] CommentForm replyForm)
    // {
    //     return Ok(await _commentServices.ReplyToComment(Id, parentCommentId, replyForm));
    // }
}