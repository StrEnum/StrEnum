using System;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;


public class ToStringTests
{
    public class ChannelWithOneKey : StringEnum<ChannelWithOneKey>
    {
        public static readonly ChannelWithOneKey Sms = Define("SMS");
    }

    [Fact]
    public void ToString_ShouldReturnTheMemberValue()
    {
        ChannelWithOneKey.Sms.ToString().Should().Be("SMS");
    }
}