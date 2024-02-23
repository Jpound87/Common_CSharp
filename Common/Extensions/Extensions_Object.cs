using System;

namespace Common.Extensions
{
    public static class Extensions_Object
    {
        #region Type Cast
        public static T CastAsType<T>(this Object castObject)
        {
#pragma warning disable IDE0018 // Inline variable declaration
            T outCast;
#pragma warning restore IDE0018 // Inline variable declaration
            TryCastAsType(castObject, out outCast);
            return outCast;
        }

        public static bool TryCastAsType<T>(this Object castObject, out T typeCast)
        {
            try
            {
                if (castObject is T castAsType)
                {
                    typeCast = castAsType;
                    return true;
                }
                else
                {
                    typeCast = default;
                }
            }
            catch
            {
                typeCast = default;
            }
            return false;
        }
        #endregion
    }
}
