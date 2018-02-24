using Solidry.Examples.Aspects.WithProcessor;
using Xunit;

namespace Solidry.Examples.Tests.Aspects.WithProcessor
{
    public class FibonacciTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(7)]
        public void Fibonacci_Should_Return_Ith_Element_Of_Row(int i)
        {
            var properlyRow = new[] {0, 1, 1, 2, 3, 5, 8, 13};

            var fibonacci = new Fibonacci();

            var result = fibonacci.Return(i);

            Assert.Equal(properlyRow[i], result);
        }

        [Fact]
        public void Fibonacci_Should_Return_Full_Row()
        {
            var properlyRow = new[] {0, 1, 1, 2, 3, 5, 8, 13};

            var fibonacci = new Fibonacci();

            var result = fibonacci.GetRow(properlyRow.Length -1);

            Assert.Equal(properlyRow, result);
        }
    }
}