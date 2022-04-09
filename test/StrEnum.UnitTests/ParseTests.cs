using System;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;

public class ParseTests
{
    public class Season : StringEnum<Season>
    {
        public static readonly Season Summer = Define("SUMMER");
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesAnEnumMembersName_ShouldReturnThatEnumMember()
    {
        var parsedMember = Season.Parse("Summer");

        parsedMember.Should().Be(Season.Summer);
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesAnEnumMembersValue_ShouldReturnThatEnumMember()
    {
        var parsedMember = Season.Parse("SUMMER");

        parsedMember.Should().Be(Season.Summer);
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatDoesntMatchAnEnumNameOrValue_ShouldThrowAndException()
    {
        Action parse = () => Season.Parse("Winter");

        parse.Should().Throw<ArgumentException>()
            .WithMessage("Requested name or value 'Winter' was not found.");
    }

    public class SeasonWithAutumnAndFall : StringEnum<SeasonWithAutumnAndFall>
    {
        public static readonly SeasonWithAutumnAndFall Autumn = Define("AUTUMN");
        public static readonly SeasonWithAutumnAndFall Fall = Define("AUTUMN");
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesTwoMembersByValue_ShouldReturnTheFirstDefinedMember()
    {
        var parsedMember = SeasonWithAutumnAndFall.Parse("AUTUMN");

        parsedMember.Should().Be(SeasonWithAutumnAndFall.Autumn);
    }

    public class SeasonWithAutumnAndFall2 : StringEnum<SeasonWithAutumnAndFall2>
    {
        public static readonly SeasonWithAutumnAndFall2 Fall = Define("Autumn");
        public static readonly SeasonWithAutumnAndFall2 Autumn = Define("AUTUMN");
    }

    [Fact]
    public void Parse_CaseSensitive_GivenAValueThatMatchesTwoMembersByValueAndName_ShouldReturnTheFirstDefinedMemberThatMatchesByName()
    {
        var parsedMember = SeasonWithAutumnAndFall2.Parse("Autumn");

        parsedMember.Should().Be(SeasonWithAutumnAndFall2.Autumn);
    }
}