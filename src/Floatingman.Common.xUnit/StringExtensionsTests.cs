using System;
using Xunit;
using Floatingman.Common;
using FluentAssertions;

namespace Floatingman.Common.Tests
{
  public class StringExtensionsTests
  {

    [Fact]
    public void DogTest()
    {
      true.Should().BeTrue();
    }

    [Theory]
    [InlineData("goodWork", "good-Work")]
    [InlineData("a", "a")]
    public void KabobWorksAsExpected(string input, string expected)
    {
      input.ToKabobCase().Should().Be(expected);
    }
  }
}
