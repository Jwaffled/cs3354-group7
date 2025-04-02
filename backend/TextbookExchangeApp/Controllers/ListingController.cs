using TextbookExchangeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TextbookExchangeApp.EntityFramework;

namespace TextbookExchangeApp.Controllers

{
    [ApiController]
    [Route("/api/[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly ApplicationDbContext cTxt;
        public ListingController(ApplicationDbContext db)
        {
            cTxt = db;
        }

        //create listings
        [HttpPost("Create-Listing")]
        public async Task<IActionResult> newListing([FromBody] TextbookListing newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Invalid Listing");
            }

            cTxt.listings.Add(newBook);
            await cTxt.SaveChangesAsync();

            return CreatedAtAction(nameof(findListing), new { id = newBook.listingID }, newBook);
        }

        //find listing using the listing id
        [HttpGet("{listingid}")]
        public async Task<IActionResult> findListing(string listingid)
        {
            var result = await cTxt.listings
                .Include(l => l.replyList)
                .FirstOrDefaultAsync(l => l.listingID == listingid);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        //get all listings
        [HttpGet]
        public async Task<IActionResult> returnAll()
        {
            var allListings = await cTxt.listings.Include(l => l.replyList).ToListAsync();
            return Ok(allListings);
        }
    }
}
