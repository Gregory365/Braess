namespace Braess.Model.Tools
{
    using System;
    using System.Collections.Generic;

    public static class Extension
    {
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> items)
        {
            if (list is null)
            {
                throw new ArgumentNullException("list");
            }

            if (items is null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        public static void RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match.Invoke(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }
    }
}
