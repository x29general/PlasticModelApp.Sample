using FluentAssertions;
using PlasticModelApp.Application.Shared.Exceptions;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Shared.Exceptions;

public class AppExceptionsTests
{
    [Fact]
    public void NotFoundException_Preserves_Properties()
    {
        var timestamp = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var details = new object[] { "id", 10 };

        var ex = new NotFoundException("E-404-001", "not found", details, timestamp);

        ex.Code.Should().Be("E-404-001");
        ex.Message.Should().Be("not found");
        ex.Details.Should().BeEquivalentTo(details);
        ex.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void ValidationException_Preserves_Properties()
    {
        var timestamp = DateTime.UtcNow;
        var details = new object[] { new { Field = "Name", Messages = new[] { "required" } } };

        var ex = new ValidationException("E-400-001", "validation error", details, timestamp);

        ex.Code.Should().Be("E-400-001");
        ex.Message.Should().Be("validation error");
        ex.Details.Should().BeEquivalentTo(details);
        ex.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void AppSystemException_Preserves_Properties()
    {
        var timestamp = DateTime.UtcNow;
        var details = new object[] { "detail" };

        var ex = new AppSystemException("E-500-01", "system error", details, timestamp);

        ex.Code.Should().Be("E-500-01");
        ex.Message.Should().Be("system error");
        ex.Details.Should().BeEquivalentTo(details);
        ex.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void ForbiddenException_Preserves_Properties()
    {
        var timestamp = DateTime.UtcNow;
        var details = new object[] { "forbidden" };

        var ex = new ForbiddenException("E-403-001", "forbidden", details, timestamp);

        ex.Code.Should().Be("E-403-001");
        ex.Message.Should().Be("forbidden");
        ex.Details.Should().BeEquivalentTo(details);
        ex.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void UnauthorizedException_Preserves_Properties()
    {
        var timestamp = DateTime.UtcNow;
        var details = new object[] { "unauthorized" };

        var ex = new UnauthorizedException("E-401-001", "unauthorized", details, timestamp);

        ex.Code.Should().Be("E-401-001");
        ex.Message.Should().Be("unauthorized");
        ex.Details.Should().BeEquivalentTo(details);
        ex.Timestamp.Should().Be(timestamp);
    }
}
