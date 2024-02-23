using System;
using System.Drawing;
using System.IO;

namespace Common.Constant
{
    public static class Tokens
    {
        #region Identity
        public const String KEY = "JLB";
        public const String FormName = nameof(Tokens);
        #endregion /Identity

        #region Windows System Constants
        private const int WM_DEVICECHANGE = 0x0219;                 // device change event 
        private const int DBT_DEVICEARRIVAL = 0x8000;               // system detected a new device 
        private const int DBT_DEVICEREMOVEPENDING = 0x8003;         // about to remove, still available 
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;        // device is gone 
        private const int DBT_DEVTYP_PORT = 0x00000003;             // serial, parallel 
        #endregion

        #region Icon Text
        public const String Floppy = "💾";
        public const String File = "📁";
        public const String Key = "🗝";
        public const String Locke = "🔒";// lock is a keyword
        public const String Unlocke = "🔓";// lock is a keyword
        public const String Hamberder = "☰";// Hamberder is how D.Trump spells hamberger so it must be korrect
        public const String Gear = "⚙";
        public const String Alert = "⚠";
        #endregion

        #region Format Strings

        #region Float
        public const String F0 = "{0:F0}";
        public const String F3 = "{0:F3}";
        #endregion

        #region Menu Items
        public const String SEPERATOR = "-";
        #endregion

        #endregion /Format Strings

        #region Types
        public static readonly Type StringType = Type.GetType("System.String");
        #endregion

        #region Binary State
        public const String LOW = "Low";
        public const String HIGH = "High";
        #endregion /Binary State

        #region Binary Data Size
        public const String bitStr8 = "8-Bit";
        public const String bitStr16 = "16-Bit";
        public const String bitStr32 = "32-Bit";
        public const String bitStr64 = "64-Bit";
        public const UInt32 ONE_KB = 1024;
        public const UInt32 ONE_MB = (ONE_KB * ONE_KB);

        public const Byte WORD_BIT_0_BIG = 0;
        public const Byte WORD_BIT_1_BIG = 1;
        public const Byte WORD_BIT_2_BIG = 2;
        public const Byte WORD_BIT_3_BIG = 3;
        public const Byte WORD_BIT_4_BIG = 4;
        public const Byte WORD_BIT_5_BIG = 5;
        public const Byte WORD_BIT_6_BIG = 6;
        public const Byte WORD_BIT_7_BIG = 7;
        public const Byte WORD_BIT_8_BIG = 8;
        public const Byte WORD_BIT_9_BIG = 9;
        public const Byte WORD_BIT_10_BIG = 10;
        public const Byte WORD_BIT_11_BIG = 11;
        public const Byte WORD_BIT_12_BIG = 12;
        public const Byte WORD_BIT_13_BIG = 13;
        public const Byte WORD_BIT_14_BIG = 14;
        public const Byte WORD_BIT_15_BIG = 15;
        public const Byte WORD_BIT_16_BIG = 16;
        public const Byte WORD_BIT_17_BIG = 17;
        public const Byte WORD_BIT_18_BIG = 18;
        public const Byte WORD_BIT_19_BIG = 19;
        public const Byte WORD_BIT_20_BIG = 20;
        public const Byte WORD_BIT_21_BIG = 21;
        public const Byte WORD_BIT_22_BIG = 22;
        public const Byte WORD_BIT_23_BIG = 23;
        public const Byte WORD_BIT_24_BIG = 24;
        public const Byte WORD_BIT_25_BIG = 25;
        public const Byte WORD_BIT_26_BIG = 26;
        public const Byte WORD_BIT_27_BIG = 27;
        public const Byte WORD_BIT_28_BIG = 28;
        public const Byte WORD_BIT_29_BIG = 29;
        public const Byte WORD_BIT_30_BIG = 30;
        public const Byte WORD_BIT_31_BIG = 31;

