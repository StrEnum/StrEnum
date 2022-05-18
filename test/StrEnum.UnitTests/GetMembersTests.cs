using System;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;

public class GetMembersTests
{
    private class Sport : StringEnum<Sport>
    {
        public static readonly Sport TrailRunning = Define("TRAIL_RUNNING");
        public static readonly Sport RoadCycling = Define("ROAD_CYCLING");

        public static Sport Add(string name, string value)
        {
            return Define(value, name);
        }
    }

    [Fact]
    public void GetMembers_GivenAnEnum_ShouldListMembersInTheOrderOfDefinition()
    {
        var quidditch = Sport.Add("Quidditch", "QUIDDITCH");
        var podracing = Sport.Add("Podracing", "PODRACING");
        
        var members = Sport.GetMembers();

        members.Should().BeEquivalentTo(new[] { Sport.TrailRunning, Sport.RoadCycling, quidditch, podracing },
            o => o.WithStrictOrdering());
    }
}