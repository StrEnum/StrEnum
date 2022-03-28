using System;
using FluentAssertions;
using Xunit;

namespace StrEnum.UnitTests;


public class DefineTests
{
    private class ChannelWithTwoDifferentKeys : StringEnum<ChannelWithTwoDifferentKeys>
    {
        public static readonly ChannelWithTwoDifferentKeys Sms = Define("SMS");
        public static readonly ChannelWithTwoDifferentKeys Email = Define("Email");
    }

    [Fact]
    public void Define_GivenDifferentKeys_ShouldCreateEnum()
    {
        ChannelWithTwoDifferentKeys.Sms.ToString().Should().Be("SMS");
        ChannelWithTwoDifferentKeys.Email.ToString().Should().Be("Email");
    }

    private class ChannelWithAMemberValueOfNull : StringEnum<ChannelWithAMemberValueOfNull>
    {
        public static readonly ChannelWithAMemberValueOfNull Sms = Define(null);
    }

    [Fact]
    public void Define_GivenAValueOfNull_ShouldThrowAnException()
    {
        var accessMember = () => ChannelWithAMemberValueOfNull.Sms;
        
        accessMember.Should().Throw<TypeInitializationException>()
            .WithInnerException<ArgumentNullException>()
            .WithParameterName("value");
    }

    private class ChannelWithAMemberNameOfNull : StringEnum<ChannelWithAMemberNameOfNull>
    {
        public static readonly ChannelWithAMemberNameOfNull Sms = Define("SMS", null);
    }

    [Fact]
    public void Define_GivenANameOfNull_ShouldThrowAnException()
    {
        var accessMember = () => ChannelWithAMemberNameOfNull.Sms;

        accessMember.Should().Throw<TypeInitializationException>()
            .WithInnerException<ArgumentNullException>()
            .WithParameterName("name");
    }
}