        public const Byte WORD_16_BIT_0_LITTLE = 15;
        public const Byte WORD_16_BIT_1_LITTLE = 14;
        public const Byte WORD_16_BIT_2_LITTLE = 13;
        public const Byte WORD_16_BIT_3_LITTLE = 12;
        public const Byte WORD_16_BIT_4_LITTLE = 11;
        public const Byte WORD_16_BIT_5_LITTLE = 10;
        public const Byte WORD_16_BIT_6_LITTLE = 9;
        public const Byte WORD_16_BIT_7_LITTLE = 8;
        public const Byte WORD_16_BIT_8_LITTLE = 7;
        public const Byte WORD_16_BIT_9_LITTLE = 6;
        public const Byte WORD_16_BIT_10_LITTLE = 5;
        public const Byte WORD_16_BIT_11_LITTLE = 4;
        public const Byte WORD_16_BIT_12_LITTLE = 3;
        public const Byte WORD_16_BIT_13_LITTLE = 2;
        public const Byte WORD_16_BIT_14_LITTLE = 1;
        public const Byte WORD_16_BIT_15_LITTLE = 0;

        public const Byte WORD_32_BIT_0_LITTLE = 31;
        public const Byte WORD_32_BIT_1_LITTLE = 30;
        public const Byte WORD_32_BIT_2_LITTLE = 29;
        public const Byte WORD_32_BIT_3_LITTLE = 28;
        public const Byte WORD_32_BIT_4_LITTLE = 27;
        public const Byte WORD_32_BIT_5_LITTLE = 26;
        public const Byte WORD_32_BIT_6_LITTLE = 25;
        public const Byte WORD_32_BIT_7_LITTLE = 24;
        public const Byte WORD_32_BIT_8_LITTLE = 23;
        public const Byte WORD_32_BIT_9_LITTLE = 22;
        public const Byte WORD_32_BIT_10_LITTLE = 21;
        public const Byte WORD_32_BIT_11_LITTLE = 20;
        public const Byte WORD_32_BIT_12_LITTLE = 19;
        public const Byte WORD_32_BIT_13_LITTLE = 18;
        public const Byte WORD_32_BIT_14_LITTLE = 17;
        public const Byte WORD_32_BIT_15_LITTLE = 16;
        public const Byte WORD_32_BIT_16_LITTLE = 15;
        public const Byte WORD_32_BIT_17_LITTLE = 14;
        public const Byte WORD_32_BIT_18_LITTLE = 13;
        public const Byte WORD_32_BIT_19_LITTLE = 12;
        public const Byte WORD_32_BIT_20_LITTLE = 11;
        public const Byte WORD_32_BIT_21_LITTLE = 10;
        public const Byte WORD_32_BIT_22_LITTLE = 9;
        public const Byte WORD_32_BIT_23_LITTLE = 8;
        public const Byte WORD_32_BIT_24_LITTLE = 7;
        public const Byte WORD_32_BIT_25_LITTLE = 6;
        public const Byte WORD_32_BIT_26_LITTLE = 5;
        public const Byte WORD_32_BIT_27_LITTLE = 4;
        public const Byte WORD_32_BIT_28_LITTLE = 3;
        public const Byte WORD_32_BIT_29_LITTLE = 2;
        public const Byte WORD_32_BIT_30_LITTLE = 1;
        public const Byte WORD_32_BIT_31_LITTLE = 0;

        public const String BinaryZer0_32Bit = "00000000000000000000000000000000";
        #endregion /Binary Data Size

        #region Virtual Drive
        public const String VirtualName = "Datam Virtual Drive";
        #endregion

        #region CiA309-3
        public const char WRITE_309_3 = 'w';
        public const char READ_309_3 = 'r';

        public const String BOOL_309_3 = "b";
        public const String INT8_309_3 = "i8";
        public const String INT16_309_3 = "i16";
        public const String INT24_309_3 = "i24";
        public const String INT32_309_3 = "i32";
        public const String INT40_309_3 = "i40";
        public const String INT48_309_3 = "i48";
        public const String INT56_309_3 = "i56";
        public const String INT64_309_3 = "i64";
        public const String UINT8_309_3 = "u8";
        public const String UINT16_309_3 = "u16";
        public const String UINT24_309_3 = "u24";
        public const String UINT32_309_3 = "u32";
        public const String UINT40_309_3 = "u40";
        public const String UINT48_309_3 = "u48";
        public const String UINT56_309_3 = "u56";
        public const String UINT64_309_3 = "u64";
        public const String REAL32_309_3 = "r32";
        public const String REAL64_309_3 = "r64";
        public const String STRING_309_3 = "vs";
        public const String OCTET_STRING_309_3 = "os";
        public const String UNICODE_309_3 = "us";
        public const String DOMAIN_309_3 = "d";
        public const String DATE_TIME_309_3 = "t";
        public const String TIMESPAN_309_3 = "td";

        #endregion

