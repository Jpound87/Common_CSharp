using Common.Constant;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class References_Allied
    {
        #region Capture State
        public static readonly IDictionary<string, CaptureState_Device> dictCaptureStateStr_CaptureStateEnum = 
            new Dictionary<string, CaptureState_Device>()
            {
                {"0", CaptureState_Device.INIT },
                {"1", CaptureState_Device.IDLE },
                {"2", CaptureState_Device.CHECK_AND_SETUP },
                {"5", CaptureState_Device.BUFFER },
                {"7", CaptureState_Device.DUMMY_TRIGGER_READ },
                {"10", CaptureState_Device.CHECK_TRIGGER },
                {"15", CaptureState_Device.START_COLLECTING },
                {"18", CaptureState_Device.COLLECTING },
                {"20", CaptureState_Device.DONE },
                {"30", CaptureState_Device.ERROR }
            };

        public static bool TryDecodeCaptureStateStr(string captureStateStr, out CaptureState_Device captureStateEnum)
        {
            return dictCaptureStateStr_CaptureStateEnum.TryLookup(captureStateStr, out captureStateEnum);
        }
        #endregion

        #region Trigger Mode
        public static readonly IDictionary<TriggerMode, string> dictTriggerModeEnum_TriggerModeStr = 
            new Dictionary<TriggerMode, string>()
            {
                {TriggerMode.SINGLE, "0" },
                {TriggerMode.ABOVE, "1" },
                {TriggerMode.BELOW, "2" },
                {TriggerMode.OUTSIDE, "3" },
                {TriggerMode.WITHIN, "4" }
            };

        public static readonly IDictionary<string, TriggerMode> dictTriggerModeStr_TriggerModeEnum =
            dictTriggerModeEnum_TriggerModeStr.ToDictionary(x => x.Value, x => x.Key);// Invert the pervious

        public static readonly IDictionary<TriggerMode, string> dictTriggerModeEnum_TriggerModeNameStr = 
            new Dictionary<TriggerMode, string>()
            {
                {TriggerMode.SINGLE, "Immediately" },
                {TriggerMode.ABOVE, "T>=P1" },
                {TriggerMode.BELOW, "T<=P1" },
                {TriggerMode.OUTSIDE, "Outside P1 and P2" },
                {TriggerMode.WITHIN, "Within P1 and P2" }
            };


     
        public static bool TryDecodeTriggerModeStr(string triggerModeStr, out TriggerMode triggerModeEnum)
        {
            return dictTriggerModeStr_TriggerModeEnum.TryLookup(triggerModeStr, out triggerModeEnum);
        }
        #endregion

        #region Fault Codes
        public static readonly IDictionary<String, FaultCode> dictFaultString_FaultEnum = 
            new Dictionary<String, FaultCode>()
            {
                //MAKE SURE HEX LETTERS (A-Z) ARE LOWERCASE OR IT WILL NOT TRANSLATE
                #region codes
                { "0x0000", FaultCode.No_Fault },
                { "0x2110", FaultCode.Short_Circuit_Earth_leakage_Input },
                #region Earth Leak Input
                { "0x2120", FaultCode.Earth_Leakage_Input },
                { "0x2121", FaultCode.Earth_Leakage_Phase_1 },
                { "0x2122", FaultCode.Earth_Leakage_Phase_2 },
                { "0x2123", FaultCode.Earth_Leakage_Phase_3 },
                #endregion
                #region Short Circuit Input
                { "0x2130", FaultCode.Short_Circuit_Input },
                { "0x2131", FaultCode.Short_Circuit_Phase_1_2 },
                { "0x2132", FaultCode.Short_Circuit_Phase_2_3 },
                { "0x2133", FaultCode.Short_Circuit_Phase_3_1 },
                { "0x2211", FaultCode.Internal_Current_1 },
                { "0x2212", FaultCode.Internal_Current_2 },
                { "0x2213", FaultCode.Over_Current_Ramp_Function },
                { "0x2214", FaultCode.Over_Current_Sequence },
                #endregion
                #region Internal Over Current
                { "0x2220", FaultCode.Continious_Over_Current_Internal },
                { "0x2221", FaultCode.Continious_Over_Current_Internal_1 },
                { "0x2222", FaultCode.Continious_Over_Current_Internal_2 },
                #endregion
                { "0x2230", FaultCode.Short_Circuit_Earth_leakage_Internal },
                { "0x2240", FaultCode.Earth_Leakage_Internal },
                { "0x2250", FaultCode.Short_Circuit_Internal },
                #region Cont Over Current Internal
                { "0x2310", FaultCode.Continious_Over_Current_Output },
                { "0x2311", FaultCode.Continious_Over_Current_Output_1 },
                { "0x2312", FaultCode.Continious_Over_Current_Output_2 },
                #endregion
                { "0x2320", FaultCode.Short_Circuit_Earth_leakage_Motor },
                #region Earth LEak Motor
                { "0x2330", FaultCode.Earth_Leakage_Motor },
                { "0x2331", FaultCode.Earth_Leakage_Motor_Phase_U },
                { "0x2332", FaultCode.Earth_Leakage_Motor_Phase_V },
                { "0x2333", FaultCode.Earth_Leakage_Motor_Phase_W },
                #endregion
                #region Sort Circuit Motor
                { "0x2340", FaultCode.Short_Circuit_Motor },
                { "0x2341", FaultCode.Short_Circuit_U_V },
                { "0x2342", FaultCode.Short_Circuit_V_W },
                { "0x2343", FaultCode.Short_Circuit_W_U },
                #endregion
                { "0x2350", FaultCode.Load_Level_Fault_Thermal_State },
                { "0x2351", FaultCode.Load_Level_Warning_Thermal_State },
                #region Main Over Voltage
                { "0x3110", FaultCode.Mains_Over_Voltage },
                { "0x3111", FaultCode.Mains_Over_Voltage_Phase_1 },
                { "0x3112", FaultCode.Mains_Over_Voltage_Phase_2 },
                { "0x3113", FaultCode.Mains_Over_Voltage_Phase_3 },
                #endregion
                #region Mains Under Voltage
                { "0x3120", FaultCode.Mains_Under_Voltage },
                { "0x3121", FaultCode.Mains_Under_Voltage_Phase_1 },
                { "0x3122", FaultCode.Mains_Under_Voltage_Phase_2 },
                { "0x3123", FaultCode.Mains_Under_Voltage_Phase_3 },
                #endregion
                #region Phase Failure
                { "0x3130", FaultCode.Phase_Failure },
                { "0x3131", FaultCode.Phase_Failure_1 },
                { "0x3132", FaultCode.Phase_Failure_2 },
                { "0x3133", FaultCode.Phase_Failure_3 },
                { "0x3134", FaultCode.Phase_Sequence },
                #endregion
                #region Mains Frequency
                { "0x3140", FaultCode.Mains_Frequency },
                { "0x3141", FaultCode.Mains_Frequency_Too_Great },
                { "0x3142", FaultCode.Mains_Frequency_Too_Small },
                #endregion
                #region DC Link Over Voltage
                { "0x3210", FaultCode.DC_Link_Over_Voltage },
                { "0x3211", FaultCode.Over_Voltage_1 },
                { "0x3212", FaultCode.Over_Voltage_2 },
                #endregion
                #region DC Link Under Voltage
                { "0x3220", FaultCode.DC_Link_Under_Voltage },
                { "0x3221", FaultCode.Under_Voltage_1 },
                { "0x3222", FaultCode.Under_Voltage_2 },
                #endregion 
                { "0x3230", FaultCode.Load_Error },
                #region Output Over Voltage
                { "0x3310", FaultCode.Output_Over_Voltage },
                { "0x3311", FaultCode.Output_Over_Voltage_U },
                { "0x3312", FaultCode.Output_Over_Voltage_V },
                { "0x3313", FaultCode.Output_Over_Voltage_W },
                #endregion
                #region Armature Circuit
                { "0x3320", FaultCode.Armature_Circuit },
                { "0x3321", FaultCode.Armature_Circuit_Interrupted },
                #endregion
                #region Field Circuit 
                { "0x3330", FaultCode.Field_Circuit },
                { "0x3331", FaultCode.Field_Circuit_Interrupted },
                { "0x4110", FaultCode.Excess_Ambient_Temperature },
                { "0x4120", FaultCode.Too_Low_Ambient_Temperature },
                { "0x4130", FaultCode.Temperature_Air_Supply },
                { "0x4140", FaultCode.Temperature_Air_Outlet },
                { "0x4210", FaultCode.Excess_Temperature_Device },
                { "0x4220", FaultCode.Too_Low_Temperature_Device },
                #endregion
                #region Temp Of Drive
                { "0x4300", FaultCode.Temperature_Drive },
                { "0x4310", FaultCode.Excess_Temperature_Drive },
                { "0x4320", FaultCode.Fault_Too_Low_Temperature_Drive },
                { "0x4381", FaultCode.Warning_Drive_Temp_Limit },///Added
                { "0x4383", FaultCode.Fault_Drive_Temp_Sensor_Fail },
                { "0x4386", FaultCode.Fault_Motor_Over_Temperature },
                { "0x4387", FaultCode.Warning_Motor_Temp_Limit },
                { "0x4388", FaultCode.Fault_Too_Low_Temperature_Drive },
                { "0x4389", FaultCode.Fault_Motor_Temp_Sensor_Fail },///
                #endregion
                #region Temp of Supply
                { "0x4400", FaultCode.Temperature_Supply },
                { "0x4410", FaultCode.Excess_Temperature_Supply },
                { "0x4420", FaultCode.Too_Low_Temperature_Supply },
                #endregion
                { "0x5100", FaultCode.Supply_Device_Hardware },
                #region Supply Low Voltage
                { "0x5110", FaultCode.Supply_Low_Voltage },
                { "0x5111", FaultCode.Supply_Is_15 },
                { "0x5112", FaultCode.Supply_Is_24 },
                { "0x5113", FaultCode.Supply_Is_5 },
                //0x5114 to 0x5119 is manufacturer specific
                #endregion
                { "0x5120", FaultCode.Supply_Intermediate_Circuit },
                #region Control Device Hardware 
                { "0x5200", FaultCode.Control_Device_hardware },
                { "0x5210", FaultCode.Measurement_CircuitOne },
                { "0x5220", FaultCode.Computing_Circuit },
                #endregion
                { "0x5300", FaultCode.Operating_Unit },
                { "0x5400", FaultCode.Power_Section },
                { "0x5410", FaultCode.Output_Stages },
                { "0x5420", FaultCode.Chopper },
                { "0x5430", FaultCode.Input_Stages },
                #region Contacts
                { "0x5440" , FaultCode.Contacts },
                //Contact 1-5 are 0x5441 to 0x5445 and are manufacture specific
                #endregion
                #region Fuses
                { "0x5450", FaultCode.Fuses },
                { "0x5451", FaultCode.S_1_Is_L_1 },
                { "0x5452", FaultCode.S_2_Is_L_2 },
                { "0x5453", FaultCode.S_3_Is_L_3 },
                //S_$ to S_9 are 0x5454 to 0x5459 and are manufacture specific
                #endregion
                #region STO
                { "0x5480", FaultCode.Fault_STO_Low },///Added
                #endregion
                #region Hardware Memory
                { "0x5500", FaultCode.Hardware_Memory },
                { "0x5510", FaultCode.RAM },
                { "0x5520", FaultCode.ROMorEEPROM },
                { "0x5530", FaultCode.EEPROM },
                #endregion
                #region Software Reset Watchdog
                { "0x6010", FaultCode.Software_Reset },
                { "0x6301", FaultCode.DataRecordNo1 },
                { "0x6302", FaultCode.DataRecordNo2 },
                { "0x6303", FaultCode.DataRecordNo3 },
                { "0x6304", FaultCode.DataRecordNo4 },
                { "0x6305", FaultCode.DataRecordNo5 },
                { "0x6306", FaultCode.DataRecordNo6 },
                { "0x6307", FaultCode.DataRecordNo7 },
                { "0x6308", FaultCode.DataRecordNo8 },
                { "0x6309", FaultCode.DataRecordNo9 },
                { "0x630a", FaultCode.DataRecordNo10 },
                { "0x630b", FaultCode.DataRecordNo11 },
                { "0x630c", FaultCode.DataRecordNo12 },
                { "0x630d", FaultCode.DataRecordNo13 },
                { "0x630e", FaultCode.DataRecordNo14 },
                { "0x630f", FaultCode.DataRecordNo15 },
                #endregion
                { "0x6310", FaultCode.Loss_Of_Parameters },
                { "0x6320", FaultCode.Parameter_Error },
                { "0x7100", FaultCode.Power_Additional_Modules },
                #region Brake Chopper
                { "0x7110", FaultCode.Brake_Chopper },
                { "0x7111", FaultCode.Failure_Brake_Chopper },
                { "0x7112", FaultCode.Over_Current_Brake_Chopper },
                { "0x7113", FaultCode.Protective_Circuit_Brake_Chopper },
                #endregion
                #region Motor
                { "0x7120", FaultCode.Motor },
                { "0x7121", FaultCode.Motor_Blocked },
                { "0x7122", FaultCode.Motor_Error_Or_Comm_Mal },
                { "0x7123", FaultCode.Motor_Tilted },
                #endregion
                { "0x7200", FaultCode.Measurement_CircuitTwo },
                #region Sensor
                { "0x7300", FaultCode.Sensor },
                { "0x7301", FaultCode.Tacho_Fault },
                { "0x7302", FaultCode.Tacho_Wrong_Polarity },
                { "0x7303", FaultCode.Resolver_1_Fault },
                { "0x7304", FaultCode.Resolver_2_Fault },
                { "0x7305", FaultCode.Incremental_Sensor_1_Fault },
                { "0x7306", FaultCode.Incremental_Sensor_2_Fault },
                { "0x7307", FaultCode.Incremental_Sensor_3_Fault },
                { "0x7310", FaultCode.Speed },
                { "0x7320", FaultCode.Position },
                #endregion
                { "0x7400", FaultCode.Computation_Circuit },
                #region Communication
                { "0x7500", FaultCode.Communication },
                { "0x7510", FaultCode.Serial_1 },
                { "0x7520", FaultCode.Serial_2 }, 
                #endregion
                { "0x7600", FaultCode.Data_Storage },
                { "0x7681", FaultCode.Fault_Data_Storage_Manufacture_Label_Read },///Added
                { "0x7682", FaultCode.Fault_Data_Storage_Manufacture_Label_Write },
                { "0x7683", FaultCode.Fault_Data_Storage_Manufacture_Label_CRC },
                { "0x7684", FaultCode.Fault_Data_Storage_Datalog_Read },
                { "0x7685", FaultCode.Fault_Data_Storage_Datalog_Write },
                { "0x7686", FaultCode.Fault_Data_Storage_Datalog_CRC },
                { "0x7687", FaultCode.Fault_Data_Storage_Config_Parameter_Read },
                { "0x7688", FaultCode.Fault_Data_Storage_Config_Parameter_Write },
                { "0x7689", FaultCode.Fault_Data_Storage_Config_Parameter_Mismatch },///
                #region Torque Control
                { "0x8300", FaultCode.Torque_Control },
                { "0x8311", FaultCode.Excess_Torque },
                { "0x8312", FaultCode.Difficult_Startup },
                { "0x8313", FaultCode.Standstill_Torque },
                { "0x8321", FaultCode.Insufficient_Torque },
                { "0x8331", FaultCode.Torque_Fault },
                #endregion
                { "0x8400", FaultCode.Velocity_Speed_Controler },
                { "0x8500", FaultCode.Position_Controller },
                #region Positioning Controller
                { "0x8600", FaultCode.Positioning_Controller },
                { "0x8611", FaultCode.Following_Error },
                { "0x8612", FaultCode.Reference_Limit },
                { "0x8613", FaultCode.Homing_Error },
                #endregion
                { "0x8700", FaultCode.Sync_Controller },
                { "0x8800", FaultCode.Winding_Controller },
                { "0x8900", FaultCode.Process_Data_Monitoring },
                #region Control Monitoring 
                { "0x8a00", FaultCode.Control_Monitoring },
                { "0xf001", FaultCode.Deceleration },
                { "0xf002", FaultCode.Subsync_Run },
                { "0xf003", FaultCode.Stroke_Operation },
                { "0xf004", FaultCode.Control_Additional_Functions },
            
                #endregion
                { "0xf401", FaultCode.The_EF_special },
                { "0xf406", FaultCode.The_NM_special },
                { "0xff20", FaultCode.BAD_GEAR_RATIO  }, 
                { "0xff30", FaultCode.DS402_FAULT_CAPTURE_GENERIC  }, // was 0xF400
                { "0xff31", FaultCode.DS402_FAULT_OVER_CAPDEPTH }, // was 0xF401
                { "0xff32", FaultCode.DS402_FAULT_PERIOD_NOT_SUPPORTED }, // was 0xF402
                { "0xff33", FaultCode.DS402_FAULT_TRIGGER_POSITION }, // was 0xF403
                { "0xff34", FaultCode.DS402_FAULT_TRIGGER_NOT_SUPPORTED }, // was 0xF404
                { "0xff35", FaultCode.DS402_FAULT_ADDRESS_1_NOT_SUPPORTED }, // was 0xF405
                { "0xff36", FaultCode.DS402_FAULT_ADDRESS_2_NOT_SUPPORTED },// was 0xF406
                { "0xff37", FaultCode.DS402_FAULT_ADDRESS_3_NOT_SUPPORTED }, // was 0xF407
                { "0xff38", FaultCode.DS402_FAULT_ADDRESS_4_NOT_SUPPORTED },// was 0xF408
                { "0xff48", FaultCode.Warning_History_Already_Clear }
                #endregion
            };

        public static readonly IDictionary<FaultCode, String> dictFaultEnum_FaultDescString =
            new Dictionary<FaultCode, String>()
            {
                #region codes
                { FaultCode.Unknown,"Unkown error, please contact manufacturer." },
                { FaultCode.NM_Unspecified_Error,"NM Fix" },
                { FaultCode.No_Fault, "No Fault" },
                { FaultCode.Short_Circuit_Earth_leakage_Input, "Short circuit/earth leakage (input)" },
                #region Earth Leak Input
                { FaultCode.Earth_Leakage_Input, "Earth leakage (input)" },
                { FaultCode.Earth_Leakage_Phase_1, "Earth leakage phase L1" },
                { FaultCode.Earth_Leakage_Phase_2, "Earth leakage phase L2" },
                { FaultCode.Earth_Leakage_Phase_3, "Earth leakage phase L3" },
                #endregion
                #region Short Circuit Input
                { FaultCode.Short_Circuit_Input, "Fault on Address 2 'Not Supported'" },
                { FaultCode.Short_Circuit_Phase_1_2, "Short circuit phases L1-L2" },
                { FaultCode.Short_Circuit_Phase_2_3, "Short circuit phases L2-L3" },
                { FaultCode.Short_Circuit_Phase_3_1, "Short circuit phases L3-L1" },
                { FaultCode.Internal_Current_1, "Internal current no.1" },
                { FaultCode.Internal_Current_2, "Internal current no.2" },
                { FaultCode.Over_Current_Ramp_Function, "Over-current in ramp function" },
                { FaultCode.Over_Current_Sequence, "Over-current in the sequence" },
                #endregion
                #region Internal Over Current
                { FaultCode.Continious_Over_Current_Internal, "Continuous over current (device internal)" },
                { FaultCode.Continious_Over_Current_Internal_1, "Continuous over current no.1 (device internal)" },
                { FaultCode.Continious_Over_Current_Internal_2, "Continuous over current no.2 (device internal)" },
                #endregion
                { FaultCode.Short_Circuit_Earth_leakage_Internal, "Short circuit/earth leakage (device internal)" },
                { FaultCode.Earth_Leakage_Internal, "Earth leakage (device internal)" },
                { FaultCode.Short_Circuit_Internal, "Short circuit (device internal)" },
                #region Cont Over Current Internal
                { FaultCode.Continious_Over_Current_Output, "Continuous over current (device output side)" },
                { FaultCode.Continious_Over_Current_Output_1, "Continuous over current no.1 (device output side)" },
                { FaultCode.Continious_Over_Current_Output_2, "Continuous over current no.2 (device output side)" }, 
                #endregion
                { FaultCode.Short_Circuit_Earth_leakage_Motor, "Short circuit/earth leakage (motor-side)" },
                #region Earth Leak Motor
                { FaultCode.Earth_Leakage_Motor, "Earth leakage (motor-side)" },
                { FaultCode.Earth_Leakage_Motor_Phase_U, "Earth leakage phase U" },
                { FaultCode.Earth_Leakage_Motor_Phase_V, "Earth leakage phase V" },
                { FaultCode.Earth_Leakage_Motor_Phase_W, "Earth leakage phase W" },
                #endregion
                #region Sort Circuit Motor
                { FaultCode.Short_Circuit_Motor, "Short circuit (motor-side)" },
                { FaultCode.Short_Circuit_U_V, "Short circuit phases U-V" },
                { FaultCode.Short_Circuit_V_W, "Earth leakage phase V-W" },
                { FaultCode.Short_Circuit_W_U, "Earth leakage phase W-U" },
                #endregion
                { FaultCode.Load_Level_Fault_Thermal_State, "Load level fault (I2t, thermal state)" },
                { FaultCode.Load_Level_Warning_Thermal_State, "Load level warning (I2t, thermal state)" },
                #region Main Over Voltage
                { FaultCode.Mains_Over_Voltage, "Mains over-voltage" },
                { FaultCode.Mains_Over_Voltage_Phase_1, "Mains over-voltage phase L1" },
                { FaultCode.Mains_Over_Voltage_Phase_2, "Mains over-voltage phase L2" },
                { FaultCode.Mains_Over_Voltage_Phase_3, "Mains over-voltage phase L3" },
                #endregion
                #region Mains Under Voltage
                { FaultCode.Mains_Under_Voltage, "Mains under-voltage" },
                { FaultCode.Mains_Under_Voltage_Phase_1, "Mains under-voltage phase L1" },
                { FaultCode.Mains_Under_Voltage_Phase_2, "Mains under-voltage phase L2" },
                { FaultCode.Mains_Under_Voltage_Phase_3, "Mains under-voltage phase L3" },
                #endregion
                #region Phase Failure
                { FaultCode.Phase_Failure, "Phase failure" },
                { FaultCode.Phase_Failure_1, "Phase failure L1" },
                { FaultCode.Phase_Failure_2, "Phase failure L2" },
                { FaultCode.Phase_Failure_3, "Phase failure L3" },
                { FaultCode.Phase_Sequence, "Phase sequence" },
                #endregion
                #region Mains Frequency
                { FaultCode.Mains_Frequency, "Mains frequency" },
                { FaultCode.Mains_Frequency_Too_Great, "Mains frequency too great" },
                { FaultCode.Mains_Frequency_Too_Small, "Mains frequency too small" },
                #endregion
                #region DC Link Over Voltage
                { FaultCode.DC_Link_Over_Voltage, "DC link over-voltage" },
                { FaultCode.Over_Voltage_1, "Over-voltage no. 1" },
                { FaultCode.Over_Voltage_2, "Over voltage no. 2" },
                #endregion
                #region DC Link Under Voltage
                { FaultCode.DC_Link_Under_Voltage, "DC link under-voltage" },
                { FaultCode.Under_Voltage_1, "Under-voltage no. 1" },
                { FaultCode.Under_Voltage_2, "Under-voltage no. 2" },
                #endregion
                { FaultCode.Load_Error, "Load error" },
                #region Output Over Voltage
                { FaultCode.Output_Over_Voltage, "Output over-voltage" },
                { FaultCode.Output_Over_Voltage_U, "Output over-voltage phase U" },
                { FaultCode.Output_Over_Voltage_V, "Output over-voltage phase V" },
                { FaultCode.Output_Over_Voltage_W, "Output over-voltage phase W" },
                #endregion
                #region Armature Circuit
                { FaultCode.Armature_Circuit, "Armature circuit" },
                { FaultCode.Armature_Circuit_Interrupted, "Armature circuit interrupted" },
                #endregion
                #region Field Circuit 
                { FaultCode.Field_Circuit, "Field circuit" },
                { FaultCode.Field_Circuit_Interrupted, "Field circuit interrupted" },
                { FaultCode.Excess_Ambient_Temperature, "Excess ambient temperature" },
                { FaultCode.Too_Low_Ambient_Temperature, "Too low ambient temperature" },
                { FaultCode.Temperature_Air_Supply, "Temperature supply air" },
                { FaultCode.Temperature_Air_Outlet, "Temperature air outlet" },
                { FaultCode.Excess_Temperature_Device, "Excess temperature device" },
                { FaultCode.Too_Low_Temperature_Device, "Too low temperature device" },
                #endregion
                #region Temp Of Drive
                { FaultCode.Temperature_Drive, "Temperature drive" },
                { FaultCode.Excess_Temperature_Drive, "Excess temperature drive" },
                { FaultCode.Fault_Too_Low_Temperature_Drive, "Too low temperature drive" },
                { FaultCode.Warning_Drive_Temp_Limit, "Drive is approacing the safe temperature limit" },///Added
                { FaultCode.Fault_Drive_Temp_Sensor_Fail, "Drive temperature sensor failure" },
                { FaultCode.Fault_Motor_Over_Temperature, "Motor over temperature limit" },
                { FaultCode.Warning_Motor_Temp_Limit, "Motor approaching the safe temperature limit" },
                { FaultCode.Fault_Too_Low_Temperature_Motor, "Too low temperature motor"  },
                { FaultCode.Fault_Motor_Temp_Sensor_Fail, "Motor temperatere sensor failure" },///
                #endregion
                #region Temp of Supply
                { FaultCode.Temperature_Supply, "Temperature supply" },
                { FaultCode.Excess_Temperature_Supply, "Excess temperature supply" },
                { FaultCode.Too_Low_Temperature_Supply, "Too low temperature supply" },
                #endregion
                { FaultCode.Supply_Device_Hardware, "Supply device hardware" },
                #region Supply Low Voltage
                { FaultCode.Supply_Low_Voltage, "Supply low voltage" },
                { FaultCode.Supply_Is_15, "U1 = supply ±15 V" },
                { FaultCode.Supply_Is_24, "U2 = supply +24 V" },
                { FaultCode.Supply_Is_5, "U3 = supply +5 V" },
                //0x5114 to 0x5119 is manufacturer specific
                #endregion
                { FaultCode.Supply_Intermediate_Circuit, "Supply intermediate circuit" },
                #region Control Device Hardware 
                { FaultCode.Control_Device_hardware, "Control device hardware" },
                { FaultCode.Measurement_CircuitOne, "Measurement circuit 1" },
                { FaultCode.Computing_Circuit, "Computing circuit" },
                #endregion
                { FaultCode.Operating_Unit, "Operating unit" },
                { FaultCode.Power_Section, "Power section" },
                { FaultCode.Output_Stages, "Output stages" },
                { FaultCode.Chopper, "Chopper" },
                { FaultCode.Input_Stages, "Input stages" },
                #region Contacts
                { FaultCode.Contacts, "Contacts" },
                //Contact 1-5 are 0x5441 to 0x5445 and are manufacture specific
                #endregion
                #region Fuses
                { FaultCode.Fuses, "Fuses" },
                { FaultCode.S_1_Is_L_1, "S1 = l1" },
                { FaultCode.S_2_Is_L_2, "S2 = l2" },
                { FaultCode.S_3_Is_L_3, "S3 = l3" },
                //S_$ to S_9 are 0x5454 to 0x5459 and are manufacture specific
                #endregion
                #region STO
                { FaultCode.Fault_STO_Low, "STO signal is low" },///Added
                #endregion
                #region Hardware Memory
                { FaultCode.Hardware_Memory, "Hardware memory" },
                { FaultCode.RAM, "RAM" },
                { FaultCode.ROMorEEPROM, "ROM/EPROM" },
                { FaultCode.EEPROM, "EEPROM" },
                #endregion
                #region Software Reset Watchdog
                { FaultCode.Software_Reset, "Software reset (watchdog)" },
                { FaultCode.DataRecordNo1, "Data record no. 1" },
                { FaultCode.DataRecordNo2, "Data record no. 2" },
                { FaultCode.DataRecordNo3, "Data record no. 3" },
                { FaultCode.DataRecordNo4, "Data record no. 4" },
                { FaultCode.DataRecordNo5, "Data record no. 5" },
                { FaultCode.DataRecordNo6, "Data record no. 6" },
                { FaultCode.DataRecordNo7, "Data record no. 7" },
                { FaultCode.DataRecordNo8, "Data record no. 8" },
                { FaultCode.DataRecordNo9, "Data record no. 9" },
                { FaultCode.DataRecordNo10, "Data record no. 10" },
                { FaultCode.DataRecordNo11, "Data record no. 11" },
                { FaultCode.DataRecordNo12, "Data record no. 12" },
                { FaultCode.DataRecordNo13, "Data record no. 13" },
                { FaultCode.DataRecordNo14, "Data record no. 14" },
                { FaultCode.DataRecordNo15, "Data record no. 15" },
                #endregion
                { FaultCode.Loss_Of_Parameters, "Loss of parameters" },
                { FaultCode.Parameter_Error, "Parameter error" },
                { FaultCode.Power_Additional_Modules, "Power additional modules" },
                #region Brake Chopper
                { FaultCode.Brake_Chopper, "Brake chopper" },
                { FaultCode.Failure_Brake_Chopper, "Failure brake chopper" },
                { FaultCode.Over_Current_Brake_Chopper, "Over current brake chopper" },
                { FaultCode.Protective_Circuit_Brake_Chopper, "Protective circuit brake chopper" },
                #endregion
                #region Motor
                { FaultCode.Motor, "Motor" },
                { FaultCode.Motor_Blocked, "Motor blocked" },
                { FaultCode.Motor_Error_Or_Comm_Mal, "Motor error or commutation malfunc." },
                { FaultCode.Motor_Tilted, "Motor tilted" },
                #endregion
                { FaultCode.Measurement_CircuitTwo, "Measurement circuit 2" },
                #region Sensor
                { FaultCode.Sensor, "Sensor" },
                { FaultCode.Tacho_Fault, "Tachometer fault" },
                { FaultCode.Tacho_Wrong_Polarity, "Tacho wrong polarity" },
                { FaultCode.Resolver_1_Fault, "Resolver 1 fault" },
                { FaultCode.Resolver_2_Fault, "Resolver 2 fault" },
                { FaultCode.Incremental_Sensor_1_Fault, "Incremental sensor 1 fault" },
                { FaultCode.Incremental_Sensor_2_Fault, "Incremental sensor 2 fault" },
                { FaultCode.Incremental_Sensor_3_Fault, "Incremental sensor 3 fault" },
                { FaultCode.Speed, "Speed" },
                { FaultCode.Position, "Position" },
                #endregion
                { FaultCode.Computation_Circuit, "Computation circuit" },
                #region Communication
                { FaultCode.Communication, "Communication" },
                { FaultCode.Serial_1, "Serial interface no. 1" },
                { FaultCode.Serial_2, "Serial interface no. 2" },
                #endregion
                { FaultCode.Data_Storage, "Data storage (external)" },
                { FaultCode.Fault_Data_Storage_Manufacture_Label_Read, "Manufacturer label data read failed" },///Added
                { FaultCode.Fault_Data_Storage_Manufacture_Label_Write, "Manufacturer label data write failed" },
                { FaultCode.Fault_Data_Storage_Manufacture_Label_CRC, "Manufacturer label data cyclic redundency check (CRC) failed" },
                { FaultCode.Fault_Data_Storage_Datalog_Read, "Datalog read failed" },
                { FaultCode.Fault_Data_Storage_Datalog_Write, "Datalog write failed" },
                { FaultCode.Fault_Data_Storage_Datalog_CRC, "Datalog cyclic redundency check (CRC) failed" },
                { FaultCode.Fault_Data_Storage_Config_Parameter_Read, "Configuration parameter read failed" },
                { FaultCode.Fault_Data_Storage_Config_Parameter_Write, "Configuration parameter write failed" },
                { FaultCode.Fault_Data_Storage_Config_Parameter_Mismatch, "Configuration parameter mismatch found" },///
                #region Torque Control
                { FaultCode.Torque_Control, "Torque control" },
                { FaultCode.Excess_Torque, "Excess torque" },
                { FaultCode.Difficult_Startup, "Difficult start up" },
                { FaultCode.Standstill_Torque, "Standstill torque" },
                { FaultCode.Insufficient_Torque, "Insufficient torque" },
                { FaultCode.Torque_Fault, "Torque fault" },
                #endregion
                { FaultCode.Velocity_Speed_Controler, "Velocity speed controller" },
                { FaultCode.Position_Controller, "Position controller" },
                #region Positioning Controller
                { FaultCode.Positioning_Controller, "Positioning controller" },
                { FaultCode.Following_Error, "Following error" },
                { FaultCode.Reference_Limit, "Reference limit" },
                { FaultCode.Homing_Error, "Homing error" },
                #endregion
                { FaultCode.Sync_Controller, "Sync controller" },
                { FaultCode.Winding_Controller, "Winding controller" },
                { FaultCode.Process_Data_Monitoring, "Process data monitoring" },
                #region Control Monitoring 
                { FaultCode.Control_Monitoring, "Control monitoring" },
                { FaultCode.Deceleration, "Deceleration" },
                { FaultCode.Subsync_Run, "Sub-synchronous run" },
                { FaultCode.Stroke_Operation, "Stroke operation" },
                { FaultCode.Control_Additional_Functions, "Control additional functions" },
                #endregion
                { FaultCode.The_EF_special, "Contact Allied Motion's worst intern" },
                { FaultCode.The_NM_special, "Contact Allied Motion" },
                { FaultCode.BAD_GEAR_RATIO, "Bad Gear Ratio"},
                { FaultCode.DS402_FAULT_CAPTURE_GENERIC, "Capture gereric" }, // was 0xF400
                { FaultCode.DS402_FAULT_OVER_CAPDEPTH, "Over capture depth" }, // was 0xF401
                { FaultCode.DS402_FAULT_PERIOD_NOT_SUPPORTED, "Period not supported" }, // was 0xF402
                { FaultCode.DS402_FAULT_TRIGGER_POSITION,"Trigger position" }, // was 0xF403
                { FaultCode.DS402_FAULT_TRIGGER_NOT_SUPPORTED, "Trigger not supported" }, // was 0xF404
                { FaultCode.DS402_FAULT_ADDRESS_1_NOT_SUPPORTED, "Trigger address 1 not supported" }, // was 0xF405
                { FaultCode.DS402_FAULT_ADDRESS_2_NOT_SUPPORTED, "Trigger address 2 not supported" },// was 0xF406
                { FaultCode.DS402_FAULT_ADDRESS_3_NOT_SUPPORTED, "Trigger address 3 not supported" }, // was 0xF407
                { FaultCode.DS402_FAULT_ADDRESS_4_NOT_SUPPORTED, "Trigger address 4 not supported" },// was 0xF408
                { FaultCode.Warning_History_Already_Clear, "The warning history is zero and was requested to reduce by 1" }
            #endregion
            };

        //TODO fault enum not text!
        public static readonly IDictionary<String, String> dictFaultString_TroubleshootString = new Dictionary<String, String>()
            {
                #region codes
                { "No Fault", "0x" },//
                { "Short circuit/earth leakage (input)", "0x2110" },//
                #region Earth Leak Input
                { "Earth leakage (input)", "0x2120" },//
                { "Earth leakage phase L1", "0x2121" },//
                { "Earth leakage phase L2", "0x2122" },//
                { "Earth leakage phase L3", "0x2123" },//
                #endregion
                #region Short Circuit Input
                { "Fault on Address 2 'Not Supported'", "0x2130" },//
                { "Short circuit phases L1-L2", "0x2131" },//
                { "Short circuit phases L2-L3", "0x2132" },//
                { "Short circuit phases L3-L1", "0x2133" },//
                { "Internal current no.1", "0x2211" },//
                { "Internal current no.2", "0x2212" },//
                { "Over-current in ramp function", "0x2213" },//
                { "Over-current in the sequence", "0x2214" },//
                #endregion
                #region Internal Over Current
                { "Continuous over current (device internal)", "0x2220" },//
                { "Continuous over current no.1 (device internal)", "0x2221" },//
                { "Continuous over current no.2 (device internal)", "0x2222" },//
                #endregion
                { "Short circuit/earth leakage (device internal)", "0x2230" },//
                { "Earth leakage (device internal)", "0x2240" },//
                { "Short circuit (device internal)", "0x2250" },//
                #region Cont Over Current Internal
                { "Continuous over current (device output side)", "0x2310" },//
                { "Continuous over current no.1 (device output side)", "0x2311" },//
                { "Continuous over current no.2 (device output side)", "0x2312" }, //
                #endregion
                { "Short circuit/earth leakage (motor-side)", "0x2320" },//
                #region Earth Leak Motor
                { "Earth leakage (motor-side)", "0x2330" },//
                { "Earth leakage phase U", "0x2331" },//
                { "Earth leakage phase V", "0x2332" },//
                { "Earth leakage phase W", "0x2333" },//
                #endregion
                #region Sort Circuit Motor
                { "Short circuit (motor-side)", "0x2340" },//
                { "Short circuit phases U-V", "0x2341" },//
                { "Earth leakage phase V-W", "0x2342" },//
                { "Earth leakage phase W-U", "0x2343" },//
                #endregion
                { "Load level fault (I2t, thermal state)", "0x2350" },//
                { "Load level warning (I2t, thermal state)", "0x2351" },//
                #region Main Over Voltage
                { "Mains over-voltage", "0x3110" },//
                { "Mains over-voltage phase L1", "0x3111" },//
                { "Mains over-voltage phase L2", "0x3112" },//
                { "Mains over-voltage phase L3", "0x3113" },//
                #endregion
                #region Mains Under Voltage
                { "Mains under-voltage", "0x3120" },//
                { "Mains under-voltage phase L1", "0x3121" },//
                { "Mains under-voltage phase L2", "0x3122" },//
                { "Mains under-voltage phase L3", "0x3123" },//
                #endregion
                #region Phase Failure
                { "Phase failure", "0x3130" },//
                { "Phase failure L1", "0x3131" },//
                { "Phase failure L2", "0x3132" },//
                { "Phase failure L3", "0x3133" },//
                { "Phase sequence", "0x3134" },//
                #endregion
                #region Mains Frequency
                { "Mains frequency", "0x3140" },//
                { "Mains frequency too great", "0x3141" },//
                { "Mains frequency too small", "0x3142" },//
                #endregion
                #region DC Link Over Voltage
                { "DC link over-voltage", "Be better about teaching your DC link about it's limits." },//
                { "Over-voltage no. 1", "0x3211" },//
                { "Over voltage no. 2", "0x3212" },//
                #endregion
                #region DC Link Under Voltage
                { "DC link under-voltage", "0x3220" },//
                { "Under-voltage no. 1", "0x3221" },//
                { "Under-voltage no. 2", "0x3222" },//
                #endregion
                { "Load error", "0x3230" },//
                #region Output Over Voltage
                { "Output over-voltage", "0x3310" },//
                { "Output over-voltage phase U", "0x3311" },//
                { "Output over-voltage phase V", "0x3312" },//
                { "Output over-voltage phase W", "0x3313" },//
                #endregion
                #region Armature Circuit
                { "Armature circuit", "0x3320" },//
                { "Armature circuit interrupted", "0x3321" },//
                #endregion
                #region Field Circuit 
                { "Field circuit", "0x3330" },//
                { "Field circuit interrupted", "0x3331" },//
                { "Excess ambient temperature", "0x4110" },//
                { "Too low ambient temperature", "0x4120" },//
                { "Temperature supply air", "0x4130" },//
                { "Temperature air outlet", "0x4140" },//
                { "Excess temperature device", "0x4210" },//
                { "Too low temperature device", "0x4220" },//
                #endregion
                #region Temp Of Drive
                { "Temperature drive", "0x4300" },//
                { "Excess temperature drive", "0x4310" },//
                { "Too low temperature drive", "0x4320" },//
                #endregion
                #region Temp of Supply
                { "Temperature supply", "0x4400" },//
                { "Excess temperature supply", "0x4410" },//
                { "Too low temperature supply", "0x4420" },//
                #endregion
                { "Supply device hardware", "0x5100" },//
                #region Supply Low Voltage
                { "Supply low voltage", "0x5110" },//
                { "U1 = supply ±15 V", "0x5111" },//
                { "U2 = supply +24 V", "0x5112" },//
                { "U3 = supply +5 V", "0x5113" },//
                //0x5114 to 0x5119 is manufacturer specific
                #endregion
                { "Supply intermediate circuit", "0x5120" },//
                #region Control Device Hardware 
                { "Control device hardware", "0x5200" },//
                { "Measurement circuit 1", "0x5210" },//
                { "Computing circuit", "0x5220" },//
                #endregion
                { "Operating unit", "0x5300" },//
                { "Power section", "0x5400" },//
                { "Output stages", "0x5410" },//
                { "Chopper", "0x5420" },//
                { "Input stages", "0x5430" },//
                #region Contacts
                { "Contacts", "0x5440" },//
                //Contact 1-5 are 0x5441 to 0x5445 and are manufacture specific
                #endregion
                #region Fuses
                { "Fuses", "0x5450" },//
                { "S1 = l1", "0x5451" },//
                { "S2 = l2", "0x5452" },//
                { "S3 = l3", "0x5453" },//
                //S_$ to S_9 are 0x5454 to 0x5459 and are manufacture specific
                #endregion
                #region Hardware Memory
                { "Hardware memory", "0x5500" },//
                { "RAM", "0x5510" },//
                { "ROM/EPROM", "0x5520" },//
                { "EEPROM", "0x5530" },//
                #endregion
                #region Software Reset Watchdog
                { "Software reset (watchdog)", "0x6010" },//
                { "Data record no. 1", "0x6301" },//
                { "Data record no. 2", "0x6302" },//
                { "Data record no. 3", "0x6303" },//
                { "Data record no. 4", "0x6304" },//
                { "Data record no. 5", "0x6305" },//
                { "Data record no. 6", "0x6306" },//
                { "Data record no. 7", "0x6307" },//
                { "Data record no. 8", "0x6308" },//
                { "Data record no. 9", "0x6309" },//
                { "Data record no. 10", "0x630a" },//
                { "Data record no. 11", "0x630b" },//
                { "Data record no. 12", "0x630c" },//
                { "Data record no. 13", "0x630d" },//
                { "Data record no. 14", "0x630e" },//
                { "Data record no. 15", "0x630f" },//
                #endregion
                { "Loss of parameters", "0x6310" },//
                { "Parameter error", "0x6320" },//
                { "Power additional modules", "0x7100" },//
                #region Brake Chopper
                { "Brake chopper", "0x7110" },//
                { "Failure brake chopper", "0x7111" },//
                { "Over current brake chopper", "0x7112" },//
                { "Protective circuit brake chopper", "0x7113" },//
                #endregion
                #region Motor
                { "Motor", "0x7120" },//
                { "Motor blocked", "0x7121" },//
                { "Motor error or commutation malfunc.", "0x7122" },//
                { "Motor tilted", "0x7123" },//
                #endregion
                { "Measurement circuit 2", "0x7200" },//
                #region Sensor
                { "Sensor", "0x7300" },//
                { "Tacho fault", "0x7301" },//
                { "Tacho wrong polarity", "0x7302" },//
                { "Resolver 1 fault", "0x7303" },//
                { "Resolver 2 fault", "0x7304" },//
                { "Incremental sensor 1 fault", "0x7305" },//
                { "Incremental sensor 2 fault", "0x7306" },//
                { "Incremental sensor 3 fault", "0x7307" },//
                { "Speed", "0x7310" },//
                { "Position", "0x7320" },//
                #endregion
                { "Computation circuit", "0x7400" },//
                #region Communication
                { "Communication", "0x7500" },//
                { "Serial interface no. 1", "0x7510" },//
                { "Serial interface no. 2", "0x7520" },//
                #endregion
                { "Data storage (external)", "0x7600" },//
                #region Torque Control
                { "Torque control", "0x8300" },//
                { "Excess torque", "0x8311" },//
                { "Difficult start up", "0x8312" },//
                { "Standstill torque", "0x8313" },//
                { "Insufficient torque", "0x8321" },//
                { "Torque fault", "0x8331" },//
                #endregion
                { "Velocity speed controller", "0x8400" },//
                { "Position controller", "0x8500" },//
                #region Positioning Controller
                { "Positioning controller", "0x8600" },//
                { "Following error", "0x8611" },//
                { "Reference limit", "0x8612" },//
                { "Homing error", "0x8613" },//
                #endregion
                { "Sync controller", "0x8700" },//
                { "Winding controller", "0x8800" },//
                { "Process data monitoring", "0x8900" },//
                #region Control Monitoring 
                { "Control monitoring", "0x8a00" },//
                { "Deceleration", "0xf001" },//
                { "Sub-synchronous run", "0xf002" },//
                { "Stroke operation", "0xf003" },//
                { "Control additional functions", "0xf004" },//
                #endregion
                { "Contact Allied Motion's best intern", "0xf401" },//
                { "Contact Allied Motion", "0xf406" },//

                { "Capture gereric", "0xff30" }, // 
                { "Over capture depth", "0xff31" }, //
                { "Period not supported", "0xff32" }, //
                { "Trigger position", "0xff33" }, //
                { "Trigger not supported", "0xff34" }, //
                { "Trigger address 1 not supported", "0xff35" },//
                { "Trigger address 2 not supported", "0xff36" },//
                { "Trigger address 3 not supported", "0xff37" },//
                { "Trigger address 4 not supported", "0xff38" } //
            
                #endregion
            };


        public static bool TryDecodeFaultCodeStringToDescriptonString(string faultCode, out string strFaultDesc)
        {
            if (dictFaultString_FaultEnum.TryLookup(faultCode.ToLower(), out FaultCode faultCodeEnum))
            {
                return dictFaultEnum_FaultDescString.TryLookup(faultCodeEnum, out strFaultDesc);
            }
            strFaultDesc = "No description found.";
            return false;
        }

        public static bool TryDecodeFaultToDescriptionString(FaultCode faultCodeEnum, out string strFaultDesc)
        {
            return dictFaultEnum_FaultDescString.TryLookup(faultCodeEnum, out strFaultDesc);
        }

        public static bool TryDecodeFaultStringToDescriptionString(string faultStr, out string strFault)
        {
            return dictFaultString_TroubleshootString.TryLookup(faultStr.ToLower(), out strFault);
        }
        #endregion
    }
}
