using System;

namespace Common
{
    /// <summary>
    /// This interface is used with objects that have an identity that can be represented in a string.
    /// </summary>
    public interface IIdentifiable
    {
        #region Accessors
        String Identity { get; }
        #endregion /Accessors
    }
}
