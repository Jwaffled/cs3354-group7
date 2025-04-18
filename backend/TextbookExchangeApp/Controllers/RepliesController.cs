using TextbookExchangeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Services.Reply;
using TextbookExchangeApp.Services.Reply.Dto;

namespace TextbookExchangeApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RepliesController : ControllerBase
    {
        private readonly IReplyService _replyService;
        public RepliesController(IReplyService replyService)
        {
            _replyService = replyService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReply([FromBody] ReplyDto dto)
        {
            try
            {
                await _replyService.CreateReplyAsync(dto);
                return Ok(new { message = "Reply created successfully." });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{replyId}")]
        public async Task<IActionResult> GetReplyById(int replyId)
        {
            var data = await _replyService.GetReplyByIdAsync(replyId);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpGet("get-profile-replies")]
        public async Task<IActionResult> GetListingReplies(string profileId)
        {
            var data = await _replyService.GetAllRepliesAsync(profileId);

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReplies()
        {
            var data = await _replyService.GetAllRepliesAsync();

            return Ok(data);
        }
    }
}
