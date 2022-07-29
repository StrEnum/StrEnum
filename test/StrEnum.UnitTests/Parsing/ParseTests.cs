using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests.Parsing;

public class ParseTests
{
    public class Sport : StringEnum<Sport>
    {
        public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");

        public static readonly Sport MountainBike = Define("MOUNTAIN_BIKE");
        public static readonly Sport Mountainbike = Define("MOUNTAIN_bike");
    }

    public static IEnumerable<object?[]> ParseNameOrValueSuccessTestCases =>
        new[]
        {
            new object?[] {"TrailRunning", false, Sport.TrailRunning},
            new object?[] { "TRAIL_RUNNING", false, Sport.TrailRunning},
            new object?[] { "trailrunning", true, Sport.TrailRunning},
            new object?[] { "trail_running", true, Sport.TrailRunning},
            new object?[] { "mountainbike", true, Sport.MountainBike},
            new object?[] { "mountain_bike", true, Sport.MountainBike},
        };

    [Theory]
    [MemberData(nameof(ParseNameOrValueSuccessTestCases))]
    public void Parse_GivenAValueThatMatchesAMember_ByNameOrValue_ShouldReturnThatEnumMember(string value, bool ignoreCase, Sport expected)
    {
        var parsedMember = Sport.Parse(value, ignoreCase);

        parsedMember.Should().Be(expected);
    }

    public static IEnumerable<object?[]> ParseValueOnlySuccessTestCases =>
        new[]
        {
            new object?[] { "TRAIL_RUNNING", false, Sport.TrailRunning},
            new object?[] { "trail_running", true, Sport.TrailRunning},
            new object?[] { "mountain_bike", true, Sport.MountainBike},
        };

    [Theory]
    [MemberData(nameof(ParseValueOnlySuccessTestCases))]
    public void Parse_GivenAValueThatMatchesAMember_ByValueOnly_ShouldReturnThatEnumMember(string value, bool ignoreCase, Sport expected)
    {
        var parsedMember = Sport.Parse(value, ignoreCase, MatchBy.ValueOnly);

        parsedMember.Should().Be(expected);
    }


    [Theory]
    [InlineData("Apples", false)]
    [InlineData("trail_running", false)]
    public void Parse_GivenAValueThatDoesntMatchAMember_ByNeitherNameOrValue_ShouldThrowAnException(string value, bool ignoreCase)
    {
        Action parse = () => Sport.Parse(value, ignoreCase);

        parse.Should().Throw<ArgumentException>()
            .WithMessage($"Requested name or value '{value}' was not found.");
    }

    [Theory]
    [InlineData("TrailRunning", false)]
    public void Parse_GivenAValueThatDoesntMatchAMember_ByValue_ShouldThrowAnException(string value, bool ignoreCase)
    {
        Action parse = () => Sport.Parse(value, ignoreCase, MatchBy.ValueOnly);

        parse.Should().Throw<ArgumentException>()
            .WithMessage($"Requested name or value '{value}' was not found.");
    }

    public class SportWithAliasForMtb : StringEnum<SportWithAliasForMtb>
    {
        public static readonly SportWithAliasForMtb MountainBike = Define("MOUNTAIN_BIKE");
        public static readonly SportWithAliasForMtb MTB = Define("MOUNTAIN_BIKE");
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesTwoMembersByValue_ShouldReturnTheFirstMatchingMember()
    {
        var parsedMember = SportWithAliasForMtb.Parse("MOUNTAIN_BIKE");

        parsedMember.Should().Be(SportWithAliasForMtb.MountainBike);
    }

    public class SportWithAliasForMtb2 : StringEnum<SportWithAliasForMtb2>
    {
        public static readonly SportWithAliasForMtb2 MTB = Define("MountainBike");
        public static readonly SportWithAliasForMtb2 MountainBike = Define("MTB");
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesTwoMembersByValueAndName_ShouldReturnTheFirstDefinedMemberThatMatchesByValue()
    {
        var parsedMember = SportWithAliasForMtb2.Parse("MountainBike");

        parsedMember.Should().Be(SportWithAliasForMtb2.MTB);
    }
}