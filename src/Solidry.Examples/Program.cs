using System;
using System.Diagnostics;
using Solidry.Examples.Aspects.WithProcessor;

namespace Solidry.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = new int[10_000];

            var random = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(1, 10000);
            }

            Console.WriteLine($"{array[0]} {array[1]} {array[2]}");

            var quickSort = new QuickSort(array.Length);

            var watch = Stopwatch.StartNew();

            var result = quickSort.DoQuickSort(array);

            watch.Stop();

            Console.WriteLine("QuickSort sort elements within {0} seconds.", watch.Elapsed.Seconds);

            Console.WriteLine($"{array[0]} {array[1]} {array[2]}");

            watch.Restart();

            Array.Sort(array);

            watch.Stop();

            Console.WriteLine($"{array[0]} {array[1]} {array[2]}");

            Console.WriteLine("Array.Sort sort elements within {0} seconds.", watch.Elapsed.Seconds);

            Console.ReadKey();
        }
    }
}
