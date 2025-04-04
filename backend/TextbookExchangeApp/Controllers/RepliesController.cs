﻿using TextbookExchangeApp.Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateReply([FromBody] ReplyDto dto)
        {
            await _replyService.CreateReplyAsync(dto);
            return Ok(new { message = "Reply created successfully." });
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

        [HttpGet("get-listing-replies")]
        public async Task<IActionResult> GetListingReplies(int listingId)
        {
            var data = await _replyService.GetAllRepliesAsync(listingId);

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
