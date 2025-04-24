using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TextbookExchangeApp.Hubs;
using TextbookExchangeApp.Models;
using TextbookExchangeApp.Services.Channel;
using TextbookExchangeApp.Services.Channel.Dto;
using TextbookExchangeApp.Services.ChannelMessage;
using TextbookExchangeApp.Services.ChannelMessage.Dto;

namespace TextbookExchangeApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ChannelsController : ControllerBase
{
    private readonly IChannelService _channelService;
    private readonly IChannelMessageService _channelMessageService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChannelsController(IChannelService channelService, IChannelMessageService channelMessageService,
        UserManager<ApplicationUser> userManager, IHubContext<ChatHub> hubContext)
    {
        _channelService = channelService;
        _channelMessageService = channelMessageService;
        _userManager = userManager;
        _hubContext = hubContext;
    }

    [Authorize]
    [HttpPost("dm")]
    public async Task<IActionResult> GetOrCreateChannel([FromBody] CreateChannelRequestDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        var userIds = dto.UserIds.Distinct().ToList();

        if (!userIds.Contains(user.Id))
        {
            userIds.Add(user.Id);
        }

        var channel = await _channelService.GetOrCreateChannelAsync(userIds);

        return Ok(channel);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllChannelsForUser()
    {
        var user = await _userManager.GetUserAsync(User);
        var channels = await _channelService.GetChannelsForUserAsync(user.Id);

        return Ok(channels);
    }

    [Authorize]
    [HttpGet("{channelId:int}")]
    public async Task<IActionResult> GetChannelById(int channelId)
    {
        var channel = await _channelService.GetChannelByIdAsync(channelId);
        if (channel == null)
        {
            return NotFound(new { message = "Channel not found." });
        }

        return Ok(channel);
    }

    [Authorize]
    [HttpGet("{channelId:int}/messages")]
    public async Task<IActionResult> GetChannelMessages(int channelId)
    {
        var messages = await _channelMessageService.GetMessagesForChannelAsync(channelId);

        return Ok(messages);
    }

    [Authorize]
    [HttpPost("{channelId:int}/messages")]
    public async Task<IActionResult> SendMessage(int channelId, [FromBody] SendChannelMessageDto dto)
    {
        var sender = await _userManager.GetUserAsync(User);
        
        var message = await _channelMessageService.SendMessageAsync(channelId, sender.Id, dto);
        
        await _hubContext.Clients
            .Group($"channel-{channelId}")
            .SendAsync("ReceiveMessage", message);

        return Ok(message);
    }
}