        #region CiA402
        public static readonly int ETHERCAT = Convert.ToInt32(ProtocolType.EtherCAT);
        public static readonly int ALLNET = Convert.ToInt32(ProtocolType.ALLNET);
        #endregion

        #region Numbers
        public const char MINUS_SIGN = '-';

        public const String TRUE = "TRUE";
        public const String FALSE = "FALSE";
    
        /// <summary>
        /// Space
        /// </summary>
        public const String _0 = "0";
        public const String _1 = "1";
        public const String _2 = "2";
        public const String _3 = "3";
        public const String _4 = "4";
        public const String _5 = "5";
        public const String _6 = "6";
        public const String _7 = "7";
        public const String _8 = "8";
        public const String _9 = "9";

        public const String _00 = "00";
        public const String _01 = "01";
        public const String _02 = "02";
        public const String _03 = "03";
        public const String _04 = "04";
        public const String _05 = "05";
        public const String _06 = "06";
        public const String _07 = "07";
        public const String _08 = "08";
        public const String _09 = "09";
        public const String _0a = "0a";
        public const String _0A = "0A";
        public const String _0b = "0b";
        public const String _0B = "0B";
        public const String _0c = "0c";
        public const String _0C = "0C";
        public const String _0d = "0d";
        public const String _0D = "0D";
        public const String _0f = "0f";
        public const String _0F = "0F";
        public const String _10 = "10";
       
        public const String _11 = "11";
        public const String _12 = "12";
        public const String _13 = "13";
        public const String _14 = "14";
        public const String _15 = "15";
        public const String _16 = "16";
        public const String _17 = "17";
        public const String _18 = "18";
        public const String _19 = "19";
        public const String _1a = "1a";
        public const String _1A = "1A";
        public const String _1b = "1b";
        public const String _1B = "1B";
        public const String _1c = "1c";
        public const String _1C = "1C";

        public const String ONE = "1";
        public const String TWO = "2";
        public const String THREE = "3";
        public const String FOUR = "4";
        public const String FIVE = "5";
        public const String SIX = "6";
        public const String SEVEN = "7";
        public const String EIGHT = "8";
        public const String NINE = "9";
        public const String TEN = "10";
        public const String ELEVEN = "11";
        public const String TWELVE = "12";
        public const String THIRTEEN = "13";
        public const String FOURTEEN = "14";
        public const String FIFTEEN = "15";
        public const String SIXTEEN = "16";
        public const String SEVENTEEN = "17";
        public const String EIGHTEEN = "18";
        public const String NINETEEN = "19";
        public const String TWENTY = "20";

        public const int THOUSAND = 1000;
        #endregion

        #region Keys
        public const Char START_TOKEN = '$';

        public const String FLASH_SAVE_KEY = "0x65766173";
        public const String AUTH_KEY = "0x58713138";
        #endregion

        #region Network Management
        public const String START_NMT = "start";
        public const String PREOP_NMT = "preop";
        public const String STOP_NMT = "stop";
        #endregion

        #region Char
        public const Char SINGLE_CHAR = '$';
        public const String SINGLE_CHAR_STRING = "$";
        #endregion

        #region Win32
        public const int WM_SET_REDRAW = 11; //TODO: numbers method?
        #endregion

        #region Key Terms
        public const String EXPONENT_CONVERSION_STRING = "{0:#.########E+0}";
        public const String EXPONENT_STRING = "E";

        public const uint INVALID_NODE_ID = uint.MaxValue;
        public const ulong INVALID_SEQUENCE_NUMBER = ulong.MaxValue;

        public const String Empty = "";
        public const String _S_ = " ";
        public const Char _s_ = ' ';
        public static readonly String _nl_ = Environment.NewLine;
        public const String FullQuotation = "\"";

        public const Char CARRAGE_RETURN_CHAR = '\r';
        public const String CARRAGE_RETURN_STRING = "\r";
        public const Char NEW_LINE_CHAR = '\n';
        public const String NEW_LINE_STRING = "\n";
        public const String NEW_LINE_STRING_2 = "\n\n";
        public const Char TERMINAL_CHAR = '\0';
        public const String TERMINAL_CHAR_STRING = "\0";

        public static readonly byte CARRAGE_RETURN_BYTE = Convert.ToByte(CARRAGE_RETURN_CHAR);
        public static readonly byte NEW_LINE_BYTE = Convert.ToByte(NEW_LINE_CHAR);
        public static readonly byte TERMINAL_CHAR_BYTE = Convert.ToByte(TERMINAL_CHAR);

