using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextbookExchangeApp.Enums;
using TextbookExchangeApp.Services.Listing;
using TextbookExchangeApp.Services.Listing.Dto;

namespace TextbookExchangeApp.Controllers

{
    [ApiController]
    [Route("/api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;
        public ListingsController(IListingService listingService)
        {
            _listingService = listingService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateListing([FromBody] ListingDto dto)
        {
            await _listingService.CreateListingAsync(dto);
            return Ok(new { message = "Listing created successfully." });
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("{listingId}/details")]
        public async Task<IActionResult> GetListingDetails(int listingId)
        {
            var data = await _listingService.GetListingDetailsAsync(listingId);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllListings()
        {
            var data = await _listingService.GetAllListingDetailsAsync();

            return Ok(data);
        }

        [Authorize]
        [HttpGet("conditions")]
        public async Task<IActionResult> GetAllListingConditions()
        {
            var data = Enum.GetValues(typeof(Enums.TextbookCondition))
                .Cast<Enums.TextbookCondition>()
                .Select(c => new
                {
                    Value = (int)c,
                    Label = c.GetDisplayName()
                })
                .ToList();

            return Ok(data);
        }
    }
}
