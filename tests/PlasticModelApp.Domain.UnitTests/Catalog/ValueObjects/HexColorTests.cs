using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.ValueObjects;

public class HexColorTests
{
    [Theory]
    [InlineData("112233")]     // 先頭#なし
    [InlineData("#123")]       // 桁不足
    [InlineData("#GG0000")]    // 非16進文字
    public void Ctor_Should_Throw_When_Invalid(string input)
    {
        Action act = () => _ = new HexColor(input);
        act.Should().Throw<ArgumentException>();
    }
}

