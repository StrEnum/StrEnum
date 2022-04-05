using System;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;


public class DefineTests
{
    private class TropicalSeason : StringEnum<TropicalSeason>
    {
        public static readonly TropicalSeason Wet = Define("WET");
        public static readonly TropicalSeason Dry = Define("DRY");
    }

    [Fact]
    public void Define_GivenDifferentKeys_ShouldCreateEnum()
    {
        TropicalSeason.Wet.ToString().Should().Be("Wet");
        TropicalSeason.Dry.ToString().Should().Be("Dry");
    }

    private class SeasonWithMemberValueOfNull : StringEnum<SeasonWithMemberValueOfNull>
    {
        public static readonly SeasonWithMemberValueOfNull Summer = Define(null);
    }

    [Fact]
    public void Define_GivenAValueOfNull_ShouldThrowAnException()
    {
        var accessMember = () => SeasonWithMemberValueOfNull.Summer;
        
        accessMember.Should().Throw<TypeInitializationException>()
            .WithInnerException<ArgumentNullException>()
            .WithParameterName("value");
    }

    private class SeasonWithAMemberNameOfNull : StringEnum<SeasonWithAMemberNameOfNull>
    {
        public static readonly SeasonWithAMemberNameOfNull Summer = Define("Summer", null);
    }

    [Fact]
    public void Define_GivenANameOfNull_ShouldThrowAnException()
    {
        var accessMember = () => SeasonWithAMemberNameOfNull.Summer;

        accessMember.Should().Throw<TypeInitializationException>()
            .WithInnerException<ArgumentNullException>()
            .WithParameterName("name");
    }
}