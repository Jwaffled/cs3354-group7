using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextbookExchangeApp.Services.Profile;
using TextbookExchangeApp.Services.Reply;
using TextbookExchangeApp.Services.Reply.Dto;

namespace TextbookExchangeApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IReplyService _replyService;

    public ProfilesController(IProfileService profileService, IReplyService replyService)
    {
        _profileService = profileService;
        _replyService = replyService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetProfiles()
    {
        var users = await _profileService.GetAllProfileDataAsync();

        return Ok(users);
    }

    [Authorize]
    [HttpGet("{profileId}")]
    public async Task<IActionResult> GetProfile(string profileId)
    {
        var user = await _profileService.GetProfileDataAsync(profileId);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(user);
    }

    [Authorize]
    [HttpPost("{profileId}/replies")]
    public async Task<IActionResult> CreateReply(string profileId, [FromBody] CreateReplyDto dto)
    {
        try
        {
            var replyId = await _replyService.CreateReplyAsync(profileId, dto);
            var reply = await _replyService.GetReplyByIdAsync(replyId);
            return Ok(reply);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [Authorize]
    [HttpGet("{profileId}/replies")]
    public async Task<IActionResult> GetReplies(string profileId)
    {
        var data = await _replyService.GetAllRepliesAsync(profileId);

        return Ok(data);
    }

    [Authorize]
    [HttpGet("{profileId}/replies/{replyId:int}")]
    public async Task<IActionResult> GetReply(string profileId, int replyId)
    {
        var data = await _replyService.GetReplyByIdAsync(replyId);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(data);
    }

}