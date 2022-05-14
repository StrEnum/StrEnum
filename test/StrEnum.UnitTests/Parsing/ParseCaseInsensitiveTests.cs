using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests.Parsing;

public class ParseCaseInsensitiveTests
{
    public class Sport : StringEnum<Sport>
    {
        public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");
    }

    [Fact]
    public void Parse_CaseInsensitive_GivenAValueThatMatchesAnEnumMembersName_ShouldReturnThatEnumMember()
    {
        var parsedMember = Sport.Parse("trailrunning", true);

        parsedMember.Should().Be(Sport.TrailRunning);
    }

    [Fact]
    public void Parse_CaseInsensitive_GivenAValueThatMatchesAnEnumMembersValue_ShouldReturnThatEnumMember()
    {
        var parsedMember = Sport.Parse("trail_running", true);

        parsedMember.Should().Be(Sport.TrailRunning);
    }

    public class SportWithAliasForMtb : StringEnum<SportWithAliasForMtb>
    {
        public static readonly SportWithAliasForMtb MountainBike = Define("MOUNTAIN_BIKE");
        public static readonly SportWithAliasForMtb Mountainbike = Define("MOUNTAIN_bike");
    }

    [Fact]
    public void Parse_CaseInsensitive_GivenAValueThatMatchesTwoMembersByName_ShouldReturnTheFirstMatchingMember()
    {
        var parsedMember = SportWithAliasForMtb.Parse("mountainbike", true);

        parsedMember.Should().Be(SportWithAliasForMtb.MountainBike);
    }

    [Fact]
    public void Parse_CaseInsensitive_GivenAValueThatMatchesTwoMembersByValue_ShouldReturnTheFirstMatchingMember()
    {
        var parsedMember = SportWithAliasForMtb.Parse("mountainbike", true);

        parsedMember.Should().Be(SportWithAliasForMtb.MountainBike);
    }
}