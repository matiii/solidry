using System;
using System.Collections.Generic;
using Solidry.Aspects;
using Solidry.Extensions;

namespace Solidry.Examples.Aspects.WithProcessor
{
    public class QuickSort: WithProcessor<int[], int[]>
    {
//        let rec quicksort2 = function
//        | [] -> []                         
//        | first::rest -> 
//        let smaller, larger = List.partition((>=) first) rest
//            List.concat [quicksort2 smaller;[first]; quicksort2 larger]
        protected override bool FinishLoop(int[] context)
        {
            return context.Length == 0;
        }

        protected override int[] Process(int[] context)
        {
            if (context.Length == 1)
            {
                return context;
            }

            var result = context.Min(2);

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

            var list = new List<int>(input.Length);

            Accumulator.Each(x => list.AddRange(x));

            return list.ToArray();
        }
    }
}