using System;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;


public class ToStringTests
{
    public class Season : StringEnum<Season>
    {
        public static readonly Season Summer = Define("SUMMER");
    }

    [Fact]
    public void ToString_ShouldReturnTheMemberName()
    {
        Season.Summer.ToString().Should().Be("Summer");
    }
}