namespace Devices.Interface
{

    #region Controlword
    public enum ControlwordState_CiA402 : byte
    {
        Unknown,
        Shutdown,
        SwitchedOn,
        SwitchOnEnableOperation,
        DisableVoltage,
        QuickStop,
        DisableOperation,
        EnableOperation,
        FaultReset
    }
    #endregion

    #region Statusword
    public enum StatuswordState_CiA402 : byte
    {
        NotReadyToSwitchOn,
        SwitchOnDisabled,
        ReadyToSwitchOn,
        SwitchedOn,
        OperationEnabled,
        QuickStopActive,
        FaultReactionActive,
        Fault
    }
    #endregion

}
