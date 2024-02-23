using System;

namespace Common
{
    /// <summary>
    /// This interface is intended to be used on objects that have a valid state. 
    /// </summary>
    public interface IValidate
    {
        #region State
        Boolean Valid { get; }
        #endregion /State
    }
}
