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
        await _forumPostService.CreateForumPostAsync(dto);

        return Ok(new { message = "Forum post created successfully." });
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
        await _forumReplyService.CreateForumReplyAsync(forumPostId, dto);
        return Ok(new { message = "Forum reply created successfully." });
    }
}