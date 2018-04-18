using Solidry.Extensions;
using Xunit;

namespace Solidry.Tests.Extensions.Analytics
{
    public class MaxTests
    {
        [Fact]
        public void Max_Should_Return_Max_Elements()
        {
            var array = new[] {1, 2, 8, 9, 1, 4, 5, 5};

            MaxResult<int> result = array.Max(3);

            Assert.True(result.Max.Length == 3);
            Assert.Equal(9, result.Max[0]);
            Assert.Equal(8, result.Max[1]);
            Assert.Equal(5, result.Max[2]);
        }
    }
}