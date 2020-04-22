namespace Braess.Model.Tools
{
    using System;
    using System.Collections.Generic;

    public static class Extension
    {
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
