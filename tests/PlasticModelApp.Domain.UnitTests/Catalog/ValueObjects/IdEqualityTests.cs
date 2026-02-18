using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.ValueObjects;

public class IdEqualityTests
{
    [Fact]
    public void TagCategoryId_Equality_By_Value()
    {
        var a = TagCategoryId.From("TC1");
        var b = TagCategoryId.From("TC1");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void TagId_Equality_By_Value()
    {
        var a = TagId.From("T1");
        var b = TagId.From("T1");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void BrandId_Equality_By_Value()
    {
        var a = BrandId.From("B1");
        var b = BrandId.From("B1");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void GlossId_Equality_By_Value()
    {
        var a = GlossId.From("G1");
        var b = GlossId.From("G1");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void PaintTypeId_Equality_By_Value()
    {
        var a = PaintTypeId.From("PT1");
        var b = PaintTypeId.From("PT1");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void UserId_Equality_By_Value()
    {
        var a = UserId.From("U1");
        var b = UserId.From("U1");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void PaintId_Equality_By_Value()
    {
        var a = new PaintId("P1");
        var b = new PaintId("P1");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void HexColor_Format_Validation()
    {
        var hex = new HexColor("#00FFAA");
        hex.Value.Should().Be("#00FFAA");
    }

    [Fact]
    public void Ids_Should_Throw_When_Whitespace()
    {
        Action tagCategory = () => _ = TagCategoryId.From("   ");
        Action tag = () => _ = TagId.From("   ");
        Action brand = () => _ = BrandId.From("   ");
        Action gloss = () => _ = GlossId.From("   ");
        Action paintType = () => _ = PaintTypeId.From("   ");
        Action user = () => _ = UserId.From("   ");
        Action paint = () => _ = new PaintId("   ");

        tagCategory.Should().Throw<ArgumentException>();
        tag.Should().Throw<ArgumentException>();
        brand.Should().Throw<ArgumentException>();
        gloss.Should().Throw<ArgumentException>();
        paintType.Should().Throw<ArgumentException>();
        user.Should().Throw<ArgumentException>();
        paint.Should().Throw<ArgumentException>();
    }
}

