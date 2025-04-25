using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TextbookExchangeApp.Enums;
using Xunit;

namespace TextbookExchangeApp.Tests.ListController
{
    public class ListingsController_Tests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ListingsController_Tests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private HttpClient CreateAuthenticatedClient(string email, string password)
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });

            client.PostAsJsonAsync("/api/auth/create-account", new
            {
                Email = email,
                Password = password,
                FirstName = "Test",
                LastName = "User"
            }).Wait();

            client.PostAsJsonAsync("/api/auth/login", new
            {
                Email = email,
                Password = password
            }).Wait();

            return client;
        }

        [Fact]
        public async Task CreateListing_Valid_ShouldReturnOk()
        {
            // Arrange
            var email = $"listing{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "Linear Algebra Textbook",
                Description = "Great condition, used for one semester.",
                Price = 25.0,
                Condition = (int)TextbookCondition.LikeNew,
                ImageUrl = "http://example.com/image.jpg"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/listings", dto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal(dto.Title, json.GetProperty("title").GetString());
            Assert.Equal(dto.Description, json.GetProperty("description").GetString());
            Assert.Equal(dto.Price, json.GetProperty("price").GetDouble());
            Assert.Equal("Like New", json.GetProperty("condition").GetString());
        }

        [Fact]
        public async Task CreateListing_MissingTitle_ShouldReturnBadRequest()
        {
            var email = $"missingtitle{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                // Title is missing
                Description = "No title field.",
                Price = 15.0,
                Condition = (int)TextbookCondition.New,
                ImageUrl = "http://example.com/img.jpg"
            };

            var response = await client.PostAsJsonAsync("/api/listings", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateListing_NegativePrice_ShouldReturnBadRequest()
        {
            var email = $"negprice{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "Faulty Price Book",
                Description = "Price is negative.",
                Price = -10.0,
                Condition = (int)TextbookCondition.New,
                ImageUrl = "http://example.com/img.jpg"
            };

            var response = await client.PostAsJsonAsync("/api/listings", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateListing_UnauthorizedUser_ShouldReturnUnauthorized()
        {
            var client = _factory.CreateClient(); // not logged in

            var dto = new
            {
                Title = "Unauthorized Attempt",
                Description = "Should fail because not logged in.",
                Price = 30.0,
                Condition = (int)TextbookCondition.LikeNew,
                ImageUrl = "http://example.com/img.jpg"
            };

            var response = await client.PostAsJsonAsync("/api/listings", dto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateListing_InvalidCondition_ShouldReturnBadRequest()
        {
            var email = $"badcondition{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "Textbook with bad condition value",
                Description = "This should fail because condition is invalid.",
                Price = 20.0,
                Condition = 999, // not in TextbookCondition enum
                ImageUrl = "http://example.com/fake.jpg"
            };

            var response = await client.PostAsJsonAsync("/api/listings", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateListing_ZeroPrice_ShouldReturnOk()
        {
            var email = $"zeroprice{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "Free Textbook",
                Description = "Giving this away.",
                Price = 0.0,
                Condition = (int)TextbookCondition.New,
                ImageUrl = "http://example.com/free.jpg"
            };

            var response = await client.PostAsJsonAsync("/api/listings", dto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateListing_MissingCondition_ShouldReturnBadRequest()
        {
            var email = $"nocon{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "No Condition Field",
                Description = "Missing condition.",
                Price = 10.0,
                // Condition is missing
                ImageUrl = "http://example.com/thing.jpg"
            };

            var response = await client.PostAsJsonAsync("/api/listings", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateListing_MissingPrice_ShouldReturnBadRequest()
        {
            var email = $"noprice{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "No Price Field",
                Description = "Missing price.",
                Condition = (int)TextbookCondition.New,
                ImageUrl = "http://example.com/thing.jpg"
            };

            var response = await client.PostAsJsonAsync("/api/listings", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateListing_NullTitle_AndNullImageUrl_ShouldReturnBadRequest()
        {
            var email = $"nulltitle{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = (string?)null,
                Description = "Testing nulls.",
                Price = 15.0,
                Condition = (int)TextbookCondition.New,
                ImageUrl = (string?)null // This should be okay
            };

        var response = await client.PostAsJsonAsync("/api/listings", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }

}
