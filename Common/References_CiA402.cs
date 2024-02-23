using Common.Constant;
using Common.Extensions;
using System;
using System.Collections.Generic;

namespace Common
{
    public static class References_CiA402
    {
        #region Access Type 
        private static readonly Dictionary<String, AccessRights> dictAccessTypeStr_AccessTypeEnum = new Dictionary<String, AccessRights>()
            {
                { Tokens.RO,  AccessRights.RO },     //M
                { Tokens.WO, AccessRights.WO},      //M
                { Tokens.RW, AccessRights.RW },     //M
                { Tokens.RWW, AccessRights.RWW },     //O
                { Tokens.RWR, AccessRights.RWR },     //M
                { Tokens.CONST, AccessRights.CONST },     //O
            };

        /// <summary>
        /// This method will attempt to decode the access type from a string. 
        /// It will retuen true if its found and loaded into the out parameter,
        /// else it will return false.
        /// </summary>
        /// <param name="accessTypeStr"></param>
        /// <param name="accessType"></param>
        /// <returns></returns>
        public static bool TryDecodeAccessTypeString(string accessTypeStr, out AccessRights accessType)
        {
            return dictAccessTypeStr_AccessTypeEnum.TryLookup(accessTypeStr, out accessType);
        }
        #endregion
    }
}
