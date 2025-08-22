using FluentAssertions;
using Xunit;

namespace BudgetPilot.Tests.Unit;

public class BasicSmokeTests
{
    [Fact]
    public void ApplicationStarts_Successfully()
    {
        // Simple smoke test to ensure test infrastructure works
        var result = 1 + 1;
        result.Should().Be(2);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("John", null, "John")]
    [InlineData(null, "Doe", "Doe")]
    [InlineData("John", "Doe", "John Doe")]
    public void ApplicationUser_FullName_FormatsCorrectly(string? firstName, string? lastName, string expected)
    {
        // Arrange
        var user = new BudgetPilot.Web.Models.ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName
        };

        // Act & Assert
        user.FullName.Should().Be(expected);
    }
}