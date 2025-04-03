using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TextbookExchangeApp.Tests;

public class AuthController_Login_Tests
{
    // PASS: Valid email and password
    [Fact]
    public async Task Login_WithValidCredentials_ShouldLogin()
    {
        var factory = new WebApplicationFactory<Program>();
        var htmlClient = factory.CreateClient();
        
        // Valid login request
        var response = await htmlClient.PostAsync("/api/auth/login", JsonContent.Create(new { Email = "admin", Password = "Password123$" }));
        
        // Assert OK response
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify response is correct
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        var message = json.GetProperty("message").GetString();
        Assert.Equal("Logged in successfully.", message);
    }

    // FAIL: Valid email invalid password
    [Fact]
    public async Task Login_WithInvalidPassword_ShouldFail()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/auth/login", JsonContent.Create(new { Email = "admin", Password = "WrongPassword!" }));

        // Assert 401 error
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        // Verify error response
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        var message = json.GetProperty("message").GetString();
        Assert.Equal("Invalid username or password.", message);
    }

    // FAIL: Invalid email
    [Fact]
    public async Task Login_WithInvalidUsername_ShouldFail()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/auth/login", JsonContent.Create(new { Email = "wronguser@example.com", Password = "Password123$" }));

        // Assert 401 error
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        // Verify error response
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        var message = json.GetProperty("message").GetString();
        Assert.Equal("Invalid username or password.", message);
    }

    // FAIL: Empty fields
    [Fact]
    public async Task Login_WithEmptyCredentials_ShouldFail()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient(); 
        var response = await client.PostAsync("/api/auth/login", JsonContent.Create(new { Email = "", Password = "" }));

        // Assert 401 error
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        // Verify error response
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        var message = json.GetProperty("message").GetString();
        Assert.Equal("Invalid username or password.", message);
    }

    // FAIL: Empty password
    [Fact]
    public async Task Login_WithMissingFields_ShouldReturnBadRequest()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/auth/login", JsonContent.Create(new { Email = "admin" }));

        // Assert 400 or 401 error
        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest || 
            response.StatusCode == HttpStatusCode.Unauthorized
        );
    }
}