        public const String EOF = "EOF";
        public const String SET = "Set";
        public const String LOAD = "Load";
        public const String SAVE = "Save";
        public const String READ = "Read";
        public const String COMPARE = "Compare";
        public const String DUPLICATE = "Duplicate";

        public const String PARAMETER = "Parameter";
        public const String PARAMETERS = "Parameters";
        public const String REGISTRANT = "Registrant";
        public const String REGISTRANTS = "Registrants";

        public const String REGISTRATION_TICKET = "RegistrationTicket";

        public const String POLYMORPHIC_CONTROL = "Polymorphic Control";

      
        public const String ERROR = "ERROR";
        public const String ERROR_RESULT = "ERROR:";
        public const String OK_RESULT = "OK";
        public const string RUNTIME_ERROR = "Runtime Error";

        public const String BITWORD_DEFAULT_VALUE_16 = "0000000000000000";
        //public const String OPEN_TREE_TEXT = ">>";
        //public const String CLOSE_TREE_TEXT = "<<";
        public const String SCAN = "Scan";
        //Device Tree
        public const String CANOPEN = "CANopen";
    
        public const String MOTOR = "Motor";

        public const String NONE = "None";

        public const String VALID = "Valid";

        public const String DS402 = "DS402";

        public const String PARAMETER_CAPTURE = "Parameter Capture";
        public const String INFORMATION = "Information";
        public const String UNKNOWN = "Unknown";

        public const String ABSOLUTE = "Absolute";
        public const String RELATIVE = "Relative";

        public const String INVALID = "Invalid";
        public const String INITIALIZED = "Initialized";
        public const String COM = "COM";

        public const String INDEX = "Index";
        public const String SUBINDEX = "Subindex";
        public const String TITLE = "Title";
        public const String NAME = "Name";
        public const String _e_= "e";
        public const String _h_ = "h";
        public const String _Blank_ = "";
        public const String EEPROM = "EEPROM";
        public const String LIST_LEVEL = "List Level";
        public const String DEVICE_ID = "DeviceID";
        public const String SERVICE = "Service";

        public const String UNDEFINED = "Undefined";

        public const String CODE = "Code";
        public const String DESCRIPTION = "Description";

        public const String GROUP = "Group";
        public const String MIN = "Min";
        public const String MAX = "Max";
        public const String TYPE = "Type";
        public const String PROPERTY = "Property";

        public const String ADDRESS = "Address";
        public const String ACCESS_RIGHTS = "Access Rights";
        public const String ACCESS_LEVEL = "Access Level";
        public const String UNIT = "Unit";
        public const String UNIT_SCALE = "Unit Scale";
        public const String DISPLAYUNIT = "Display Unit";
        public const String DISPLAYSCALE = "Display Scale";
        public const String DEFAULT_VALUE = "DefaultValue";
        public const String SECTION = "Section";
        public const String INDEXADDRESS = "IndexAddress";
        public const String PARAMETERNAME = "Parameter Name";
        public const String PERRATED = "Per Rated";

        public const String TOP = "TOP";
        public const String ITEMS = "Items";
        public const String VALUE = "Value";
        public const String ENUMERATION = "Enumeration";
        public const String GENERAL = "General";
        public const String ENCODER = "Encoder";
        public const String REMOTE = "Remote";
        public const String BRAKE = "Brake";
        public const String WATCHDAWG = "Watchdog";
        public const String MANUFACTURER = "Manufacturer Data";
        public const String TEMPERATURE = "Temperature";
        public const String VL = "Velocity Mode";
        public const String PV = "Profile Velocity Mode";
        public const String TQ = "Profile Torque Mode";
        public const String PHASE_A = "Phase A";
        public const String PHASE_B = "Phase B";
        public const String PHASE_C = "Phase C";
        public const String DIRECT_AXIS = "Direct Axis";
        public const String QUADRATURE_AXIS = "Quadrature Axis";
        public const String ALPHA_MOTOR_VOLTAGE = "Alpha Motor Voltage";
        public const String BETA_MOTOR_VOLTAGE = "Beta Motor Voltage";
        public const String VERSION = "Version";
        public const String VELOCITY_LOOP = "Velocity Loop";
        public const String POSITION = "Position";
        public const String VELOCITY = "Velocity";

        public const String JSON_STRING = "JsonString";

