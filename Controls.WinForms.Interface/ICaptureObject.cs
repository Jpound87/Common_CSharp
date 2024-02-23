using Common;
using System;

namespace Datam.WinForms.Interface
{
    public interface ICaptureObject : IIdentifiable, IValidate
    {
        #region Accessors
        uint ActiveVariables { get; }
        IDatamVariableCaptureData this[uint index] { get; }
        #endregion /Accessors

        #region Methods
        void SetTime(Double[] times);
        uint[] GetActiveIndicies();
        void UpdateVariableRelationship(Double plotAxisWeightFactor);
        #endregion /Methods
    }
}
