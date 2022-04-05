using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;

public class EqualityTests
{
    public class TropicalSeason : StringEnum<TropicalSeason>
    {
        public static readonly TropicalSeason Wet = Define("WET");
        public static readonly TropicalSeason Dry = Define("DRY");
    }

    public class CleaningType : StringEnum<CleaningType>
    {
        public static readonly CleaningType Wet = Define("WET");
        public static readonly CleaningType Dry = Define("DRY");
    }

    public static IEnumerable<object?[]> EqualsTestCases =>
        new[]
        {
            new object?[] { TropicalSeason.Wet, TropicalSeason.Wet, true },
            new object?[] { TropicalSeason.Wet, TropicalSeason.Dry, false },
            new object?[] { TropicalSeason.Wet, CleaningType.Wet, false },
            new object?[] { TropicalSeason.Wet, null, false},
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
            new object?[] { TropicalSeason.Wet, TropicalSeason.Wet, true },
            new object?[] { TropicalSeason.Wet, TropicalSeason.Dry, false },
            new object?[] { TropicalSeason.Wet, null, false },
            new object?[] { null, TropicalSeason.Wet, false},
            new object?[] { null, null, true}
        };

    [Theory]
    [MemberData(nameof(EqualityOperatorTestCases))]
    public void InequalityOperator_GivenTwoEnumMembers_ShouldConsiderThemNotEqualIfTheirValuesAreDifferentAndTheyAreBothNotNull(TropicalSeason? enum1,
        TropicalSeason? enum2, bool expectedEqual)
    {
        var expectedDifferent = !expectedEqual;

        (enum1 != enum2).Should().Be(expectedDifferent);
    }

    [Fact]
    public void GetHashCode_ShouldReturnTheHashCodeOfTheMemberValue()
    {
        TropicalSeason.Wet.GetHashCode().Should().Be("WET".GetHashCode());
    }
}