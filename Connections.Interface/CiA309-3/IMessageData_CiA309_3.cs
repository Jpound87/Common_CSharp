using Parameters.Interface;
using System;

namespace Connections.Interface.CiA309_3
{
    #region Delegtes
    public delegate bool ParameterProcessionEnqueue_Delegate(IMessageData_CiA309_3 messageData);
    #endregion /Delegtes

    public interface IMessageData_CiA309_3 : IMessageData, ISequencedTransmission, IEquatable<IMessageData_CiA309_3>
    {
        #region Parameter
        IParameter Parameter { get; }
        IAddress Address { get; }
        #endregion /Parameter
    }
}
