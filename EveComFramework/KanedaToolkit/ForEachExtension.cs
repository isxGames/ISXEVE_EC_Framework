using System;
using System.Collections.Generic;
using System.Linq;

namespace EveComFramework.KanedaToolkit
{

    public static class ForEachExtension
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> method)
        {
            foreach (T item in items)
            {
                method(item);
            }
        }
    }

}
