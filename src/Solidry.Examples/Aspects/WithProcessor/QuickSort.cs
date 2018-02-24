using System;
using Solidry.Aspects;
using Solidry.Extensions;

namespace Solidry.Examples.Aspects.WithProcessor
{
    public class QuickSort : WithProcessor<int[], int[]>
    {
        public QuickSort(int capacity): base(capacity) { }

        public QuickSort() { }

        protected override bool FinishLoop(int[] context)
        {
            return context.Length == 0;
        }

        protected override int[] Process(int[] context)
        {
            if (context.Length == 1)
            {
                SetInput(Array.Empty<int>());

                return context;
            }

            var result = context.Min(3 > context.Length ? context.Length : 3);

            SetInput(result.Rest);

            return result.Min;
        }

        public int[] DoQuickSort(int[] input)
        {
            var result = Invoke(input);

            if (!result.HasValue)
            {
                return Array.Empty<int>();
            }

            return Accumulator.ToOne();
        }
    }
}