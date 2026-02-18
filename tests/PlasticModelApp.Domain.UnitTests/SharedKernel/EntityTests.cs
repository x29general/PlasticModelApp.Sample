using PlasticModelApp.Domain.SharedKernel.Entities;

namespace PlasticModelApp.Domain.UnitTests.SharedKernel.Entities;

public class EntityTests
{
    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameTypeAndSameId()
    {
        var left = new TestEntity("id-1");
        var right = new TestEntity("id-1");

        Assert.True(left.Equals(right));
        Assert.True(left.IsSameEntityAs(right));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentId()
    {
        var left = new TestEntity("id-1");
        var right = new TestEntity("id-2");

        Assert.False(left.Equals(right));
        Assert.False(left.IsSameEntityAs(right));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentConcreteTypeEvenSameId()
    {
        var left = new TestEntity("id-1");
        var right = new AnotherTestEntity("id-1");

        Assert.False(left.Equals(right));
        Assert.False(left.IsSameEntityAs(right));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenEitherIdIsNull()
    {
        var left = new TestEntity(null);
        var right = new TestEntity("id-1");

        Assert.False(left.Equals(right));
        Assert.False(left.IsSameEntityAs(right));
    }

    [Fact]
    public void GetHashCode_ShouldMatch_WhenSameTypeAndSameId()
    {
        var left = new TestEntity("id-1");
        var right = new TestEntity("id-1");

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    private sealed class TestEntity : Entity<string?>
    {
        public TestEntity(string? id)
        {
            Id = id;
        }
    }

    private sealed class AnotherTestEntity : Entity<string?>
    {
        public AnotherTestEntity(string? id)
        {
            Id = id;
        }
    }
}
