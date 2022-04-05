using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;

public class ConversionsTests
{
    public class Season : StringEnum<Season>
    {
        public static readonly Season Summer = Define("SUMMER");
    }

    [Fact]
    public void ExplicitConversionToString_ShouldReturnTheMemberValue()
    {
        var actualValue = (string)Season.Summer;

        actualValue.Should().Be("SUMMER");
    }
}