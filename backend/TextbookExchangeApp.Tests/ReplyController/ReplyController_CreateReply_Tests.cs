using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using TextbookExchangeApp;

namespace TextbookExchangeApp.Tests.Controllers
{
    public class RepliesControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public RepliesControllerTests()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        // PASS: Create reply successfully :)
        [Fact]
        public async Task CreateReply_WithValidData_ShouldSucceed()
        {
            // Create a valid reply
            var response = await _client.PostAsync("/api/replies", JsonContent.Create(new
            {
                Message = "This is a test reply",
                ListingId = 1,
                AuthorID = "36b5d449-ef8d-49ea-aa48-058efd6ef079"
            }));

            // Assert 200/OK response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify content is correct
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);
            var message = json.GetProperty("message").GetString();
            Assert.Equal("Reply created successfully.", message);
        }

        // FAIL: Create reply with a missing Message :)
        [Fact]
        public async Task CreateReply_MissingContent_ShouldReturnBadRequest()
        {
            // Create an invalid reply; missing Message
            var response = await _client.PostAsync("/api/replies", JsonContent.Create(new
            {
                ListingId = 1,
                AuthorID = "36b5d449-ef8d-49ea-aa48-058efd6ef079"
            }));

            // Assert 400/BadRequest response
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // PASS: Retrieve reply by ID :)
        [Fact]
        public async Task GetReplyById_ValidId_ShouldReturnReply()
        {
            // Get listing using ID 
            var response = await _client.GetAsync("/api/replies/1");

            // Assert 200/OK response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // FAIL: Retrieve reply with invalid ID :)
        [Fact]
        public async Task GetReplyById_InvalidId_ShouldReturnNotFound()
        {
            // Get listing using invalid ID
            var response = await _client.GetAsync("/api/replies/99999");

            // Assert 404/NotFound response
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // PASS: Get all replies :)
        [Fact]
        public async Task GetAllReplies_ShouldReturnReplies()
        {
            // Get all replies
            var response = await _client.GetAsync("/api/replies");

            // Assert 200/OK response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
