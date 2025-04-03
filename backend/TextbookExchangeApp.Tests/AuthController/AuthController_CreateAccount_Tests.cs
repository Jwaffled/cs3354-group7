using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TextbookExchangeApp.Tests;

public class AuthController_CreateAccount_Tests
{
    // PASS: Valid email and password
    [Fact]
    public async Task CreateAccount_WithValidData_ShouldSucceed()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // POST request with valid email and password
        var response = await client.PostAsync("/api/auth/create-account", JsonContent.Create(new
        {
            Email = $"test{Guid.NewGuid()}@example.com", // Creates unique email
            Password = "ValidPassword123$"
        }));

        // Assert that OK status is reached
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify response is correct
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        var message = json.GetProperty("message").GetString();
        Assert.Equal("Account Created Successfully", message);
    }

    // FAIL: Valid email weak password
    [Fact]
    public async Task CreateAccount_WithWeakPassword_ShouldFail()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // Create weak password
        var response = await client.PostAsync("/api/auth/create-account", JsonContent.Create(new
        {
            Email = $"weakpass{Guid.NewGuid()}@example.com",
            Password = "123" // No non-alphanumeric or capital chars
        }));

        // Assert a 400 bad request
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // FAIL: Create account with used email
    [Fact]
    public async Task CreateAccount_WithDuplicateEmail_ShouldFail()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var email = $"dupe{Guid.NewGuid()}@example.com";

        // Create the first account. Should succeed.
        var first = await client.PostAsync("/api/auth/create-account", JsonContent.Create(new
        {
            Email = email,
            Password = "Password123$"
        }));
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        // Try to create the account again. Should now fail.
        var second = await client.PostAsync("/api/auth/create-account", JsonContent.Create(new
        {
            Email = email,
            Password = "Password123$"
        }));
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
    }

    // FAIL: Empty email
    [Fact]
    public async Task CreateAccount_MissingEmail_ShouldReturnBadRequest()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.PostAsync("/api/auth/create-account", JsonContent.Create(new
        {
            Password = "Password123$"
        }));

        // Assert a 400 bad request
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // FAIL: Empty email and password
    [Fact]
    public async Task CreateAccount_EmptyFields_ShouldReturnBadRequest()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/auth/create-account", JsonContent.Create(new
        {
            Email = "",
            Password = ""
        }));

        // Assert a 400 bad request
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
