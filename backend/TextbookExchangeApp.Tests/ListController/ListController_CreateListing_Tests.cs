using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using TextbookExchangeApp;

namespace TextbookExchangeApp.Tests.Controllers
{
    public class ListingControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ListingControllerTests()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        // PASS: Create listing successfully :)
        [Fact]
        public async Task CreateListing_WithValidData_ShouldSucceed()
        {
            // Create valid listing
            var response = await _client.PostAsync("/api/listing/create", JsonContent.Create(new
            {
                Title = "Test Book",
                Description = "A book for testing",
                Price = 20.0,
                Condition = 1,
                AuthorID = "36b5d449-ef8d-49ea-aa48-058efd6ef079"
            }));

            // Assert 200/OK response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify content is correct
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);
            var message = json.GetProperty("message").GetString();
            Assert.Equal("Listing created successfully.", message);
        }

        // FAIL: Create listing with missing title :)
        [Fact]
        public async Task CreateListing_MissingTitle_ShouldReturnBadRequest()
        {
            // Create an invalid listing; missing title
            var response = await _client.PostAsync("/api/listing/create", JsonContent.Create(new
            {
                Description = "A book with no title",
                Condition = 1,
                Price = 15.0M,
                AuthorID = "36b5d449-ef8d-49ea-aa48-058efd6ef079"
            }));

            // Assert 400/BadRequest response
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // PASS: Retrieve listing by ID :)
        [Fact]
        public async Task GetListingById_ValidId_ShouldReturnListing()
        {

            // Get listing using ID
            var response = await _client.GetAsync("/api/listing/1");

            // Assert 200/OK response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // FAIL: Retrieve listing with invalid ID :)
        [Fact]
        public async Task GetListingById_InvalidId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync("/api/listing/99999");

            // Assert 404/NotFound response
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // PASS: Get all listings :)
        [Fact]
        public async Task GetAllListings_ShouldReturnListings()
        {
            // Get all listings
            var response = await _client.GetAsync("/api/listing");

            // Assert 200/OK response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}