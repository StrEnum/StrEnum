using System;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests.Parsing;

public class ParseCaseSensitiveTests
{
    public class Sport : StringEnum<Sport>
    {
        public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesAnEnumMembersName_ShouldReturnThatEnumMember()
    {
        var parsedMember = Sport.Parse("TrailRunning");

        parsedMember.Should().Be(Sport.TrailRunning);
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesAnEnumMembersValue_ShouldReturnThatEnumMember()
    {
        var parsedMember = Sport.Parse("TRAIL_RUNNING");

        parsedMember.Should().Be(Sport.TrailRunning);
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatDoesntMatchAnEnumNameOrValue_ShouldThrowAndException()
    {
        Action parse = () => Sport.Parse("Apples");

        parse.Should().Throw<ArgumentException>()
            .WithMessage("Requested name or value 'Apples' was not found.");
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatDoesntMatchAnEnumNameOrValueByCase_ShouldThrowAndException()
    {
        Action parse = () => Sport.Parse("trail_running");

        parse.Should().Throw<ArgumentException>()
            .WithMessage("Requested name or value 'trail_running' was not found.");
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
    public void Parse_CaseSensitive_GivenAValueThatMatchesTwoMembersByValueAndName_ShouldReturnTheFirstDefinedMemberThatMatchesByName()
    {
        var parsedMember = SportWithAliasForMtb2.Parse("MountainBike");

        parsedMember.Should().Be(SportWithAliasForMtb2.MountainBike);
    }


}