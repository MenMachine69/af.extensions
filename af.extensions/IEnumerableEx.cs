using System;
using System.Collections.Generic;
using System.Linq;

namespace af.extensions
{
    public static class IEnumerableEx
    {
        /// <summary>
        /// Eine Aktion (Delegate) für jedes Element der Liste ausführen 
        /// 
        /// <example>
        /// <code>
        /// List&lt;User&gt; liste = new List&lt;User&gt;();
        /// ...
        /// liste.Foreach( l => Console.WriteLine(l.USR_NAME));
        /// </code></example>
        /// </summary>
        /// <typeparam name="T">Typ der in der Liste enthaltenen Objekte</typeparam>
        /// <param name="items">die Liste</param>
        /// <param name="action">auszuführende Aktion (Delegate)</param>
        public static void Foreach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
    }
}
