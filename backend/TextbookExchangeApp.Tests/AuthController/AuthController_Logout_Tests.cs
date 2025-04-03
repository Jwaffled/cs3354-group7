using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TextbookExchangeApp.Tests;

public class AuthController_Logout_Tests
{
    // PASS: Login then logout
    [Fact]
    public async Task Logout_WhenAuthenticated_ShouldReturnOk()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // Login first
        var loginResponse = await client.PostAsync("/api/auth/login", JsonContent.Create(new
        {
            Email = "admin",
            Password = "Password123$"
        }));

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        // Logout
        var logoutResponse = await client.PostAsync("/api/auth/logout", null);

        // Assert OK status
        Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);

        // Assert message
        var content = await logoutResponse.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        var message = json.GetProperty("message").GetString();
        Assert.Equal("Signed out successfully.", message);
    }

    // FAIL: No login before logout
    [Fact]
    public async Task Logout_WhenNotAuthenticated_ShouldReturnUnauthorized()
    {
        var factory = new WebApplicationFactory<Program>();
        var unauthenticatedClient = factory.CreateClient();

        // No login beforehand
        var logoutResponse = await unauthenticatedClient.PostAsync("/api/auth/logout", null);

        // Expect 401 Unauthorized
        Assert.Equal(HttpStatusCode.Unauthorized, logoutResponse.StatusCode);
    }
}
