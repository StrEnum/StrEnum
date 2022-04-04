using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;

public class EqualityTests
{
    public class Channel : StringEnum<Channel>
    {
        public static readonly Channel Sms = Define("SMS");
        public static readonly Channel Email = Define("EMAIL");
    }

    public class AnotherChannel : StringEnum<AnotherChannel>
    {
        public static readonly AnotherChannel Sms = Define("SMS");
    }

    public static IEnumerable<object?[]> EqualsTestCases =>
        new[]
        {
            new object?[] { Channel.Sms, Channel.Sms, true },
            new object?[] { Channel.Sms, Channel.Email, false },
            new object?[] { Channel.Sms, AnotherChannel.Sms, false },
            new object?[] { Channel.Sms, null, false},
        };

    [Theory]
    [MemberData(nameof(EqualsTestCases))]
    public void Equals_GivenTwoEnumMembers_ShouldConsiderThemEqualIfTheirTypesAndValuesAreEqual(object enum1,
        object? enum2, bool expectedEqual)
    {
        enum1.Equals(enum2).Should().Be(expectedEqual);
    }

    public static IEnumerable<object?[]> EqualityOperatorTestCases =>
        new[]
        {
            new object?[] { Channel.Sms, Channel.Sms, true },
            new object?[] { Channel.Sms, Channel.Email, false },
            new object?[] { Channel.Sms, null, false },
            new object?[] { null, Channel.Sms, false},
            new object?[] { null, null, true}
        };

    [Theory]
    [MemberData(nameof(EqualityOperatorTestCases))]
    public void InequalityOperator_GivenTwoEnumMembers_ShouldConsiderThemNotEqualIfTheirValuesAreDifferentAndTheyAreBothNotNull(Channel? enum1,
        Channel? enum2, bool expectedEqual)
    {
        var expectedDifferent = !expectedEqual;

        (enum1 != enum2).Should().Be(expectedDifferent);
    }

    [Fact]
    public void GetHashCode_ShouldReturnTheHashCodeOfTheMemberValue()
    {
        Channel.Sms.GetHashCode().Should().Be("SMS".GetHashCode());
    }
}