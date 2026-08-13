using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;

public class ComparisonTests
{
    public class Sport : StringEnum<Sport>
    {
        public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");
        public static readonly Sport RoadCycling = Define("ROAD_CYCLING");
        public static readonly Sport MountainBike = Define("MOUNTAIN_BIKE");
    }

    public class SportWithAliasForMtb : StringEnum<SportWithAliasForMtb>
    {
        public static readonly SportWithAliasForMtb MountainBike = Define("MOUNTAIN_BIKE");
        public static readonly SportWithAliasForMtb MTB = Define("MOUNTAIN_BIKE");

        public static readonly SportWithAliasForMtb RoadCycling = Define("ROAD_CYCLING");
    }

    public class SportWithMixedCaseValues : StringEnum<SportWithMixedCaseValues>
    {
        public static readonly SportWithMixedCaseValues Bouldering = Define("BOULDERING");
        public static readonly SportWithMixedCaseValues Athletics = Define("athletics");
    }

    [Fact]
    public void StringEnum_ShouldImplementTheGenericComparableInterface()
    {
        typeof(Sport).Should().Implement<IComparable<Sport>>();
    }

    [Fact]
    public void StringEnum_ShouldImplementTheNonGenericComparableInterface()
    {
        typeof(Sport).Should().Implement<IComparable>();
    }

    public static IEnumerable<object?[]> CompareToTestCases =>
        new[]
        {
            new object?[] { Sport.MountainBike, Sport.RoadCycling, -1 },
            new object?[] { Sport.RoadCycling, Sport.MountainBike, 1 },
            new object?[] { Sport.RoadCycling, Sport.RoadCycling, 0 },
            new object?[] { Sport.RoadCycling, null, 1 },
        };

    [Theory]
    [MemberData(nameof(CompareToTestCases))]
    public void CompareTo_GivenTwoEnumMembers_ShouldOrderThemByTheirValues(Sport enum1, Sport? enum2, int expectedSign)
    {
        Math.Sign(enum1.CompareTo(enum2)).Should().Be(expectedSign);
    }

    public static IEnumerable<object[]> MemberPairs =>
        from enum1 in SportWithAliasForMtb.GetMembers()
        from enum2 in SportWithAliasForMtb.GetMembers()
        select new object[] { enum1, enum2 };

    [Theory]
    [MemberData(nameof(MemberPairs))]
    public void CompareTo_GivenTwoEnumMembers_ShouldConsiderThemEquallyOrderedIfAndOnlyIfTheyAreEqual(SportWithAliasForMtb enum1,
        SportWithAliasForMtb enum2)
    {
        var comparedEqual = enum1.CompareTo(enum2) == 0;

        comparedEqual.Should().Be(enum1.Equals(enum2));
    }

    [Theory]
    [InlineData("")]
    [InlineData("da-DK")]
    public void CompareTo_GivenTwoEnumMembersWhoseValuesDifferInCase_ShouldOrderThemOrdinallyRegardlessOfTheCurrentCulture(string culture)
    {
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);

            SportWithMixedCaseValues.Bouldering.CompareTo(SportWithMixedCaseValues.Athletics).Should().BeNegative();
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    public static IEnumerable<object?[]> NonGenericCompareToTestCases =>
        new[]
        {
            new object?[] { Sport.MountainBike, 1 },
            new object?[] { Sport.TrailRunning, -1 },
            new object?[] { Sport.RoadCycling, 0 },
            new object?[] { null, 1 },
        };

    [Theory]
    [MemberData(nameof(NonGenericCompareToTestCases))]
    public void NonGenericCompareTo_GivenAnEnumMemberOrNull_ShouldOrderThemByTheirValues(object? obj, int expectedSign)
    {
        var comparable = (IComparable)Sport.RoadCycling;

        Math.Sign(comparable.CompareTo(obj)).Should().Be(expectedSign);
    }

    [Fact]
    public void NonGenericCompareTo_GivenAnObjectOfAnotherType_ShouldThrowAnException()
    {
        var comparable = (IComparable)Sport.RoadCycling;

        Action compare = () => comparable.CompareTo("ROAD_CYCLING");

        compare.Should().Throw<ArgumentException>()
            .WithMessage("Object must be of type Sport.");
    }

    [Fact]
    public void ComparisonOperators_GivenTwoEnumMembers_ShouldOrderThemByTheirValues()
    {
        (Sport.MountainBike < Sport.RoadCycling).Should().BeTrue();
        (Sport.MountainBike <= Sport.RoadCycling).Should().BeTrue();
        (Sport.TrailRunning > Sport.RoadCycling).Should().BeTrue();
        (Sport.TrailRunning >= Sport.RoadCycling).Should().BeTrue();
    }

    [Fact]
    public void ComparisonOperators_GivenANullMember_ShouldOrderItBeforeAnyOtherMember()
    {
        var noSport = (Sport?)null;

        (noSport < Sport.RoadCycling).Should().BeTrue();
        (noSport > Sport.RoadCycling).Should().BeFalse();
    }

    [Fact]
    public void DefaultComparer_GivenTwoEnumMembers_ShouldOrderThemByTheirValues()
    {
        Comparer<Sport>.Default.Compare(Sport.MountainBike, Sport.RoadCycling).Should().BeNegative();
    }

    [Fact]
    public void OrderBy_GivenTheEnumMembers_ShouldOrderThemByTheirValues()
    {
        var orderedMembers = Sport.GetMembers().OrderBy(sport => sport);

        orderedMembers.Should().Equal(Sport.MountainBike, Sport.RoadCycling, Sport.TrailRunning);
    }

    [Fact]
    public void SortedSet_GivenTheEnumMembers_ShouldOrderThemByTheirValues()
    {
        var sortedMembers = new SortedSet<Sport>(Sport.GetMembers());

        sortedMembers.Should().Equal(Sport.MountainBike, Sport.RoadCycling, Sport.TrailRunning);
    }

    [Fact]
    public void SortedDictionary_GivenTheEnumMembersAsKeys_ShouldOrderThemByTheirValues()
    {
        var sortedMembers = new SortedDictionary<Sport, string>
        {
            { Sport.TrailRunning, "Trail Running" },
            { Sport.RoadCycling, "Road Cycling" },
            { Sport.MountainBike, "Mountain Bike" }
        };

        sortedMembers.Keys.Should().Equal(Sport.MountainBike, Sport.RoadCycling, Sport.TrailRunning);
    }
}
