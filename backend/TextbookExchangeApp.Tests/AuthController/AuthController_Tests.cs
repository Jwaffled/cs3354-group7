using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TextbookExchangeApp.Tests;

public class AuthController_Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthController_Tests(WebApplicationFactory<Program> factory)
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
    public async Task CreateAccount_Valid_ShouldReturnOk()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/create-account", new
        {
            Email = $"valid{Guid.NewGuid()}@example.com",
            Password = "StrongPass123$",
            FirstName = "Test",
            LastName = "User"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Account Created Successfully", content.GetProperty("message").GetString());
    }

    [Fact]
    public async Task CreateAccount_WeakPassword_ShouldFail()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/create-account", new
        {
            Email = $"weak{Guid.NewGuid()}@example.com",
            Password = "123",
            FirstName = "Test",
            LastName = "User"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateAccount_DuplicateEmail_ShouldFail()
    {
        var client = _factory.CreateClient();
        var email = $"dupe{Guid.NewGuid()}@example.com";

        var first = await client.PostAsJsonAsync("/api/auth/create-account", new
        {
            Email = email,
            Password = "Password123$",
            FirstName = "Test",
            LastName = "User"
        });
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        var second = await client.PostAsJsonAsync("/api/auth/create-account", new
        {
            Email = email,
            Password = "Password123$",
            FirstName = "Test",
            LastName = "User"
        });
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
    }

    [Fact]
    public async Task CreateAccount_EmptyFields_ShouldFail()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/create-account", new { });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_Valid_ShouldSucceed()
    {
        var client = _factory.CreateClient();
        var email = $"user{Guid.NewGuid()}@example.com";

        await client.PostAsJsonAsync("/api/auth/create-account", new
        {
            Email = email,
            Password = "Valid123$",
            FirstName = "Jane",
            LastName = "Doe"
        });

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            Email = email,
            Password = "Valid123$"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Logged in successfully.", json.GetProperty("message").GetString());
    }

    [Fact]
    public async Task Login_InvalidPassword_ShouldFail()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            Email = "admin",
            Password = "WrongPassword!"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_InvalidEmail_ShouldFail()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            Email = "notarealuser@example.com",
            Password = "Whatever123$"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Logout_WhenLoggedIn_ShouldSucceed()
    {
        var email = $"logout{Guid.NewGuid()}@example.com";
        var password = "Password123$";
        var client = CreateAuthenticatedClient(email, password);

        var logout = await client.PostAsync("/api/auth/logout", null);
        Assert.Equal(HttpStatusCode.OK, logout.StatusCode);

        var json = await logout.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Signed out successfully.", json.GetProperty("message").GetString());
    }

    [Fact]
    public async Task Logout_WithoutLogin_ShouldReturnUnauthorized()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/api/auth/logout", null);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