        public const String TRIGGER_MODE = "Trigger Mode";
        public const String TRIGGER_TYPE = "Trigger Type";

        public const String DATAM_CONFIGURATION_DATA = "DatamConfigurationData";

        public const String RO = "ro";
        public const String WO = "wo";
        public const String RW = "rw";
        public const String RWW = "rww";
        public const String RWR = "rwr";
        public const String CONST = "const";
        #endregion

        #region User Application Directory
        public const String IMAGE_DIRECTORY_COMMON = "Common.Image.";
        public const String IMAGE_DIRECTORY_DATAM = "Datam.Image.";
        public const String IMAGE_DIRECTORY_DATAM_WINFORMS = "Datam.WinForms.Image.";

        public static readonly String Path_ApplicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly String Path_DatamApplicationDirectory = Path.Combine(Path_ApplicationDataDirectory, DATAM);
        public static readonly String Path_DatamConfigurationDirectory = Path.Combine(Path_DatamApplicationDirectory, "Configuration");
        public static readonly String DATAM_CONFIG_PREFIX = "Datam_";
        public const String JSON_EXT = ".json";
        public const String DATAM_CONFIG_EXT = ".dad";
        #endregion

        #region Datam Data Dictionary
     
        public const String DATAM = "Datam";
        private const String BINARIES = "Binaries";
        private const String USER_DATA = "User Data";
        private const String MANUALS = "Manuals";
        private const String JSON_DATA = "JsonData";
        private const String ALLIED_MOTION = "Allied Motion";
        public const String DAD_DIR_PATH = "DatumAddressDictionary";
        private const String DAD_FILENAME = "dad.bin";
        private const String DOCK_STATE_DIR_PATH = "DockState";
        private const String STATE_FILENAME = "dockstate.bin";
        public const String DEFAULT_LOG_NAME = DATAM;
        private const String GUI_MANUAL_FILENAME = "DatamUserGuide.pdf";

        private const string DIAGNOSTICS_NOTES_FILENAME = "N.bin";

        public const String FILE_DIALOG_LOG_EXT = ".log";
        public const String FILE_DIALOG_FILTER_LOGS = "log files (*.log)|*.log|txt files (*.txt)|*.txt|All files (*.*)|*.*";

        //Check the program files folder
        public static readonly String PROGRAM_FILES_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static readonly String PROGRAMFILES_DATAM_PATH = Path.Combine(PROGRAM_FILES_PATH,ALLIED_MOTION, DATAM);
        public static readonly String PROGRAMFILES_JSON_DATA_PATH = Path.Combine(PROGRAMFILES_DATAM_PATH, JSON_DATA);


        public static readonly String DOCUMENTS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static readonly String DOCUMENTS_DATAM_PATH = Path.Combine(DOCUMENTS_PATH, DATAM);
        // Roaming App data is user specific info.
        public static readonly String APP_DATA_ROAMING_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DATAM);
        // Local App Data is installed files.
        public static readonly String APP_DATA_LOCAL_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly String APP_DATA_ALLIED_MOTION_PATH = Path.Combine(APP_DATA_LOCAL_PATH, ALLIED_MOTION);
        public static readonly String APP_DATA_DATAM_PATH = Path.Combine(APP_DATA_ALLIED_MOTION_PATH, DATAM);
        public static readonly String MANUALS_PATH = Path.Combine(APP_DATA_DATAM_PATH, MANUALS);
        public static readonly String LOG_FILES_PATH = Path.Combine(APP_DATA_DATAM_PATH, "Logs");
    
        public static readonly String GUI_MANUAL_PATH = Path.Combine(MANUALS_PATH, GUI_MANUAL_FILENAME);
        public static readonly String JSON_DATA_PATH = Path.Combine(APP_DATA_DATAM_PATH, JSON_DATA);
       

        public static readonly String BINARIES_PATH = Path.Combine(APP_DATA_DATAM_PATH, BINARIES);
        public static readonly String NOTES_FILE_PATH = Path.Combine(BINARIES_PATH, DIAGNOSTICS_NOTES_FILENAME);
        public static readonly String USER_DATA_PATH = Path.Combine(APP_DATA_DATAM_PATH, USER_DATA);
        public static readonly String DockStateDirectory = Path.Combine(APP_DATA_DATAM_PATH, DOCK_STATE_DIR_PATH);
        public static readonly String DockStateFilePath = Path.Combine(DockStateDirectory, STATE_FILENAME);
        public static readonly String DataAddressDictionaryDirectory = Path.Combine(APP_DATA_DATAM_PATH, DAD_DIR_PATH);
        #endregion /Datam Data Dictionary

