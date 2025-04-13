using Microsoft.AspNetCore.Mvc;
using TextbookExchangeApp.Services.Listing;
using TextbookExchangeApp.Services.Listing.Dto;

namespace TextbookExchangeApp.Controllers

{
    [ApiController]
    [Route("/api/[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;
        public ListingController(IListingService listingService)
        {
            _listingService = listingService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateListing([FromBody] ListingDto dto)
        {
            await _listingService.CreateListingAsync(dto);
            return Ok(new { message = "Listing created successfully." });
        }

        [HttpGet("{listingId}")]
        public async Task<IActionResult> GetListingById(int listingId)
        {
            var data = await _listingService.GetListingByIdAsync(listingId);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        //get all listings
        [HttpGet]
        public async Task<IActionResult> GetAllListings()
        {
            var data = await _listingService.GetAllListingsAsync();

            return Ok(data);
        }
    }
}
