using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Entities;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.Entities;

public class PaintTests
{
    private static readonly DateTimeOffset Now = new(2020, 1, 2, 3, 4, 5, TimeSpan.Zero);
    private static readonly DateTimeOffset Later = new(2020, 1, 2, 4, 0, 0, TimeSpan.Zero);

    private static (BrandId brand, PaintTypeId type, GlossId gloss) Ids() =>
        (BrandId.From("B1"), PaintTypeId.From("T1"), GlossId.From("G1"));

    private static Paint CreatePaint(string id = "P1")
    {
        var (brand, type, gloss) = Ids();
        return Paint.Create(
            new PaintId(id),
            "Paint1",
            brand,
            type,
            gloss,
            new ColorSpec("#3366CC"),
            new Price(120m),
            new ModelNumber("BR-1"),
            Now);
    }

    [Fact]
    public void Create_Should_Set_Properties()
    {
        var p = CreatePaint();
        p.Id.Value.Should().Be("P1");
        p.Name.Should().Be("Paint1");
        p.BrandId.Value.Should().Be("B1");
        p.Color.Hex.Value.Should().Be("#3366CC");
        p.ModelNumber.Value.Should().Be("BR-1");
    }

    [Fact]
    public void Create_Should_Set_ImageUrl_When_Provided()
    {
        var (brand, type, gloss) = Ids();
        var p = Paint.Create(
            new PaintId("P1"),
            "Paint1",
            brand,
            type,
            gloss,
            new ColorSpec("#3366CC"),
            new Price(120m),
            new ModelNumber("BR-1"),
            Now,
            imageUrl: "img");

        p.ImageUrl.Should().Be("img");
    }

    [Fact]
    public void Create_Should_Generate_New_Id_When_Id_Not_Provided()
    {
        var (brand, type, gloss) = Ids();
        var p = Paint.Create(
            "Paint1",
            brand,
            type,
            gloss,
            new ColorSpec("#3366CC"),
            new Price(120m),
            new ModelNumber("BR-1"),
            Now);

        p.Id.Should().NotBeNull();
        p.Id.Value.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void UpdateDetails_Should_Change_Fields()
    {
        var p = CreatePaint();
        p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: new ModelNumber("BR-2"),
            price: new Price(999m),
            brandId: BrandId.From("B2"),
            paintTypeId: PaintTypeId.From("T2"),
            glossId: GlossId.From("G2"),
            color: new ColorSpec("#112233"),
            imageUrl: "img",
            now: Later);
        p.Name.Should().Be("New");
        p.BrandId.Value.Should().Be("B2");
        p.Color.Hex.Value.Should().Be("#112233");
    }

    [Fact]
    public void MarkAsDeleted_Should_Set_Flag()
    {
        var p = CreatePaint();
        p.MarkAsDeleted(Later);
        p.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void MarkAsDeleted_Should_Be_Idempotent()
    {
        var p = CreatePaint();
        p.MarkAsDeleted(Later);

        Action act = () => p.MarkAsDeleted(Later.AddMinutes(1));

        act.Should().NotThrow();
        p.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void UpdateDetails_Should_Throw_When_Deleted()
    {
        var p = CreatePaint();
        p.MarkAsDeleted(Later);

        Action act = () => p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: new ModelNumber("BR-2"),
            price: new Price(999m),
            brandId: BrandId.From("B2"),
            paintTypeId: PaintTypeId.From("T2"),
            glossId: GlossId.From("G2"),
            color: new ColorSpec("#112233"),
            imageUrl: "img",
            now: Later);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UpdateDetails_Should_Validate_Null_PaintType()
    {
        var p = CreatePaint();
        Action act = () => p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: new ModelNumber("BR-2"),
            price: new Price(999m),
            brandId: BrandId.From("B2"),
            paintTypeId: null!,
            glossId: GlossId.From("G2"),
            color: new ColorSpec("#112233"),
            imageUrl: "img",
            now: Later);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdateDetails_Should_Validate_Null_Gloss()
    {
        var p = CreatePaint();
        Action act = () => p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: new ModelNumber("BR-2"),
            price: new Price(999m),
            brandId: BrandId.From("B2"),
            paintTypeId: PaintTypeId.From("T2"),
            glossId: null!,
            color: new ColorSpec("#112233"),
            imageUrl: "img",
            now: Later);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdateDetails_Should_Validate_Null_Brand()
    {
        var p = CreatePaint();
        Action act = () => p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: new ModelNumber("BR-2"),
            price: new Price(999m),
            brandId: null!,
            paintTypeId: PaintTypeId.From("T2"),
            glossId: GlossId.From("G2"),
            color: new ColorSpec("#112233"),
            imageUrl: "img",
            now: Later);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdateDetails_Should_Validate_Null_Color()
    {
        var p = CreatePaint();
        Action act = () => p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: new ModelNumber("BR-2"),
            price: new Price(999m),
            brandId: BrandId.From("B2"),
            paintTypeId: PaintTypeId.From("T2"),
            glossId: GlossId.From("G2"),
            color: null!,
            imageUrl: "img",
            now: Later);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdateDetails_Should_Validate_Null_Price()
    {
        var p = CreatePaint();
        Action act = () => p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: new ModelNumber("BR-2"),
            price: null!,
            brandId: BrandId.From("B2"),
            paintTypeId: PaintTypeId.From("T2"),
            glossId: GlossId.From("G2"),
            color: new ColorSpec("#112233"),
            imageUrl: "img",
            now: Later);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void UpdateDetails_Should_Validate_Null_ModelNumber()
    {
        var p = CreatePaint();
        Action act = () => p.UpdateDetails(
            name: "New",
            description: "d",
            modelNumber: null!,
            price: new Price(999m),
            brandId: BrandId.From("B2"),
            paintTypeId: PaintTypeId.From("T2"),
            glossId: GlossId.From("G2"),
            color: new ColorSpec("#112233"),
            imageUrl: "img",
            now: Later);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_Should_Validate_Arguments()
    {
        var (brand, type, gloss) = Ids();
        Action emptyName = () => _ = Paint.Create(new PaintId("PX"), " ", brand, type, gloss,
            new ColorSpec("#3366CC"), new Price(1m), new ModelNumber("M"), Now);
        emptyName.Should().Throw<ArgumentException>();

        Action nullBrand = () => _ = Paint.Create(new PaintId("PX"), "N", null!, type, gloss,
            new ColorSpec("#3366CC"), new Price(1m), new ModelNumber("M"), Now);
        nullBrand.Should().Throw<ArgumentNullException>();

        Action nullModel = () => _ = Paint.Create(new PaintId("PX"), "N", brand, type, gloss,
            new ColorSpec("#3366CC"), new Price(1m), null!, Now);
        nullModel.Should().Throw<ArgumentNullException>();

        Action nullColor = () => _ = Paint.Create(new PaintId("PX"), "N", brand, type, gloss,
            null!, new Price(1m), new ModelNumber("M"), Now);
        nullColor.Should().Throw<ArgumentNullException>();
    }
}