        #region Format String
        public const string X4 = "X4";
        public const string X1 = "X1";
        #endregion

        #region Communicator Commands

        #region General Commands
        public const String CANCEL = "_cancel";
        #endregion

        #region Motion Commands
        public const String MOTION_START = "_motion start";
        public const String MOTION_CLEAR = "_motion clear";// Clears the sotred motions.
        public const String MOTION_DELAY = "_motion a delay ";// Requires additional specification on delay time in ms.
        public const String MOTION_ABSOLUTE = "_motion a moveabs ";// Requires additional specification on position, velocity, accel limits, decel limits.
        public const String MOTION_RELATIVE = "_motion a moverel ";// Requires additional specification on position, velocity, accel limits, decel limits.
        public const String MOTION_WAIT_TARGET = "_motion a wftr";// Wait for target reached.
        #endregion

        #endregion /Communicator Commands

        #region Communicator Lookup
        public const String COMMUNICATOR_ID = @"VID_058B&PID_0058";
        #endregion

        #region Hex
        public const String Ox = "0x";
        public const String OOOO = "0000";// the land of oooo.
        #endregion /Hex

        #region Size
        public static readonly Size ZeroSize = new Size(0, 0);
        #endregion /Size 

        #region Max Size
        public const Double UBoolMaxSize = 1;
        public const Double UBoolMinSize = 0;
        public const Double U8MaxSize = 255;
        public const Double U8MinSize = 0;
        public const Double U16MaxSize = 65535;
        public const Double U16MinSize = 0;
        public const Double U24MaxSize = 16777215;
        public const Double U24MinSize = 0;
        public const Double U32MaxSize = 4294967295;
        public const Double U32MinSize = 0;
        public const Double U48MaxSize = 281474976710655;
        public const Double U48MinSize = 0;
        public const Double U56MaxSize = 72057594037927935;
        public const Double U56MinSize = 0;
        public const Double U64MaxSize = 18446744073709551615;
        public const Double U64MinSize = 0;

        public const Double I8MaxSize = 127;
        public const Double I8MinSize = -128;
        public const Double I16MaxSize = 32767;
        public const Double I16MinSize = -32768;
        public const Double I24MaxSize = 8388607;
        public const Double I24MinSize = -8388608;
        public const Double R32MaxSize = 2147483647;
        public const Double R32MinSize = -2147483648;
        public const Double I32MaxSize = 2147483647;
        public const Double I32MinSize = -2147483648;
        public const Double I48MaxSize = 140737488355327;
        public const Double I48MinSize = -140737488355328;
        public const Double I56MaxSize = 36028797018963967;
        public const Double I56MinSize = -36028797018963968;
        public const Double R64MaxSize = 9223372036854775807;
        public const Double R64MinSize = -9223372036854775808;
        public const Double I64MaxSize = 9223372036854775807;
        public const Double I64MinSize = -9223372036854775808;

       
        #endregion

        #region Unsigned
        public const uint U1 = 1;
        public const uint U2 = 2;
        public const uint U4 = 4;
        public const uint U8 = 8;
        public const uint U16 = 16;
        public const uint U32 = 32;
        public const uint U64 = 64;
        public const uint U128 = 128;
        public const uint U256 = 256;
        public const uint U512 = 512;
        public const uint U1024 = 1024;
        public const uint U2048 = 2048;
        public const uint U4096 = 4096;
        public const uint U8192 = 8192;
        public const uint U16384 = 16384;
        public const uint U32768 = 32768;
        public const uint U65536 = 65536;
        public const uint U131072 = 131072;
        public const uint U262144 = 262144;
        public const uint U524288 = 524288;
        public const uint U1048576 = 1048576;
        public const uint U2097152 = 2097152;
        public const uint U4194304 = 4194304;
        public const uint U8388608 = 8388608;
        public const uint U16777216 = 16777216;
        public const uint U33554432 = 33554432;
        public const uint U67108864 = 67108864;
        public const uint U134217728 = 134217728;
        public const uint U268435456 = 268435456;
        public const uint U536870912 = 536870912;
        public const uint U1073741824 = 1073741824;
        public const uint U2147483648 = 2147483648;
        #endregion
    }
}
