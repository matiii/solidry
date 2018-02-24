using Solidry.Examples.Aspects.WithProcessor;
using Xunit;

namespace Solidry.Examples.Tests.Aspects.WithProcessor
{
    public class QuickSortTests
    {
        [Fact]
        public void QuickSort_Should_Sort_Array()
        {
            int[] source = {10, 15, 9, 8, 7, 3, 12, 2};
            int[] expected = {2, 3, 7, 8, 9, 10, 12, 15};

            var quickSort = new QuickSort(source.Length);

            var result = quickSort.DoQuickSort(source);

            Assert.Equal(expected, result);
        }
    }
}