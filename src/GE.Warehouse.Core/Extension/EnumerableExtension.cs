using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GE.Warehouse.Core.Extension
{
    public static class EnumerableExtension
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Random<T>(
           this IEnumerable<T> source, int count, bool allowDuplicates)
        {
            if (source == null) throw new ArgumentNullException("source");
            return RandomIterator<T>(source, count, -1, allowDuplicates);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Random<T>(
        this IEnumerable<T> source, int count, int seed,
           bool allowDuplicates)
        {
            if (source == null) throw new ArgumentNullException("source");
            return RandomIterator<T>(source, count, seed,
                allowDuplicates);
        }

        [DebuggerStepThrough]
        static IEnumerable<T> RandomIterator<T>(IEnumerable<T> source,
            int count, int seed, bool allowDuplicates)
        {

            // take a copy of the current list
            var buffer = new List<T>(source);

            // create the "random" generator, time dependent or with 
            // the specified seed
            Random random = seed < 0 ? new Random() : new Random(seed);

            count = Math.Min(count, buffer.Count);

            if (count > 0)
            {
                // iterate count times and "randomly" return one of the 
                // elements
                for (int i = 1; i <= count; i++)
                {
                    // maximum index actually buffer.Count -1 because 
                    // Random.Next will only return values LESS than 
                    // specified.
                    int randomIndex = random.Next(buffer.Count);
                    yield return buffer[randomIndex];
                    if (!allowDuplicates)
                        // remove the element so it can't be selected a 
                        // second time
                        buffer.RemoveAt(randomIndex);
                }
            }
        }
    }
}