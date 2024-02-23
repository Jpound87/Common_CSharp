using System.Collections.Generic;
using System.Linq;

namespace Common.Utility
{
    public static class Utility_Dictionary
    {
        #region Merge
        public static Dictionary<T1,T2> Merge<T1, T2>(this Dictionary<T1, T2> dictA, Dictionary<T1, T2> dictB) 
        {
            return dictA.Concat(dictB).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        #endregion /Merge
    }
}
