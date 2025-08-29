using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

namespace BudgetPilot.Tests.Integration;

public class HealthCheckTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthCheckTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthyWithTimestamp()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");

        var content = await response.Content.ReadAsStringAsync();
        var healthResponse = JsonSerializer.Deserialize<JsonElement>(content);
        
        healthResponse.GetProperty("status").GetString().Should().Be("Healthy");
        healthResponse.GetProperty("timestamp").GetString().Should().NotBeNullOrEmpty();
        healthResponse.TryGetProperty("checks", out var checks).Should().BeTrue();
        
        // Verify timestamp format (ISO 8601) and is recent
        var timestamp = healthResponse.GetProperty("timestamp").GetString();
        timestamp.Should().NotBeNullOrEmpty();
        timestamp.Should().MatchRegex(@"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}Z");  // ISO 8601 format
        
        var parsedTimestamp = DateTime.Parse(timestamp!).ToUniversalTime();
        parsedTimestamp.Should().BeAfter(DateTime.UtcNow.AddMinutes(-10));  // Should be within last 10 minutes
    }
}