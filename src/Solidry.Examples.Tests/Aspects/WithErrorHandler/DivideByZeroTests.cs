using System;
using Solidry.Examples.Aspects.WithErrorHandler;
using Xunit;

namespace Solidry.Examples.Tests.Aspects.WithErrorHandler
{
    public class DivideByZeroTests
    {
        [Fact]
        public void Should_Throw_Exception()
        {
            var d = new DivideByZero();

            Exception e = Assert.Throws<Exception>(() => d.Execute(1));

            Assert.Equal("Not handled exception.", e.Message);
        }
    }
}