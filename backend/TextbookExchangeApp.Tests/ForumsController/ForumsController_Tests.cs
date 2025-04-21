using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TextbookExchangeApp.Tests.ForumController
{
    public class ForumsController_Tests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ForumsController_Tests(WebApplicationFactory<Program> factory)
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
        public async Task CreateForumPost_Valid_ShouldReturnOk()
        {
            var email = $"forumposter{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "Test Forum Post",
                Description = "This is a test forum post body."
            };

            var response = await client.PostAsJsonAsync("/api/forums", dto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal(dto.Title, json.GetProperty("title").GetString());
            Assert.Equal(dto.Description, json.GetProperty("description").GetString());
        }

        [Fact]
        public async Task CreateForumPost_MissingTitle_ShouldReturnBadRequest()
        {
            var email = $"missingtitle{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Description = "Missing title only."
            };

            var response = await client.PostAsJsonAsync("/api/forums", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateForumPost_MissingDescription_ShouldReturnBadRequest()
        {
            var email = $"missingdesc{Guid.NewGuid()}@example.com";
            var password = "StrongPass123$";
            var client = CreateAuthenticatedClient(email, password);

            var dto = new
            {
                Title = "Missing Description Post"
            };

            var response = await client.PostAsJsonAsync("/api/forums", dto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateForumPost_Unauthorized_ShouldReturnUnauthorized()
        {
            var client = _factory.CreateClient(); // Not authenticated

            var dto = new
            {
                Title = "Unauthorized Post",
                Description = "This should fail."
            };

            var response = await client.PostAsJsonAsync("/api/forums", dto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
