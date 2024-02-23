using Common.Utility;

namespace Common.Extensions
{
    public static class Extensions_Random
    {
        #region Byte Array
        public static void FillByteArray_Random(this byte[] bytes)// This bytes.
        {
            Utility_Random.Random.NextBytes(bytes);
        }
        #endregion /Byte Array
    }
}
