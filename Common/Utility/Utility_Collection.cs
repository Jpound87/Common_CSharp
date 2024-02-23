using Common.Constant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Utility
{
    public static class Utility_Collection
    {
        #region Identity 
        public static String ClassName = nameof(Utility_Collection);
        #endregion

        #region Take
        public static void TakeLast<T>(ref ICollection<T> collection, int numTaking)
        {
            numTaking = Math.Max(numTaking, collection.Count());
            IEnumerable<T> container = collection.Reverse().Take(numTaking);
            collection.Clear();
            foreach (T value in container)
            {
                collection.Prepend(value);
            }
        }
        #endregion

        #region Add
        public static CollectionOperationResult ParallelAdd<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection != null)
            {
                bool added = false;
                ConcurrentBag<T> dBag = new ConcurrentBag<T>();// its 'd' bag (pronouced da bag, not dee bag)
                Parallel.ForEach(items, (item) =>
                {
                    if (!collection.Contains(item))
                    {
                        dBag.Add(item);
                        added = true;
                    }
                });
                return added ? CollectionOperationResult.Succeeded : CollectionOperationResult.Duplicate;
            }
            return CollectionOperationResult.Failed;
        }
        #endregion /Add
    }
}
