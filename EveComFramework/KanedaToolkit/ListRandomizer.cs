#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;

namespace EveComFramework.KanedaToolkit
{

    public static class ListRandomizer
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            Random rnd = new Random();
            return source.OrderBy((item) => rnd.Next());
        }
    }

}
