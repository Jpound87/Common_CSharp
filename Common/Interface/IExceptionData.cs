using System;

namespace Common.Interface
{
    public interface IExceptionData
    {
        #region Accessors
        String Name { get; }
        String Source { get; }
        String Message { get; }
        #endregion /Accessors
    }
}
