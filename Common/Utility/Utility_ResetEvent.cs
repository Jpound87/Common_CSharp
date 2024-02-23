using System.Threading;

namespace Common.Utility
{
    /// <summary>
    /// These creation classes for reset events are here as helper functions so they can be configured globally,
    /// should that prove helpful later. TODO: will that prove helpful later???
    /// </summary>
    public static class Utility_ResetEvent
    {
        #region Manual
        public static ManualResetEventSlim Create_ManualResetEventSlim(bool locked = false)
        {
            return new ManualResetEventSlim(!locked); 
        }

        public static ManualResetEvent Create_ManualResetEvent(bool locked = false)
        {
            return new ManualResetEvent(!locked);
        }
        #endregion /Manual

        #region Auto
        public static AutoResetEvent Create_AutoResetEvent(bool locked = false)
        {
            return new AutoResetEvent(!locked);
        }
        #endregion /Auto
    }
}
