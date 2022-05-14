using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;

public class IsStringEnumTests
{
    public class Sport : StringEnum<Sport>
    {
    }

    public static IEnumerable<object?[]> IsStringEnumTestCases =>
        new[]
        {
            new object?[] { typeof(int), false },
            new object?[] { typeof(string), false },
            new object?[] { typeof(object), false },
            new object?[] { typeof(List<string>), false },
            new object?[] { typeof(StringEnum<Sport>), false },
            new object?[] { typeof(Sport), true },
        };

    [Theory]
    [MemberData(nameof(IsStringEnumTestCases))]
    public void IsStringEnum_GivenAType_ShouldCheckWhetherItIsAStringEnum(Type type, bool isStringEnum)
    {
        type.IsStringEnum().Should().Be(isStringEnum);
    }
}