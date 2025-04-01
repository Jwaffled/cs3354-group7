using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TextbookExchangeApp.Tests;

public class AuthController_Tests
{
    [Fact]
    public async Task Login_WithValidCredentials_ShouldLogin()
    {
        var factory = new WebApplicationFactory<Program>();
        var htmlClient = factory.CreateClient();
        
        var response = await htmlClient.PostAsync("/api/auth/login", JsonContent.Create(new { Email = "admin", Password = "Password123$" }));
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        var message = json.GetProperty("message").GetString();
        Assert.Equal("Logged in successfully.", message);
    }
}