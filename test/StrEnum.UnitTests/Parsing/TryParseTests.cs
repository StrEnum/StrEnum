using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests.Parsing;

public class TryParseTests
{
    public class Sport : StringEnum<Sport>
    {
        public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");
    }

    [Theory]
    [InlineData("TrailRunning", false)]
    [InlineData("TRAIL_RUNNING", false)]
    [InlineData("trailrunning", true)]
    [InlineData("trail_running", true)]
    public void TryParse_GivenAMatchingMember_ByNameOrValue_ShouldFindThatMemberAndReturnTrue(
        string nameOrValue, bool ignoreCase)
    {
        var parsed = Sport.TryParse(nameOrValue, out var trailRunning, ignoreCase);

        parsed.Should().BeTrue();
        trailRunning.Should().Be(Sport.TrailRunning);
    }

    [Fact]
    public void TryParse_GivenAValueThatDoesntMatchAMember_ByNameOrValue_ShouldReturnFalseAndSetMemberToNull()
    {
        var trailRunning = Sport.TrailRunning;

        var parsed = Sport.TryParse("TRAIL", out trailRunning, true);

        parsed.Should().BeFalse();
        trailRunning.Should().BeNull();
    }
}