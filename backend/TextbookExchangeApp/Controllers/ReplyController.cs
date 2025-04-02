using TextbookExchangeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;

namespace TextbookExchangeApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ReplyController : ControllerBase
    {
        private readonly ApplicationDbContext dbCtxt;
        public ReplyController(ApplicationDbContext db)
        {
            dbCtxt = db;
        }

        //add reply
        [HttpPost]
        public async Task<IActionResult> addReply([FromBody] reply r)
        {
            if (r == null)
            {
                return BadRequest("Invalid Reply");
            }

            var listingResult = await dbCtxt.listings.FindAsync(r.listingID);

            if (listingResult == null)
            {
                return NotFound("Listing was not found");
            }

            dbCtxt.allReplies.Add(r);

            await dbCtxt.SaveChangesAsync();

            return CreatedAtAction(nameof(findReply), new { rID = r.replyID }, r);
        }

        //find reply using id
        [HttpGet("{rID}")]
        public async Task<IActionResult> findReply(int rID)
        {
            var replyResult = await dbCtxt.allReplies.FindAsync(rID);
            if (replyResult == null)
            {
                return NotFound();
            }

            return Ok(replyResult);
        }

        [HttpGet]
        public async Task<IActionResult> allListingReplies(string listID)
        {
            var result = await dbCtxt.allReplies
                .Where(reply => reply.listingID == listID)
                .ToListAsync();

            return Ok(result);
        }
    }
}
