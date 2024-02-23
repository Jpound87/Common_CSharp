namespace Common.Constant
{
    #region Enumerations

    #region Allied

    #region Authorization
    /// <summary>
    /// Enumeration of the available user authorization levels.
    /// </summary>
    public enum AuthorizationLevel : int
    {
        All = int.MinValue,
        Safety = 0,
        Standard = 8,
        Advanced = 32,
        Authorized = 128,//Engineering Mode
        Allied = 1024,//Development
        None = int.MaxValue//Display to no one
    }
    #endregion Authorization

    #region Configuration
    public enum ConfigurationSaveLoadType : byte
    {
        Unknown,
        ConfigParametersCompare,
        ConfigParametersSave,
        ConfigParametersLoad,
        ConfigManufacturerLabelCompare,
        ConfigManufacturerLabelLoad
    }
    #endregion

    #region Fault Codes
    public enum FaultCode
    {
        #region Codes

        Unknown = 0xFFFF,
        NM_Unspecified_Error = 0x01058120,
        No_Fault = 0x0000,
        Short_Circuit_Earth_leakage_Input = 0x2110,

        #region Earth Leak Input
        Earth_Leakage_Input = 0x2120,
        Earth_Leakage_Phase_1 = 0x2121,
        Earth_Leakage_Phase_2 = 0x2122,
        Earth_Leakage_Phase_3 = 0x2123,
        #endregion

        #region Short Circuit Input
        Short_Circuit_Input = 0x2130,
        Short_Circuit_Phase_1_2 = 0x2131,
        Short_Circuit_Phase_2_3 = 0x2132,
        Short_Circuit_Phase_3_1 = 0x2133,
        Internal_Current_1 = 0x2211,
        Internal_Current_2 = 0x2212,
        Over_Current_Ramp_Function = 0x2213,
        Over_Current_Sequence = 0x2214,
        #endregion

        #region Internal Over Current
        Continious_Over_Current_Internal = 0x2220,
        Continious_Over_Current_Internal_1 = 0x2221,
        Continious_Over_Current_Internal_2 = 0x2222,
        #endregion

        Short_Circuit_Earth_leakage_Internal = 0x2230,
        Earth_Leakage_Internal = 0x2240,
        Short_Circuit_Internal = 0x2250,

        #region Cont Over Current Internal
        Continious_Over_Current_Output = 0x2310,
        Continious_Over_Current_Output_1 = 0x2311,
        Continious_Over_Current_Output_2 = 0x2312,
        #endregion

        Short_Circuit_Earth_leakage_Motor = 0x2320,

        #region Earth LEak Motor
        Earth_Leakage_Motor = 0x2330,
        Earth_Leakage_Motor_Phase_U = 0x2331,
        Earth_Leakage_Motor_Phase_V = 0x2332,
        Earth_Leakage_Motor_Phase_W = 0x2333,
        #endregion

        #region Sort Circuit Motor
        Short_Circuit_Motor = 0x2340,
        Short_Circuit_U_V = 0x2341,
        Short_Circuit_V_W = 0x2342,
        Short_Circuit_W_U = 0x2343,
        #endregion

        Load_Level_Fault_Thermal_State = 0x2350,
        Load_Level_Warning_Thermal_State = 0x2351,

        #region Main Over Voltage
        Mains_Over_Voltage = 0x3110,
        Mains_Over_Voltage_Phase_1 = 0x3111,
        Mains_Over_Voltage_Phase_2 = 0x3112,
        Mains_Over_Voltage_Phase_3 = 0x3113,
        #endregion

        #region Mains Under Voltage
        Mains_Under_Voltage = 0x3120,
        Mains_Under_Voltage_Phase_1 = 0x3121,
        Mains_Under_Voltage_Phase_2 = 0x3122,
        Mains_Under_Voltage_Phase_3 = 0x3123,
        #endregion

        #region Phase Failure
        Phase_Failure = 0x3130,
        Phase_Failure_1 = 0x3131,
        Phase_Failure_2 = 0x3132,
        Phase_Failure_3 = 0x3133,
        Phase_Sequence = 0x3134,
        #endregion

        #region Mains Frequency
        Mains_Frequency = 0x3140,
        Mains_Frequency_Too_Great = 0x3141,
        Mains_Frequency_Too_Small = 0x3142,
        #endregion

        #region DC Link Over Voltage
        DC_Link_Over_Voltage = 0x3210,
        Over_Voltage_1 = 0x3211,
        Over_Voltage_2 = 0x3212,
        #endregion

        #region DC Link Under Voltage
        DC_Link_Under_Voltage = 0x3220,
        Under_Voltage_1 = 0x3221,
        Under_Voltage_2 = 0x3222,
        #endregion

        Load_Error = 0x3230,

        #region Output Over Voltage
        Output_Over_Voltage = 0x3310,
        Output_Over_Voltage_U = 0x3311,
        Output_Over_Voltage_V = 0x3312,
        Output_Over_Voltage_W = 0x3313,
        #endregion

        #region Armature Circuit
        Armature_Circuit = 0x3320,
        Armature_Circuit_Interrupted = 0x3321,
        #endregion

        #region Field Circuit 
        Field_Circuit = 0x3330,
        Field_Circuit_Interrupted = 0x3331,
        Excess_Ambient_Temperature = 0x4110,
        Too_Low_Ambient_Temperature = 0x4120,
        Temperature_Air_Supply = 0x4130,
        Temperature_Air_Outlet = 0x4140,
        Excess_Temperature_Device = 0x4210,
        Too_Low_Temperature_Device = 0x4220,
        #endregion

        #region Temp Of Drive
        Temperature_Drive = 0x4300,
        Excess_Temperature_Drive = 0x4310,
        Fault_Too_Low_Temperature_Drive = 0x4320,
        Warning_Drive_Temp_Limit = 0x4321,
        Fault_Drive_Temp_Sensor_Fail = 0x4383,
        Fault_Motor_Over_Temperature = 0x4386,
        Warning_Motor_Temp_Limit = 0x4387,
        Fault_Too_Low_Temperature_Motor = 0x4388,
        Fault_Motor_Temp_Sensor_Fail = 0x4389,
        #endregion

        #region Temp of Supply
        Temperature_Supply = 0x4400,
        Excess_Temperature_Supply = 0x4410,
        Too_Low_Temperature_Supply = 0x4420,
        #endregion

        Supply_Device_Hardware = 0x5100,

        #region Supply Low Voltage
        Supply_Low_Voltage = 0x5110,
        Supply_Is_15 = 0x5111,
        Supply_Is_24 = 0x5112,
        Supply_Is_5 = 0x5113, //0x5114 to 0x5119 is manufacturer specific
        #endregion

        Supply_Intermediate_Circuit = 0x5120,

        #region Control Device Hardware 
        Control_Device_hardware = 0x5200,
        Measurement_CircuitOne = 0x5210,
        Computing_Circuit = 0x5220,
        #endregion

        Operating_Unit = 0x5300,
        Power_Section = 0x5400,
        Output_Stages = 0x5410,
        Chopper = 0x5420,
        Input_Stages = 5430,

        #region Contacts
        Contacts = 0x5440, //Contact 1-5 are 0x5441 to 0x5445 and are manufacture specific
        #endregion

        #region Fuses
        Fuses = 0x5450,
        S_1_Is_L_1 = 0x5451,
        S_2_Is_L_2 = 0x5452,
        S_3_Is_L_3 = 0x5453,//S_$ to S_9 are 0x5454 to 0x5459 and are manufacture specific
        #endregion

        #region STO
        Fault_STO_Low = 0x5480,
        #endregion

        #region Hardware Memory
        Hardware_Memory = 0x5500,
        RAM = 0x5510,
        ROMorEEPROM = 0x5520,
        EEPROM = 0x5530,
        #endregion

        #region Software Reset Watchdog
        Software_Reset = 0x6010,
        DataRecordNo1 = 0x6301,
        DataRecordNo2 = 0x6302,
        DataRecordNo3 = 0x6303,
        DataRecordNo4 = 0x6304,
        DataRecordNo5 = 0x6305,
        DataRecordNo6 = 0x6306,
        DataRecordNo7 = 0x6307,
        DataRecordNo8 = 0x6308,
        DataRecordNo9 = 0x6309,
        DataRecordNo10 = 0x630A,
        DataRecordNo11 = 0x630B,
        DataRecordNo12 = 0x630C,
        DataRecordNo13 = 0x630D,
        DataRecordNo14 = 0x630E,
        DataRecordNo15 = 0x630F,
        #endregion

        Loss_Of_Parameters = 0x6310,
        Parameter_Error = 0x6320,
        Power_Additional_Modules = 0x7100,

        #region Brake Chopper
        Brake_Chopper = 0x7110,
        Failure_Brake_Chopper = 0x7111,
        Over_Current_Brake_Chopper = 0x7112,
        Protective_Circuit_Brake_Chopper = 0x7113,
        #endregion

        #region Motor
        Motor = 0x7120,
        Motor_Blocked = 0x7121,
        Motor_Error_Or_Comm_Mal = 0x7122,
        Motor_Tilted = 0x7123,
        #endregion

        Measurement_CircuitTwo = 0x7200,

        #region Sensor
        Sensor = 0x7300,
        Tacho_Fault = 0x7301,
        Tacho_Wrong_Polarity = 0x7302,
        Resolver_1_Fault = 0x7303,
        Resolver_2_Fault = 0x7304,
        Incremental_Sensor_1_Fault = 0x7305,
        Incremental_Sensor_2_Fault = 0x7306,
        Incremental_Sensor_3_Fault = 0x7307,
        Speed = 0x7310,
        Position = 0x7320,
        #endregion

        Computation_Circuit = 0x7400,

        #region Communication
        Communication = 0x7500,
        Serial_1 = 0x7510,
        Serial_2 = 0x7520,
        #endregion

        Data_Storage = 0x7600,
        Fault_Data_Storage_Manufacture_Label_Read = 0x7681,
        Fault_Data_Storage_Manufacture_Label_Write = 0x7682,
        Fault_Data_Storage_Manufacture_Label_CRC = 0x7683,
        Fault_Data_Storage_Datalog_Read = 0x7684,
        Fault_Data_Storage_Datalog_Write = 0x7685,
        Fault_Data_Storage_Datalog_CRC = 0x7686,
        Fault_Data_Storage_Config_Parameter_Read = 0x7687,
        Fault_Data_Storage_Config_Parameter_Write = 0x7688,
        Fault_Data_Storage_Config_Parameter_Mismatch = 0x7689,

        #region Torque Control
        Torque_Control = 0x8300,
        Excess_Torque = 0x8311,
        Difficult_Startup = 0x8312,
        Standstill_Torque = 0x8313,
        Insufficient_Torque = 0x8321,
        Torque_Fault = 0x8331,
        #endregion

        Velocity_Speed_Controler = 0x8400,
        Position_Controller = 0x8500,

        #region Positioning Controller
        Positioning_Controller = 0x8600,
        Following_Error = 0x8611,
        Reference_Limit = 0x8612,
        Homing_Error = 0x8613,
        #endregion

        Sync_Controller = 0x8700,
        Winding_Controller = 0x8800,
        Process_Data_Monitoring = 0x8900,


        #region Control Monitoring
        Control_Monitoring = 0x8A00,
        Deceleration = 0xF001,
        Subsync_Run = 0xF002,
        Stroke_Operation = 0xF003,
        Control_Additional_Functions = 0xF004,
        #endregion

        The_EF_special = 0xF401,
        The_NM_special = 0xF406,
        BAD_GEAR_RATIO = 0xFF20,  // was 0xF400
        DS402_FAULT_CAPTURE_GENERIC = 0xFF30,  // was 0xF400
        DS402_FAULT_OVER_CAPDEPTH = 0xFF31,  // was 0xF401
        DS402_FAULT_PERIOD_NOT_SUPPORTED = 0xFF32,  // was 0xF402
        DS402_FAULT_TRIGGER_POSITION = 0xFF33,  // was 0xF403
        DS402_FAULT_TRIGGER_NOT_SUPPORTED = 0xFF34,  // was 0xF404
        DS402_FAULT_ADDRESS_1_NOT_SUPPORTED = 0xFF35,  // was 0xF405
        DS402_FAULT_ADDRESS_2_NOT_SUPPORTED = 0xFF36,  // was 0xF406
        DS402_FAULT_ADDRESS_3_NOT_SUPPORTED = 0xFF37,  // was 0xF407
        DS402_FAULT_ADDRESS_4_NOT_SUPPORTED = 0xFF38,  // was 0xF408
        Warning_History_Already_Clear = 0xFF48
        //0xFF01 to 0xFFFF are man specific

        #endregion Codes
    }
    #endregion

    #region IO Linker

    #region Linker Source
    public enum LinkerSourceType : byte
    {
        None,
        Address,
        AnalogPin,
        DigitalPin,
        Bitfield
    }
    #endregion

    #region Linker Trigger Modes
    public enum LinkerTriggerModes
    {
        MODE_SCALING = 0x0,
        TRIG_ABOVE = 0x1, // NM - True when triggerSource goes above triggerUpperLimit
        TRIG_BELOW = 0x2, // NM - "" when triggerSource goes below triggerLowerLimit
        TRIG_OUTSIDE = 0x3, // NM - "" when triggerSource is outside the range of triggerUpperLimit and triggerLowerLimit
        TRIG_WITHIN = 0x4,// NM - "" when triggerSource is within the range of triggerUpperLimit and triggerLowerLimit
        MODE_BITWISE = 0x5
    }
    #endregion

    #endregion

    #region Logging
    /// <summary>
    /// Enumeration of the log priority levels inteneded to be used with the 
    /// Logger class or with the logEntryData struct
    /// </summary>
    public enum Priority_Log
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Assert
    };
    #endregion

    #region Maths

    #region Averaging
    /// <summary>
    /// Graph axes enumeration.
    /// </summary>
    public enum FILTER_TYPE : byte
    {
        None,
        LinearInterpolation,
        SimpleMovingAverage,
        ExponentialMovingAverage
    }
    #endregion

    #endregion

    #region Motion Schedule
    public enum ScheduleItemType : byte
    {
        None,
        Absolute,
        Relative,
        Delay,
        Wait
    };
    #endregion

    #region Networking

    #region Network Type
    public enum NetworkType : byte
    {
        ALLNET,
        CiA402,
        Virtual_CiA402,
        All
    };
    #endregion

    #region Protocol Type
    public enum ProtocolType : byte
    {
        None = 0,
        CANopen = 1,
        EtherCAT = 2,
        ALLNET = 3,
        Virtual = byte.MaxValue
    };
    #endregion

    #endregion Networking

    #region Plot

    #region Axis
    /// <summary>
    /// Graph axes enumeration.
    /// </summary>
    public enum PlotAxis : byte
    {
        None,
        Left,
        Right
    }
    #endregion /Axis

    #region Capture Trigger Mode
    public enum TriggerMode : sbyte
    {
        INVALID = -1,
        SINGLE = 0x0000, // Immediate
        ABOVE = 0x0001, // Above set point 1
        BELOW = 0x0002, // Below set point 1
        OUTSIDE = 0x0003, // Outside set point 1 & 2
        WITHIN = 0x0004 // Inside set point 1 & 2 
    }
    #endregion /Capture Trigger Mode

    #region Capture Trigger Type
    public enum TriggerType
    {
        ABSOLUTE = 0x0000,
        RELATIVE = 0x0001
    }
    #endregion

    #region Data Capture State
    /// <summary>
    /// This enumeration contains the representation of the current state of capture
    /// </summary>
    public enum CaptureState_Datam : byte
    {
        NoCapture,// Capture is not initiated and none is avalable
        Capturing,// Capture is in progress
        HasCapture // Capture has completed and dtate is available
    }

    public enum CaptureState_Device : int
    {
        INIT = 0,
        IDLE = 1,
        CHECK_AND_SETUP = 2,
        BUFFER = 5,
        DUMMY_TRIGGER_READ = 7,
        CHECK_TRIGGER = 10,// Non-immediate capture 
        START_COLLECTING = 15,// Immediate capture
        COLLECTING = 18,// 1-800-C-O-L-L-E-C-T (A joke for anyone who remebers the 80-90's)
        DONE = 20,
        ERROR = 30
    }
    #endregion

    #region Capture Controls
    /// <summary>
    /// This enumeration contains the representation of the current state of capture
    /// </summary>
    public enum Capture_Controls : int
    {
        REINIT = CaptureState_Device.INIT,// Capture is not initiated and none is avalable
        START = CaptureState_Device.CHECK_AND_SETUP,// Capture is in progress
        CANCEL = CaptureState_Device.IDLE// Capture has completed and dtate is available
    }
    #endregion

    #endregion /Plot

    #region Save/Load

    #region Save Result
    public enum SaveResult : byte
    {
        EmptyFileName,
        Failure,
        Success
    }
    #endregion

    #region Load Result
    public enum LoadResult : byte
    {
        EmptyFileName,
        FileDoesNotExist,
        FileCurrupt,
        Success
    }
    #endregion

    #endregion Save/Load

    #region Operation 
    public enum CollectionOperationResult : byte
    {
        Succeeded,
        Duplicate,
        Failed
    }
    #endregion /Operation

    #endregion /Allied

    #region Data Definitions

    #region Time
    public enum TimeScale
    {
        Hours,
        Minutes,
        Seconds,
        Milliseconds,
        Microseconds
    }
    #endregion /Time

    #region IO
    public enum IO : byte
    {
        Input,
        Output,
        Unknown
    };
    #endregion

    #region R/W
    public enum IO_Operation : byte
    {
        Write,
        Read,
        None
    }

    public enum ValueStorageOption : byte
    {
        RAM, // Only in drive with power on
        ROM, // Stored directly in ROM with no procedure
        BufferedROM // Save procedure required to move from RAM to ROM
    }
    #endregion

    #region Priority
    public enum Priority_Packet : byte
    {
        Communicator = 0,
        Firmware = 1,
        Write = 2,
        Immediate = 3,
        High = 4,
        Low = 5,
        Virtual = byte.MaxValue// 255
    }
    #endregion

    #region Word Size
    public enum WordSize : byte
    {
        Bit_1 = 1,
        Bit_8 = 8,
        Bit_16 = 16,
        Bit_24 = 24,
        Bit_32 = 32,
        Bit_48 = 48,
        Bit_56 = 56,
        Bit_64 = 64,
        Variable = 0
    }
    #endregion

    #region Endianess
    public enum Endianess : byte
    {
        Big,
        Little
    }
    #endregion

    #endregion Data Definitions

    #region FAST UI

    #region Device Tree 
    /// <summary>
    /// The enumeration of the types of nodes that we place on the device tree. 
    /// </summary>
    public enum TreeNodeType
    {
        Communicator,
        Network,
        Device
    }
    #endregion

    #region Layout
    public enum LayoutContainerStyle : byte
    {
        Flow,
        Table
    }
    #endregion /Layout

    #region Button
    public enum ButtonState : byte
    {
        Normal = 0,
        Pressed,
        Focused,
        Hover,
        Disabled,
        Checked
    }
    #endregion Button

    #region Check
    public enum CheckedStyle : byte
    {
        None = 0,
        Border,
        ColorChange
    }
    #endregion /Check

    #region Display
    public enum ValueDisplayOption : byte
    {
        None,
        Scalable
    }
    #endregion /Display

    #region Form Order
    /// <summary>
    /// This enum organizes the order changing commands for open forms.
    /// </summary>
    public enum FormOrderZ : byte
    {
        NoAdjustment,
        SwapOrder,
        SendToBack
    };
    #endregion

    #region Open State
    public enum OpenState : byte
    {
        Open,
        Closed
    };
    #endregion

    #region Output Text
    public enum PrintType : byte
    {
        Regular,
        Success,
        Error,
        Bullet,
        Alert
    }

    public enum HighlightColor : byte
    {
        Blue,
        Yellow,
        Red
    }
    #endregion /Output Text

    #region Theme

    #region Choice
    public enum Themes : byte
    {
        Pastel = 0,
        Hei = 1,
        Allied = 2,
        Ormec = 3,
        Grey = 4
    }
    #endregion

    #region Definitions
    public enum ThemeDefinition : byte
    {
        DarkText,
        LightText,
        SecondaryText,
        BrandText,
        PrimaryBrand,
        SecondaryBrand,
        TertiaryBrand,
        PrimaryAccent,
        SecondaryAccent,
        PrimaryBackground,
        SecondaryBackground,
        TertiaryBackground,
        PrimaryFill,
        SecondaryFill,
        TertiaryFill,
        QuaternaryFill,
        Seperator,
        EnabledOn,
        EnabledOff
    }
    #endregion

    #region Color Layer
    public enum ColorLayer : byte
    {
        Fore,
        Back,
        Background,
        Glow,
        Border,
        Border_Checked,
        All,
        Checked,
        CheckedText,
        Unchecked,
        UncheckedText,
        On,
        OnText,
        Off,
        OffText,
        MouseOver_Back,
        MouseDown_Back,
        IconAlter_Background,
        None
    }
    #endregion /Color Layer

    #endregion /Theme

    #region Shapes
    public enum LineFill : byte
    {
        None = 0,
        Gradient,
        Solid
    }

    public enum Indicator_Shape : byte
    {
        Rectangle,
        Circle
    }

    public enum Indicator_State : byte
    {
        On,
        Off,
        Override,
        Inactive
    }
    #endregion /Shapes

    #region Iconograpy 
    public enum ControlIconography : byte
    {
        Text,
        Image,
        None
    }
    #endregion /Iconograpy

    #endregion /FAST UI

    #region CiA309
    public enum ScheduledUpdateMapping : byte
    {
        PDO,
        SDO,
        PDO_SlowSDO,
        Virtual,
        None
    }
    #endregion /CiA309

    #region CiA402 

    #region CiA402 Access Rights
    /// <summary>
    /// Enumeration of the DS402 access types.
    /// </summary>
    public enum AccessRights : int
    {
        RO,
        WO,
        RW,
        RWW,
        RWR,
        CONST,
        ALLIED
    }
    #endregion CiA402 Access Rights

    #region Operating Mode
    /// <summary>
    /// Enumeration of the DS402 defined device modes
    /// </summary>
    public enum OperatingMode
    {
        Invalid = -1,
        None = 0,
        ProfilePosition = 1,
        Velocity = 2,
        ProfileVelocity = 3,
        Torque = 4,
        Reserved0 = 5,
        Homing = 6,
        InterpolatedPosition = 7,
        CyclicSynchronousPosition = 8,
        CyclicSynchronousVelocity = 9,
        CyclicSynchronousTorque = 10,
        CyclicSynchronousTorqueComutationAngle = 11,
        Reserved1 = 12,
        Reserved2 = 13,
        Reserved3 = 14,
        Reserved4 = 15,
        Reserved5 = 16,
        ManufacturerSpecific1 = 17,
        ManufacturerSpecific2 = 18,
        ManufacturerSpecific3 = 19,
        ManufacturerSpecific4 = 20,
        ManufacturerSpecific5 = 21,
        ManufacturerSpecific6 = 22,
        ManufacturerSpecific7 = 23,
        ManufacturerSpecific8 = 24,
        ManufacturerSpecific9 = 25,
        ManufacturerSpecific10 = 26,
        ManufacturerSpecific11 = 27,
        ManufacturerSpecific12 = 28,
        ManufacturerSpecific13 = 29,
        ManufacturerSpecific14 = 30,
        ManufacturerSpecific15 = 31,
        ManufacturerSpecific16 = 32,
        Phasing = -121,
        OpenLoopVoltage = -122,
        UncommutatedCurrent = -123,
        InertialMeasurement = -124
    };
    #endregion

    #endregion CiA402

    #region Language
    public enum Languages : byte
    {
        English = 0,
        Español = 1,
        Deutsch = 2,
        Português = 3,
        中文 = 4,
        Français = 5,
        Hindi = 6
    }
    #endregion /Language

    #region Orientation

    #region Horizontal Direction
    public enum HorizontalDirection : byte
    {
        Left,
        Right
    };
    #endregion

    #endregion Orientation

    #endregion /Enumerations
}
