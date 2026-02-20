using FluentAssertions;
using PlasticModelApp.Application.Shared;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Shared;

public class ApplicationMappingProfileTests
{
    [Fact]
    public void Constructor_Can_Be_Initialized()
    {
        var profile = new ApplicationMappingProfile();
        profile.Should().NotBeNull();
    }
}
