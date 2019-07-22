using System;
using System.Collections.Generic;

namespace MyTestApp.ViewModels.Extensions
{
    public static class ListExtensions
    {
        private static Random randomizer = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int startIndex = list.Count;
            while (startIndex > 1)
            {
                startIndex--;
                int index = randomizer.Next(startIndex + 1);
                T value = list[index];
                list[index] = list[startIndex];
                list[startIndex] = value;
            }
        }
    }
}
