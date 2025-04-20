using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextbookExchangeApp.Services.ForumPost;
using TextbookExchangeApp.Services.ForumPost.Dto;
using TextbookExchangeApp.Services.ForumReply;
using TextbookExchangeApp.Services.ForumReply.Dto;

namespace TextbookExchangeApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ForumsController : ControllerBase
{
    private readonly IForumPostService _forumPostService;
    private readonly IForumReplyService _forumReplyService;

    public ForumsController(IForumPostService forumPostService, IForumReplyService forumReplyService)
    {
        _forumPostService = forumPostService;
        _forumReplyService = forumReplyService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllForumPosts()
    {
        var data = await _forumPostService.GetAllForumPostsAsync();

        return Ok(data);
    }

    [Authorize]
    [HttpGet("{forumPostId:int}")]
    public async Task<IActionResult> GetForumPostById(int forumPostId)
    {
        var data = await _forumPostService.GetForumPostByIdAsync(forumPostId);

        if (data == null)
        {
            return NotFound("Forum post not found.");
        }

        return Ok(data);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateForumPost([FromBody] CreateForumPostDto dto)
    {
        var forumPostId = await _forumPostService.CreateForumPostAsync(dto);
        var forumPost = await _forumPostService.GetForumPostByIdAsync(forumPostId);
        return Ok(forumPost);
    }

    [Authorize]
    [HttpGet("{forumPostId:int}/replies")]
    public async Task<IActionResult> GetForumPostReplies(int forumPostId)
    {
        var data = await _forumReplyService.GetForumPostRepliesAsync(forumPostId);

        return Ok(data);
    }

    [Authorize]
    [HttpPost("{forumPostId:int}/replies")]
    public async Task<IActionResult> CreateForumPostReply(int forumPostId, [FromBody] CreateForumReplyDto dto)
    {
        var replyId = await _forumReplyService.CreateForumReplyAsync(forumPostId, dto);
        var reply = await _forumReplyService.GetForumReplyByIdAsync(replyId);
        return Ok(reply);
    }
}