using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TextbookExchangeApp.Tests.ChannelsController
{
    public class ChannelsController_Tests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ChannelsController_Tests(WebApplicationFactory<Program> factory)
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

        private HttpClient LoginAuthenticatedClient(string email, string password)
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });

            var loginResponse = client.PostAsJsonAsync("/api/auth/login", new
            {
                Email = email,
                Password = password
            }).Result;

            if (!loginResponse.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Login failed. Ensure the account exists before using this method.");
            }

            return client;
        }

        [Fact]
        public async Task GetOrCreateChannel_WithValidChannelId_ShouldReturnOk()
        {
            // New Login
            var client = CreateAuthenticatedClient($"channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var userId = meJson.GetProperty("id").GetString();

            var dto = new { 
                UserIds = new[] { userId } 
            };

            // Assert
            var response = await client.PostAsJsonAsync("/api/channels/dm", dto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetOrCreateChannel_WithInvalidChannelId_ShouldReturnBadRequest()
        {
            // Login
            var client = LoginAuthenticatedClient($"channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var invalidUserId = 12345;

            var dto = new
            {
                UserIds = new[] { invalidUserId }
            };

            // Assert
            var response = await client.PostAsJsonAsync("/api/channels/dm", dto);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetChannelById_WithValidChannelId_ShouldReturnOk()
        {
            // Login
            var client = LoginAuthenticatedClient($"channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var userId = meJson.GetProperty("id").GetString();

            var dto = new { UserIds = new[] { userId } };
            var createResponse = await client.PostAsJsonAsync("/api/channels/dm", dto);
            var channelJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var channelId = channelJson.GetProperty("id").GetInt32();

            // Assert
            var response = await client.GetAsync($"/api/channels/{channelId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetChannelById_WithInvalidChannelId_ShouldReturnNotFound()
        {
            // Login
            var client = LoginAuthenticatedClient($"channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var userId = meJson.GetProperty("id").GetString();

            var dto = new { UserIds = new[] { userId } };
            var createResponse = await client.PostAsJsonAsync("/api/channels/dm", dto);
            var invalidChannelId = 12345;

            // Assert
            var response = await client.GetAsync($"/api/channels/{invalidChannelId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllChannelsForUser_WithValidId_ShouldReturnOk()
        {
            // Login
            var client = LoginAuthenticatedClient($"channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);
            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var userId = meJson.GetProperty("id").GetString();

            var response = await client.GetAsync("/api/channels");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(responseContent), "Expected non-empty response for channels.");
        }

        [Fact]
        public async Task GetChannelMessagesById_WithValidChannelId_ShouldReturnOk()
        {
            // Login
            var client = LoginAuthenticatedClient($"channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var userId = meJson.GetProperty("id").GetString();
            var dto = new { UserIds = new[] { userId } };
            var createResponse = await client.PostAsJsonAsync("/api/channels/dm", dto);
            var channelJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var channelId = channelJson.GetProperty("id").GetInt32();

            // Assert
            var response = await client.GetAsync($"/api/channels/{channelId}/messages");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendMessage_WithValidChannelId_ShouldReturnOk()
        {
            // Login
            var client = LoginAuthenticatedClient("channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            Assert.True(meResponse.IsSuccessStatusCode, "Failed to get current user info.");
            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var userId = meJson.GetProperty("id").GetString();

            var createDto = new { UserIds = new[] { userId } };
            var createResponse = await client.PostAsJsonAsync("/api/channels/dm", createDto);
            Assert.True(createResponse.IsSuccessStatusCode, "Failed to create or get DM channel.");
            var channelJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var channelId = channelJson.GetProperty("id").GetInt32();

            // Assert
            var messageDto = new { Message = "Hello there!" };
            var response = await client.PostAsJsonAsync($"/api/channels/{channelId}/messages", messageDto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendMessage_WithInvalidChannelId_ShouldReturnBadRequest()
        {
            // Login
            var client = LoginAuthenticatedClient($"channelUserTortoisePterodactyl77@example.com", "StrongPass123$");

            // Assert
            var messageDto = new { Content = "Invalid channel test" };
            var response = await client.PostAsJsonAsync("/api/channels/999999/messages", messageDto);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}