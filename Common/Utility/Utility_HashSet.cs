using System;
using System.Collections.Generic;

namespace Common.Utility
{
    public static class Utility_HashSet
    {
        #region Identity
        public const string ClassName = nameof(Utility_HashSet);
        #endregion

        #region Create
        public static Func<HashSet<T>> Func_HashSet<T>()
        {
            return () => { return new HashSet<T>(); };
        }

        public static HashSet<T> CreateHashSet<T>()
        {
            return new HashSet<T>();
        }
        #endregion
    }
}
