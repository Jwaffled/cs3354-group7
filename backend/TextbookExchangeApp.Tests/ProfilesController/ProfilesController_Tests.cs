using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TextbookExchangeApp.Tests.ProfilesController
{
    public class ProfilesController_Tests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProfilesController_Tests(WebApplicationFactory<Program> factory)
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
        public async Task GetProfile_WithValidId_ShouldReturnOk()
        {
            // New login
            var client = CreateAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            var meResponse = await client.GetAsync("/api/auth/me");
            Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);

            var meJson = await meResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = meJson.GetProperty("id").GetString();

            // Act
            var response = await client.GetAsync($"/api/profiles/{profileId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var profileJson = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal(profileId, profileJson.GetProperty("id").GetString());
            Assert.Equal("Test", profileJson.GetProperty("firstName").GetString());
            Assert.Equal("User", profileJson.GetProperty("lastName").GetString());
            Assert.True(profileJson.TryGetProperty("averageRating", out var _));
        }

        [Fact]
        public async Task GetProfile_WithInvalidId_ShouldReturnNotFound()
        {
            // Login
            var client = LoginAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            var invalidProfileId = Guid.NewGuid().ToString();

            // Act
            var response = await client.GetAsync($"/api/profiles/{invalidProfileId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var errorJson = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal("User not found.", errorJson.GetProperty("message").GetString());
        }

        [Fact]
        public async Task CreateReply_WithValidRating_ShouldReturnOk()
        {
            // Login
            var client = LoginAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            // Get Profile ID
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            var validReplyDto = new
            {
                RecipientId = Guid.NewGuid(),
                Rating = 5,
                Message = "Great experience!"
            };

            // Act
            var response = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", validReplyDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(responseBody.GetProperty("id").GetInt32() > 0);
        }

        [Fact]
        public async Task CreateReply_WithInvalidRating_ShouldReturnBadRequest()
        {
            // Login
            var client = LoginAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            // Get Profile ID
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Act
            var invalidReplyDto = new
            {
                RecipientId = Guid.NewGuid(),
                Rating = 0,
                Message = "This rating is invalid."
            };

            var response = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", invalidReplyDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetStarAvg_WithValidAvgRating_ShouldReturnOk()
        {
            // New Login
            var client = CreateAuthenticatedClient($"validStarAvg{Guid.NewGuid()}@example.com", "StrongPass123$");

            // Get profile ID
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Act Post
            await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = 4,
                Message = "Great transaction!"
            });

            await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = 2,
                Message = "It was okay."
            });

            // Act
            var profileResponse = await client.GetAsync($"/api/profiles/{profileId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, profileResponse.StatusCode);

            var profileJson = await profileResponse.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal(profileId, profileJson.GetProperty("id").GetString());
            Assert.Equal(3.0, profileJson.GetProperty("averageRating").GetDouble());
        }

        [Fact]
        public async Task GetStarAvg_WithInvalidAvgRating_ShouldReturnBadRequest()
        {
            // Login
            var client = LoginAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            // Get profile ID
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Act
            var response = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = -1, // Invalid rating
                Message = "Invalid rating test"
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetReplyRating_WithValidRating_ShouldReturnOk()
        {
            // New Login
            var client = CreateAuthenticatedClient($"valid{Guid.NewGuid()}@example.com", "StrongPass123$");

            // Get Profile ID
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Act
            var postResponse = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = 4,
                Message = "Smooth transaction!"
            });

            postResponse.EnsureSuccessStatusCode();

            var replyJson = await postResponse.Content.ReadFromJsonAsync<JsonElement>();

            // Assert
            Assert.True(replyJson.TryGetProperty("rating", out var ratingElement), "Rating field not found in response.");
            Assert.Equal(4, ratingElement.GetInt32());
        }

        [Fact]
        public async Task GetReplyRating_WithInvalidRating_ShouldReturnBadRequest()
        {
            // Login
            var client = LoginAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            // Get profile ID
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Act
            var postResponse = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = -1,
                Message = "Bad rating test"
            });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }

        [Fact]
        public async Task GetReplies_ByValidProfileId_ShouldReturnOk()
        {
            // Login
            var client = LoginAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            // Get profile ID
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Add a reply to the profile
            var replyResponse = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = 5,
                Message = "Excellent!"
            });

            replyResponse.EnsureSuccessStatusCode();

            // Add another reply to the profile
            var replyResponse1 = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = 4,
                Message = "Almost Perfect!"
            });

            replyResponse1.EnsureSuccessStatusCode();

            // Act
            var getRepliesResponse = await client.GetAsync($"/api/profiles/{profileId}/replies");

            // Assert
            Assert.Equal(HttpStatusCode.OK, getRepliesResponse.StatusCode);
            var repliesJson = await getRepliesResponse.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(repliesJson.GetArrayLength() > 0);
        }

        [Fact]
        public async Task GetReply_ByValidReplyId_ShouldReturnOk()
        {
            // New Login
            var client = CreateAuthenticatedClient($"valid{Guid.NewGuid()}@example.com", "StrongPass123$");

            // Get profile Id
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Create a reply
            var createReplyResponse = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = 5,
                Message = "Do not delete this reply!"
            });

            Assert.Equal(HttpStatusCode.OK, createReplyResponse.StatusCode);

            var replyJson = await createReplyResponse.Content.ReadFromJsonAsync<JsonElement>();
            var validReplyId = replyJson.GetProperty("id").GetInt32();

            // Act
            var response = await client.GetAsync($"/api/profiles/{profileId}/replies/{validReplyId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var stream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(stream);
            var root = json.RootElement;

            Assert.True(root.TryGetProperty("id", out var idElement));
            Assert.Equal(validReplyId, idElement.GetInt32());

            Assert.True(root.TryGetProperty("message", out var messageElement));
            Assert.Equal("Do not delete this reply!", messageElement.GetString());

            Assert.True(root.TryGetProperty("rating", out var ratingElement));
            int rating = ratingElement.GetInt32();
            Assert.InRange(rating, 1, 5);

            Assert.True(root.TryGetProperty("createdAt", out var createdAtElement));
            Assert.NotEqual(default, createdAtElement.GetDateTime());

            Assert.True(root.TryGetProperty("authorName", out var authorElement));
            Assert.False(string.IsNullOrWhiteSpace(authorElement.GetString()));
        }

        [Fact]
        public async Task GetReply_ByInvalidReplyId_ShouldReturnNotFound()
        {
            // Login
            var client = LoginAuthenticatedClient($"skibidiChungusDeluxeXXL@example.com", "StrongPass123$");

            // Get profile Id
            var userResponse = await client.GetAsync("/api/auth/me");
            var userJson = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
            var profileId = userJson.GetProperty("id").GetString();

            // Create a reply
            var createReplyResponse = await client.PostAsJsonAsync($"/api/profiles/{profileId}/replies", new
            {
                Rating = 5,
                Message = "Do not delete this reply!"
            });

            Assert.Equal(HttpStatusCode.OK, createReplyResponse.StatusCode);

            var replyJson = await createReplyResponse.Content.ReadFromJsonAsync<JsonElement>();
            var invalidReplyId = 99999;

            // Act
            var response = await client.GetAsync($"/api/profiles/{profileId}/replies/{invalidReplyId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}