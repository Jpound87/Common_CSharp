using Common;
using Common.Constant;
using Common.Extensions;
using Common.Utility;
using Runtime.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

/**********************************************************************************************************************************************************
------------------------------------------------------------------=General Info=---------------------------------------------------------------------------
This is the class that handles translations for the Datam program. To add a word or phrase to the dictionary, a public const string must be added as a key.
Next, the word or phrase must be added to the dictionary “Words”. It has one key and a string array of the available translations. The number of strings in
the array is equal to the number of available language translations. The location in the array corresponds to the language the translation is. For the list
of available translations in order, see the ComboBox “languages” in frmSettings. Once the word or phrase has been added to the dictionary, a getter must be
made for it (see the bottom of this file). This getter allows for the use of translations to follow the syntax: TranslationManager.word. The getter then
automatically (through the use of the Find function) finds the proper translation based on the current language setting. When translating a word in a form,
it’s important to use the lowercase (camelCase?) version of the word in ‘TranslationManager.word’, as this is the one with the getter that will look at the
current language setting.
-CAH
--------------------------------------------------------------=ComboBox Translations=----------------------------------------------------------------------
For ComboBoxItem translation, arrays of keys and values must be provided in each individual form. Once this is done, the
"PopulateComboBoxItemDictionary" function can be called with these arrays to generate a dictionary containing all the translations for that
ComboBox. Whenever the form is retranslated, a function is called to retranslate the CBOs. For an example, see  the Translate function in
frmSettings.cs.
-CAH
**********************************************************************************************************************************************************/

namespace Runtime
{
    public static class Translation_Manager
    {
        #region Identity
        public const String ClassName = nameof(Translation_Manager);
        #endregion /Identity

        #region ComboBox Choice Array

        private static readonly ComboBoxItem[] choiceBoxes_Languages = new ComboBoxItem[LANGUAGE_COUNT]
        {
            new ComboBoxItem("English", Languages.English),
            new ComboBoxItem("Español", Languages.Español),
            new ComboBoxItem("Deutsch", Languages.Deutsch),
            new ComboBoxItem("Português", Languages.Português),
            new ComboBoxItem("中文", Languages.中文),
            new ComboBoxItem("Français", Languages.Français),
            new ComboBoxItem("Hindi", Languages.Hindi)
        };

        public static ComboBoxItem[] ChoiceBoxes_Languages
        {
            get
            {
                return choiceBoxes_Languages;
            }
        }
        #endregion /ComboBox Choice Array

        #region Language Tokens

        private static readonly String[] UnknownTokens = EnumArrays.GetLanguageTokens();

        private static readonly String E = UnknownTokens[(byte)Languages.English];
        private static readonly String S = UnknownTokens[(byte)Languages.Español];
        private static readonly String G = UnknownTokens[(byte)Languages.Deutsch];
        private static readonly String P = UnknownTokens[(byte)Languages.Português];
        private static readonly String C = UnknownTokens[(byte)Languages.中文];
        private static readonly String F = UnknownTokens[(byte)Languages.Français];
        private static readonly String H = UnknownTokens[(byte)Languages.Hindi];

        #endregion /Language Tokens

        #region Language Settings
        public static event MethodInvoker LanguageChanged;
        public static void SettingUpdate(Languages languageSetting)
        {
            if (currentLanguage != languageSetting)
            {
                currentLanguage = languageSetting;
                LanguageChanged?.Invoke();
            }
        }
        private static Languages currentLanguage = Languages.English;
        /// <summary>
        /// This integer stores the current language value. It Defaults to zero (English), and it is set through events like the language setting in Settings Changing.
        /// </summary>
        public static int CurrentLanguageIndex
        {
            get
            {
                return currentLanguage.GetValue_Int();
            }
        }

        public static Languages CurrentLanguage
        {
            get
            {
                return currentLanguage;
            }
        }
        #endregion /Language Settings

        #region Combobox Lists

        #region User Level
        public static readonly IDictionary<Func<String>, Object> cboUserLevels = new Dictionary<Func<String>, Object>
        {
            { ()=>{return $"{Safety}"; }, 0 },
            { ()=>{return $"{Standard}"; }, 1 },
            { ()=>{return $"{Advanced}"; }, 2 },
            { ()=>{return $"{Authorized}"; }, 3 },
            { ()=>{return $"{Allied}"; }, 4 },
        };
        private static readonly ComboBoxTranslationData cboTranslation_UserLevel = new ComboBoxTranslationData(cboUserLevels);
        public static ComboBoxItem[] ComboBoxItems_UserLevel
        {
            get
            {
                return cboTranslation_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion /User Level

        #region Fault Injection
        private static readonly IDictionary<Func<String>, Object> cboFaultInjection = new Dictionary<Func<String>, Object>
        {
            { ()=>{return $"{Reset}"; }, "0" },
            { ()=>{return $"{DriveTempLimit}"; }, "1" },
            { ()=>{return $"{MotorTempLimit}"; }, "2" },
            { ()=>{return $"{LoadLimit}"; }, "3" },
            { ()=>{return $"{ResetWarnings}"; }, "4" },
            { ()=>{return $"{FaultOnAddress}"; }, "5" },
            { ()=>{return $"{Overvoltage}"; }, "6" }
        };
        private static readonly ComboBoxTranslationData cboFaultInjection_UserLevel = new ComboBoxTranslationData(cboFaultInjection);
        public static ComboBoxItem[] ComboBoxItems_FaultInjection
        {
            get
            {
                return cboFaultInjection_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion /Fault Injection

        #region Polarity
        private static readonly IDictionary<Func<String>, Object> cboPolarity = new Dictionary<Func<String>, Object>
        {
            { ()=>{return $"{Positive}"; }, 0 },
            { ()=>{return $"{Negative}"; }, 1 }
        };
        private static readonly ComboBoxTranslationData cboPolarity_UserLevel = new ComboBoxTranslationData(cboPolarity);
        public static ComboBoxItem[] ComboBoxItems_Polarity
        {
            get
            {
                return cboPolarity_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion /Polarity

        #region Motion Profile
        private static readonly IDictionary<Func<String>, Object> cboMotionProfileType = new Dictionary<Func<String>, Object>
        {
            { ()=>{return $"{LinearRamp}"; }, 0 }
        };
        private static readonly ComboBoxTranslationData cboMotionProfileType_UserLevel = new ComboBoxTranslationData(cboMotionProfileType);
        public static ComboBoxItem[] ComboBoxItems_MotionProfileType
        {
            get
            {
                return cboMotionProfileType_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion Motion Profile

        #region Halt Options
        private static readonly IDictionary<Func<String>, Object> cboHaltOptions = new Dictionary<Func<String>, Object>
        {
            { ()=>{return $"{SlowDownRamp}"; }, "1" },
            { ()=>{return $"{QuickStopRamp}"; }, "2" },
            { ()=>{return $"{CurrentLim}"; }, "3" },
            { ()=>{return $"{VoltageLim}"; }, "4" }
        };
        private static readonly ComboBoxTranslationData cboHaltOptions_UserLevel = new ComboBoxTranslationData(cboHaltOptions);
        public static ComboBoxItem[] ComboBoxItems_HaltOptions
        {
            get
            {
                return cboHaltOptions_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion /Halt Options

        #region Dock Areas
        private static readonly IDictionary<Func<String>, Object> cboDockAreas = new Dictionary<Func<String>, Object>
        {
            {()=>{return $"{Document}"; }, DockStyle.Fill },
            {()=>{return $"{Right}"; }, DockStyle.Right },
            {()=>{return $"{Bottom}"; }, DockStyle.Bottom },
            {()=>{return $"{Top}"; }, DockStyle.Top },
            {()=>{return $"{Left}"; }, DockStyle.Left }
        };
        private static readonly ComboBoxTranslationData cboDockAreas_UserLevel = new(cboDockAreas);
        public static ComboBoxItem[] ComboBoxItems_DockAreas
        {
            get
            {
                return cboDockAreas_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion /Dock Areas

        #region Log Level
        private static readonly IDictionary<Func<String>, Object> cboLogLevel = new Dictionary<Func<String>, Object>
        {
            {()=>{return $"{Verbose}"; }, Priority_Log.Verbose },
            {()=>{return $"{Debug}"; }, Priority_Log.Debug },
            {()=>{return $"{Info}"; }, Priority_Log.Information },
            {()=>{return $"{Warning}"; }, Priority_Log.Warning },
            {()=>{return $"{Error}"; }, Priority_Log.Error },
            {()=>{return $"{Assert}"; }, Priority_Log.Assert }
        };
        private static readonly ComboBoxTranslationData cboLogLevelArray_UserLevel = new ComboBoxTranslationData(cboLogLevel);
        public static ComboBoxItem[] ComboBoxItems_LogLevel
        {
            get
            {
                return cboLogLevelArray_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion /Log Level

        #region Trigger
        private static readonly IDictionary<Func<String>, Object> cboTriggerMode = new Dictionary<Func<String>, Object>
        {
            {()=>{return $"{Immediate}"; }, References_Allied.dictTriggerModeEnum_TriggerModeStr[TriggerMode.SINGLE] },
            {()=>{return "T>=SP"; }, References_Allied.dictTriggerModeEnum_TriggerModeStr[TriggerMode.ABOVE] },
            {()=>{return "T<=SP"; }, References_Allied.dictTriggerModeEnum_TriggerModeStr[TriggerMode.BELOW] },
            {()=>{return $"{OutsideSPs}"; }, References_Allied.dictTriggerModeEnum_TriggerModeStr[TriggerMode.OUTSIDE] },
            {()=>{return $"{WithinSPs}"; }, References_Allied.dictTriggerModeEnum_TriggerModeStr[TriggerMode.WITHIN] }
        };
        private static readonly ComboBoxTranslationData cboTriggerMode_UserLevel = new ComboBoxTranslationData(cboTriggerMode);
        public static ComboBoxItem[] ComboBoxItems_TriggerMode
        {
            get
            {
                return cboTriggerMode_UserLevel[CurrentLanguageIndex];
            }
        }
        #endregion /Trigger

        #region Translation R3gistration
        // That was a typo but I kept it.
        private static readonly IDictionary<Object, Action> dictRegistrant_OnLanguageChanged = new Dictionary<Object, Action>();
        private static void RegisterForTranslation(Object registrant, Object translationObject)
        {
            // TODO: this may not be as useful as I was thinking, maybe later
        }
        #endregion /Translation R3gistration

        #region Right & Left

        private static readonly IDictionary<Func<String>, Object> cboHorizontalDirection = new Dictionary<Func<String>, Object>
        {
            {()=>{return $"{Right}"; }, HorizontalDirection.Right },
            {()=>{return $"{Left}"; }, HorizontalDirection.Left },
        };
        private static readonly ComboBoxTranslationData cboHorizontalDirection_Translation = new ComboBoxTranslationData(cboFaultInjection);
        public static ComboBoxItem[] CboHorizontalDirection_Translation
        {
            get
            {
                return cboHorizontalDirection_Translation[CurrentLanguageIndex];
            }
        }
        #endregion /Right & Left

        #region Network Management State

        private static readonly IDictionary<Func<String>, Object> cboNetworkManagementState = new Dictionary<Func<String>, Object>
        {
            {()=>{return $"{PreOp}"; }, Tokens.PREOP_NMT },
            {()=>{return $"{Start}"; }, Tokens.START_NMT },
            {()=>{return $"{Stop}"; }, Tokens.STOP_NMT }
        };
        private static readonly ComboBoxTranslationData cboNetworkManagementState_Translation = new ComboBoxTranslationData(cboFaultInjection);
        public static ComboBoxItem[] CboNetworkManagementState_Translation
        {
            get
            {
                return cboNetworkManagementState_Translation[CurrentLanguageIndex];
            }
        }
        #endregion /Network Management State

        #region Update List
        public static void UpdateComboboxTranslations(this ComboBox comboBox, ComboBoxItem[] items)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(items);
        }

        #endregion

        #endregion /Combobox Lists

        #region Avalable Languages Information

        // This is used to count number of languages that are translatable.
        // (Used in Words dictionary to determine necessary string length)
        // It needs to be a constant (hard-coded) so that the Words dictionary can use it.
        public const int LANGUAGE_COUNT = 7;

        //CAH - Added this
        /// <summary>
        /// This dictionary enumerates the languages translatable
        /// </summary>
        private static readonly IDictionary<Languages, String> languages = new Dictionary<Languages, String>
        {
            { Languages.English, "English" },
            { Languages.Español, "Español" },
            { Languages.Deutsch, "Deutsch" },
            { Languages.Português, "Português"},
            { Languages.中文, "中文"},
            { Languages.Français, "Français"},
            { Languages.Hindi, "Hindi"}
        };
        public static IDictionary<Languages, String> AvailableLanguagesList
        {
            get
            {
                return languages;
            }
        }
        #endregion /Avalable Languages Information

        #region Constants
        //These are constants that correspond to a dictionary string key
        #region General

        #region A
        public const string ABORT = "Abort";
        public const string ABOUT = "About";
        public const string ABSOLUTE = "Absolute";
        public const string ACCELERATION = "Acceleration";
        public const string ACCEL_DELTA_SPEED = "Acceleration Delta Speed";
        public const string ACCEL_DELTA_TIME = "Acceleration Delta Time";
        public const string ACCEL_LIMIT = "Acceleration Limit";
        public const string ACCEPT = "Accept";
        public const string ACCEPT_VALUES = "Accept Values";
        public const string ACCEL_LEVEL = "Acceleration Level Selected";
        public const string ACCEL_NONNEG = "Acceleration can't be negative";
        public const string ACCEL_UPDATED = "Acceleration updated to {0}";
        public const string ACTIVE = "Active";
        public const string ACTIVE_FAULT_COUNT = "Active Fault Count";
        public const string ACTIVE_WARN_COUNT = "Active Warning Count";
        public const string ACTUAL_POSITION = "Actual Position";
        public const string ACTUAL_VELOCITY = "Actual Velocity";
        public const string ADD = "Add";
        public const string ADD_AXIS_PARAMETER = "Add Axis Parameter";
        public const string ADDED_DATASHEETS = "Added Datasheets";
        public const string ADD_SERIAL_NUMBER = "Add Serial Number";
        public const string ADD_UPDATE = "Add/Update";
        public const string ADD_UPDATE_DATASHEET = "Open a dialog to add or update a datasheet from a file.";
        public const string ADD_UPDATE_DATASHEET_INSTR = "Add/Update Datasheet Instructions";
        public const string ADDRESS = "Address";
        public const string ADVANCED = "Advanced";
        public const string ALIGNMENT = "Alignment";
        public const string ALLIED = "Allied";
        public const string ALLNET = "ALLNET device found at IP: ";
        public const string ALLOW_WINDOWS = "Allow All Windows";
        public const string ANALOG = "Analog";
        public const string ANALOG_INPUT1 = "Analog Input 1";
        public const string ANALOG_INPUT2 = "Analog Input 2";
        public const string ANALYZE = "Analyze";
        public const string APPLICATION = "Application";
        public const string APP_SETTINGS = "App Settings";
        public const string APPLY = "Apply";
        public const string AQUIRING_CONFIG_PARAMS = "Acquiring Configuration Parameters...";
        public const string AQUIRING_MANUFAC_PARAMS = "Acquiring Manufacturing Configuration Parameters...";
        public const string ASSERT = "Assert";
        public const string ATTACHING_A_COMMUNICATOR = "Attaching a Communicator";
        public const string ATTEMPT_CONNECT = "Attempting to connect directly with {0}...";
        public const string AUTHORIZED = "Authorized";
        public const string AUTOMATIC_AXIS = "Automatic Axis";
        public const string AUTOMATIC_CORRELATION_WEIGHT = "Auto Correlation Weighting";
        public const string AUTOMATIC = "Automatic";
        public const string AVAILABLE_DRIVES = "Available Drives";
        public const string AVERAGE_TIME = "Avg Time";
        public const string AWAITING_TRIGGER = "Awaiting Trigger";
        public const string AXIS = "Axis";
        public const string AXIS_SCALING = "Axis Scaling";
        #endregion

        #region B
        public const String BACKGROUND_COLOR = "Background Color";
        public const String BAD_FILE_NAME = "Bad File Name";
        public const String BAUD_RATE = "Baud Rate";
        public const String BIN_FILE_NO_MATCH_FILE = "Bin file crc at does not match file data.";
        public const String BIT_DETECTION = "Input Bit Detection";
        public const String BIT_DETECTOR = "Bit Detector";
        public const String BITFIELD = "Bitfield";
        public const String BIT_SELECTOR = "Bit Selector";
        public const String BIT_SELECTOR_LOW = "Bit Selector Low";
        public const String BOTTOM = "Bottom";
        public const String BRAKE = "Brake";
        public const String BROWSE_CONFIGURATION_FILES = "Browse Config Parameters File";
        public const String BUFFER = "Buffer";
        public const String BUFFERING = "Buffering";
        public const String BUILD_DATETIME = "Build Date/Time";
        #endregion

        #region C
        public const String CALCULATE = "Calculate";
        public const String CALCULATE_GAINS = "Calculate Gains";
        public const String CALCULATING = "Calculating";
        public const String CANCEL = "Cancel";
        public const String CANCEL_CAPTURE = "Cancel Capture";
        public const String CANOPEN = "CANopen";
        public const String CAPTURE_RANGE = "Capture Range";
        public const String CAPTURE_SIZE = "Capture Size";
        public const String CAPTURE_STATUS = "Capture Status";
        public const String CAPTURE = "Capture";
        public const String CAPTURED_POINTS = "Captured {0} of {1} Points";
        public const String CAPTURED_POINTS_APPEND = "; {2}";
        public const String CAPTURING = "Capturing";
        public const String CAPTURE_ON_START = "Capture On Startup";
        public const String CENTER_PLOT = "Ceneter Plot";
        public const String CHECK_CONNECTION = "Please Check Physical Network Connection";
        public const String CHECK_IP = "Please Check IP";
        public const String CLEAR = "Clear";
        public const String CLEARING_FLASH = "Clearing flash ...";
        public const String CLOSE = "Close";
        public const String CLOSE_APP = "Stopping - Closing Application";
        public const String CLOSED_LOOP_FREQ = "Closed Loop 3dB frequency";
        public const String CODE = "Code";
        public const String COIL_INDUCTANCE = "Phase to Phase Inductance";
        public const String COIL_RESISTANCE = "Phase to Phase Resistance";
        public const String COLLATING_DATA = "Collating Data";
        public const String COLOR = "Color";
        public const String COMBOBOX_ERROR = "Error making ComboBox";
        public const String COMMANDS = "Commands";
        public const String COMMAND_VAL = "Command Value";
        public const String COMMUNICATOR = "Communicator";
        public const String COMMUNICATOR_DETECTION = "Communicator Detection";
        public const String COMMUNICATOR_NOT_DETECTED = "Communicator no longer detected!";
        public const String CONFIGURATION_PARAMS = "Configuration Parameters";
        public const String CONFIGURATION_PARAMS_FILE = "Configuration Parameters File";
        public const String CONFIGURATION_SCAN = "Configuration Scan";
        public const String CONFIGURATION = "Configuration";
        public const String CONFIGURE = "Configure";
        public const String CONFIG_SAVE_LOAD_TOOL = "DATAM Configuration Save & Load Tool";
        public const String CONNECT = "Connect";
        public const String CONNECT_ABORT = "Connection Aborted!";
        public const String CONNECT_FAIL = "Connection Failed!";
        public const String CONNECTION_DETECTION = "Connection Detection";
        public const String CONNECT_SUCCESS = "Connection Successfull";
        public const String CONNECT_UNDERWAY = "Connect Underway";
        public const String CONNECTING_MOTOR_DEVICE = "Connecting to your Motor Control Device";
        public const String CONTROLLER = "Controller";
        public const String CONTROLLED_STOP = "Controlled Stop";
        public const String CONTROLWORD = "Controlword";
        public const String COMM_FAILED = "Communication with devices failed";
        public const String COMPARE_FILE = "Compare With File";
        public const String COMPOUND_WARNING = "Compound unit described using non base scale.";
        public const String COMPLETED = "Completed";
        public const String COMPLETED_MANUFAC_DATA = "Completed Manufacturer Data Download.";
        public const String COMPLETED_SERIAL_UPDATE = "Completed Serial Number Update.";
        public static readonly String COPYRIGHT = $"Copyright © {DateTime.Now.Year} Allied Motion Technologies";
        public const String CORRELATION = "Correlation";
        public const String CORRESPONDANCE = "Correspondance";
        public const String CURRENT_ACTUAL_VALUE = "Current Actual Value";
        public const String CURRENT_ACTUAL_VALUE_INFO = "The instantaneous current in the motor.";
        public const String CURRENT_FIRMWARE_INFO = "Current Firmware Information";
        public const String CURRENT_GAINS_CALCULATOR = "CurrentGainsCalculator";
        public const String CURRENT_STATE = "Current State";
        public const String CURRENT_LIMIT = "Current Limit";
        public const String CWU = "Connection Worker Uninitialized";
        public const String CYCLE_TIME = "Cycle Time";
        public const String CYCLIC_SYNC_PROFILE = "Cyclic Synchronous Profile";
        public const String CYCLIC_SYNC_TORQUE = "Cyclic Synchronous Torque";
        public const String CYCLIC_SYNC_TORQUE_ANGLE = "Cyclic Synchronous Torque Commutation Angle";
        public const String CYCLIC_SYNC_VELOCITY = "Cyclic Synchronous Velocity";
        #endregion C

        #region D
        public const string DATA_CAPTURE = "Data Capture";
        public const string DATA_DOWN = "Data Link Down";
        public const string DATA_LINK = "Data Link";
        public const string DATASHEET = "Datasheet";
        public const string DATASHEET_ADD_UPDATE = "Datasheet Add/Update";
        public const string DATASHEET_DIALOG = "Datasheet Dialog";
        public const string DATASHEET_LOADING_TOOL = "DATAM Datasheet Loading Tool";
        public const string DATASHEET_SCAN = "Datasheet Scan";
        public const string DATASHEET_SCAN_COMPLETE = "Datasheet scan complete.";
        public const string DATASHEET_SCAN_NO_CANDIDATES = "Datasheet scan found no candidates.";
        public const string DATASHEET_SCAN_INSTR = "Datasheet Scan Instructions";
        public const string DATASHEET_VERSION = "Datasheet Version";
        public const string DATASHEET_VIEWER = "Datasheet Viewer";
        public const string DATASHEET_REMOVAL = "Datasheet Removal";
        public const string DC_LINK_VOLTAGE = "DC Link Voltage";
        public const string DC_LINK_VOLTAGE_INFO = "The instantaneous DC link current voltage at the drive device.";
        public const string DEBUG = "Debug";
        public const string DECEL = "Deceleration";
        public const string DECEL_LEVEL = "Deceleration Level Selected";
        public const string DECEL_LIMIT = "Deceleration Limit";
        public const string DECEL_NON_NEG = "Deceleration can't be negative";
        public const string DECEL_UPDATED = "Deceleration updated to {0}";
        public const string DECEL_DELTA_SPEED = "Deceleration Delta Speed";
        public const string DECEL_DELTA_TIME = "Deceleration Delta Time";
        public const string DEFAULT = "Default";
        public const string DEFAULT_LAYOUT = "Default Layout";
        public const string DEFAULT_WINDOWS = "Default Windows";
        public const string DELAY_TIME = "Delay Time";
        public const string DELETE = "Delete";
        public const string DELETE_DATASHEET = "Opens a dialog to delete a saved datasheet.";
        public const string DEMAND = "Demand";
        public const string DESCRIPTION = "Description";
        public const string DEVICE = "Device";
        public const string DEVICE_CONFIG = "Device Configuration";
        public const string DEVICE_DIRECT = "Device Direct:";
        public const string DEVICE_FOUND = "Device Found";
        public const string DEVICE_NMT_STATE_CHANGE = "Device Network Management State Changed to ";
        public const string DEVICE_SCAN = "Device Scan";
        public const string DEVICE_SCAN_START = "Device Scan Started";
        public const string DEVICE_USER_SETTINGS = "Device User Settings";
        public const string DIAGNOSTICS = "Diagnostics";
        public const string DIALOGUE = "Dialogue";
        public const string DIGITAL = "Digital";
        public const string DIGITAL_ANALOG_MAPPING = "Digital to Analog Mapping";
        public const string DIGITAL_INPUTS = "Digital Inputs";
        public const string DIGITAL_OUTPUTS = "Digital Outputs";
        public const string DIGITIZATION = "Digitization";
        public const string DIGITIZATION_SET_POINT_1 = "Digitization Set Point 1 Input";
        public const string DIGITIZATION_SET_POINT_2 = "Digitization Set Point 2 Input";
        public const string DIPL1 = "Digital Input to Parameter Link 1";
        public const string DIPL2 = "Digital Input to Parameter Link 2";
        public const string DIRECT_COMMAND = "Direct Command";
        public const string DISABLE = "Disable";
        public const string DISABLE_DRIVE = "Disable Drive";
        public const string DISABLED = "Disabled";
        public const string DISABLED_MOTOR_OP = "Disabled motor operation.";
        public const string DISCONNECT = "Disconnect";
        public const string DISCONNECT_WATCHDAWG = "Disconnect Watchdog";
        public const string DISCONNECT_SUCCESS = "Disconnection from {0} Successful!";
        public const string DISCOVERING = "Discovering...";
        public const string DISPLACEMENT = "Displacement";
        public const string DISPLACE_UPDATED = "Displacement Updated to {0}";
        public const string DOCKING_AREA = "Docking Area";
        public const string DOCUMENT = "Document";
        public const string DRIVE_COMMANDS = "Drive Commands";
        #endregion D

        #region E
        public const string EDIT = "Edit";
        public const string ELEVATED = "Elevated";
        public const string ENABLED = "Enabled";
        public const string ENABLE_DEBUG = "Enable Debug";
        public const string ENABLE_DISCON_TIMER = "Enable Disconnection ShutdownTimer";
        public const string ENABLED_MOTOR_OP = "Enabled motor operation.";
        public const string ENABLE_DRIVE = "Enable Drive";
        public const string ENABLE_OPERATION = "Enable Operation";
        public const string ENABLE_RAMP = "Enable Ramp";
        public const string ENABLE_VOLTAGE = "Enable Voltage";
        public const string ENCODER_ALIGNMENT_STARTED = "Encoder alignment starting. The motor will draw current and emit noise, and may move.";
        public const string ENCODER_ORIENTATION_INFO = "This button will set the 'positive spin' orientation for the encoder.\n" +
                "• Note: This setting cannot be changed while enabled.";
        public const string ENTERED_PHASING_MODE = "Entered phasing mode.";
        public const string EMERGENCY = "Emergency";
        public const string EMERGENCY_MESSAGES = "Emergency Messages";
        public const string EMERGENCY_STOP = "Emergency Stop!";
        public const string ERROR = "Error";
        public const string ERROR_MESSAGES = "Error Messages";
        public const string ESTIMATED_TIME = "Estimated {0}s Remaining";
        public const string EXISTING_DATASHEETS = "Existing Datasheets Found";
        public const string ETHERCAT = "EtherCAT";
        public const string EXIT = "Exit";
        public const string EXPORT_CAPTURE = "Export Capture";
        #endregion E

        #region F
        public const string FACTORY_RESET = "Factory Reset";
        public const string FAILURE = "Failure";
        public const string FAULT = "Fault";
        public const string FAULTED = "Faulted";
        public const string FAULT_HISTORY = "Fault History";
        public const string FAULT_INJECT = "Fault Injection";
        public const string FAULT_RESET = "Fault Reset";
        public const string FAULT_STATUS = "Fault Status";
        public const string FILE = "File";
        public const string FILE_TRAJECTORY = "File: {0} of {1}";
        public const string FILL_AREA = "Fill Area";
        public const string FILTER_TEXT = "Filter Text";
        public const string FINAL_S = "Final S";
        public const string FIND_FILE = "Find File";
        public const string FIND_MAG_ENC_OFFSET = "Find Magnetic Encoder Offset";
        public const string FIRMWARE_LOADING_TOOL = "DATAM Firmware Loading Tool";
        public const string FIRMWARE_NO_UPDATE = "Firmware does not need an update.";
        public const string FIRMWARE_UPLOAD = "Firmware Upload";
        public const string FIRMWARE_UPDATE_SUCCESS = "Firmware update success.";
        public const string FIRMWARE_VERSION = "Firmware Version";
        public const string FONT = "Font";
        public const string FONT_SIZE = "Font Size";
        public const string FORCE_POWER_RESET = "Force Power Reset";
        #endregion F

        #region G
        public const string GATHERED = "Gathered {0} of {1}";
        public const string GAUGE_PANEL = "Gauge Panel";
        public const string GAUGE = "Gauge";
        public const string GAUGES = "Gauges";
        public const string FOREGROUND_COLOR = "Foreground Color";
        public const string GAUGE_LIST_SETTINGS = "Gauge List Settings";
        public const string GAUGE_SETT = "Gauge Settings";
        public const string GENERAL = "General";
        public const string GET_CAPTURE = "Get Capture";
        public const string GLOBAL_USER_LEVEL = "Global User Level";
        public const string GPIO_PANEL = "GPIO Panel";
        public const string GRAPH_COLORS = "Graph Colors";
        public const string GRAPH_SETTINGS = "Graph Settings";
        public const string GRID_COLOR = "Grid Color";
        public const string GRIDLINE = "Gridline";
        public const string GRIDLINES = "Gridlines";
        public const string GROUP = "Group";
        #endregion G

        #region H
        public const string HALT = "Halt";
        public const string HALT_OPTION = "Halt Option";
        public const string HELP = "Help";
        public const string HIDE_DEVICE_TREE = "Hide the device tree";
        public const string HIDE_STATUS_BAR = "Hide the status bar";
        public const string HIGH = "High";
        public const string HISTORY = "History";
        public const string HOMING = "Homing";
        #endregion H

        #region I
        public const string ICONS_PROVIDED_BY = "Icons provided by Icons8.com"; // https://linkprotect.cudasvc.com/url?a=https%3a%2f%2fIcons8.com&c=E,1,iohn8f93LEd6_746RH2KmaeH5eXI90TnF-7NsMubZManZh1qgW9kZiIjN0ijUFBGaPR7FhhKkMTvvqHpAplpiSax0KtC6qwN8FKU11HG_VWKZ8rdTVfr&typo=1";
        public const string IDLE = "Idle";
        public const string IGNORE_CONNECTION_LOSS = "Ignore Connection Loss";
        public const string IMMEDIATE = "Immediate";
        public const string INERTIAL_OBSERVER = "Inertial Observer";
        public const string INFO = "Info";
        public const string INFORMATION = "Information";
        public const string INITIALIZATION = "Initialization";
        public const string INITIAL_S = "Initial S";
        public const string INPUT = "Input";
        public const string INPUTS = "Inputs";
        public const string INPUT_SETTINGS = "Input Settings";
        public const string INPUT_SOURCE = "Input Source";
        public const string INPUT_VALUE = "Input Value";
        public const string INPUT_1 = "Input 1";
        public const string INPUT_2 = "Input 2";
        public const string INPUT_3 = "Input 3";
        public const string INPUT_4 = "Input 4";
        public const string INPUT5 = "Input 5";
        public const string INTERPOLATED_POS = "Interpolated Position";
        public const string INTERVAL = "Interval";
        public const string INVALID = "Invalid";
        public const string IO = "I/O";
        public const string IO_LINKER = "I/O Linker";
        public const string IP_ADD = "IP Address";
        public const string IP_SCAN = "IP Scan";
        #endregion I

        #region J
        public const string JERK_LEVEL = "Jerk Level Selected";
        public const string JERK_NEG = "Jerk (-)";
        public const string JERK_NEG_UPDATED = "Jerk (-) updated to {0}";
        public const string JERK_NON_NEG = "Jerk can't be negative";
        public const string JERK_POS = "Jerk (+)";
        public const string JERK_POS_UPDATED = "Jerk (+) updated to {0}";
        public const string JOG = "Jog";
        public const string JOG_DEMAND = "This value represents the magnatude of the demand for a jog. This is relative to the current target.";
        public const string JOG_LEFT_BUTTON = "Jog at the demand value in the backwards orientation.";
        public const string JOG_RIGHT_BUTTON = "Jog at the demand value in the forward orientation.";
        public const string JOG_TIME = "This value represents the interval, in seconds, over which the jog will be asserted if timed mode is selected.";
        public const string JOG_TIME_CHECK = "When checked, the jog buttons will induce a movement for the specified period of time.";
        public const string JSON_FORMAT_ERROR = "Json format error in config parameter file.";
        #endregion J

        #region L
        public const string L = "L";
        public const string LANGUAGE = "Language";
        public const string LAST_COMMANDS = "Last Commands";
        public const string LEFT = "Left";
        public const string LEFT_AXIS = "Left Axis";
        public const string LEFT_AXIS_VAR = "Left Axis Variable";
        public const string LEVEL = "Level";
        public const string LINEAR_RAMP = "Linear Ramp";
        public const string LIVE_POSITION = "Live Position";
        public const string LIVE_UPDATE = "Live Update";
        public const string LIMIT_ACTIVE = "Limit Active";
        public const string LINK = "Link";
        public const string LOAD = "Load";
        public const string LOAD_CAPTURE = "Load Capture";
        public const string LOAD_CONFIG = "Load Configuration File";
        public const string LOAD_CONFIG_INSTR = "Load Configuration Instructions";
        public const string LOAD_FIRMWARE_INSTR = "Load Firmware Instructions";
        public const string LOAD_FROM_FILE = "Load From File";
        public const string LOAD_MOTIONS = "Load Motions From";
        public const string LOAD_PARAM_ADDRESS = "Load Parameter Address Data";
        public const string LOAD_PANEL = "Device Scan Panel";
        public const string LOADING = "Loading";
        public const string LOADING_COMPLETE = "Loading Complete";
        public const string LOADING_DATA_CAPTURE = "Loading your Data Capture Panel";
        public const string LOADING_DATASHEETS = "Loading Datasheets";
        public const string LOADING_MANUFAC_CONFIG_PARAM = "Loading Manufacturing Configuration Parameters...";
        public const string LOADING_PARAMETER_PANEL = "Loading your Parameter Panel";
        public const string LOADING_STATE = "Loading Program State";
        public const string LOADING_YOUR_DATA = "Loading your data...";
        public const string LOAD_LABEL_DATA = "Load Label Data Instructions";
        public const string LOCKED = "Locked";
        public const string LOCK_LEFT_AXIS = "Lock Left Axis";
        public const string LOCK_OPEN = "Lock Open";
        public const string LOCK_RIGHT_AXIS = "Lock Right Axis";
        public const string LOCK_TIME_AXIS = "Lock Time Axis";
        public const string LOG_DISPLAY = "Log Display";
        public const string LOGS = "Logs";
        public const string LOG_FILE_NAME = "Log File Name";
        public const string LOG_FILE_PATH = "Log File Path";
        public const string LOG_FILE_TYPE = "Log File Type";
        public const string LOG_MESSAGE = "Log Message";
        public const string LOG_SETTINGS = "Log Settings";
        public const string LOG_SIZE = "Size of Logs (MB)";
        public const string LOGGING = "Logging";
        public const string LOW = "Low";
        #endregion L

        #region M
        public const string MAGNETIC_ALIGNMENT_OFFSET = "Magnetic Alignment Offset";
        public const string MAGNITUDE = "Magnitude";
        public const string MAINTAIN = "Maintain";
        public const string MAJOR = "Major";
        public const string MANUFACTURER_LABEL = "Manufacture Label";
        public const string MANUFACTURER_LABEL_DATA = "DATAM Manufacturer Label Data Loading Tool";
        public const string MANUFACTURER_LABEL_PARAMS = "Manufacturer label parameters";
        public const string MANUFACTURER = "Manufacturer";
        public const string MANUFACTURER_PARAM_SAVED = "Manufacturer Parameters Saved to Flash.";
        public const string MANUFACTURER_SPECIFIC = "Manufacturer Specific";
        public const string MAPPING_I0 = "I0 Mapping";
        public const string MARQEE_SCROLL = "Marquee Scroll";
        public const string MATCHCASE = "Match Case";
        public const string MAXIMUM = "Maximum";
        public const string MAXIMUM_ACCELERATION = "Maximum Acceleration";
        public const string MAX_CURRENT = "Maximum Current";
        public const string MAX_CURRENT_INFO = "Maximum permissible torque creating current in the motor.";
        public const string MAX_DECEL = "Maximum Deceleration";
        public const string MAX_MOTOR_SPEED = "Maximum Motor Speed";
        public const string MAX_PROFILE_VELOCITY = "Maximum Profile Velocity";
        public const string MAX_TORQUE = "Maximum Torque";
        public const string MAX_TORQUE_INFO = "Maximum permissible torque in the motor.";
        public const string MAX_VELOCITY = "Maximum Velocity";
        public const string MESSAGES = "Messages";
        public const string MENU = "Menu";
        public const string MINIMUM = "Minimum";
        public const string MINIMUM_LOG_LEVEL = "Minimum Log Level";
        public const string MINOR = "Minor";
        public const string MIN_VELOCITY = "Minimum Velocity";
        public const string MODIFY_AXIS_PARAMS = "Modify Axis Parameters";
        public const string MODE = "Mode";
        public const string MODE_OF_OPERATION = "Mode of Operation";
        public const string MODIFIED = "Modified";
        public const string MORE_AREA = "More Area";
        public const string MOTION = "Motion";
        public const string MOTIONS = "Motions";
        public const string MOTION_CONTROL = "Motion Control";
        public const string MOTION_CONTROLLER = "Motion Controller";
        public const string MOTION_CONTROLLER_WELCOME = "Stimulus Welcome Message";
        public const string MOTION_CREATION = "Motion Creation";
        public const string MOTION_PLOT = "Motion Plot";
        public const string MOTION_PROFILE_TYPE = "Motion Profile Type";
        public const string MOTION_SCHEDULE = "Motion Schedule";
        public const string MOTION_TYPE = "Motion Type";
        public const string MOTOR = "Motor";
        public const string MOTOR_CONTROLLER = "Motor Controller";
        public const string MOTOR_IN_MOTION = "Motor in Motion";
        public const string MOTOR_RATED_CURRENT = "Motor Rated Current";
        public const string MOTOR_RATED_CURRENT_INFO = "The configured motor rated current from the motor nameplate.";
        public const string MOTOR_RATED_TORQUE = "Motor Rated Torque";
        public const string MOTOR_RATED_TORQUE_INFO = "The configured motor rated torque from the motor nameplate.";
        public const string MOVE = "Move";
        public const string MOVE_AXES = "Move Axes";
        public const string MOVE_IMMEDIATE = "Move Immediate";
        public const string MOVE_SCHEDULED = "Move Scheduled";
        public const string MOVE_TO_LEFT = "Move to Left Axis";
        public const string MOVE_TO_RIGHT = "Move to Right Axis";
        public const string MULTIPLIER = "Multiplier";
        #endregion M

        #region N
        public const string NAME = "Name";
        public const string NAMED_STIM_PANEL = "Named Stimulus Panel";
        public const string NEG = "Negative";
        public const string NEG_TORQUE_INFO = "Maximum negative torque in the motor.";
        public const string NEG_TORQUE_LIMIT = "Negative Torque Limit";
        public const string NET_MANAGEMENT_STATE = "Network Management State";
        public const string NETWORK = "Network";
        public const string NETWORK_ADAPT = "Network Adapters";
        public const string NEW = "New";
        public const string NEXT_FILE = "Next File";
        public const string NO = "No";
        public const string NO_CAPTURE = "No Capture to Display";
        public const string NODE_ID = "Node ID";
        public const string NO_DEVICES = "No Devices Found";
        public const string NO_MODE_SELECTED = "No Mode Selected";
        public const string NONCAPTURABLE_PARAMS = "Non-capturable parameters";
        public const string NONE = "None";
        public const string NOT_CONNECTED = "Not Connected!";
        public const string NOTES = "Notes";
        public const string NOT_FAULTED = "Not Faulted";
        public const string NO_DEVICE_FOUND = "No Device Found";
        public const string NO_VARIABLES = "No Variables Selected";
        public const string NUMBER_GAUGES = "Number of Gauges";
        public const string NUMBER_LOGS = "Number of Logs";
        #endregion N

        #region O
        public const string OFF = "Off";
        public const string OFFSET = "Offset";
        public const string OK = "OK";
        public const string ON = "On";
        public const string OPEN_ALLIED_DEVICES = "Open Allied Devices Found";
        public const string OPEN_FOUND_DEVICES = "Open Found Devices";
        public const string OPEN_LOOP_GAIN = "Open Loop Gain";
        public const string OPEN_LOOP_PHASE = "Open Loop Phase";
        public const string OPEN_LOOP_VOLTAGE = "Open Loop Voltage";
        public const string OPEN_CLOSED_LOOP = "Open & Closed Loop";
        public const string OPENING_LOG = "Opening Log";
        public const string OPERATION_MODE_SPECIFIC = "Operation Mode Specific";
        public const string OPERATIONAL = "Operational";
        public const string OPTIONS = "Options";
        public const string OUTPUT = "Output";
        public const string OUTPUT_BIT_LOGIC_HIGH = "Output on Logic High";
        public const string OUTPUT_BIT_LOGIC_LOW = "Output on Logic Low";
        public const string OUTPUT_SETTINGS = "Output Settings";
        public const string OUTPUT_SOURCE = "Output Source";
        public const string OUTPUT_VALUE = "Output Value";
        public const string OUTPUT1 = "Output 1";
        public const string OUTPUT2 = "Output 2";
        public const string OUTPUT3 = "Output 3";
        public const string OUTSIDE_SPS = "Outside SP1 and SP2";
        public const string OVERRIDE = "Override";
        #endregion O

        #region P
        public const string PARAMETER = "Parameter";
        public const string PARAM1 = "Parameter 1";
        public const string PARAM2 = "Parameter 2";
        public const string PARAM3 = "Parameter 3";
        public const string PARAM4 = "Parameter 4";
        public const string PARAMETER_PANEL = "Parameter Panel";
        public const string PARAM_DIRECTOR = "Parameter Director";
        public const string PASSWORD = "Password";
        public const string PDOL1 = "Parameter to Digital Output Link 1";
        public const string PDOL2 = "Parameter to Digital Output Link 2";
        public const string PENDING = "Pending";
        public const string PHASING = "Phasing";
        public const string PHASING_MODE_INSTR = "Phasing Mode Instructions";
        public const string PID_NOT_VALID = "This file is invalid or not meant for the device selected.";
        public const string PLEASE_REBOOT_DEVICE = "Please reboot device to use new firmware ...";
        public const string PLOT = "Plot";
        public const string POB_SENDER = "POB Sender";
        public const string POINT = "Point";
        public const string POINTS = "Points";
        public const string POINTS_TO_CAPTURE = "Points To Capture";
        public const string POLARITY = "Polarity";
        public const string PORT = "Port";
        public const string POS = "Positive";
        public const string POSITION_DEMAND = "Position Demand";
        public const string POS_SPIN_ORIENTATION = "Positive Spin Orientation";
        public const string POST = "Post";
        public const string POS_TORQUE_INFO = "Maximum positive torque in the motor.";
        public const string POS_TORQUE_LIMIT = "Positive Torque Limit";
        public const string POST_TRIGGER = "Post-Trigger";
        public const string POST_TRIGGER_25 = "25% Post-Trigger";
        public const string POST_TRIGGER_50 = "50% Post-Trigger";
        public const string POST_TRIGGER_75 = "75% Post-Trigger";
        public const string PRE = "Pre";
        public const string PRE_OPERATIONAL = "Pre-Operational";
        public const string PRE_TRIGGER = "Pre-Trigger";
        public const string PREPARING_DATA = "Preparing Data";
        public const string PREVIEW = "Preview";
        public const string PREVIOUS = "Previous";
        public const string PREVIOUS_FILE = "Previous File";
        public const string PROCEED = "Proceed";
        public const string PRODUCT_NAME = "Product Name";
        public const string PROFILE_ACCEL = "Profile Acceleration";
        public const string PROFILE_DATA_FILE = "Profile Data File";
        public const string PROFILE_DECEL = "Profile Deceleration";
        public const string PROFILE_POSITION = "Profile Position";
        public const string PROFILE_TORQUE = "Profile Torque";
        public const string PROFILE_VELOCITY = "Profile Velocity";
        #endregion P

        #region Q
        public const string QUEUE_CLEAR = "Queue Clear";
        public const string QUICK_STOP = "Quick Stop";
        public const string QUICK_STOP_DECEL = "Quick Stop Deceleration";
        public const string QUICK_STOP_RAMP = "'Quick Stop' Ramp";
        #endregion Q

        #region R
        public const string R = "R";
        public const string REACTIONS = "Reactions";
        public const string READING_BIN_FILE = "Reading bin file ...";
        public const string READING_FLASH_INFO = "Reading flash information ...";
        public const string REBOOTING_DEVICE = "Rebooting device to update application ...";
        public const string RECEIVED = "Received";
        public const string RECEIVED_MESSAGES = "Received Messages";
        public const string REDO = "Redo";
        public const string REFERENCE_RAMP = "Reference Ramp";
        public const string REJECT = "Reject";
        public const string RELATIVE = "Relative";
        public const string REMOTE = "Remote";
        public const string REMOVE = "Remove";
        public const string REMOVE_DATASHEET = "Remove Datasheet";
        public const string REMOVE_DATASHEET_INSTR = "Remove Datasheet Instructions";
        public const string REPLACE_CONFIG = "Replace Configuration";
        public const string RESERVED = "Reserved";
        public const string RESET = "Reset";
        public const string RESET_DATAM = "Reset Datam";
        public const string RESET_FAULTS = "Reset Faults";
        public const string RESET_NEEDED = "Datam needs to Reset to complete the operation";
        public const string RESET_OPTIONS = "Reset Options";
        public const string RESET_UNDERWAY = "Reset Underway";
        public const string RESPONSE_NOT_VALID = "The response was not valid.";
        public const string RESULT = "Result";
        public const string RETRIEVING_DATA = "Retrieving Data";
        public const string REVISION_NUMBER = "Revision Number";
        public const string RIGHT = "Right";
        public const string RIGHT_AXIS = "Right Axis";
        public const string RIGHT_AXIS_VAR = "Right Axis Variable";
        public const string RMS = "RMS";
        public const string RUN = "Run";
        public const string RWU = "Reset Worker Uninitialized";
        #endregion R

        #region S
        public const string SAFE_TORQUE_OFF = "Safe Torque Off";
        public const string SAFETY = "Safety";
        public const string SAVE = "Save";
        public const string SAVE_CONFIG = "Save Configuration File";
        public const string SAVE_CONFIG_INSTR = "Save Configuration Instructions";
        public const string SAVE_PARAMETER_STATE = "Save Parameter State";
        public const string SAVE_FLASH = "Save Flash";
        public const string SAVE_LAYOUT = "Save Layout";
        public const string SAVE_MOTIONS_FILE = "Save Motions To File";
        public const string SAVE_RESET_HISTOGRAM = "Save and Reset Histogram";
        public const string SAVE_TO_FILE = "Save To File";
        public const string SAVING_CONFIG_PARAMS = "Saving Configuration Parameters...";
        public const string SAVED_PASS = "Saved Password";
        public const string SCALE = "Scale";
        public const string SCAN = "Scan";
        public const string SCANNING = "Scanning";
        public const string SCAN_COMMUNICATORS = "Scan for Communicators";
        public const string SCAN_COMPLETED = "Scan Completed";
        public const string SCAN_FOR_DEVICES_ON = "Scan for devices on {0}";
        public const string SCAN_FOR_DATASHEETS = "Opens a dialog to scan for datasheets in a folder.";
        public const string SCAN_INIT = "Scan Initialized";
        public const string SCAN_PERIOD = "Scan Interval";
        public const string SCAN_PROGRESS = "Scan Progress";
        public const string SCAN_SETTINGS_CAN = "CANopen Scan Settings";
        public const string SCANNING_DEVICES_ALLNET = "Scanning for ALLNET Devices...";
        public const string SCANNING_DEVICES_COMMUNICATOR = "Scanning for Communicator Devices...";
        public const string SCHEDULE = "Schedule";
        public const string SCIENTIFIC_NOTATION = "Scientific Notation";
        public const string SEARCH = "Search";
        public const string SEARCH_FROM_NODE = "Search From Node ID";
        public const string SEARCH_TO_NODE = "Search To Node ID";
        public const string SECONDS_AFTER = "seconds after data link is lost";
        public const string SECTION = "Section";
        public const string SELECT = "Select";
        public const string SELECT_ADAPT = "Select Adapter";
        public const string SELECT_LOG_FILE_LOCATION = "Select Log File Location";
        public const string SELECTED_FIRM_INFO = "Selected Firmware Information";
        public const string SELECTED_FIRM_FILE_LOCATION = "Selected Firmware File Location";
        public const string SELECTED_LINKER = "Selected Linker";
        public const string SELECT_DEVICE = "Select Device";
        public const string SENT = "Sent";
        public const string SENT_MESSAGES = "Sent Messages";
        public const string SEND = "Send";
        public const string SENSOR_SELECTION_CODE = "Sensor Selection Code";
        public const string SERIAL_NUMBER = "Serial Number";
        public const string SET = "Set";
        public const string SETPOINT1 = "Set Point 1";
        public const string SETPOINT2 = "Set Point 2";
        public const string SETTINGS = "Settings";
        public const string SETTING_UP = "Setting Up";
        public const string SETUP = "Setup";
        public const string SHOW_ALL = "Show All Found Devices";
        public const string SHOW_ALL_TEXT = "Show All";
        public const string SHOW_DATASHEET = "Show Datasheet";
        public const string SHOW_DEVICE_TREE = "Show the device tree.";
        public const string SHOW_HIDDEN_PARAMS = "Show Hidden Parameters";
        public const string SHOW_HISTORY = "Show History";
        public const string SHOW_STATUS_BAR = "Show Status Bar";
        public const string SHOW_WINDOWS = "Show Windows";
        public const string SHUTDOWN = "Shutdown";
        public const string SHUTDOWN_ON_DISCONNECT = "Shutdown on Disconnect";
        public const string SIGNAL = "Signal";
        public const string SIGNALS = "Signals";
        public const string SIGNAL_SELECTION = "Signal Selection";
        public const string SLOW_DOWN_RAMP = "Slow Down Ramp";
        public const string SOFTWARE_IDENTIFICATION_NOT_UPDATED = "Original software id still being read. Might need a reboot.";
        public const string SOURCE_PANEL = "Source Panel";
        public const string SPIN = "Spin";
        public const string STANDARD = "Standard";
        public const string START = "Start";
        public const string START_CAPTURE = "Start Capture";
        public const string START_TIME = "Start Time";
        public const string STARTUP_SCANNING_OPTIONS = "Startup Scanning Options";
        public const string START_MAXIMIZED = "Start Maximized";
        public const string STATUS = "Status";
        public const string STATUS_BAR = "Status Bar";
        public const string STATUS_COMPLETE = "Status: Scan Complete";
        public const string STATUS_IN_PROGRESS = "Status: Scan In Progress";
        public const string STATUS_MONITOR = "Status Monitor";
        public const string STATUS_NOT_RUN = "Status: Scan Not Run";
        public const string STATUS_PANEL = "Status Panel";
        public const string STATUSWORD = "Statusword";
        public const string STIMULUS = "Stimulus";
        public const string STOP = "Stop";
        public const string STOP_IMMEDIATE = "Stop Immediate";
        public const string STOPPED = "Stopped/Safeop";
        public const string STOP_SCHEDULED = "Stop Scheduled";
        public const string SUCCESS = "Success";
        public const string SWITCH_AXIS = "Switch Axis";
        public const string SYSTEM = "System";
        #endregion S

        #region T
        public const string TARGET_POSITION = "Target Position";
        public const string TARGET_REACHED = "Target Reached";
        public const string TARGET_TORQUE = "Target Torque";
        public const string TARGET_TORQUE_INFO = "The configured input value for the torque controller.";
        public const string TARGET_VELOCITY = "Target Velocity";
        public const string TEST = "Test";
        public const string TEST_LOG_GENERATION = "Test Log Generation";
        public const string THEME = "Theme";
        public const string TIME = "Time";
        public const string TIME_BASE = "Time Base";
        public const string TIMED_OUT_WAITING_FOR_RESPONSE = "Timed out waiting for response.";
        public const string TIME_W_UNIT = "Time (s)";
        public const string TO_NODE_ID = "to Node ID";
        public const string TOOLS = "Tools";
        public const string TOO_MANY_PARAMETERS = "Too many parameters!";
        public const string TOP = "Top";
        public const string TORQUE = "Torque";
        public const string TORQUE_ACTUAL_VALUE = "Torque Actual Value";
        public const string TORQUE_ACTUAL_VALUE_INFO = "The instantaneous torque in the motor.";
        public const string TORQUE_DEMAND = "Torque Demand";
        public const string TORQUE_DEMAND_INFO = "The output value of the trajectory generator.";
        public const string TORQUE_PROFILE = "Torque Profile";
        public const string TORQUE_PROFILE_INFO = "The type of profile used to perform a torque change.";
        public const string TORQUE_SLOPE = "Torque Slope";
        public const string TORQUE_SLOPE_INFO = "The configured rate of change of torque";
        public const string TRAJECTORY = "Trajectory";
        public const string TRIGGER_SETTINGS = "Trigger Settings";
        public const string TROUBLESHOOTING = "Troubleshooting";
        public const string TYPE = "Type";
        public const string TYPE_OF_MOTION = "Type of Motion";
        #endregion T

        #region U
        public const string UDP_MULTICAST = "UDP Multicast";
        public const string UNABLE_TO_CHANGE_BAUD = "Unable to change baud rate";
        public const string UNABLE_READ_BIN = "Unable to read bin file.";
        public const string UNAVAILABLE = "Unavailable";
        public const string UNCOMMUTATED_CURR = "Uncomutated Current";
        public const string UNDO = "Undo";
        public const string UNIDENTIFIED_DEVICE = "Unidentified device found at IP: ";
        public const string UNINITIALIZED = "Uninitialized";
        public const string UNKNOWN = "Unknown";
        public const string UNKNOWN_STATE = "Unknown State!";
        public const string UNLINK = "Unlink";
        public const string UNLINK_ALL = "Unlink All";
        public const string UNLINK_ON_CHANGE = "Unlink on Value Change";
        public const string UNLOCKED = "Unlocked";
        public const string UNLOCK_RAMP = "Unlock Ramp";
        public const string UNIT = "Unit";
        public const string USE = "Use";
        public const string USE_MULTICAST = "Use Multicast";
        public const string USER_AUTHORIZATION = "User Authorization";
        public const string USER_LEVEL = "User Level";
        public const string USER_LEVEL_INFO = "This will only apply to devices that allow the selected access level";
        public const string USER_OUTPUT = "User Output";
        public const string USER_SETTINGS = "User Settings";
        public const string UPDATE_RATE = "Update Rate";
        public const string UPDATING_FIRMWARE = "Updating firmware ...";
        public const string UPDATING_FIRMWARE_FAILED = "Updating of firmware failed due to an exception.";
        public const string UPLOAD_FILE = "Upload File";
        #endregion U

        #region V
        public const string VALID_PATH = "Validated Log Path";
        public const string VALIDATE = "Validate";
        public const string VALUE = "Value";
        public const string VARIABLE = "Variable";
        public const string VELOCITY = "Velocity";
        public const string VELOCITY_ACTUAL_VALUE = "Velocity Actual Value";
        public const string VELOCITY_DEMAND = "Velocity Demand";
        public const string VELOCITY_LIMIT = "Velocity Limit";
        public const string VELOCITY_LEVEL = "Velocity Level Selected";
        public const string VELOCITY_THRESHOLD = "Velocity Threshold";
        public const string VELOCITY_THRESHOLD_TIME = "Velocity Threshold Time";
        public const string VELOCITY_UPDATED = "Velocity updated to {0}";
        public const string VELOCITY_WINDOW = "Velocity Window";
        public const string VELOCITY_WINDOW_TIME = "Velocity Window Time";
        public const string VERBOSE = "Verbose";
        public const string VERSION = "Version";
        public const string VF_MODE_FREQUENCY = "VF Mode Ferequency";
        public const string VIEW = "View";
        public const string VIRTUAL = "Virtual";
        public const string VISIBLE = "Visible";
        public const string VOLTAGE_DISABLED = "Voltage Disabled";
        public const string VOLTAGE_ENABLED = "Voltage Enabled";
        public const string VOLTAGE_LIMIT = "Voltage Limit";
        #endregion V

        #region W
        public const string WAIT_TARGET_REACHED = "Wait for target reached";
        public const string WARNING = "Warning";
        public const string WARNING_HISTORY = "Warning History";
        public const string WELCOME = "Welcome";
        public const string WELCOME_TO_DATAM = "Welcome to Datam!";
        public const string WINDOWS = "Windows";
        public const string WITHIN_SPS = "Within SP1 and SP2";
        public const string WRITING_FLASH_FILE = "Writing flash file ...";
        #endregion W

        #region Y
        public const string YES = "Yes";
        #endregion Y

        #region Z
        public const string ZERO_TARGET_PARAMS = "Zero Target Parameters on Disable";
        #endregion Z

        #endregion General

        #region Special

        #region Dynamic
        public const string CONFIGPARAM_LOADFAIL_DYNAMIC = "Config parameters load failed due to exception: {0}.";
        public const string CONNECTED_TO_X_DYNAMIC = "Connected to {0}";
        public static readonly string DATE_DYNAMIC = "Date: {0}" + Tokens._nl_;
        public const string DATASHEET_REMOVAL_DYNAMIC = "Datasheet version {0} removed successfully.";
        public static readonly string ENCODER_OFFSET_DYNAMIC = Tokens._nl_ + "Encoder offset found to be {0} counts.";
        public const string EXISTING_BAUDRATE_DYNAMIC = "Existing ({0} Mbps)";
        public const string EXISTING_NODE_ID_DYNAMIC = "Existing ({0})";
        public const string FAILURE_ADDRESS_DYNAMIC = "Point of failure address {0}";
        public const string FOUND_DEVICE = "Found {0} device(s). Last device {1} at NodeID {2}, Serial Number {3}";
        public const string JSON_FORMAT_ERROR_LINE_DYNAMIC = "Json format error: at line {0}.";
        public const string JSON_FORMAT_ERROR_LINE_COLUMN_DYNAMIC = "Json format error: at line {0} and column {0}.";
        public const string JSON_FORMAT_ERROR_PATH_DYNAMIC = "Json format error: path {0}.";
        public const string JSON_FORMAT_ERROR_PATH_LINE_DYNAMIC = "Json format error: path {0} at line {0}.";
        public const string JSON_FORMAT_ERROR_PATH_LINE_COLUMN_DYNAMIC = "Json format error: path {0} at line {0} and column {0}.";
        public const string OUTPUT_FILE_DYNAMIC = "Output file saved at {0}";
        public const string OPERATION_CONFIGMANUFAC_DYNAMIC = "Unable to {0} {0} parameter file.";
        public static readonly string PATH_DYNAMIC = "Path: {0}" + Tokens._nl_;
        public static readonly string PARAM_RESET_ATTEMPT_DYNAMIC = "Attempting to reset {0} from {0} to {0}." + Tokens._nl_;
        public const string NODE_DEFAULT_DYNAMIC = "Default ({0})";
        public static readonly string TIME_DYNAMIC = "Time: {0}" + Tokens._nl_;
        public static readonly string VERSION_DYNAMIC = "Version: {0}" + Tokens._nl_;
        #endregion Dynamic

        #region Error
        public const string ERROR_DEVICE_NOT_SUPPORTED = "Error: {0} is not supported.";
        public const string ERROR_PANEL_NOT_SUPPORTED = "Error: This window is not supported.";
        public const string ERROR_CLOSE_UNEXPECTED = "Error: The window closed unexpectedly.";
        public const string ERROR_ERROR_COUNT = "Error count exceeds stable running conditions. Shutdown is recommended. Proceed?";
        public const string ERROR_MANUFACT_PARAM_SAVE_FAIL = "ERROR: Manufacturer Parameters Flash Save Failed!";
        public const string ERROR_RUNTIME = "Runtime Error";
        #endregion /Error

        #region HTX Debug Fault Injection
        public const string DRIVE_TEMP_NEAR_LIMIT = "Drive Temp Near Limit";
        public const string MOTOR_TEMP_NEAR_LIMIT = "Motor Temp Near Limit";
        public const string I2T_LOAD_LIMITING_WARN = "I²T Load Limiting Warning";
        public const string RESET_WARNINGS = "Reset Warnings";
        public const string FAULT_ON_ADDRESS2 = "Fault on Address 2 'Not Supported' Error";
        public const string OVER_VOLTAGE = "Overvoltage Error";
        #endregion /HTX Debug Fault Injection

        #region Instructions
        public const string INSTRUCTIONS = "Instructions";
        public const string INSTRUCTIONS_2 = "Instructions2";
        public const string INSTRUCTIONS_CGC = "Instructions Current Gains Calculator";
        public const string INSTRUCTIONS_CONFIGURATIONS_SAVE_LOAD_TOOL = "Instructions Configuration Save & Load Tool";
        public const string INSTRUCTIONS_CONFIGURATIONS_SAVE_INSTR = "Instructions Configuration Save Instructions";
        public const string INSTRUCTIONS_DATASHEET_ADD_UPDATE = "Instructions Datasheet Add/Update";
        public const string INSTRUCTIONS_DATASHEET_DIALOG = "Instructions Datasheet Dialog";
        public const string INSTRUCTIONS_DATASHEET_REMOVE = "Instructions Remove Datasheet";
        public const string INSTRUCTIONS_DATASHEET_SCAN = "Intsructions Datasheet Scan";
        public const string INSTRUCTIONS_DEV_USER_SETTINGS = "Instructions Device User Settings";
        public const string INSTRUCTIONS_FIRMWARE_LOADING = "Instructions Load Firmware";
        public const string INSTRUCTIONS_LOAD_MANUFACT_LABEL = "Instructions Load Manufacturer Lavel Data";
        public const string INSTRUCTIONS_MOVE_AXES = "Instructions1";
        public const string INSTRUCTIONS_PHASING_MODE = "Instructions Phasing Mode";
        public const string INSTRUCTIONS_SEL_ADAPT = "Instructions Select Adapter";
        public const string INSTRUCTIONS_WELCOME_COMMUNICATOR = "Instructions Welcome Communicator";
        public const string INSTRUCTION_WELCOME_DEVICE = "Instructions Welcome Device";
        #endregion /Instructions

        #region Numbers
        public const string ONE = "One";
        public const string TWO = "Two";
        public const string THREE = "Three";
        public const string FOUR = "Four";
        public const string FIVE = "Five";
        public const string SIX = "Six";
        public const string SEVEN = "Seven";
        public const string EIGHT = "Eight";
        public const string NINE = "Nine";
        public const string TEN = "Ten";
        public const string ELEVEN = "Eleven";
        public const string TWELVE = "Twelve";
        public const string THIRTEEN = "Thirteen";
        public const string FOURTEEN = "Fourteen";
        public const string FIFTEEN = "Fifteen";
        public const string SIXTEEN = "Sixteen";
        public const string SEVENTEEN = "Seventeen";
        public const string EIGHTEEN = "Eighteen";
        public const string NINETEEN = "Nineteen";
        public const string TWENTY = "Twenty";
        #endregion Numbers

        #region Messages
        public const string MESSAGE_COMMUNICATOR_SCAN_DISABLED = "Communicator detection has been disabled.\nWould you like to enable it?";
        public const string MESSAGE_ADD_DEVICE_NODE = "Adding a device node to the device tree for {0}.";
        public const string MESSAGE_ADD_NETWORK_NODE = "Adding a network node to the device tree for {0}.";
        public const string MESSAGE_ADD_TO_NETWORK_NODE = "Adding devices to network node {0}";
        public const string MESSAGE_BTN_PROCEED = "Are you sure? Proceeding will cause irreversible effects.";
        public const string MESSAGE_BEGIN_FACTORY_RESET = "Beginning Factory Reset";
        public const string MESSAGE_CANT_ADD_CAPTURING = "Can not add while capturing!";
        public const string MESSAGE_CLEAR_CURRENT_CAPTURE = "This will clear the existing capture. Continue?";
        public const string MESSAGE_CONFIGURATION_ADDED = "Version '{0}' configuration has been added.";
        public const string MESSAGE_CONFIGURATION_INVALID = "The selected configuration file was found to be invalid.";
        public const string MESSAGE_CONFIGURATION_UPDATED = "Version '{0}' configuration has been updated.";
        public const string MESSAGE_CONFIGPRESENT = "The selected configuration file version is already present. Do you want to replace it?";
        public const string MESSAGE_CONNECTIONLOST = "The connection to {0} was lost, close the associated windows?";
        public const string MESSAGE_CONTROLWORD_DISABLE = "Change controlword to the disabled state.";
        public const string MESSAGE_CONTROLWORD_ENABLE = "Change controlword to the enabled state.";
        public const string MESSAGE_CURRENT_FIRMWARE_INFO = "Current firmware information is displayed in a panel on the left hand side of this window.";
        public const string MESSAGE_DATASHEETINVALID = "Datasheet is invalid!";
        public const string MESSAGE_DATASHEETERROR = "Datasheet Error";
        public const string MESSAGE_DEVICEINVALID = "Device is invalid!";
        public const string MESSAGE_DEVICE_READY_CONNECT = "{0} is ready to connect!";
        public const string MESSAGE_DISCONNECT_WATCHDAWG = "Disconnect watchdog disabled due to the interval being set to 0ms.\n " +
                    "If you would like to have this function please change the interval in the 'Settings' menu.";
        public const string MESSAGE_EXISTING_CAPTURE = "Existing static capture must be cleared before axes variables can be modified.";
        public const string MESSAGE_FAULT_ON_DEVICE = "A fault has been detected on {0}, go to the fault window? Pressing 'No' will disable this pop-up message and must be re-enabled in Settings.";
        public const string MESSAGE_FAULT_OCCURRED = "Fault Occurred";
        public const string MESSAGE_FAILED_SAVE = "Failed to Save";
        public const string MESSAGE_INVALIDPARAMS = "This plot contains parameters that can't be captured on the attached device! They must be removed. Continue loading?";
        public const string MESSAGE_LEFT_CLICK_SET = "Left Click to Select Parameter";
        public const string MESSAGE_MAXPARAMS = "Maximum of four parameters per capture!";
        public const string MESSAGE_MISSING_DATASHEET = "Could not find a datasheet for device. ID string(s): {0}!";
        public const string MESSAGE_NET_MAN_STATE_CHANGE = "The 'Network Management State' will need to be changed\n" + "to Operational to have an effect. Do this?";
        public const string MESSAGE_NET_CHANGE_REQ = "NMT State Change Required";
        public const string MESSAGE_NEED_DEVICE_SELECT = "A device must be selected from the device tree.\n " +
                                            "This can be done by double clicking on an element in the tree." +
                                            "If none is avalable, please connect one to an available USB port.\n";
        public const string MESSAGE_NOCAPPARAMS = "There are no parameters set to capture!";
        public const string MESSAGE_NOSAVEDADDRESS = "Device(s): {0} have no saved address configuration(s).";
        public const string MESSAGE_NO_OPERATING_MODES_FOUND = "No operating modes found for this device.";
        public const string MESSAGE_NO_OPERATING_MODES = "No Operating Modes";
        public const string MESSAGE_POWER_CYCLE = "Power Cycle";
        public const string MESSAGE_PREPARING_DEVICE = "Preparing {0} for connection.";
        public const string MESSAGE_REJECT_DATASHEET = "Are you sure you want to reject the current datasheet?";
        public const string MESSAGE_REMOVE_DATASHEET = "Are you sure you want to remove the selected datasheet? It will be permanently deleted.";
        public const string MESSAGE_REPLACE_FAILED = "The replace operation failed!";
        public const string MESSAGE_REQUIRED_ADDRESS = "Required Address Configuration";
        public const string MESSAGE_RESET_DEVICE = "Reset {0}?";
        public const string MESSAGE_RIGHT_CLICK_SET = "Right Click to Select Parameter";
        public const string MESSAGE_SCAN_COMPLETE = "Communicator device scan completed, proceeding to open device panels.";

        public const string MESSAGE_WINDOW_NOT_SUPPORTED = "This device does not support this window, and it will be closed to prevent issues. If you believe this is an error, please check you device configuration is properly loaded.";
        #endregion /Messages

        #region Will Wright
        public const string RETICULATING_SPLINES = "Reticulating Splines";
        #endregion /Will Wright

        #region Statusword
        public const string NOT_READY_SWITCH_ON = "Not ready to switch on";
        public const string SWITCH_ON_DISABLED = "Switch on disabled";
        public const string READY_SWITCH_ON = "Ready to switch on";
        public const string SWITCHED_ON = "Switched on";
        public const string SWITCH_ON_ENABLE_OPERATION = "Switch on and enable operation";
        public const string SWITCH_ON = "Switch on";
        public const string OPERATION_DISABLED = "Operation disabled";
        public const string OPERATION_ENABLED = "Operation enabled";
        public const string QUICK_STOP_ACTIVE = "Quick stop active";
        public const string FAULT_REACTION_ACTIVE = "Fault reaction active";
        #endregion Statusword

        #region Trigger
        public const string ABOVE_UPPER = "Above Upper Limit";
        public const string BELOW_LOWER = "Below Lower Limit";
        public const string TRIGGER_OUTSIDE = "Trigger Outside Limits";
        public const string TRIGGER_POSITION = "Trigger Position";
        public const string TRIGGER_BETWEEN = "Trigger Between Limits";
        #endregion /Trigger

        #endregion Special

        #endregion /Constants

        #region Translation Dictionary
        //TODO: break into letter dictionaries to reduce possible collisions?

        /// <summary>
        /// This dictionary contains the translations of particular words by
        /// public constant strings refrenced from this class to to the
        /// translated word as a string
        /// </summary>
        private static readonly IDictionary<String, String[]> Words = new Dictionary<String, String[]>()
        {
            #region General

            #region A
            { ABORT, new string[LANGUAGE_COUNT] { ABORT, "Anular", "Stornieren", "Cancelar", C, "Annuler" , H } },
            { ABOUT, new string[LANGUAGE_COUNT] { ABOUT, "Sobre", "über", P, C, F , H } },
            { ABOVE_UPPER, new string[LANGUAGE_COUNT] { "Above Upper Limit", "Por encima del límite superior", "über Obergrenze", P, C, F , H } },
            { ABSOLUTE, new string[LANGUAGE_COUNT] { ABSOLUTE, "Absoluto", "Absolut", "Absoluto", C, "Absolu" , H } },
            { ACCELERATION, new string[LANGUAGE_COUNT] { ACCELERATION, "Aceleración", "Beschleunigung", "Aceleração", C, "Accélération" , H } },
            { ACCEL_DELTA_SPEED, new string[LANGUAGE_COUNT] { ACCEL_DELTA_SPEED, S, "Geschwindigkeitsänderung über Zeitbasis ", P, C, F , H } },
            { ACCEL_DELTA_TIME, new string[LANGUAGE_COUNT] { ACCEL_DELTA_TIME, S, "Zeitbasis für die Geschwindigkeitsänderung", P, C, F , H } },
            { ACCEL_LIMIT, new string[LANGUAGE_COUNT] { ACCEL_LIMIT, S, "Beschleunigungslimit", P, C, F , H } },
            { ACCEPT, new string[LANGUAGE_COUNT] { ACCEPT, "Aceptar", "Annehmen", P, C, F , H } },
            { ACCEPT_VALUES, new string[LANGUAGE_COUNT] { ACCEPT_VALUES, S, G, P, C, F , H } },
            { ACCEL_LEVEL, new string[LANGUAGE_COUNT]
            {
                ACCEL_LEVEL,
                "Nivel de aceleración seleccionado",
                "Beschleunigungsstufe ausgewählt",
                "Nível de aceleração selecionado",
                C,
                "Niveau d'accélération sélectionné"
            , H } },
            { ACCEL_NONNEG, new string[LANGUAGE_COUNT]
            {
                ACCEL_NONNEG,
                "Aceleración no puede ser negativa",
                "Beschleunigung kann nicht negativ sein",
                "Aceleração não pode ser negativa",
                C,
                "Accélération ne peut être négative"
            , H } },
            { ACCEL_UPDATED, new string[LANGUAGE_COUNT]
            {
                ACCEL_UPDATED,
                "Aceleración actualizada a {0}",
                "Beschleunigung aktualisiert auf {0}",
                "Aceleração atualizada para {0}",
                C,
                "Accélération mise à jour à {0}"
            , H } },
            { ACTIVE, new string[LANGUAGE_COUNT] { ACTIVE, S, G, P, C, F , H } },
            { ACTIVE_FAULT_COUNT, new string[LANGUAGE_COUNT] { ACTIVE_FAULT_COUNT, "Recuento de Errores Activo", "Fehlerzähler aktiv", P, C, F , H } },
            { ACTIVE_WARN_COUNT, new string[LANGUAGE_COUNT] { ACTIVE_WARN_COUNT, "Recuento de Advertencias Activas", "Warnungszähler aktiv", P, C, F , H } },
            { ACTUAL_POSITION, new string[LANGUAGE_COUNT] { ACTUAL_POSITION, S, "Aktuell Position", P, C, F , H } },
            { ACTUAL_VELOCITY, new string[LANGUAGE_COUNT] { ACTUAL_VELOCITY, S, "Aktuelle Geschwindigkeit", P, C, F , H } },
            { ADD, new string[LANGUAGE_COUNT] { ADD, "Agregar", "Hinzufügen", "Adicionar", C, "Ajouter" , H } },
            { ADD_AXIS_PARAMETER, new string[LANGUAGE_COUNT] { ADD_AXIS_PARAMETER, "Agregue Parámetro de Eje", "Achsenparameter hinzufügen", P, C, F , H } },
            { ADDED_DATASHEETS, new string[LANGUAGE_COUNT] { ADDED_DATASHEETS, S, G, P, C, F , H } },
            { ADD_SERIAL_NUMBER, new string[LANGUAGE_COUNT] { ADD_SERIAL_NUMBER, "Agregue Número de Serie", "Seriennummer hinzufügen", P, C, F , H } },
            { ADD_UPDATE, new string[LANGUAGE_COUNT] { ADD_UPDATE, S, "Update hinzufügen", P, C, F , H } },
            { ADD_UPDATE_DATASHEET, new string[LANGUAGE_COUNT] { ADD_UPDATE_DATASHEET, S, "Update Datenblatt", P, C, F , H } },
            { ADD_UPDATE_DATASHEET_INSTR, new string[LANGUAGE_COUNT] { ADD_UPDATE_DATASHEET_INSTR, S, G, P, C, F , H } },
            { ADDRESS, new string[LANGUAGE_COUNT] { ADDRESS, "Dirección", "Adresse", "Endereço", C, "Adresse" , H } },
            { ADVANCED, new string[LANGUAGE_COUNT] { ADVANCED, "Avanzado", "Erweitert", P, C, F , H } },
            { ALIGNMENT, new string[LANGUAGE_COUNT] { ALIGNMENT, "Alineación", "Ausrichtung", "Alinhamento", C, "Alignement" , H } },
            { ALLIED, new string[LANGUAGE_COUNT] { ALLIED, "Allied", "Allied", "Allied", "Allied", "Allied" , H } },
            { ALLNET, new string[LANGUAGE_COUNT]
            {
                ALLNET,
                "Dispositivo ALLNET encontrado en IP: ",
                "ALLNET-Gerät in IP gefunden: ",
                "Dispositivo ALLNET encontrado no IP: ",
                C,
                "Dispositif ALLNET trouvé à IP: "
            , H } },
            { ALLOW_WINDOWS, new string[LANGUAGE_COUNT] { ALLOW_WINDOWS, "Permita todas las ventanas", "Alle Fenster zulassen", P, C, F , H } },
            { ANALOG, new string[LANGUAGE_COUNT] { ANALOG, S, "Analog", P, C, F , H } },
            { ANALOG_INPUT1, new string[LANGUAGE_COUNT] { ANALOG_INPUT1, S, "Analogeingang_1", P, C, F , H } },
            { ANALOG_INPUT2, new string[LANGUAGE_COUNT] { ANALOG_INPUT2, S, "Analogeingang_2", P, C, F , H } },
            { ANALYZE, new string[LANGUAGE_COUNT] { ANALYZE, "Analizar", "Analysieren", "Analisar", C, "Analyser" , H } },
            { APPLICATION, new string[LANGUAGE_COUNT] { APPLICATION, "Aplicación", "Anwendung", P, C, F , H } },
            { APP_SETTINGS, new string[LANGUAGE_COUNT] { "Application Settings", "Configuración General", "Anwendungseinstellungen", "Configurações do aplicativo", C, "Paramètres de application" , H } },
            { APPLY, new string[LANGUAGE_COUNT] { APPLY, "Aplicar", "Anwenden", "Aplicar", C, "Appliquer" , H } },
            { AQUIRING_CONFIG_PARAMS, new string[LANGUAGE_COUNT] { AQUIRING_CONFIG_PARAMS, S, G, P, C, F , H } },
            { AQUIRING_MANUFAC_PARAMS, new string[LANGUAGE_COUNT] { AQUIRING_MANUFAC_PARAMS, S, G, P, C, F , H } },
            { ASSERT, new string[LANGUAGE_COUNT] { ASSERT, "Afirmar", "Behaupten", "Afirmar", C, "Affirmer" , H } },
            { ATTACHING_A_COMMUNICATOR, new string[LANGUAGE_COUNT] { ATTACHING_A_COMMUNICATOR, S, "Einen Kommunikator Verbinden", P, C, F , H } },
            { ATTEMPT_CONNECT, new string[LANGUAGE_COUNT]
            {
                ATTEMPT_CONNECT,
                "Intentando conectar directamente con {0}...",
                "Es wird versucht, eine direkte Verbindung mit {0} herzustellen...",
                "Tentando se conectar diretamente com {0}...",
                C,
                "Tente de se connecter directement à {0}"
            , H } },
            { AUTHORIZED, new string[LANGUAGE_COUNT] { AUTHORIZED, "Autorizado", "Autorisiert", P, C, F , H } },
            { AUTOMATIC, new string[LANGUAGE_COUNT] { AUTOMATIC, S, "Auto", P, C, F , H } },
            { AUTOMATIC_AXIS, new string[LANGUAGE_COUNT] { AUTOMATIC_AXIS, "Eje Automático", "Automatische Achsenskalierung", P, C, F , H } },
            { AUTOMATIC_CORRELATION_WEIGHT, new string[LANGUAGE_COUNT]
            {
                AUTOMATIC_CORRELATION_WEIGHT,
                "Ponderación de correlación automática",
                "Automatische Korrelationsgewichtung",
                "Ponderação de correlação automática",
                C,
                "Pondération automatique de corrélation"
            , H } },
            { AVAILABLE_DRIVES, new string[LANGUAGE_COUNT] { AVAILABLE_DRIVES, "Transmisiones Disponibles", "Verfügbare Laufwerke", "Transmissões Disponíveis", C, "Dispositifs disponibles" , H } },
            { AVERAGE_TIME, new string[LANGUAGE_COUNT] { AVERAGE_TIME, "Tiempo promedio", "Durchschnittszeit", P, C, F , H } },
            { AWAITING_TRIGGER, new string[LANGUAGE_COUNT] { AWAITING_TRIGGER, "Esperando Desencadenador", "Trigger aktiv", P, C, F , H } },
            { AXIS, new string[LANGUAGE_COUNT] { AXIS, "Ejes", "Achse", "Eixos", C, "Axes" , H } },
            { AXIS_SCALING, new string[LANGUAGE_COUNT] { AXIS_SCALING, "Escalando los Ejes", "Achsenskalierung", "Escalando os Eixos", C, "Mise à l'échelle des axes" , H } },
            #endregion A

            #region B
            { BACKGROUND_COLOR, new string[LANGUAGE_COUNT] { BACKGROUND_COLOR, "Color de Fondo", "Hintergrundfarbe", P, C, F , H } },
            { BAD_FILE_NAME, new string[LANGUAGE_COUNT] { BAD_FILE_NAME, S, G, P, C, F , H } },
            { BAUD_RATE, new string[LANGUAGE_COUNT] { BAUD_RATE, "Velocidad de Baudios", "Baudrate", P, C, F , H } },
            { BELOW_LOWER, new string[LANGUAGE_COUNT] { BELOW_LOWER, "Por debajo del límite inferior", "unter Untergrenze", P, C, F , H } },
            { BIN_FILE_NO_MATCH_FILE, new string[LANGUAGE_COUNT] { BIN_FILE_NO_MATCH_FILE, S, G, P, C, F , H } },
            { BIT_DETECTION, new string[LANGUAGE_COUNT] { BIT_DETECTION, S, "BIT_DETECTION", P, C, F , H } },
            { BIT_DETECTOR, new string[LANGUAGE_COUNT] { BIT_DETECTOR, S, "BIT_DETECTOR", P, C, F , H } },
            { BITFIELD, new string[LANGUAGE_COUNT] { BITFIELD, S, "BITFIELD", P, C, F , H } },
            { BIT_SELECTOR, new string[LANGUAGE_COUNT] { BIT_SELECTOR, S, "BIT_SELECTOR", P, C, F , H } },
            { BIT_SELECTOR_LOW, new string[LANGUAGE_COUNT] { BIT_SELECTOR_LOW, S, "BIT_SELECTOR_LOW", P, C, F , H } },
            { BOTTOM, new string[LANGUAGE_COUNT] { BOTTOM, "Fondo", "Unterseite", P, C, F , H } },
            { BRAKE, new string[LANGUAGE_COUNT] { BRAKE, "Freno", "Bremse", "Freio", C, "Frein" , H } },
            { BROWSE_CONFIGURATION_FILES, new string[LANGUAGE_COUNT] { BROWSE_CONFIGURATION_FILES, S, "Config-files durchsuchen", P, C, F , H } },
            { BUFFER, new string[LANGUAGE_COUNT] { BUFFER, "Búfer", "Puffer", "Tampão", "缓冲区", "Tampon" , H } },
            { BUFFERING, new string[LANGUAGE_COUNT] { BUFFERING, "Cargando", "Pufferung", P, C, F , H } },
            { BUILD_DATETIME, new string[LANGUAGE_COUNT] { BUILD_DATETIME, "Compilación fecha/hora", "Build Datum", P, C, F , H } },
            #endregion B

            #region C
            { CALCULATE_GAINS, new string[LANGUAGE_COUNT] { CALCULATE_GAINS, S, "Verstärkungen berechnen", P, C, F , H } },
            { CALCULATE, new string[LANGUAGE_COUNT] { CALCULATE, S, "Berechnen", P, C, F , H } },
            { CALCULATING, new string[LANGUAGE_COUNT] { CALCULATING, "Calculando", "Berechne", P, C, F , H } },
            { CANCEL, new string[LANGUAGE_COUNT] { CANCEL, "Cancelar", "Abbrechen", "Cancelar", C, "Annuler" , H } },
            { CANCEL_CAPTURE, new string[LANGUAGE_COUNT] { CANCEL_CAPTURE, "Cancele Captura", "Aufnahme abbrechen", P, C, F , H } },
            { CANOPEN, new string[LANGUAGE_COUNT] { CANOPEN, CANOPEN, CANOPEN, CANOPEN, CANOPEN, CANOPEN , H } },
            { CAPTURE_RANGE, new string[LANGUAGE_COUNT] { CAPTURE_RANGE, "Rango de captura", "Erfassungsbereich", "Alcance De Captura", C, "Plage de capture" , H } },
            { CAPTURE_SIZE, new string[LANGUAGE_COUNT] { CAPTURE_SIZE, S, "Aufnahmegröße", P, C, F , H } },
            { CAPTURE_STATUS, new string[LANGUAGE_COUNT] { CAPTURE_STATUS, "Estado de Captura", "Aufnahmestatus", P, C, F , H } },
            { CAPTURE, new string[LANGUAGE_COUNT] { CAPTURE, "Capturar", "Erfassen", "Capturar", C, "Capturer" , H } },
            { CAPTURED_POINTS, new string[LANGUAGE_COUNT] { CAPTURED_POINTS, "Capturado {0} de {1} puntos", "{0} von {1} Punkten aufgenommen", "{0} P {1}", "{0} C {1}", "{0} F {1}" , H } },
            { CAPTURING, new string[LANGUAGE_COUNT] { CAPTURING, "Capturando", "aufzeichnend", P, C, F , H } },
            { CAPTURE_ON_START, new string[LANGUAGE_COUNT] { CAPTURE_ON_START, "Captura en el Inicio", "Beim starten aufzeichnen", P, C, F , H } },
            { CENTER_PLOT, new string[LANGUAGE_COUNT] { CENTER_PLOT, "Centre el Gráfico", "Zentrale Anzeige", P, C, F , H } },
            { CHECK_IP, new string[LANGUAGE_COUNT] { CHECK_IP, "Por favor, comprobar IP", "IP bitte überprüfen ", "Por favor, verificar IP", C, "Vérifier IP" , H } },
            { CHECK_CONNECTION, new string[LANGUAGE_COUNT]
            {
                CHECK_CONNECTION,
                "Por favor compruebe la conexión de la red física",
                "Bitte überprüfen Sie die physische Netzwerkverbindung",
                "Por favor, verifique a conexão de rede física",
                C,
                "Veuillez vérifier la connexion physique au réseau"
            , H } },
            { CLEAR, new string[LANGUAGE_COUNT] { CLEAR, "Despejar", "Löschen", "Limpar", C, "Effacer" , H } },
            { CLEARING_FLASH, new string[LANGUAGE_COUNT] { CLEARING_FLASH, S, G, P, C, F , H } },
            { CLOSE, new string[LANGUAGE_COUNT] { CLOSE, "Cerrar", "Schließen", P, C, F , H } },
            { CLOSE_APP, new string[LANGUAGE_COUNT]
            {
                CLOSE_APP,
                "Paradando - Cerrando la Aplicación",
                "Ende - Programm Schließend",
                "Parando - Fechando Aplicativo",
                C,
                "Arrêt - Fermeture de l'application"
            , H } },
            { CLOSED_LOOP_FREQ, new string[LANGUAGE_COUNT] { CLOSED_LOOP_FREQ, S, G, P, C, F , H } },
            { CODE, new string[LANGUAGE_COUNT] { CODE, S, G, P, C, F , H } },
            { COIL_INDUCTANCE, new string[LANGUAGE_COUNT] { COIL_INDUCTANCE, S, "Wicklungsinduktivität", P, C, F , H } },
            { COIL_RESISTANCE, new string[LANGUAGE_COUNT] { COIL_RESISTANCE, S, "Wichlungswiderstand", P, C, F , H } },
            { COLLATING_DATA, new string[LANGUAGE_COUNT] { COLLATING_DATA, "Recopilando los datos", "Strom im geschlossenen Regelkreis", P, C, F , H } },
            { COLOR, new string[LANGUAGE_COUNT] { COLOR, "Color", "Farbe", P, C, F , H } },
            { COMBOBOX_ERROR, new string[LANGUAGE_COUNT] { COMBOBOX_ERROR, "Error al realizar ComboBox", "Kombinationsfeld Fehler", P, C, F , H } },
            { COMMANDS, new string[LANGUAGE_COUNT] { COMMANDS, S, "Befehle", P, C, F , H } },
            { COMMAND_VAL, new string[LANGUAGE_COUNT] { COMMAND_VAL, "Valor del Comando", "Sollwert", P, C, F , H } },
            { COMM_FAILED, new string[LANGUAGE_COUNT] { COMM_FAILED, "Falló la comunicación con los dispositivos", "Gerätekommunikaiton fehlgeschlagen", P, C, F , H } },
            { COMMUNICATOR, new string[LANGUAGE_COUNT] { COMMUNICATOR, "Comunicador", "Communikator", P, C, F , H } },
            { COMMUNICATOR_DETECTION, new string[LANGUAGE_COUNT] { COMMUNICATOR_DETECTION, S, G, P, C, F , H } },
            { COMMUNICATOR_NOT_DETECTED, new string[LANGUAGE_COUNT] { COMMUNICATOR_NOT_DETECTED, S, G, P, C, F , H } },
            { CONFIGURATION_PARAMS, new string[LANGUAGE_COUNT] { CONFIGURATION_PARAMS, "Parámetros de Configuración ", "Parameter konfigurieren", P, C, F , H } },
            { CONFIGURATION_PARAMS_FILE, new string[LANGUAGE_COUNT] { CONFIGURATION_PARAMS_FILE, "Archivo de Parámetros Configuración", "Parameter Konfigurierendatei", P, C, F , H } },
            { CONFIGURATION_SCAN, new string[LANGUAGE_COUNT] { CONFIGURATION_SCAN, "Escaneo de Configuración", "Konfiguration scannen", P, C, F , H } },
            { CONFIGURATION, new string[LANGUAGE_COUNT] { CONFIGURATION, "Configurar", "Konfigurieren", "Configurar", C, "Configurer" , H } },
            { CONFIGURE, new string[LANGUAGE_COUNT] { CONFIGURE, "Configuración", "Konfigurieren", "Configuración", C, F , H } },
            { CONFIG_SAVE_LOAD_TOOL, new string[LANGUAGE_COUNT] { CONFIG_SAVE_LOAD_TOOL, S, G, P, C, F , H } },
            { CONNECT, new string[LANGUAGE_COUNT] { CONNECT, "Conectar", "Verbinden", "Conectar", C, "Connecter" , H } },
            { CONNECT_ABORT, new string[LANGUAGE_COUNT] { CONNECT_ABORT, "Conexión Anulada", "Verbindung Abgebrochen", "Conexão Abortada", C, "Connexion Interrompue" , H } },
            { CONNECT_FAIL, new string[LANGUAGE_COUNT] { CONNECT_FAIL, "Error de conexión", "Verbindung fehlgeschlagen", "Falha na conexão", C, "Connexion a échoué" , H } },
            { CONNECTION_DETECTION, new string[LANGUAGE_COUNT] { CONNECTION_DETECTION, S, "Verbindungserkennung", P, C, F , H } },
            { CONNECT_SUCCESS, new string[LANGUAGE_COUNT] { CONNECT_SUCCESS, "¡Conexión exitosa!", "Verbindung erfolgreich!", "Conexão bem-sucedida!", C, "Connexion réussie!" , H } },
            { CONNECT_UNDERWAY, new string[LANGUAGE_COUNT] { CONNECT_UNDERWAY, "Conexión en Curso", "Verbindung Läuft", "Conexão em Andamento", C, "Connexion en Cours" , H } },
            { CONNECTING_MOTOR_DEVICE, new string[LANGUAGE_COUNT] { CONNECTING_MOTOR_DEVICE, S, "Verbindung zur Motorsteuerung", P, C, F , H } },
            { CONTROLLED_STOP, new string[LANGUAGE_COUNT] { CONTROLLED_STOP, "Parada Controlada", "Kontrollierter Halt", "Parada Controlada", C, "Arrêt Contrôlé" , H } },
            { CONTROLLER, new string[LANGUAGE_COUNT] { CONTROLLER, "Controlador", "Regler", "Controlador", C, "Contrôleur" , H } },
            { CONTROLWORD, new string[LANGUAGE_COUNT] { CONTROLWORD, "Controlpalabra", "Controlword", P, C, F , H } },
            { COMPARE_FILE, new string[LANGUAGE_COUNT] { COMPARE_FILE, S, "Datei vergleichen", P, C, F , H } },
            { COMPOUND_WARNING, new string[LANGUAGE_COUNT] { COMPOUND_WARNING, S, G, P, C, F , H } },
            { COMPLETED, new string[LANGUAGE_COUNT] { COMPLETED, "Terminado", "Vollständig", P, C, F , H } },
            { COMPLETED_MANUFAC_DATA, new string[LANGUAGE_COUNT] { COMPLETED_MANUFAC_DATA, S, G, P, C, F , H } },
            { COMPLETED_SERIAL_UPDATE, new string[LANGUAGE_COUNT] { COMPLETED_SERIAL_UPDATE, S, G, P, C, F , H } },
            { COPYRIGHT, new string[LANGUAGE_COUNT] { COPYRIGHT, S, G, P, C, F , H } },
            { CORRELATION, new string[LANGUAGE_COUNT] { CORRELATION, "Correlación", "Korrelation", "Correlação", C, "Corrélation" , H } },
            { CORRESPONDANCE, new string[LANGUAGE_COUNT] { CORRESPONDANCE, S, "Korrespondenz", P, C, F , H } },
            { CURRENT_ACTUAL_VALUE, new string[LANGUAGE_COUNT] { CURRENT_ACTUAL_VALUE, "Valor Real de la Corriente", "Aktueller Stromwert", P, C, F , H } },
            { CURRENT_ACTUAL_VALUE_INFO, new string[LANGUAGE_COUNT] { CURRENT_ACTUAL_VALUE_INFO, S, G, P, C, F , H } },
            { CURRENT_FIRMWARE_INFO, new string[LANGUAGE_COUNT] { CURRENT_FIRMWARE_INFO, S, G, P, C, F , H } },
            { CURRENT_GAINS_CALCULATOR, new string[LANGUAGE_COUNT] { CURRENT_GAINS_CALCULATOR, S, "Stromverstärkung berechnen", P, C, F , H } },
            { CURRENT_LIMIT, new string[LANGUAGE_COUNT] { CURRENT_LIMIT, "Límite de Corriente", "Stromlimit", P, C, F , H } },
            { CURRENT_STATE, new string[LANGUAGE_COUNT] { CURRENT_STATE, "Estado Actual", "Aktueller Zustand", P, C, F , H } },
            { CWU, new string[LANGUAGE_COUNT]
            {
                CWU,
                "Trabajador de conexión no inicializado",
                "Verbindungsarbeiter nicht initialisiert",
                "Trabalhador de conexão não inicializado",
                C,
                "Travailleur de connexion non initialisé"
            , H } },
            { CYCLE_TIME, new string[LANGUAGE_COUNT] { CYCLE_TIME, "Tiempo de ciclo", "Zykluszeit", P, C, F , H } },
            { CYCLIC_SYNC_PROFILE, new string[LANGUAGE_COUNT] { CYCLIC_SYNC_PROFILE, S, "Cyclic Synchronous Profile", P, C, F , H } },
            { CYCLIC_SYNC_TORQUE, new string[LANGUAGE_COUNT] { CYCLIC_SYNC_TORQUE, S, "Cyclic Synchronous Torque", P, C, F , H } },
            { CYCLIC_SYNC_TORQUE_ANGLE, new string[LANGUAGE_COUNT] { CYCLIC_SYNC_TORQUE_ANGLE, S, "Cyclic Synchronous Torque Commutation Angle", P, C, F , H } },
            { CYCLIC_SYNC_VELOCITY, new string[LANGUAGE_COUNT] { CYCLIC_SYNC_VELOCITY, S, "Cyclic Synchronous Velocity", P, C, F , H } },
            #endregion C

            #region D
            { DATA_DOWN, new string[LANGUAGE_COUNT] { DATA_DOWN, "Vínculo de Datos Hacia Abajo", "Datenverbindung unterbrochen", P, C, F , H } },
            { DATASHEET, new string[LANGUAGE_COUNT] { DATASHEET, "Hoja de Datos", "Datenblatt", P, C, F , H } },
            { DATASHEET_ADD_UPDATE, new string[LANGUAGE_COUNT] { DATASHEET_ADD_UPDATE, S, G, P, C, F , H } },
            { DATASHEET_DIALOG, new string[LANGUAGE_COUNT] { DATASHEET_DIALOG, S, "Datenblatt Dialog", P, C, F , H } },
            { DATASHEET_LOADING_TOOL, new string[LANGUAGE_COUNT] { DATASHEET_LOADING_TOOL, S, G, P, C, F , H } },
            { DATASHEET_REMOVAL, new string[LANGUAGE_COUNT] { DATASHEET_REMOVAL, S, G, P, C, F , H } },
            { DATASHEET_SCAN, new string[LANGUAGE_COUNT] { DATASHEET_SCAN, S, G, P, C, F , H } },
            { DATASHEET_SCAN_NO_CANDIDATES, new string[LANGUAGE_COUNT] { DATASHEET_SCAN_NO_CANDIDATES, S, G, P, C, F , H } },
            { DATASHEET_SCAN_COMPLETE, new string[LANGUAGE_COUNT] { DATASHEET_SCAN_COMPLETE, S, G, P, C, F , H } },
            { DATASHEET_SCAN_INSTR, new string[LANGUAGE_COUNT] { DATASHEET_SCAN_INSTR, S, G, P, C, F , H } },
            { DATASHEET_VERSION, new string[LANGUAGE_COUNT] { DATASHEET_VERSION, S, G, P, C, F , H } },
            { DATASHEET_VIEWER, new string[LANGUAGE_COUNT] { DATASHEET_VIEWER, "Visor de Hojas de Datos", "Datenblatt ansehen", P, C, F , H } },
            { DATA_CAPTURE, new string[LANGUAGE_COUNT] { DATA_CAPTURE, S, "Datenmonitor", P, C, F , H } },
            { DATA_LINK, new string[LANGUAGE_COUNT] { DATA_LINK, "Vínculo de datos", "Datenverbindung", P, C, F , H } },
            { DC_LINK_VOLTAGE, new string[LANGUAGE_COUNT] { DC_LINK_VOLTAGE, "Voltaje de Enlace de CC", "DC-Link Spannung", P, C, F , H } },
            { DC_LINK_VOLTAGE_INFO, new string[LANGUAGE_COUNT] { DC_LINK_VOLTAGE_INFO, S, G, P, C, F , H } },
            { DEBUG, new string[LANGUAGE_COUNT] { DEBUG, "Depurar", "Debuggen", "Debugar", C, "Déboguer" , H } },
            { DECEL, new string[LANGUAGE_COUNT] { DECEL, "Deceleración", "Verzögerung", "Desaceleração", C, "Décélération" , H } },
            { DECEL_LEVEL, new string[LANGUAGE_COUNT]
            {
                DECEL_LEVEL,
                "Nivel de desaceleración seleccionado",
                "Verzögerungsstufe ausgewählt",
                "Nível de desaceleração selecionado",
                C,
                "Niveau de décélération sélectionné"
            , H } },
            { DECEL_LIMIT, new string[LANGUAGE_COUNT] { DECEL_LIMIT, S, "Verzögerungslimit", P, C, F , H } },
            { DECEL_NON_NEG, new string[LANGUAGE_COUNT]
            {
                DECEL_NON_NEG,
                "Desaceleración no puede ser negativa",
                "Verzögerung kann nicht negativ sein",
                "Desaceleração não pode ser negativa",
                C,
                "Décélération ne peut être négative"
            , H } },
            { DECEL_UPDATED, new string[LANGUAGE_COUNT]
            {
                DECEL_UPDATED,
                "Desaceleración actualizada a {0}",
                "Verzögerung aktualisiert auf {0}",
                "Desaceleração atualizada para {0}",
                C,
                "Décélération mise à jour à {0}"
            , H } },
            { DECEL_DELTA_SPEED, new string[LANGUAGE_COUNT] { DECEL_DELTA_SPEED, S, "Verzögerungsänderung über Zeibasis", P, C, F , H } },
            { DECEL_DELTA_TIME, new string[LANGUAGE_COUNT] { DECEL_DELTA_TIME, "Diseño predeterminado", "Zeitbasis zur Verzögerungsänderung", P, C, F , H } },
            { DEFAULT, new string[LANGUAGE_COUNT] { DEFAULT, "Valor Predeterminado", "Standard", P, C, F , H } },
            { DEFAULT_LAYOUT, new string[LANGUAGE_COUNT] { DEFAULT_LAYOUT, "Diseño predeterminado", "Standard Layout", P, C, F , H } },
            { DEFAULT_WINDOWS, new string[LANGUAGE_COUNT] { DEFAULT_WINDOWS, "Ventanas Predeterminadas", "Standardfenster", "Janelas da Padrão", C, "Fenêtres par Défaut" , H } },//TODO: Check German -=CAH=-
            { DELAY_TIME, new string[LANGUAGE_COUNT] { DELAY_TIME, S, "Verzögerungszeit", P, C, F , H } },
            { DELETE, new string[LANGUAGE_COUNT] { DELETE, "Eliminar", "Löschen", "Deletar", C, "Effacer" , H } },
            { DELETE_DATASHEET, new string[LANGUAGE_COUNT] { DELETE_DATASHEET, S, "Datenblatt löschen", P, C, F , H } },
            { DEMAND, new string[LANGUAGE_COUNT] { DEMAND, S, "Bedarf", P, C, F , H } },
            { DEVICE_FOUND, new string[LANGUAGE_COUNT] { DEVICE_FOUND, "Dispositivo Encontrado", "Gerät gefunden", P, C, F , H } },
            { DEVICE, new string[LANGUAGE_COUNT] { DEVICE, "Aparato", "Gerät", P, C, F , H } },
            { DEVICE_CONFIG, new string[LANGUAGE_COUNT] { DEVICE_CONFIG, "Configuración del Dispositivo", "Gerätekonfiguration", P, C, F , H } },
            { DEVICE_DIRECT, new string[LANGUAGE_COUNT] { DEVICE_DIRECT, "Dispositivo directo:", "Gerät direkt ansprechen", P, C, F , H } },
            { DEVICE_SCAN, new string[LANGUAGE_COUNT] { DEVICE_SCAN, "Escaneo de dispositivos", "Gerätescan", "Digitalização do dispositivo", C, "Balayer de appareil" , H } },
            { DEVICE_USER_SETTINGS, new string[LANGUAGE_COUNT] { DEVICE_USER_SETTINGS, "Configuración del Usuario del Dispositivo", "Benutzerspezifische Geräteeinstellungen", P, C, F , H } },
            { DEVICE_NMT_STATE_CHANGE, new string[LANGUAGE_COUNT]
            {
                DEVICE_NMT_STATE_CHANGE,
                "Estado de administración de red de dispositivos Cambie a",
                "Status der Netzgeräteverwaltung geändert zu",
                P,
                C,
                F
            , H } },
            { DEVICE_SCAN_START, new string[LANGUAGE_COUNT] { DEVICE_SCAN_START, "Análisis de Dispositivos Iniciado", "Gerätescan gestartet", P, C, F , H } },
            { DIAGNOSTICS, new string[LANGUAGE_COUNT] { DIAGNOSTICS, S, "Diagnose", P, C, F , H } },
            { DIALOGUE, new string[LANGUAGE_COUNT] { DIALOGUE, "Diálogo", "Dialog", "Diálogo", C, "Dialogue" , H } },
            { DIGITAL, new string[LANGUAGE_COUNT] { DIGITAL, S, "Digital", P, C, F , H } },
            { DIGITAL_ANALOG_MAPPING, new string[LANGUAGE_COUNT] { DIGITAL_ANALOG_MAPPING, S, "Dig/Ang Mapping", P, C, F , H } },
            { DIGITAL_INPUTS, new string[LANGUAGE_COUNT] { DIGITAL_INPUTS, S, "Digital Input", P, C, F , H } },
            { DIGITAL_OUTPUTS, new string[LANGUAGE_COUNT] { DIGITAL_OUTPUTS, S, "Digital Output", P, C, F , H } },
            { DIGITIZATION, new string[LANGUAGE_COUNT] { DIGITIZATION, S, "Digitalisierung", P, C, F , H } },
            { DIGITIZATION_SET_POINT_1, new string[LANGUAGE_COUNT] { DIGITIZATION_SET_POINT_1, S, "DIGITIZATION_SET_POINT_1", P, C, F , H } },
            { DIGITIZATION_SET_POINT_2, new string[LANGUAGE_COUNT] { DIGITIZATION_SET_POINT_2, S, "DIGITIZATION_SET_POINT_2", P, C, F , H } },
            { DIPL1, new string[LANGUAGE_COUNT] { DIPL1, S, "Digitaler Input zu Parameter Link 1", P, C, F , H } },
            { DIPL2, new string[LANGUAGE_COUNT] { DIPL2, S, "Digitaler Input zu Parameter Link 2", P, C, F , H } },
            { DIRECT_COMMAND, new string[LANGUAGE_COUNT] { DIRECT_COMMAND, S, "Befehl", P, C, F , H } },
            { DISABLE, new string[LANGUAGE_COUNT] { DISABLE, "Apagar", "Deaktivieren", "Desativar", C, "Désactiver" , H } },
            { DISABLE_DRIVE, new string[LANGUAGE_COUNT] { DISABLE_DRIVE, "Desactivar Unidad", "Antrieb abschalten", P, C, F , H } },
            { DISABLED, new string[LANGUAGE_COUNT] { DISABLED, "Desactivado", "Deaktiviert", "Desativado", C, "Désactivé" , H } },
            { DISABLED_MOTOR_OP, new string[LANGUAGE_COUNT] { DISABLED_MOTOR_OP, S, G, P, C, F , H } },
            { DISCONNECT, new string[LANGUAGE_COUNT] { DISCONNECT, "Desconectar", "Trennen", "Desligar", C,    "Déconnecter" , H } },
            { DISCONNECT_WATCHDAWG, new string[LANGUAGE_COUNT] { DISCONNECT_WATCHDAWG, S, G, P, C, F , H } },
            { DISCONNECT_SUCCESS, new string[LANGUAGE_COUNT]
            {
                DISCONNECT_SUCCESS,
                "¡Desconecta de {0} con éxito!",
                "Trennung von {0} erfolgreich!",
                "Desconecte de {0} com sucesso!",
                C,
                "Déconnexion de {0} réussie!"
            , H } },
            { DISCOVERING, new string[LANGUAGE_COUNT] { DISCOVERING, S, G, P, C, F , H } },
            { DISPLACEMENT, new string[LANGUAGE_COUNT] { DISPLACEMENT, "Desplazamiento", "Verschiebung", "Deslocamento", C, "Déplacement" , H } },
            { DISPLACE_UPDATED, new string[LANGUAGE_COUNT]
            {
                DISPLACE_UPDATED,
                "Desplazamiento actualizado a {0}",
                "Verschiebung aktualisiert auf {0}",
                "Deslocamento atualizado para {0}",
                C,
                "Déplacement mis à jour à {0}"
            , H } },
            { DOCKING_AREA, new string[LANGUAGE_COUNT] { DOCKING_AREA, "El área de acoplamiento", "Anschlussbereich", P, C, F , H } },
            { DOCUMENT, new string[LANGUAGE_COUNT] { DOCUMENT, "Documento", "Dokument", P, C, F , H } },
            { DRIVE_COMMANDS, new string[LANGUAGE_COUNT] { DRIVE_COMMANDS, S, "Fahrbefehl", P, C, F , H } },
            { DRIVE_TEMP_NEAR_LIMIT, new string[LANGUAGE_COUNT] { DRIVE_TEMP_NEAR_LIMIT, "Tempe de la Unidad Cerca del Límite", "Motortemperatur nahe der Grenze", P, C, F , H } },
            #endregion D

            #region E
            { EDIT, new string[LANGUAGE_COUNT] { EDIT, "Corregir", "Bearbeiten", "Editar", C, "Editer" , H } },
            { ELEVATED, new string[LANGUAGE_COUNT] { ELEVATED, "Elevado", "Erhöht", P, C, F , H } },
            { ENABLED, new string[LANGUAGE_COUNT] { ENABLED, "Activado", "Aktiviert", "Ativado", C, "Activé" , H } },
            { ENABLE_DEBUG, new string[LANGUAGE_COUNT] { ENABLE_DEBUG, "Habilitar la Depuración", "Debugger aktivieren", P, C, F , H } },
            { ENABLE_DISCON_TIMER, new string[LANGUAGE_COUNT] { ENABLE_DISCON_TIMER, "Activar temporizador de apagado de desconexión", "Abschalttimer aktivieren", P, C, F, H } },
            { ENABLED_MOTOR_OP, new string[LANGUAGE_COUNT] { ENABLED_MOTOR_OP, S, G, P, C, F , H } },
            { ENABLE_DRIVE, new string[LANGUAGE_COUNT] { ENABLE_DRIVE, "Activar Unidad", "Antrieb aktivieren", P, C, F , H } },
            { ENABLE_OPERATION, new string[LANGUAGE_COUNT] { ENABLE_OPERATION, "Activar Operativo", "Operation aktivieren", P, C, F , H } },
            { ENABLE_RAMP, new string[LANGUAGE_COUNT] { ENABLE_RAMP, "Habilite Rampa", "Rampe aktivieren", P, C, F , H } },
            { ENABLE_VOLTAGE, new string[LANGUAGE_COUNT] { ENABLE_VOLTAGE, "Habilite Voltaje", "Spannung aktivieren", P, C, F , H } },
            { ENCODER_ALIGNMENT_STARTED, new string[LANGUAGE_COUNT] { ENCODER_ALIGNMENT_STARTED, S, G, P, C, F , H } },
            { ENCODER_ORIENTATION_INFO, new string[LANGUAGE_COUNT] { ENCODER_ORIENTATION_INFO, S, G, P, C, F , H } },
            { ENTERED_PHASING_MODE, new string[LANGUAGE_COUNT] { ENTERED_PHASING_MODE, S, G, P, C, F , H } },
            { EMERGENCY, new string[LANGUAGE_COUNT] { EMERGENCY, "Emergencia", "Notfall", "Emergência", C, "Urgence" , H } },
            { EMERGENCY_MESSAGES, new string[LANGUAGE_COUNT] { EMERGENCY_MESSAGES, "Mensajes de emergencia", "Notfall Warnung", P, C, F , H } },
            { EMERGENCY_STOP, new string[LANGUAGE_COUNT] { EMERGENCY_STOP, "¡Parada de emergencia!", "Not-Halt!", "Parada de emergência!", C, "Arrêt d'urgence!" , H } },
            { ERROR, new string[LANGUAGE_COUNT] { ERROR, "Error", "Error", "Erro", C, "Erreur" , H } },
            { ERROR_MESSAGES, new string[LANGUAGE_COUNT] { ERROR_MESSAGES, "Mensajes de error", "Fehlermeldung", P, C, F , H } },
            { ESTIMATED_TIME, new string[LANGUAGE_COUNT] { ESTIMATED_TIME, "Estimado {0}s restantes", "{0} G", "{0} P", "{0} C", "{0} F" , H } },
            { EXISTING_DATASHEETS, new string[LANGUAGE_COUNT] { EXISTING_DATASHEETS, S, G, P, S, F , H } },
            { ETHERCAT, new string[LANGUAGE_COUNT] { ETHERCAT, ETHERCAT, ETHERCAT, ETHERCAT, ETHERCAT, ETHERCAT , H } },
            { EXIT, new string[LANGUAGE_COUNT] { EXIT, "Salir", "Ende", "Sair", C, "Quitter" , H } },
            { EXPORT_CAPTURE, new string[LANGUAGE_COUNT] { EXPORT_CAPTURE, "Exporte Captura", "Aufzeichnung exportieren", P, C, F , H } },
            #endregion E

            #region F
            { FACTORY_RESET, new string[LANGUAGE_COUNT] { FACTORY_RESET, F, G, P, C, F , H } },
            { FAILURE, new string[LANGUAGE_COUNT] { FAILURE, "Fracaso", "Fehleverhalten", P, C, F , H } },
            { FAULT, new string[LANGUAGE_COUNT] { FAULT, "Falla", "Fehlerzustand", P, C, "Faute" , H } },
            { FAULTED, new string[LANGUAGE_COUNT] { FAULTED, "Fallado", "Fehlerhaft", P, C, F , H } },
            { FAULT_HISTORY, new string[LANGUAGE_COUNT] { FAULT_HISTORY, "Historia de Fallas", "Fehlerhistorie", P, C, F , H } },
            { FAULT_INJECT, new string[LANGUAGE_COUNT] { FAULT_INJECT, "Inyección de Fallas", "Fehlerinjektion", P, C, F , H } },
            { FAULT_ON_ADDRESS2, new string[LANGUAGE_COUNT] { FAULT_ON_ADDRESS2, "Error en la dirección 2: Error 'No compatible'", "Fehler auf Adresse 2", P, C, F , H } },
            { FAULT_REACTION_ACTIVE, new string[LANGUAGE_COUNT] { FAULT_REACTION_ACTIVE, "Reacción de falla activa", "Fehlerreaktion aktive", P, C, F , H } },
            { FAULT_RESET, new string[LANGUAGE_COUNT] { FAULT_RESET, "Restablecimiento de fallas", "Fehlerquittierung", P, C, F , H } },
            { FAULT_STATUS, new string[LANGUAGE_COUNT] { FAULT_STATUS, "Estado de error", "Fehlerstatus", P, C, F , H } },
            { FILE, new string[LANGUAGE_COUNT] { FILE, "Archivo", "Datei", "Arquivo", C, "Fichier" , H } },
            { FILE_TRAJECTORY, new string[LANGUAGE_COUNT] { FILE_TRAJECTORY, S, "Trajektoriendatei", P, C, F , H } },
            { FILL_AREA, new string[LANGUAGE_COUNT] { FILL_AREA, "Llenar área", "Bereich füllen", "Preencher área", C, "Remplir région" , H } },
            { FILTER_TEXT, new string[LANGUAGE_COUNT] { FILTER_TEXT, S, "Filter text", P, C, F , H } },
            { FINAL_S, new string[LANGUAGE_COUNT] { FINAL_S, S, "Final_S", P, C, F , H } },
            { FIND_FILE, new string[LANGUAGE_COUNT] { FIND_FILE, "Busque Archivo", "Datei suchen", P, C, F , H } },
            { FIND_MAG_ENC_OFFSET, new string[LANGUAGE_COUNT] { FIND_MAG_ENC_OFFSET, "Buscar Desplazamiento del Codificador Magnético", "Magnetischen Encoder Offset detektieren", P, C, F , H } },
            { FIRMWARE_LOADING_TOOL, new string[LANGUAGE_COUNT] { FIRMWARE_LOADING_TOOL, S, G, P, C, F , H } },
            { FIRMWARE_NO_UPDATE, new string[LANGUAGE_COUNT] { FIRMWARE_NO_UPDATE, S, G, P, C, F , H } },
            { FIRMWARE_UPLOAD, new string[LANGUAGE_COUNT] { FIRMWARE_UPLOAD, "Carga de Firmware", "Firmware hochladen", P, C, F , H } },
            { FIRMWARE_UPDATE_SUCCESS, new string[LANGUAGE_COUNT] { FIRMWARE_UPDATE_SUCCESS, S, G, P, C, F , H } },
            { FIRMWARE_VERSION, new string[LANGUAGE_COUNT] { FIRMWARE_VERSION, S, G, P, C, F , H } },
            { FONT, new string[LANGUAGE_COUNT] { FONT, "Fuente", "Schriftart", "Fonte", C, "Police de caractère" , H } },
            { FONT_SIZE, new string[LANGUAGE_COUNT] { FONT_SIZE, "Tamaño de fuente", "Schriftgröße", "Tamanho da fonte", C, "Taille de police" , H } },
            { FORCE_POWER_RESET, new string[LANGUAGE_COUNT] { FORCE_POWER_RESET, "Fuerce un Reinicio de Energía", "Hardwarereset erzwingen", P, C, F , H } },
            { FOREGROUND_COLOR, new string[LANGUAGE_COUNT] { FOREGROUND_COLOR, "Color de Primer Plano", "Vordergrundfarbe", P, C, F , H } },
            { FOUND_DEVICE, new string[LANGUAGE_COUNT]
            {
                FOUND_DEVICE,
                "Encontró {0} dispositivo(s). Último dispositivo {1} en el nodo {2}, Número de Serie {3}",
                "G {0}. {1} {2}, {3}",
                "P {0}. {1} {2}, {3}",
                "C {0}. {1} {2}, {3}",
                "F {0}. {1} {2}, {3}"
            , H } },
            #endregion F

            #region G
            { GATHERED, new string[LANGUAGE_COUNT] { GATHERED, "Recogido {0} de {1}", "{0} G {1}", "{0} P {1}", "{0} C {1}", "{0} F {1}" , H } },
            { GAUGE_PANEL, new string[LANGUAGE_COUNT] { GAUGE_PANEL, "Lista de Indicadores", "Messgeräteliste", "Lista de Indicadores", C, "Liste de Jauge" , H } },
            { GAUGE, new string[LANGUAGE_COUNT] { GAUGE, S, G, P, C, F, H } },
            { GAUGES, new string[LANGUAGE_COUNT] { GAUGES, "Indicadores", "Messgeräte", "Medidores", C, "Jauges", H } },
            { GAUGE_LIST_SETTINGS, new string[LANGUAGE_COUNT] { GAUGE_LIST_SETTINGS, "Configuración de la Lista de Indicadores", "Messwertanzeige Einstellungen", P, C, F , H } },
            { GAUGE_SETT, new string[LANGUAGE_COUNT] { GAUGE_SETT, "Configuraciones de Indicadores", "Messgeräteeinstellungen", "Configuração do Indicador", C, "Paramètres de jauge" , H } },
            { GENERAL, new string[LANGUAGE_COUNT] { GENERAL, "General", "Allgemeines", "Geral", C, "Général", H } },
            { GET_CAPTURE, new string[LANGUAGE_COUNT] { GET_CAPTURE, "Obtener Captura", "Aufzeichnung laden", P, C, F , H } },
            { GLOBAL_USER_LEVEL, new string[LANGUAGE_COUNT] { GLOBAL_USER_LEVEL, "Nivel de Usuario Global", "Gloabel User Level", P, C, F, H } },
            { GPIO_PANEL, new string[LANGUAGE_COUNT] { GPIO_PANEL, S, "GPIO Anzeige", P, C, F , H } },
            { GRAPH_COLORS, new string[LANGUAGE_COUNT] { GRAPH_COLORS, "Colores del Gráfico", "Diagramm Farben", P, C, F , H } },
            { GRAPH_SETTINGS, new string[LANGUAGE_COUNT] { GRAPH_SETTINGS, "Configuración del gráfico", "Grafikeinstellungen", "Configuração do gráfico", C, "Paramètres du graphique" , H } },
            { GRID_COLOR, new string[LANGUAGE_COUNT] { GRID_COLOR, "Color de la cuadrícula", "Gitternetz Farbe", P, C, F , H } },
            { GRIDLINE, new string[LANGUAGE_COUNT] { GRIDLINE, "Línea de Cuadrícula", "Rasterlinie", "Linha de Grade", C, "Ligne de la Grille" , H } },//TODO: check Spanish
            { GRIDLINES, new string[LANGUAGE_COUNT] { GRIDLINES, S, "Rasterlinien", P, C, F , H } },
            { GROUP, new string[LANGUAGE_COUNT] { GROUP, S, G, P, C, F , H } },
            { HALT, new string[LANGUAGE_COUNT] { HALT, "Detener", "Halt", P, C, F , H } },
            { HALT_OPTION, new string[LANGUAGE_COUNT] { HALT_OPTION, "Opción Detención", "Halt Optionen", P, C, F , H } },
            { HELP, new string[LANGUAGE_COUNT] { HELP, "Ayuda", "Hilfe", "Ajuda", C, "Aide" , H } },
            { HIDE_DEVICE_TREE, new string[LANGUAGE_COUNT] { HIDE_DEVICE_TREE, "Ocultar el árbol de dispositivos", "Gerätebaum ausblenden", P, C, F , H } },
            { HIDE_STATUS_BAR, new string[LANGUAGE_COUNT] { HIDE_STATUS_BAR, S, "Statusleiste ausblenden", P, C, F , H } },
            { HIGH, new string[LANGUAGE_COUNT] { HIGH, S, "Hoch", P, C, F , H } },
            { HISTORY, new string[LANGUAGE_COUNT] { HISTORY, "Historia", "Historie", P, C, F , H } },
            { HOMING, new string[LANGUAGE_COUNT] { HOMING, "Guiado", "Homing", P, C, F , H } }, //TODO: no good spanish translation for homing?
            { ICONS_PROVIDED_BY, new string[LANGUAGE_COUNT] { ICONS_PROVIDED_BY, "Iconos proporcionados por https://linkprotect.cudasvc.com/url?a=https%3a%2f%2fIcons8.com&c=E,1,IAm6dpoHRfox_WPU2hpT8i5vyB2jVpEcCWIwnAca4TMe-ad_vLVLOJ2l8KPv12bbsycLnEmKgFLzUXpSiIgVgIWhVBMC2oud4kzDa0gTKBzZFGpLZQ,,&typo=1", "Icons provided by https://linkprotect.cudasvc.com/url?a=https%3a%2f%2fIcons8.com&c=E,1,NIOIh9bog3jnGfspwxkByfI_lpY5WWbGlbg6RV_HXUE_zC6Cbuv1n6jX9hRmyWAuFxTs685XgGKiqDErBYt7HGQYewQ2M8BDFT0_2lrl8FTvkoVqjEL38ORxoA,,&typo=1", P, C, F , H } },
            { IDLE, new string[LANGUAGE_COUNT] { IDLE, "Inactivo", "Leerlauf", P, C, F , H } },
            { IGNORE_CONNECTION_LOSS, new string[LANGUAGE_COUNT] { IGNORE_CONNECTION_LOSS, S, "Verbindungsabbruch Ignorieren", P, C, F , H } },
            { IMMEDIATE, new string[LANGUAGE_COUNT] { IMMEDIATE, "Inmediato", "Sofortig", "Imediato", C, "Immédiat" , H } },
            { INERTIAL_OBSERVER, new string[LANGUAGE_COUNT] { INERTIAL_OBSERVER, S, "Trägheitsbeobachter", P, C, "Immédiat" , H } },
            { INFO, new string[LANGUAGE_COUNT] { INFO, "Info", "Info", "Info", C, "Info" , H } },
            { INFORMATION, new string[LANGUAGE_COUNT] { INFORMATION, "Información", "Informationen", "Informação", C, "Information", H } },
            { INITIALIZATION, new string[LANGUAGE_COUNT] { INITIALIZATION, "Inicialización", "Initialisierunge", P, C, F , H } },
            { INITIAL_S, new string[LANGUAGE_COUNT] { INITIAL_S, S, "Initial", P, C, F , H } },
            { INPUT, new string[LANGUAGE_COUNT] { INPUT, S, "Eingang", P, C, F , H } },
            { INPUTS, new string[LANGUAGE_COUNT] { INPUTS, S, "Eingänge", P, C, F , H } },
            { INPUT_SETTINGS, new string[LANGUAGE_COUNT] { INPUT_SETTINGS, S, "Eingangs Einstellungen", P, C, F , H } },
            { INPUT_SOURCE, new string[LANGUAGE_COUNT] { INPUT_SOURCE, S, "Eingangs Quelle", P, C, F , H } },
            { INPUT_VALUE, new string[LANGUAGE_COUNT] { INPUT_VALUE, S, "Eingangswert", P, C, F , H } },
            { INPUT_1, new string[LANGUAGE_COUNT] { INPUT_1, S, "INPUT1", P, C, F , H } },
            { INPUT_2, new string[LANGUAGE_COUNT] { INPUT_2, S, "INPUT2", P, C, F , H } },
            { INPUT_3, new string[LANGUAGE_COUNT] { INPUT_3, S, "INPUT3", P, C, F , H } },
            { INPUT_4, new string[LANGUAGE_COUNT] { INPUT_4, S, "INPUT4", P, C, F , H } },
            { INPUT5, new string[LANGUAGE_COUNT] { INPUT5, S, "INPUT5", P, C, F , H } },
            #endregion G

            #region I
            { INTERPOLATED_POS, new string[LANGUAGE_COUNT] { INTERPOLATED_POS, "Posición Interpolada", "Interpolated Position", P, C, F , H } },
            { INTERVAL, new string[LANGUAGE_COUNT] { INTERVAL, "Intervalo", "Intervall", "Intervalo", C, "Intervalle" , H } },
            { INVALID, new string[LANGUAGE_COUNT] { INVALID, "Inválido", "Ungültig", P, C, F , H } },
            { IO, new string[LANGUAGE_COUNT] { IO, "E/S", "I/O", "E/S", C, F , H } },
            { IO_LINKER, new string[LANGUAGE_COUNT] { IO_LINKER, "Vinculador de E/S", "I/O Linker", P, C, F , H } },
            { IP_ADD, new string[LANGUAGE_COUNT] { IP_ADD, "Dirección IP", "IP Adresse", "Endereço IP", C, "Adresse IP" , H } },
            { IP_SCAN, new string[LANGUAGE_COUNT] { IP_SCAN, "Escaneo IP", "IP Scan", "Digitalização IP", C, "Balayer IP" , H } },
            { I2T_LOAD_LIMITING_WARN, new string[LANGUAGE_COUNT] { I2T_LOAD_LIMITING_WARN, S, "I²T Warnung", P, C, F , H } },
            #endregion I

            #region J
            { JERK_LEVEL, new string[LANGUAGE_COUNT] { JERK_LEVEL, S, "Ruckwert (mech.) ausgewählt ", P, C, F , H } },
            { JERK_NEG, new string[LANGUAGE_COUNT] { JERK_NEG, S, "Ruck (mech.)", P, C, F , H } },
            { JERK_NEG_UPDATED, new string[LANGUAGE_COUNT] { JERK_NEG_UPDATED, S, "Ruck (-) updated to {0}", P, C, F , H } },
            { JERK_NON_NEG, new string[LANGUAGE_COUNT] { JERK_NON_NEG, S, "Ruck kann nicht negativ sein", P, C, F , H } },
            { JERK_POS, new string[LANGUAGE_COUNT] { JERK_POS, S, "Ruck (+)", P, C, F , H } },
            { JERK_POS_UPDATED, new string[LANGUAGE_COUNT] { JERK_POS_UPDATED, S, "Ruck (+) aktualisiert zu {0}", P, C, F , H } },
            { JOG, new string[LANGUAGE_COUNT] { JOG, S, "JOG", P, C, F , H } },
            { JOG_DEMAND, new string[LANGUAGE_COUNT] { JOG_DEMAND, S, G, P, C, F , H } },
            { JOG_LEFT_BUTTON, new string[LANGUAGE_COUNT] { JOG_LEFT_BUTTON, S, G, P, C, F , H } },
            { JOG_RIGHT_BUTTON, new string[LANGUAGE_COUNT] { JOG_RIGHT_BUTTON, S, G, P, C, F , H } },
            { JOG_TIME, new string[LANGUAGE_COUNT] { JOG_TIME, S, G, P, C, F , H } },
            { JOG_TIME_CHECK, new string[LANGUAGE_COUNT] { JOG_TIME_CHECK, S, G, P, C, F , H } },
            { JSON_FORMAT_ERROR, new string[LANGUAGE_COUNT] { JSON_FORMAT_ERROR, S, G, P, C, F , H } },
            #endregion J

            #region L
            { L, new string[LANGUAGE_COUNT] { L, "I", "L", P, C, F , H } },//TODO: Check if other languages will abbreviate Right/Left with R/L (i.e. the first letter of the word) like English does
            { LANGUAGE, new string[LANGUAGE_COUNT] { LANGUAGE, "Idioma", "Sprache", "Idioma", C, "Langue" , H } },
            { LAST_COMMANDS, new string[LANGUAGE_COUNT] { LAST_COMMANDS, "Últimos Comandos", "Letzte Befehle", P, C, F , H } },
            { LEFT, new string[LANGUAGE_COUNT] { LEFT, "Izquierdo", "Links", P, C, F , H } },
            { LEFT_AXIS, new string[LANGUAGE_COUNT] { LEFT_AXIS, "Eje Izquierdo", "Linke Achse", "Eixo Esquerda", C, "Axe gauche" , H } },
            { LEFT_AXIS_VAR, new string[LANGUAGE_COUNT] { LEFT_AXIS_VAR, "Variable de Eje Izquierdo", "Varaible der Linken Achse", "Variável do Eixo Esquerdo", C, "Variable d'axe gauche" , H } },
            { LEVEL, new string[LANGUAGE_COUNT] { LEVEL, "Nivel", "Level", P, C, F, H } },
            { LINEAR_RAMP, new string[LANGUAGE_COUNT] { LINEAR_RAMP, "Rampa Lineal", "Lineare Rampe", P, C, F , H } },
            { LIVE_POSITION, new string[LANGUAGE_COUNT] { LIVE_POSITION, S, "Live Position", P, C, F , H } },
            { LIVE_UPDATE, new string[LANGUAGE_COUNT] { LIVE_UPDATE, "Actualización en Directo", "Live Update", P, C, F , H } },
            { LIMIT_ACTIVE, new string[LANGUAGE_COUNT] { LIMIT_ACTIVE, S, "Limit aktive", P, C, F , H } },
            { LINK, new string[LANGUAGE_COUNT] { LINK, S, "Link", P, C, F , H } },//TODO: Check the Spanish translation
            { LOAD, new string[LANGUAGE_COUNT] { LOAD, "Cargar", "Laden", "Carregar", C, "Charger" , H } },
            { LOAD_CAPTURE, new string[LANGUAGE_COUNT] { LOAD_CAPTURE, "Cargar Captura", "Laden Erfassung", "Carregar Captura", C, "Charger capture" , H } },
            { LOADING_COMPLETE, new string[LANGUAGE_COUNT] { LOADING_COMPLETE, "Carga Completa", "Vollständig geladen", P, C, F , H } },
            { LOAD_CONFIG, new string[LANGUAGE_COUNT] { LOAD_CONFIG, "Cargar archivo de configuración", "Konfigurationsdatei laden", "Carregar arquivo de configuração", C, "Charger fichier de configuration" , H } },
            { LOAD_CONFIG_INSTR, new string[LANGUAGE_COUNT] { LOAD_CONFIG_INSTR, S, G, P, C, F , H } },
            { LOAD_FIRMWARE_INSTR, new string[LANGUAGE_COUNT] { LOAD_FIRMWARE_INSTR, S, G, P, C, F , H } },
            { LOAD_FROM_FILE, new string[LANGUAGE_COUNT] { LOAD_FROM_FILE, "Cargar del Archivo", "Aus Datei laden", P, C, F , H } },
            { LOADING, new string[LANGUAGE_COUNT] { LOADING, "Cargando", "lade", P, C, F , H } },
            { LOADING_DATA_CAPTURE, new string[LANGUAGE_COUNT] { LOADING_DATA_CAPTURE, "Cargando la ventana de análisis", "Data Capture laden", P, C, F , H } },
            { LOADING_MANUFAC_CONFIG_PARAM, new string[LANGUAGE_COUNT] { LOADING_MANUFAC_CONFIG_PARAM, S, G, P, C, F , H } },
            { LOADING_DATASHEETS, new string[LANGUAGE_COUNT] { LOADING_DATASHEETS, "Cargando Hojas de Datos", "Lade Datenblätter", P, C, F , H } },
            { LOADING_PARAMETER_PANEL, new string[LANGUAGE_COUNT] { LOADING_PARAMETER_PANEL, S, "Parameteranzeige laden", P, C, F , H } },
            { LOADING_STATE, new string[LANGUAGE_COUNT] { LOADING_STATE, "Cargando Estado del Programa", "lade Programmstatus", P, C, F , H } },
            { LOADING_YOUR_DATA, new string[LANGUAGE_COUNT] { LOADING_YOUR_DATA, S, "Daten hochladen", P, C, F , H } },
            { LOAD_LABEL_DATA, new string[LANGUAGE_COUNT] { LOAD_LABEL_DATA, S, G, P, C, F , H } },
            { LOAD_MOTIONS, new string[LANGUAGE_COUNT] { LOAD_MOTIONS, S, "Lastbewegung", P, C, F , H } },
            { LOAD_PARAM_ADDRESS, new string[LANGUAGE_COUNT] { LOAD_PARAM_ADDRESS, "Cargar Datos de Dirección de Parámetro", "Parameter Adressdaten laden", P, C, F , H } },
            { LOAD_PANEL, new string[LANGUAGE_COUNT] { LOAD_PANEL, S, G, P, C, F , H } },
            { LOCKED, new string[LANGUAGE_COUNT] { LOCKED, "Bloqueado", "Gesperrt", "Bloqueado", C, "Verrouillé" , H } },
            { LOCK_LEFT_AXIS, new string[LANGUAGE_COUNT] { LOCK_LEFT_AXIS, "Bloquear Eje Izquierdo", "Linke Achse sperren", P, C, F , H } },
            { LOCK_OPEN, new string[LANGUAGE_COUNT] { LOCK_OPEN, "Bloquear en Estado Abierto", "Sperre geöffnet", P, C, F , H } },
            { LOCK_RIGHT_AXIS, new string[LANGUAGE_COUNT] { LOCK_RIGHT_AXIS, "Bloquear Eje Derecho", "Rechte Achse sperren", P, C, F , H } },
            { LOCK_TIME_AXIS, new string[LANGUAGE_COUNT] { LOCK_TIME_AXIS, "Bloquear Eje de Tiempo", "Zeitachse sperren", P, C, F , H } },
            { LOG_DISPLAY, new string[LANGUAGE_COUNT] { LOG_DISPLAY, "Registro", "Protokollierung", "Registro", C, "Journal" , H } },
            { LOG_FILE_NAME, new string[LANGUAGE_COUNT] { LOG_FILE_NAME, "Nombre del Archivo de Registro", "Name der Protokolldatei", "Nome do ficheiro de Registro", C, "Nom du fichier journal" , H } },
            { LOG_FILE_PATH, new string[LANGUAGE_COUNT] { LOG_FILE_PATH, "Ruta del Archivo de Registro", "Protokolldateipfad", "Caminho do ficheiro de Registro", C, "Chemin du fichier journal" , H } },
            { LOG_FILE_TYPE, new string[LANGUAGE_COUNT] { LOG_FILE_TYPE, "Tipo del Archivo de Registro", "Protokolldateityp", "Tipo de ficheiro de Registro", C, "Type de fichier journal" , H } },
            { LOG_MESSAGE, new string[LANGUAGE_COUNT] { LOG_MESSAGE, "Mensaje de Registro", "Protokollnachricht", "Mensagem de Registro", C, "Message du journal" , H } },
            { LOGS, new string[LANGUAGE_COUNT] { LOGS, "Registros", "Protokolle", "Registros", C, "Journaux", H } },
            { LOG_SETTINGS, new string[LANGUAGE_COUNT] { LOG_SETTINGS, "Configuración de Registro", "Protokolleinstellungen", "Configuração do registro", C, "Paramètres du journal" , H } },
            { LOG_SIZE, new string[LANGUAGE_COUNT] { LOG_SIZE, "Tamaño de Registros (MB)", "Größe der Protokolle (MB)", "Tamanho dos Registros (MB)", C, "Taille de journal (MB)" , H } },
            { LOGGING, new string[LANGUAGE_COUNT] { LOGGING, "Registrando", "Protokollierung", "Registrando", C, "Enregistrement" , H } },
            { LOW, new string[LANGUAGE_COUNT] { LOW, S, "Niedrig", P, C, F , H } },
            #endregion L

            #region M
            { MAGNETIC_ALIGNMENT_OFFSET, new string[LANGUAGE_COUNT] { MAGNETIC_ALIGNMENT_OFFSET, S, "Offset der magnetischen Ausrichtung", P, C, F , H } },
            { MAGNITUDE, new string[LANGUAGE_COUNT] { MAGNITUDE, "Magnitud", "Größe", "Magnitude", C, "Magnitude" , H } },
            { MAINTAIN, new string[LANGUAGE_COUNT] { MAINTAIN, "Mantener", "Pflegen", "Manter", C, " Maintenir" , H } },
            { MAJOR, new string[LANGUAGE_COUNT] { MAJOR, "Principal", "Haupt", P, C, F , H } },//Note: Spanish translation taken from whole phras "major axis"
            { MANUFACTURER_LABEL, new string[LANGUAGE_COUNT] { MANUFACTURER_LABEL, "Etiqueta de la Fabricación", "Herstellerlabel", P, C, F , H } },
            { MANUFACTURER_LABEL_DATA, new string[LANGUAGE_COUNT] { MANUFACTURER_LABEL_DATA, S, G, P, C, F , H } },
            { MANUFACTURER_LABEL_PARAMS, new string[LANGUAGE_COUNT] { MANUFACTURER_LABEL_PARAMS, S, G, P, C, F , H } },
            { MANUFACTURER, new string[LANGUAGE_COUNT] { MANUFACTURER, "Fabricante", "Hersteller", P, C, F , H } },
            { MANUFACTURER_PARAM_SAVED, new string[LANGUAGE_COUNT] { MANUFACTURER_PARAM_SAVED, S, G, P, C, F , H } },
            { MANUFACTURER_SPECIFIC, new string[LANGUAGE_COUNT] { MANUFACTURER_SPECIFIC, "Específico del Fabricante", "Herstellerspezifisch", P, C, F , H } },
            { MAPPING_I0, new string[LANGUAGE_COUNT] { MAPPING_I0, S, "IO Mapping", P, C, F , H } },
            { MARQEE_SCROLL, new string[LANGUAGE_COUNT] { MARQEE_SCROLL, "Desplazamiento de Marquesina", "Scrollende Buchstaben", P, C, F , H } },
            { MATCHCASE, new string[LANGUAGE_COUNT] { MATCHCASE, "Coincidir mayúsculas y minúsculas", "Groß- und Kleinschreibung abgleichen", "Corresponder maiúsculas e minúsculas", C, "Match en majuscules et minuscules" , H } },
            { MAXIMUM, new string[LANGUAGE_COUNT] { MAXIMUM, "Máxima", "Maximale", P, C, F , H } },
            { MAXIMUM_ACCELERATION, new string[LANGUAGE_COUNT] { MAXIMUM_ACCELERATION, "Aceleración Máxima", "Maximale Beschleunigung", P, C, F , H } },
            { MAX_CURRENT_INFO, new string[LANGUAGE_COUNT] { MAX_CURRENT_INFO, S, G, P, C, F , H } },
            { MAX_CURRENT, new string[LANGUAGE_COUNT] { MAX_CURRENT, "Corriente Máxima", "Maximaler Strom", P, C, F , H } },
            { MAX_DECEL, new string[LANGUAGE_COUNT] { MAX_DECEL, "Desaceleración máxima", "Maximale Verzögerung", P, C, F , H } },
            { MAX_MOTOR_SPEED, new string[LANGUAGE_COUNT] { MAX_MOTOR_SPEED, "Velocidad Máxima del Motor", "Maximale Motordrehzahl", P, C, F , H } },
            { MAX_PROFILE_VELOCITY, new string[LANGUAGE_COUNT] { MAX_PROFILE_VELOCITY, "Velocidad Máxima del Perfil", "Maximale Profilegeschwindigkeit", P, C, F , H } },
            { MAX_TORQUE, new string[LANGUAGE_COUNT] { MAX_TORQUE, "Par máximo", "Maximales Drehmoment", P, C, F , H } },
            { MAX_TORQUE_INFO, new string[LANGUAGE_COUNT] { MAX_TORQUE_INFO, S, G, P, C, F , H } },
            { MAX_VELOCITY, new string[LANGUAGE_COUNT] { MAX_VELOCITY, "Velocidad Máxima", "Maximale Geschwindigkeit", P, C, F , H } },
            { MENU, new string[LANGUAGE_COUNT] { MENU, S, G, P, C, F , H } },
            { MESSAGES, new string[LANGUAGE_COUNT] { MESSAGES, S, "Nachricht", P, C, F , H } },
            { MINIMUM, new string[LANGUAGE_COUNT] { MINIMUM, "Nivel Mínimo de Registro", "Mindestprotokollierungsstufe", "Nível mínimo de Registro", C, "Niveau de journal minimum" , H } },
            { MINIMUM_LOG_LEVEL, new string[LANGUAGE_COUNT] { MINIMUM_LOG_LEVEL, "Mínimo", G, "Mínimo", C, "Minimum" , H } },
            { MINOR, new string[LANGUAGE_COUNT] { MINOR, "Menor", "gering", P, C, F , H } },
            { MIN_VELOCITY, new string[LANGUAGE_COUNT] { MIN_VELOCITY, "Velocidad mínima", "Minimale Geschwindigkeit", P, C, F , H } },
            { MODIFY_AXIS_PARAMS, new string[LANGUAGE_COUNT] { MODIFY_AXIS_PARAMS, "Modificar Parámetros de Eje", "Achsenparameter bearbeiten", P, C, F , H } },
            { MODE, new string[LANGUAGE_COUNT] { MODE, "Modo", "Zustand", "Modo", C, "Mode" , H } },
            { MODE_OF_OPERATION, new string[LANGUAGE_COUNT] { MODE_OF_OPERATION, "Modo de Operación", "Betriebsmodi", P, C, F , H } },
            { MODIFIED, new string[LANGUAGE_COUNT] { MODIFIED, S, "Geändert", P, C, F , H } },
            { MORE_AREA, new string[LANGUAGE_COUNT] { MORE_AREA, "Más área", "Mehr Fläche", "Mais área", C, "Plus de région" , H } },
            { MOTION, new string[LANGUAGE_COUNT] { MOTION, "Movimiento", "Bewegung", "Movimento", C, "Mouvement" , H } },
            { MOTIONS, new string[LANGUAGE_COUNT] { MOTIONS, S, "Bewegungen", P, C, F , H } },
            { MOTION_CONTROL, new string[LANGUAGE_COUNT] { MOTION_CONTROL, "Control de movimiento", "Bewegungskontrolle", "Controlo do movimento", C, "Contrôle de mouvement" , H } },
            { MOTION_CONTROLLER, new string[LANGUAGE_COUNT] { MOTION_CONTROLLER, "Control de movimiento", "Bewegungskontrolle", "Controlo do movimento", C, "Contrôle de mouvement" , H } },
            { MOTION_CONTROLLER_WELCOME, new string[LANGUAGE_COUNT]
            {
                "Thank you for using the DS404 Automated Toolset of Allied Motion (DATAM)!\nTo get started, select a device operating mode from the menu above.",
                "¡Gracias por utilizar el conjunto de herramientas automatizadas DS404 de Allied Motion (DATAM)!\nPara empezar, seleccione un modo de funcionamiento del dispositivo en el menú de arriba.",
                "Danke das Sie DS404 Automated Toolset of Allied Motion (DATAM) nutzen!\nZum Starten Bitte einen Kontrollmodus aus dem Hauptmenü auswählen.",
                P,
                C,
                F
            , H } },
            { MOTION_CREATION, new string[LANGUAGE_COUNT] { MOTION_CREATION, S, "MOTION_CREATION", P, C, F , H } },
            { MOTION_PLOT, new string[LANGUAGE_COUNT] { MOTION_PLOT, S, "MOTION_PLOT", P, C, F , H } },
            { MOTION_PROFILE_TYPE, new string[LANGUAGE_COUNT] { MOTION_PROFILE_TYPE, "Tipo de Perfil de Movimiento", "Bewegungsprofil", P, C, F , H } },
            { MOTION_SCHEDULE, new string[LANGUAGE_COUNT] { MOTION_SCHEDULE, "Horario de movimiento", "Bewegungsplan", "Horário de movimento", C, "Programme des mouvements" , H } },
            { MOTION_TYPE, new string[LANGUAGE_COUNT] { MOTION_TYPE, "Tipo del movimiento", "Bewegungsart", "Tipo de movimento", C, "Type de mouvement" , H } },
            { MOTOR, new string[LANGUAGE_COUNT] { MOTOR, "Motor", "Motor", "Motor", C, "Moteur" , H } },
            { MOTOR_CONTROLLER, new string[LANGUAGE_COUNT] { MOTOR_CONTROLLER, S, "Motor Controller", P, C, F , H } },
            { MOTOR_IN_MOTION, new string[LANGUAGE_COUNT] { MOTOR_IN_MOTION, S, "Motor in Bewegung", P, C, F , H } },
            { MOTOR_RATED_CURRENT, new string[LANGUAGE_COUNT] { MOTOR_RATED_CURRENT, "Corriente Nominal del Motor", "Nennstrom", P, C, F , H } },
            { MOTOR_RATED_CURRENT_INFO, new string[LANGUAGE_COUNT] { MOTOR_RATED_CURRENT_INFO, S, G, P, C, F , H } },
            { MOTOR_RATED_TORQUE, new string[LANGUAGE_COUNT] { MOTOR_RATED_TORQUE, "Par Nominal del Motor", "Nenndrehmoment", P, C, F , H } },
            { MOTOR_RATED_TORQUE_INFO, new string[LANGUAGE_COUNT] { MOTOR_RATED_TORQUE_INFO, S, G, P, C, F , H } },
            { MOTOR_TEMP_NEAR_LIMIT, new string[LANGUAGE_COUNT] { MOTOR_TEMP_NEAR_LIMIT, "Temp del Motor Cerca del Límite", "Motortemperatur nahe dem Limit", P, C, F , H } },
            { MOVE, new string[LANGUAGE_COUNT] { MOVE, "Mover", "Bewegung", "Mover", C, "Déplacer" , H } },
            { MOVE_AXES, new string[LANGUAGE_COUNT] { MOVE_AXES, "Mover Ejes", "Achsen Bewegen", "Mover Eixos", C, "Déplacer axes" , H } },
            { MOVE_IMMEDIATE, new string[LANGUAGE_COUNT] { MOVE_IMMEDIATE, "Movimiento Inmediato", "Sofort Verschieben", "Movimento Imediato", C, "Déplacer Immédiatement" , H } },
            { MOVE_SCHEDULED, new string[LANGUAGE_COUNT] { MOVE_SCHEDULED, "Movimiento Programado", "Geplant Verschieben", "Movimento Agendao", C, "Déplacement Prévu" , H } },
            { MOVE_TO_LEFT, new string[LANGUAGE_COUNT] { MOVE_TO_LEFT, S, "Zur linken Achse bewegen", P, C, F , H } },
            { MOVE_TO_RIGHT, new string[LANGUAGE_COUNT] { MOVE_TO_RIGHT, S, "Zur rechten Achse bewegen", P, C, F , H } },
            { MULTIPLIER, new string[LANGUAGE_COUNT] { MULTIPLIER, S, "MULTIPLIER", P, C, F , H } },
            #endregion M

            #region N
            { NAME, new string[LANGUAGE_COUNT] { NAME, S, G, P, C, F , H } },
            { NAMED_STIM_PANEL, new string[LANGUAGE_COUNT] { NAMED_STIM_PANEL, S, "NAMED_STIM_PANEL", P, C, F , H } },
            { NEG, new string[LANGUAGE_COUNT] { NEG, "Negativo", "negativ", P, C, F , H } },
            { NEG_TORQUE_INFO, new string[LANGUAGE_COUNT] { NEG_TORQUE_INFO, S, G, P, C, F , H } },
            { NEG_TORQUE_LIMIT, new string[LANGUAGE_COUNT] { NEG_TORQUE_LIMIT, "Límite de Par Negativo", "Negatives Drehmomentlimit", P, C, F , H } },
            { NET_MANAGEMENT_STATE, new string[LANGUAGE_COUNT] { NET_MANAGEMENT_STATE, "Estado de Administración de Red", "Netzwerkverwaltungsstatus", P, C, F , H } },
            { NETWORK, new string[LANGUAGE_COUNT] { NETWORK, "Red", "Netzwerk", "Rede", C, "Réseau" , H } },
            { NETWORK_ADAPT, new string[LANGUAGE_COUNT] { NETWORK_ADAPT, "Adaptadores de red", "Netzwerkadapter", "Adaptadores de rede", C, "Adaptateurs réseau" , H } },
            { NEW, new string[LANGUAGE_COUNT] { NEW, S, G, P, C, F , H } },
            { NEXT_FILE, new string[LANGUAGE_COUNT] { NEXT_FILE, S, "Nächste Datei", P, C, F , H } },
            { NO, new string[LANGUAGE_COUNT] { NO, "No", "Nein", P, C, F , H } },
            { NO_CAPTURE, new string[LANGUAGE_COUNT] { NO_CAPTURE, "No hay captura que mostrar", "Keine Aufzeichnung auf dem Bildschirm", P, C, F , H } },
            { NODE_ID, new string[LANGUAGE_COUNT] { NODE_ID, S, G, P, C, F , H } },
            { NO_DEVICES, new string[LANGUAGE_COUNT] { NO_DEVICES, S, "Kein Gerät verbunden", P, C, F , H } },
            { NO_MODE_SELECTED, new string[LANGUAGE_COUNT] { NO_MODE_SELECTED, "No Hay Modo Seleccionado", "Kein Modus ausgewählt", P, C, F , H } },
            { NONCAPTURABLE_PARAMS, new string[LANGUAGE_COUNT] { NONCAPTURABLE_PARAMS, "Parámetros no capturables", "Keine aufzeichenbaren Parameter", P, C, F , H } },
            { NONE, new string[LANGUAGE_COUNT] { NONE, "Ninguno", "Keiner", "Nenhum", C, "Aucun", H } },
            { NOT_CONNECTED, new string[LANGUAGE_COUNT] { NOT_CONNECTED, "¡No conectado!", "Nicht verbunden!", "Não conectado!", C, "Pas connecté!" , H } },
            { NOTES, new string[LANGUAGE_COUNT] { NOTES, "Notas", "Notizen", P, C, F , H } },
            { NOT_FAULTED, new string[LANGUAGE_COUNT] { NOT_FAULTED, "Sin Fallas", "Nicht fehlerhaft", P, C, F , H } },
            { NO_DEVICE_FOUND, new string[LANGUAGE_COUNT] { NO_DEVICE_FOUND, S, G, P, C, F , H } },
            { NOT_READY_SWITCH_ON, new string[LANGUAGE_COUNT] { NOT_READY_SWITCH_ON, "No está listo para encenderse", "Gerät noch nicht bereit zum Einschalten", P, C, F , H } },
            { NO_VARIABLES, new string[LANGUAGE_COUNT] { NO_VARIABLES, "No hay variables seleccionadas", "Keine Variablen ausgewählt", P, C, F , H } },
            { NUMBER_GAUGES, new string[LANGUAGE_COUNT] { NUMBER_GAUGES, "Número de Indicadores", "Anzahl der Messgeräte", "Número de Indicadores", C, "Nombre de jauges" , H } },
            { NUMBER_LOGS, new string[LANGUAGE_COUNT] { NUMBER_LOGS, "Número de Registros", "Anzahl der Protokolle", "Número de Registros", C, "Nombre de journal" , H } },
            #endregion N

            #region O
            { OFF, new string[LANGUAGE_COUNT] { OFF, "Apagado", "Aus", "Desligado", C, "Éteint" , H } },
            { OFFSET, new string[LANGUAGE_COUNT] { OFFSET, S, "OFFSET", P, C, F , H } },
            { OK, new string[LANGUAGE_COUNT] { OK, "Aceptar", "Okay", "OK", C, "Ok" , H } },
            { ON, new string[LANGUAGE_COUNT] { ON, "Encendido", "An", "Ligado", C, "Allumé" , H } },
            { OPEN_CLOSED_LOOP, new string[LANGUAGE_COUNT] { OPEN_CLOSED_LOOP, "Abierto y Bucle Cerrado", "Offener & geschlossener Regelkreis", P, C, F , H } },
            { OPEN_ALLIED_DEVICES, new string[LANGUAGE_COUNT] { OPEN_ALLIED_DEVICES, S, "Allied-Geräte öffnen", P, C, F , H } },
            { OPEN_FOUND_DEVICES, new string[LANGUAGE_COUNT] { OPEN_FOUND_DEVICES, "Abra los dispositivos encontrados", "Offene angeschlossene Geräte gefunden", P, C, F , H } },
            { OPEN_LOOP_GAIN, new string[LANGUAGE_COUNT] { OPEN_LOOP_GAIN, S, "Verstärkung des offenen Regelkreises", P, C, F , H } },
            { OPEN_LOOP_PHASE, new string[LANGUAGE_COUNT] { OPEN_LOOP_PHASE, S, "Phasenverschiebung des offenen Regelkreises", P, C, F , H } },
            { OPEN_LOOP_VOLTAGE, new string[LANGUAGE_COUNT] { OPEN_LOOP_VOLTAGE, "Voltaje de Bucle Abierto", "Spannung des offenen Regelkreises", P, C, F , H } },
            { OPENING_LOG, new string[LANGUAGE_COUNT] { OPENING_LOG, "Abriendo el Registro", "Start Protokoll", P, C, F , H } },
            { OPERATIONAL, new string[LANGUAGE_COUNT] { OPERATIONAL, "Operativo", "Betriebsbereit", P, C, F , H } },
            { OPERATION_MODE_SPECIFIC, new string[LANGUAGE_COUNT] { OPERATION_MODE_SPECIFIC, S, "Betriebsmodi spezifisch", P, C, F , H } },
            { OPERATION_DISABLED, new string[LANGUAGE_COUNT] { OPERATION_DISABLED, S, G, P, C, F , H } },
            { OPERATION_ENABLED, new string[LANGUAGE_COUNT] { OPERATION_ENABLED, "Operación activada", "Betriebsfreigabe", P, C, F , H } },
            { OPTIONS, new string[LANGUAGE_COUNT] { OPTIONS, "Opciones", "Optionen", "Opções", C, "Options" , H } },
            { OUTPUT, new string[LANGUAGE_COUNT] { OUTPUT, S, "OUTPUT", P, C, F , H } },
            { OUTPUT_BIT_LOGIC_HIGH, new string[LANGUAGE_COUNT] { OUTPUT_BIT_LOGIC_HIGH, S, "OUTPUT_BIT_LOGIC_HIGH", P, C, F , H } },
            { OUTPUT_BIT_LOGIC_LOW, new string[LANGUAGE_COUNT] { OUTPUT_BIT_LOGIC_LOW, S, "OUTPUT_BIT_LOGIC_LOW", P, C, F , H } },
            { OUTPUT_SETTINGS, new string[LANGUAGE_COUNT] { OUTPUT_SETTINGS, S, "Output Einstellungen", P, C, F , H } },
            { OUTPUT_SOURCE, new string[LANGUAGE_COUNT] { OUTPUT_SOURCE, S, "Output Quelle", P, C, F , H } },
            { OUTPUT_VALUE, new string[LANGUAGE_COUNT] { OUTPUT_VALUE, S, "Output Wert", P, C, F , H } },
            { OUTSIDE_SPS, new string[LANGUAGE_COUNT] { OUTSIDE_SPS, "Fuera de SP1 y SP2", "Externe SP1 und SP2", P, C, F , H } },
            { OVERRIDE, new string[LANGUAGE_COUNT] { OVERRIDE, "Anular", "Überschreiben", P, C, F , H } },
            { OVER_VOLTAGE, new string[LANGUAGE_COUNT] { OVER_VOLTAGE, "Error de sobretensión", "Überspannungsfehler", P, C, F , H } },
            #endregion O

            #region P
            { PARAMETER, new string[LANGUAGE_COUNT] { PARAMETER, "Parámetro", "Parameter", P, C, F , H } },
            { PARAM1, new string[LANGUAGE_COUNT] { PARAM1, "Parámetro 1", "Parameter 1", P, C, F , H } },
            { PARAM2, new string[LANGUAGE_COUNT] { PARAM2, "Parámetro 2", "Parameter 2", P, C, F , H } },
            { PARAM3, new string[LANGUAGE_COUNT] { PARAM3, "Parámetro 3", "Parameter 3", P, C, F , H } },
            { PARAM4, new string[LANGUAGE_COUNT] { PARAM4, "Parámetro 4", "Parameter 4", P, C, F , H } },
            { PARAMETER_PANEL, new string[LANGUAGE_COUNT] { PARAMETER_PANEL, "Parámetros", "Parameters", "Parâmetros", C, "Paramètres" , H } },
            { PARAM_DIRECTOR, new string[LANGUAGE_COUNT] { PARAM_DIRECTOR, "Director de Parámetros", "Parameterverwaltung", P, C, F , H } },
            { PASSWORD, new string[LANGUAGE_COUNT] { PASSWORD, "Contraseña", "Passwort", P, C, F , H } },
            { PDOL1, new string[LANGUAGE_COUNT] { PDOL1, S, "Parameter zu Digitalem Output Link 1", P, C, F , H } },
            { PDOL2, new string[LANGUAGE_COUNT] { PDOL2, S, "Parameter zu Digitalem Output Link 1", P, C, F , H } },
            { PENDING, new string[LANGUAGE_COUNT] { PENDING, "Pendiente", "ausstehend", P, C, F , H } },
            { PHASING, new string[LANGUAGE_COUNT] { PHASING, "Etapa", "Phasenverschiebung", P, C, F , H } },//Check etapa
            { PHASING_MODE_INSTR, new string[LANGUAGE_COUNT] { PHASING_MODE_INSTR, S, "PHASING_MODE_INSTR", P, C, F , H } },
            { PID_NOT_VALID, new string[LANGUAGE_COUNT] { PID_NOT_VALID, S, G, P, C, F , H } },
            { PLEASE_REBOOT_DEVICE, new string[LANGUAGE_COUNT] { PLEASE_REBOOT_DEVICE, S, G, P, C, F , H } },
            { PLOT, new string[LANGUAGE_COUNT] { PLOT, "Gráfico", "Graph", "Gráfico", C, "Graphique" , H } },
            { POB_SENDER, new string[LANGUAGE_COUNT] { POB_SENDER, "Remitente de POB", "POB-Absender", "Remetente de POB", C, "Expéditeur POB" , H } },
            { POINT, new string[LANGUAGE_COUNT] { POINT, "Punto", "Punkte", "Ponto", C, "Point" , H } },
            { POINTS, new string[LANGUAGE_COUNT] { POINTS, "Puntos para capturar", "Punkte", "Pontos", C, "Points" , H } },
            { POINTS_TO_CAPTURE, new string[LANGUAGE_COUNT] { POINTS_TO_CAPTURE, "Puntos para capturar", "Zu erfassende Punkte", "Pontos a Capturar", "Ch", "Points à capturer" , H } },
            { POLARITY, new string[LANGUAGE_COUNT] { POLARITY, "Polaridad", "Polarität", "Polaridade", C, "Polarité" , H } },
            { PORT, new string[LANGUAGE_COUNT] { PORT, "Puerto", "Anschluss", "Porta", C, "Port" , H } },
            { POS, new string[LANGUAGE_COUNT] { POS, "Positivo", "Positiv", P, C, F , H } },
            { POSITION_DEMAND, new string[LANGUAGE_COUNT] { POSITION_DEMAND, S, "Zielposition", P, C, F , H } },
            { POS_SPIN_ORIENTATION, new string[LANGUAGE_COUNT] { POS_SPIN_ORIENTATION, "Orientación de Espín Positivo", "Positive Drehrichtung", P, C, F , H } },
            { POST, new string[LANGUAGE_COUNT] { POST, "Post", "Post", "Pós", C, "Post" , H } },
            { POS_TORQUE_INFO, new string[LANGUAGE_COUNT] { POS_TORQUE_INFO, S, G, P, C, F , H } },
            { POS_TORQUE_LIMIT, new string[LANGUAGE_COUNT] { POS_TORQUE_LIMIT, "Límite de Par Positivo", "Positives Drehzahllimit", P, C, F , H } },
            { POST_TRIGGER, new string[LANGUAGE_COUNT] { POST_TRIGGER, "Post-disparador", "Post-Trigger", "Pós-desencadeamento", C, "Post-déclenchement" , H } },
            { POST_TRIGGER_75, new string[LANGUAGE_COUNT] { POST_TRIGGER_75, "75% Post-disparador", "75% Post-Trigger", "75% Pós-desencadeamento", C, "75% Post-déclenchement" , H } },
            { POST_TRIGGER_50, new string[LANGUAGE_COUNT] { POST_TRIGGER_50, "50% Post-disparador", "50% Post-Trigger", "50% Pós-desencadeamento", C, "50% Post-déclenchement" , H } },
            { POST_TRIGGER_25, new string[LANGUAGE_COUNT] { POST_TRIGGER_25, "25% Post-disparador", "25% Post-Trigger", "25% Pós-desencadeamento", C, "25% Post-déclenchement" , H } },
            { PRE, new string[LANGUAGE_COUNT] { PRE, "Pre", "Pre", "Pré", C, "Pré" , H } },
            { PRE_OPERATIONAL, new string[LANGUAGE_COUNT] { PRE_OPERATIONAL, "Pre-operacional", "Vor Betriebsfreigabe", P, C, F , H } },
            { PREPARING_DATA, new string[LANGUAGE_COUNT] { PREPARING_DATA, "Preparando Datos", "Bereite Daten vor", P, C, F , H } },
            { PRE_TRIGGER, new string[LANGUAGE_COUNT] { PRE_TRIGGER, "Pre-disparador", "Pre-Trigger", "Pré-desencadeamento", C, "Pré-déclenchement" , H } },
            { PREVIEW, new string[LANGUAGE_COUNT] { PREVIEW, S, "Vorschau", P, C, F , H } },
            { PREVIOUS, new string[LANGUAGE_COUNT] { PREVIOUS, S, G, P, C, F , H } },
            { PREVIOUS_FILE, new string[LANGUAGE_COUNT] { PREVIOUS_FILE, S, "Vorgängerdatei", P, C, F , H } },
            { PROCEED, new string[LANGUAGE_COUNT] { PROCEED, S, G, P, C, F , H } },
            { PRODUCT_NAME, new string[LANGUAGE_COUNT] { PRODUCT_NAME, S, G, P, C, F , H } },
            { PROFILE_ACCEL, new string[LANGUAGE_COUNT] { PROFILE_ACCEL, "Aceleración de Perfil", "Beschleunigungsprofil", P, C, F , H } },
            { PROFILE_DATA_FILE, new string[LANGUAGE_COUNT] { PROFILE_DATA_FILE, S, "Profildaten", P, C, F , H } },
            { PROFILE_DECEL, new string[LANGUAGE_COUNT] { PROFILE_DECEL, "Desaceleración del Perfil", "Verzögerungsprofil", P, C, F , H } },
            { PROFILE_POSITION, new string[LANGUAGE_COUNT] { PROFILE_POSITION, "Posición del Perfil", "Positionsprofil", P, C, F , H } },
            { PROFILE_TORQUE, new string[LANGUAGE_COUNT] { PROFILE_TORQUE, "Par de Perfil", "Drehmomentprofil", P, C, F , H } },
            { PROFILE_VELOCITY, new string[LANGUAGE_COUNT] { PROFILE_VELOCITY, "Velocidad del Perfil", "Geschwindigkeitsprofil", P, C, F , H } },
            #endregion P

            #region Q
            { QUEUE_CLEAR, new string[LANGUAGE_COUNT] { QUEUE_CLEAR, S, "Warteliste leer", P, C, F , H } },
            { QUICK_STOP, new string[LANGUAGE_COUNT] { QUICK_STOP, "Parada Rápida", "Schnell Halt", "Parada Rápida", C, "Arrêt Rapide" , H } },
            { QUICK_STOP_ACTIVE, new string[LANGUAGE_COUNT] { QUICK_STOP_ACTIVE, "Parada rápida activa", "Schnell halt aktiv", P, C, F , H } },
            { QUICK_STOP_DECEL, new string[LANGUAGE_COUNT] { QUICK_STOP_DECEL, "Desaceleración de Parada Rápida", "Schnell Halt Verzögerung", P, C, F , H } },
            { QUICK_STOP_RAMP, new string[LANGUAGE_COUNT] { QUICK_STOP_RAMP, "Rampa de parada rápida", "Schnellhalt Rampe", P, C, F , H } },
            #endregion Q

            #region R
            { R, new string[LANGUAGE_COUNT] { R, "D", "R", P, C, F , H } },
            { REACTIONS, new string[LANGUAGE_COUNT] { REACTIONS, S, "Reaktion", P, C, F , H } },
            { READING_BIN_FILE, new string[LANGUAGE_COUNT] { READING_BIN_FILE, S, G, P, C, F , H } },
            { READING_FLASH_INFO, new string[LANGUAGE_COUNT] { READING_FLASH_INFO, S, G, P, C, F , H } },
            { READY_SWITCH_ON, new string[LANGUAGE_COUNT] { READY_SWITCH_ON, "Listo para encender", "Bereit zum Einschalten", P, C, F , H } },
            { REBOOTING_DEVICE, new string[LANGUAGE_COUNT] { REBOOTING_DEVICE, S, G, P, C, F , H } },
            { RECEIVED, new string[LANGUAGE_COUNT] { RECEIVED, "Aceptado", "Erhalten", "Recebido", C, "Reçu" , H } },
            { RECEIVED_MESSAGES, new string[LANGUAGE_COUNT] { RECEIVED_MESSAGES, S, "Empfangene Nachrichten", P, C, F , H } },
            { REDO, new string[LANGUAGE_COUNT] { REDO, "Rehacer", "Wiederholen", "Refazer", C, "Refaire" , H } },
            { REFERENCE_RAMP, new string[LANGUAGE_COUNT] { REFERENCE_RAMP, "Rampa de Referencia", "Sollwert Rampe", P, C, F , H } },
            { REJECT, new string[LANGUAGE_COUNT] { REJECT, "Rechazar", "Verwerfen", P, C, F , H } },
            { RELATIVE, new string[LANGUAGE_COUNT] { RELATIVE, "Relativo", "Relativ", "Relativo", C, "Relatif" , H } },
            { REMOTE, new string[LANGUAGE_COUNT] { REMOTE, "Remoto", "Remote", P, C, F , H } },
            { REMOVE, new string[LANGUAGE_COUNT] { REMOVE, "Eliminar", "Löschen", P, C, F , H } },
            { REMOVE_DATASHEET, new string[LANGUAGE_COUNT] { REMOVE_DATASHEET, "Eliminar hoja de datos", "Datenblatt löschen", P, C, F , H } },
            { REMOVE_DATASHEET_INSTR, new string[LANGUAGE_COUNT] { REMOVE_DATASHEET_INSTR, S, G, P, C, F , H } },
            { REPLACE_CONFIG, new string[LANGUAGE_COUNT] { REPLACE_CONFIG, "Reemplazar configuración", "Konfiguration ersetzten", P, C, F , H } },
            { RESERVED, new string[LANGUAGE_COUNT] { RESERVED, "Reservado", "Reserviert", P, C, F , H } },
            { RESET, new string[LANGUAGE_COUNT] { RESET, "Restablecer", "Zurücksetzen", "Redefinir", C, "Réinitialiser" , H } },
            { RESET_DATAM, new string[LANGUAGE_COUNT] { RESET_DATAM, "Restablecer Datam", "Reset Datum", P, C, F , H } },
            { RESET_FAULTS, new string[LANGUAGE_COUNT] { RESET_FAULTS, "Restablecer Fallas", "Fehler zurücksetzen", P, C, F , H } },
            { RESET_NEEDED, new string[LANGUAGE_COUNT] { RESET_NEEDED, "Datam tiene que reiniciar para completar la operación", "Reset notwendig", P, C, F , H } },
            { RESET_OPTIONS, new string[LANGUAGE_COUNT] { RESET_OPTIONS, F, G, P, C, F , H } },
            { RESET_UNDERWAY, new string[LANGUAGE_COUNT] { RESET_UNDERWAY, "Restablecer en Curso", "Zurücksetzen Läuft", "Redefinir em Andamento", C, "Réinitialiser en Cours" , H } },
            { RESET_WARNINGS, new string[LANGUAGE_COUNT] { RESET_WARNINGS, "Restablecer las Advertencias", "Reset Warnung", P, C, F , H } },
            { RESPONSE_NOT_VALID, new string[LANGUAGE_COUNT] { RESPONSE_NOT_VALID, F, G, P, C, F , H } },
            { RESULT, new string[LANGUAGE_COUNT] { RESULT, S, "Ergebnis", P, C, F , H } },
            { RETICULATING_SPLINES, new string[LANGUAGE_COUNT] { RETICULATING_SPLINES, "Reticulating Splines", "Reticulating Splines", "Reticulating Splines", "Reticulating Splines", "Reticulating Splines" , H } },
            { RETRIEVING_DATA, new string[LANGUAGE_COUNT] { RETRIEVING_DATA, "Recuperando datos", "Daten abrufen", P, C, F , H } },
            { REVISION_NUMBER, new string[LANGUAGE_COUNT] { REVISION_NUMBER, S, G, P, C, F , H } },
            { RIGHT, new string[LANGUAGE_COUNT] { RIGHT, "Derecho", "Rechts", P, C, F , H } },
            { RIGHT_AXIS, new string[LANGUAGE_COUNT] { RIGHT_AXIS, "Eje Derecho", "Rechte Achse", "Eixo Direito", C, "Axe droit" , H } },
            { RIGHT_AXIS_VAR, new string[LANGUAGE_COUNT] { RIGHT_AXIS_VAR, "Variable de Eje Derecho", "Variable der Rechten Achse", "Variável do Eixo Direito", C, "Variable d'axe droit" , H } },
            { RMS, new string[LANGUAGE_COUNT] { RMS, S, "RMS", P, C, F , H } },
            { RUN, new string[LANGUAGE_COUNT] { RUN, "Ejecutarse", "Rennen", "Executar", C, "Exécuter" , H } },
            { RWU, new string[LANGUAGE_COUNT]
            {
                RWU,
                "Trabajador de restablecer no inicializado",
                "Arbeiter zurücksetzen nicht initialisiert",
                "Trabalhador de redefinir não inicializado",
                C,
                "Travailleur de réinitialisation non initialisé"
            , H } },
            #endregion R

            #region S
            { SAFE_TORQUE_OFF, new string[LANGUAGE_COUNT] { SAFE_TORQUE_OFF, S, "Safe Torque Off", P, C, F , H } },
            { SAFETY, new string[LANGUAGE_COUNT] { SAFETY, "Seguridad", "Safety", P, C, F , H } },
            { SAVE, new string[LANGUAGE_COUNT] { SAVE, "Guardar", "Speichern", P, C, F , H } },
            { SAVE_CONFIG_INSTR, new string[LANGUAGE_COUNT] { SAVE_CONFIG_INSTR, S, G, P, C, F , H } },
            { SAVE_CONFIG, new string[LANGUAGE_COUNT] { SAVE_CONFIG, "Guardar archivo de configuración", "Konfigurationsdatei speichern", "Salvar arquivo de configuração", C, "Sauvegarder fichier de configuration" , H } },
            { SAVED_PASS, new string[LANGUAGE_COUNT] { SAVED_PASS, "Contraseña Guardada", "Passwort speichern", P, C, F, H } },
            { SAVE_FLASH, new string[LANGUAGE_COUNT] { SAVE_FLASH, S, "Flash speichern", P, C, F , H } },
            { SAVE_LAYOUT, new string[LANGUAGE_COUNT] { SAVE_LAYOUT, "Guardar diseño", "Layout speichern", "Salvar disposição", C, "Sauvegarder mise en page" , H } },
            { SAVE_MOTIONS_FILE, new string[LANGUAGE_COUNT] { SAVE_MOTIONS_FILE, S, "Speichere Bewegungsdatei", P, C, F, H } },
            { SAVE_RESET_HISTOGRAM, new string[LANGUAGE_COUNT] { SAVE_RESET_HISTOGRAM, S, "Speichere Histogram", P, C, F, H } },
            { SAVE_PARAMETER_STATE, new string[LANGUAGE_COUNT] { SAVE_PARAMETER_STATE, "Guarde el Estado de Parámetros", "Parametereinstellungen speichern", P, C, F , H } },
            { SAVE_TO_FILE, new string[LANGUAGE_COUNT] { SAVE_TO_FILE, "Guardar en Archivo", "In Datei speichern", P, C, F , H } },
            { SAVING_CONFIG_PARAMS, new string[LANGUAGE_COUNT] { SAVING_CONFIG_PARAMS, S, G, P, C, F , H } },
            { SCALE, new string[LANGUAGE_COUNT] { SCALE, S, "Skala", P, C, F , H } },
            { SCAN, new string[LANGUAGE_COUNT] { SCAN, "Escanear", "Scannen", "Escanear", C, "Balayer" , H } },
            { SCANNING, new string[LANGUAGE_COUNT] { SCANNING, S, G, P, C, F , H } },
            { SCAN_COMMUNICATORS, new string[LANGUAGE_COUNT] { SCAN_COMMUNICATORS, S, "SCAN_COMMUNICATORS", P, C, F , H } },
            { SCAN_COMPLETED, new string[LANGUAGE_COUNT] { SCAN_COMPLETED, "Escaneado Completado", "Scan abgeschlossen", P, C, F , H } },
            { SCAN_SETTINGS_CAN, new string[LANGUAGE_COUNT] { SCAN_SETTINGS_CAN, S, "Einstellungen CANopen scannen", P, C, F , H } },
            { SCAN_FOR_DEVICES_ON, new string[LANGUAGE_COUNT] { SCAN_FOR_DEVICES_ON, "Buscar dispositivos en {0}", "G {0}", "P {0}", "C {0}", "F {0}" , H } },
            { SCAN_FOR_DATASHEETS, new string[LANGUAGE_COUNT] { SCAN_FOR_DATASHEETS, S, "Datenblatt suchen", P, C, F , H } },
            { SCANNING_DEVICES_ALLNET, new string[LANGUAGE_COUNT] { SCANNING_DEVICES_ALLNET, S, "Geräte suchen", P, C, F , H } },
            { SCANNING_DEVICES_COMMUNICATOR, new string[LANGUAGE_COUNT] { SCANNING_DEVICES_COMMUNICATOR, S, "Geräte suchen", P, C, F , H } },
            { SCAN_INIT, new string[LANGUAGE_COUNT] { SCAN_INIT, "Análisis Inicializado", "Scan intialisiert", P, C, F , H } },
            { SCAN_PERIOD, new string[LANGUAGE_COUNT] { SCAN_PERIOD, "Intervalo de escaneo", "Scan-Intervall", "Intervalo de digitalização", C, "Intervalle de balayage", H } },
            { SCAN_PROGRESS, new string[LANGUAGE_COUNT] { SCAN_PROGRESS, "Progreso del Escaneo", "Scan Fortschritt", "Progresso Da Digitalização", C, "Progression de balayage" , H } },
            { SCHEDULE, new string[LANGUAGE_COUNT] { SCHEDULE, "Programar", "Planen", "Agendar", C, "Planifier" , H } },
            { SCIENTIFIC_NOTATION, new string[LANGUAGE_COUNT] { SCIENTIFIC_NOTATION, "Notación Científica", "Wissenschaftliche Notation", P, C, F , H } },
            { SEARCH, new string[LANGUAGE_COUNT] { SEARCH, "Buscar", "Suchen", "Procurar", C, "Chercher" , H } },
            { SEARCH_FROM_NODE, new string[LANGUAGE_COUNT] { SEARCH_FROM_NODE, "Buscar desde el ID de nodo", "Suche beginnen bei Node ID", P, C, F , H } },
            { SEARCH_TO_NODE, new string[LANGUAGE_COUNT] { SEARCH_TO_NODE, "Buscar desde el ID de nodo", "Suche beenden bei Node ID", P, C, F , H } },
            { SECONDS_AFTER, new string[LANGUAGE_COUNT]
            {
                SECONDS_AFTER,
                "segundos después de que se pierda el vínculo de datos",
                "sekunden nach dem Verbindungsabbruch",
                P,
                C,
                F
            , H } },
            { SECTION, new string[LANGUAGE_COUNT] { SECTION, S, G, P, C, F , H } },
            { SELECT, new string[LANGUAGE_COUNT] { SELECT, "Seleccionar", "Wählen", "Seleccionar", C, "Sélectionner" , H } },
            { SELECT_ADAPT, new string[LANGUAGE_COUNT] { SELECT_ADAPT, "Seleccionar Adaptador", "Wählen Sie Adapter", "Seleccionar Adaptador", C, "Sélectionnez Adaptateur" , H } },
            { SELECT_LOG_FILE_LOCATION, new string[LANGUAGE_COUNT] { SELECT_LOG_FILE_LOCATION, S, G, P, C, F , H } },
            { SELECTED_FIRM_INFO, new string[LANGUAGE_COUNT] { SELECTED_FIRM_INFO, "Información de Firmware Seleccionada", "Aktuelle Firmwareinformationen", P, C, F , H } },
            { SELECTED_FIRM_FILE_LOCATION, new string[LANGUAGE_COUNT] { SELECTED_FIRM_FILE_LOCATION, S, G, P, C, F , H } },
            { SELECTED_LINKER, new string[LANGUAGE_COUNT] { SELECTED_LINKER, S, "Ausgewähltes Linker", P, C, F , H } },
            { SELECT_DEVICE, new string[LANGUAGE_COUNT] { SELECT_DEVICE, "Seleccionar Dispositivo", "Ausgewähltes Gerät", P, C, F , H } },
            { SEND, new string[LANGUAGE_COUNT] { SEND, "Enviar", "Schicken", "Enviar", C, "Envoyer" , H } },
            { SENSOR_SELECTION_CODE, new string[LANGUAGE_COUNT] { SENSOR_SELECTION_CODE, "Código de selección del sensor", "Sensorauswahlcode", P, C, F , H } },
            { SENT, new string[LANGUAGE_COUNT] { SENT, "Enviada", "Gesendet", "Enviada", C, "Envoyé" , H } },
            { SENT_MESSAGES, new string[LANGUAGE_COUNT] { SENT_MESSAGES, S, "Nachricht senden", P, C, F , H } },
            { SERIAL_NUMBER, new string[LANGUAGE_COUNT] { SERIAL_NUMBER, "Número de Serie", "Ordnungsnummer", "Número de Série", C, "Numéro de Série" , H } },
            { SET, new string[LANGUAGE_COUNT] { SET, "Poner", "Setzen", "Definir", C, "Fixer" , H } },
            { SETPOINT1, new string[LANGUAGE_COUNT] { SETPOINT1, "Punto de disparador 1", "Auslösepunkt 1", "Ponto Desencadeamento 1", C, "Point de déclenchement 1" , H } },
            { SETPOINT2, new string[LANGUAGE_COUNT] { SETPOINT2, "Punto de disparador 2", "Auslösepunkt 2", "Ponto Desencadeamento 2", C, "Point de déclenchement 2" , H } },
            { SETTINGS, new string[LANGUAGE_COUNT] { SETTINGS, "Configuraciones", "Datei", "Configurações", C, "Paramètres" , H } },
            { SETTING_UP, new string[LANGUAGE_COUNT] { SETTING_UP, "Configurando", "Einrichten", P, C, F , H } },
            { SETUP, new string[LANGUAGE_COUNT] { SETUP, "Arreglo", "Einrichtung","Configuração", "设置", "Coup Monté" , H } },
            { SHOW_ALL, new string[LANGUAGE_COUNT]
            {
                SHOW_ALL,
                "Mostrar Todos Dispositivos Encontrados",
                "Anzeige alle gefundenen Geräte",
                "Mostrar Todos Dispositivos Encontrados",
                C,
                "Montrer tous dispositifs trouvés"
            , H } },
            { SHOW_ALL_TEXT, new string[LANGUAGE_COUNT] { SHOW_ALL_TEXT, "Muestra Todo", "Alles anzeigen", P, C, F , H } },
            { SHOW_DATASHEET, new string[LANGUAGE_COUNT] { SHOW_DATASHEET, "Muestre la Hoja de Datos", "Datenblatt anzeigen", P, C, F , H } },
            { SHOW_DEVICE_TREE, new string[LANGUAGE_COUNT] { SHOW_DEVICE_TREE, "Mostrar el árbol de dispositivos.", "Gerätebaum anzeigen", P, C, F , H } },
            { SHOW_HISTORY, new string[LANGUAGE_COUNT] { SHOW_HISTORY, S, "Verlauf anzeigen", P, C, F , H } },
            { SHOW_HIDDEN_PARAMS, new string[LANGUAGE_COUNT] { SHOW_HIDDEN_PARAMS, "Muestre los Parámetros Ocultos", "Versteckte Parameter anzeigen", P, C, F , H } },
            { SHOW_STATUS_BAR, new string[LANGUAGE_COUNT] { SHOW_STATUS_BAR, S, "Statusleiste anzeigen", P, C, F , H } },
            { SHOW_WINDOWS, new string[LANGUAGE_COUNT] { SHOW_WINDOWS, S, "Fenster anzeigen", P, C, F , H } },
            { SHUTDOWN, new string[LANGUAGE_COUNT] { SHUTDOWN, "Apagar", G, P, C, F, H } },
            { SHUTDOWN_ON_DISCONNECT, new string[LANGUAGE_COUNT] { SHUTDOWN_ON_DISCONNECT, "Apagar al Desconectar", "Bei Verbindungsunterbrechung abschalten", P, C, F, H } },
            { SIGNAL, new string[LANGUAGE_COUNT] { SIGNAL, S, "Signal", P, C, F, H } },
            { SIGNALS, new string[LANGUAGE_COUNT] { SIGNALS, S, "Signale", P, C, F, H } },
            { SIGNAL_SELECTION, new string[LANGUAGE_COUNT] { SIGNAL_SELECTION, S, "Signalauswahl", P, C, F, H } },
            { SLOW_DOWN_RAMP, new string[LANGUAGE_COUNT] { SLOW_DOWN_RAMP, "Rampa de desaceleración", "Verzögerungsrampe", P, C, F , H } },
            { SOFTWARE_IDENTIFICATION_NOT_UPDATED, new string[LANGUAGE_COUNT] { SOFTWARE_IDENTIFICATION_NOT_UPDATED, S, G, P, C, F , H } },
            { SOURCE_PANEL, new string[LANGUAGE_COUNT] { SOURCE_PANEL, S, "SOURCE_PANEL", P, C, F , H } },
            { SPIN, new string[LANGUAGE_COUNT] { SPIN, S, G, P, C, F , H } },
            { STANDARD, new string[LANGUAGE_COUNT] { STANDARD, "Estándar", "Standard", P, C, F , H } },
            { START, new string[LANGUAGE_COUNT] { START, S, "Start", P, C, F , H } },
            { START_CAPTURE, new string[LANGUAGE_COUNT] { START_CAPTURE, "Inicie la Captura", "Aufzeichnung starten", P, C, F , H } },
            { START_MAXIMIZED, new string[LANGUAGE_COUNT] { START_MAXIMIZED, "Comience maximizado", "Start maximiert", "Início maximizado", C, "Commencer maximisé" , H } },
            { START_TIME, new string[LANGUAGE_COUNT] { START_TIME, S, "Startzeit", P, C, F , H } },
            { STARTUP_SCANNING_OPTIONS, new string[LANGUAGE_COUNT] { STARTUP_SCANNING_OPTIONS, S, "Startoptionen für das Scannen", P, C, F , H } },
            { STATUS, new string[LANGUAGE_COUNT] { STATUS, S, "Status", P, C, F , H } },
            { STATUS_BAR, new string[LANGUAGE_COUNT] { STATUS_BAR, S, "Statusleiste", P, C, F , H } },
            { STATUS_COMPLETE, new string[LANGUAGE_COUNT]
            {
                STATUS_COMPLETE,
                "Estado: Escaneo completo",
                "Status: Scan abgeschlossen",
                "Estado: Digitalização está concluída",
                C,
                "Statut: Balayage terminé"
            , H } },
            { STATUS_IN_PROGRESS, new string[LANGUAGE_COUNT]
            {
                STATUS_IN_PROGRESS,
                "Estado: Escaneo se está ejecutando",
                "Status: Scan in bearbeitung",
                "Estado: Digitalização em curso",
                C,
                "Statut: Balayage en cours"
            , H } },
            { STATUS_MONITOR, new string[LANGUAGE_COUNT] { STATUS_MONITOR, S, "Statusmonitor", P, C, F , H } },
            { STATUS_NOT_RUN, new string[LANGUAGE_COUNT]
            {
                STATUS_NOT_RUN,
                "Estado: Escaneo no se ejecuta",
                "Status: Scan nicht ausgeführt",
                "Estado: Digitalização não é executada",
                C,
                "Statut: Balayage non exécutée"
            , H } },
            { STATUS_PANEL, new string[LANGUAGE_COUNT] { STATUS_PANEL, "Panel de estado", "Statusanzeige", P, C, F , H } },
            { STATUSWORD, new string[LANGUAGE_COUNT] { STATUSWORD, "Palabra de estado", "Statusword", P, C, F , H } },
            { STIMULUS, new string[LANGUAGE_COUNT] { STIMULUS, "Estímulo", "Stimulus", "Estímulo", C, "Stimulus" , H } },
            { STOP, new string[LANGUAGE_COUNT] { STOP, "Parar", "Halt", "Parar", C, "Arrêter" , H } },
            { STOP_IMMEDIATE, new string[LANGUAGE_COUNT] { STOP_IMMEDIATE, "Parada Inmediata", "Sofort Halt", "Parada Imediata", C, "Arrêt Immédiat" , H } },
            { STOPPED, new string[LANGUAGE_COUNT] { STOPPED, S, "Stopped/Safeop", P, C, F , H } },
            { STOP_SCHEDULED, new string[LANGUAGE_COUNT] { STOP_SCHEDULED, "Parada programada", "Geplanter Halt", "Parada Agendada", C, "Arrêt Prévu" , H } },
            { SUCCESS, new string[LANGUAGE_COUNT] { SUCCESS, "Éxito", "Gucci Gang", P, C, F , H } },
            { SWITCH_AXIS, new string[LANGUAGE_COUNT] { SWITCH_AXIS, "Cambiar el eje", "Achsen vertauschen", P, C, F , H } },
            { SWITCHED_ON, new string[LANGUAGE_COUNT] { SWITCHED_ON, "Enciende", "Eingeschaltet", P, C, F , H } },
            { SWITCH_ON, new string[LANGUAGE_COUNT] { SWITCH_ON, "Enciende", "Einschalten", P, C, F , H } },
            { SWITCH_ON_ENABLE_OPERATION, new string[LANGUAGE_COUNT] { SWITCH_ON_ENABLE_OPERATION, S, G, P, C, F , H } },
            { SWITCH_ON_DISABLED, new string[LANGUAGE_COUNT] { SWITCH_ON_DISABLED, S, "Ausschalten", P, C, F , H } },
            { SYSTEM, new string[LANGUAGE_COUNT] { SYSTEM, S, "System", P, C, F , H } },
            #endregion S

            #region T
            { TARGET_POSITION, new string[LANGUAGE_COUNT] { TARGET_POSITION, S, "Zielposition", P, C, F , H } },
            { TARGET_REACHED, new string[LANGUAGE_COUNT] { TARGET_REACHED, S, "Ziel erreicht", P, C, F , H } },
            { TARGET_TORQUE, new string[LANGUAGE_COUNT] { TARGET_TORQUE, "Par Objetivo", "Solldrehmoment", P, C, F , H } },
            { TARGET_TORQUE_INFO, new string[LANGUAGE_COUNT] { TARGET_TORQUE_INFO, S, G, P, C, F , H } },
            { TARGET_VELOCITY, new string[LANGUAGE_COUNT] { TARGET_VELOCITY, "Velocidad Objetivo", "Sollgeschwindigkeit", P, C, F , H } },
            { TEST, new string[LANGUAGE_COUNT] { TEST, S, G, P, C, F , H } },
            { TEST_LOG_GENERATION, new string[LANGUAGE_COUNT] { TEST_LOG_GENERATION, "Probar generación de registros", "Testen Protokollgenerierung", "Testar geração de logs", C, "Tester génération de journal" , H } },
            { THEME, new string[LANGUAGE_COUNT] { THEME, "Tema", "Thema", P, C, "Thème" , H } },
            { TIME, new string[LANGUAGE_COUNT] { TIME, "Tiempo", "Ziet", "Tempo", C, "Temps" , H } },
            { TIME_BASE, new string[LANGUAGE_COUNT] { TIME_BASE, "Base de tiempo", "Zeitbasis", "Base de tempo", C, "Base de temps" , H } },
            { TIMED_OUT_WAITING_FOR_RESPONSE, new string[LANGUAGE_COUNT] { TIMED_OUT_WAITING_FOR_RESPONSE, S, G, P, C, F , H } },
            { TIME_W_UNIT, new string[LANGUAGE_COUNT] { TIME_W_UNIT, S, "Zeiteinheit", P, C, F , H } },
            { TO_NODE_ID, new string[LANGUAGE_COUNT] { TO_NODE_ID, "al ID de nodo", "Zur Node ID", P, C, F , H } },
            { TOOLS, new string[LANGUAGE_COUNT] { TOOLS, "Herramienta", "Werkzeuge", P, C, F , H } },
            { TOO_MANY_PARAMETERS, new string[LANGUAGE_COUNT] { TOO_MANY_PARAMETERS, "¡Demasiados parámetros!", "Zuviele Parameter", P, C, F , H } },
            { TOP, new string[LANGUAGE_COUNT] { TOP, "Cima", "Oben", P, C, F , H } },
            { TORQUE, new string[LANGUAGE_COUNT] { TORQUE, "Par de Torsión", "Drehmoment", P, C, F , H } },
            { TORQUE_ACTUAL_VALUE, new string[LANGUAGE_COUNT] { TORQUE_ACTUAL_VALUE, "Valor Real del Par", "Aktuelles Drehmoment", P, C, F , H } },
            { TORQUE_ACTUAL_VALUE_INFO, new string[LANGUAGE_COUNT] { TORQUE_ACTUAL_VALUE_INFO, S, G, P, C, F , H } },
            { TORQUE_DEMAND, new string[LANGUAGE_COUNT] { TORQUE_DEMAND, "Demanda de Par", "Drehmomentbedarf", P, C, F , H } },
            { TORQUE_DEMAND_INFO, new string[LANGUAGE_COUNT] { TORQUE_DEMAND_INFO, S, G, P, C, F , H } },
            { TORQUE_PROFILE, new string[LANGUAGE_COUNT] { TORQUE_PROFILE, "Perfil de Par", "Drehmomentprofil", P, C, F , H } },
            { TORQUE_PROFILE_INFO, new string[LANGUAGE_COUNT] { TORQUE_PROFILE_INFO, S, G, P, C, F , H } },
            { TORQUE_SLOPE, new string[LANGUAGE_COUNT] { TORQUE_SLOPE, "Pendiente de Par", "Drehmomentrampe", P, C, F , H } },
            { TORQUE_SLOPE_INFO, new string[LANGUAGE_COUNT] { TORQUE_SLOPE_INFO, S, G, P, C, F , H } },
            { TRAJECTORY, new string[LANGUAGE_COUNT] { TRAJECTORY, S, "Trajektorie", P, C, F , H } },
            { TRIGGER_BETWEEN, new string[LANGUAGE_COUNT] { TRIGGER_BETWEEN, "Desencadenador entre límites", "Trigger zwischen den Limits", P, C, F , H } },
            { TRIGGER_OUTSIDE, new string[LANGUAGE_COUNT] { TRIGGER_OUTSIDE, "Desencadenador fuera de límites", "Trigger außerhalb den Limits", P, C, F , H } },
            { TRIGGER_POSITION, new string[LANGUAGE_COUNT] { TRIGGER_POSITION, S, "Trigger Position", P, C, F , H } },
            { TRIGGER_SETTINGS, new string[LANGUAGE_COUNT] { TRIGGER_SETTINGS, "Disparador", "Trigger Einstellungen", "Disparador", C, "Déclenchement" , H } },
            { TROUBLESHOOTING, new string[LANGUAGE_COUNT] { TROUBLESHOOTING, "Solución de Problemas", "Fehlerbehebung", P, C, F , H } },
            { TYPE, new string[LANGUAGE_COUNT] { TYPE, "Tipo", "Typ", P, C, F , H } },
            { TYPE_OF_MOTION, new string[LANGUAGE_COUNT] { TYPE_OF_MOTION, S, "Bewegungsart", P, C, F , H } },
            #endregion T

            #region U
            { UDP_MULTICAST, new string[LANGUAGE_COUNT] { UDP_MULTICAST, S, "UDP Mulicast", P, C, F , H } },
            { UNABLE_TO_CHANGE_BAUD, new string[LANGUAGE_COUNT] { UNABLE_TO_CHANGE_BAUD, "No se puede cambiar la velocidad en baudios", "Baudrate konnte nicht geändert werden", P, C, F , H } },
            { UNABLE_READ_BIN, new string[LANGUAGE_COUNT] { UNABLE_READ_BIN, S, G, P, C, F , H } },
            { UNAVAILABLE, new string[LANGUAGE_COUNT] { UNAVAILABLE, "No disponible", "Nicht erreichbar", P, C, F , H } },
            { UNCOMMUTATED_CURR, new string[LANGUAGE_COUNT] { UNCOMMUTATED_CURR, "Corriente de Bucle Cerrado", "Closed_Loop_Curr", P, C, F , H } },
            { UNDO, new string[LANGUAGE_COUNT] { UNDO, "Deshacer", "Rückgängig", P, C, F , H } },
            { USE_MULTICAST, new string[LANGUAGE_COUNT] { USE_MULTICAST, "Use Multidifusión", "Use Multicast", P, C, F , H } },
            { UNIDENTIFIED_DEVICE, new string[LANGUAGE_COUNT]
            {
                UNIDENTIFIED_DEVICE,
                "Dispositivo no identificado encontrado en IP: ",
                "Nicht identifiziertes Gerät in IP gefunden: ",
                "Dispositivo não identificado encontrado no IP: ",
                C,
                "Dispositif non identifié trouvé à IP: "
            , H } },
            { UNINITIALIZED, new string[LANGUAGE_COUNT] { UNINITIALIZED, "No Inicializado", "Nicht initialisiert", P, C, F , H } },
            { UNKNOWN, new string[LANGUAGE_COUNT] { UNKNOWN, "Desconocido", "Unbekannt", P, C, F , H } },
            { UNKNOWN_STATE, new string[LANGUAGE_COUNT] { UNKNOWN_STATE, "Estado Desconocido!", "Unbekannter Zustand", P, C, F , H } },
            { UNIT, new string[LANGUAGE_COUNT] { UNIT, S, G, P, C, F , H } },
            { UNLINK, new string[LANGUAGE_COUNT] { UNLINK, "Desconectar", "Verbindung beenden", P, C, F , H } },
            { UNLINK_ALL, new string[LANGUAGE_COUNT] { UNLINK_ALL, S, "UNLINK_ALL", P, C, F , H } },
            { UNLINK_ON_CHANGE, new string[LANGUAGE_COUNT] { UNLINK_ON_CHANGE, S, "UNLINK_ON_CHANGE", P, C, F , H } },
            { UNLOCKED, new string[LANGUAGE_COUNT] { UNLOCKED, "Desbloqueado", "Entsperrt", "Desbloqueado", C, "Déverrouillé" , H } },
            { USE, new string[LANGUAGE_COUNT] { USE, S, "Verwenden", P, C, F , H } },
            { UNLOCK_RAMP, new string[LANGUAGE_COUNT] { UNLOCK_RAMP, "Rampa de Desbloqueo", "Rampe entsperren", P, C, F , H } },
            { USER_AUTHORIZATION, new string[LANGUAGE_COUNT] { USER_AUTHORIZATION, "Autorización de Usuario", "Benutzer Autorisierung", P, C, F , H } },
            { USER_LEVEL, new string[LANGUAGE_COUNT] { USER_LEVEL, "Nivel de Usuario", "Benutzerlevel", P, C, F , H } },
            { USER_LEVEL_INFO, new string[LANGUAGE_COUNT]
            {
                USER_LEVEL_INFO,
                "Esto solo se aplicará a los dispositivos que permiten el nivel de acceso seleccionado",
                "Diese Nachricht wird nur auf Geräten angezeigt, die den Benutzerlevel unterstützen.",
                P,
                C,
                F
            , H } },
            { USER_OUTPUT, new string[LANGUAGE_COUNT] { USER_OUTPUT, "Salida de usuario", "Benutzerausgabe", "Saída do usuário", C, "Sortie utilisateur" , H } },
            { USER_SETTINGS, new string[LANGUAGE_COUNT] { USER_SETTINGS, "Configuración del Usuario", "Benutzereinstellungen", P, C, F , H } },
            { UPDATE_RATE, new string[LANGUAGE_COUNT] { UPDATE_RATE, S, "Update Rate", P, C, F , H } },
            { UPDATING_FIRMWARE, new string[LANGUAGE_COUNT] { UPDATING_FIRMWARE, S, G, P, C, F , H } },
            { UPDATING_FIRMWARE_FAILED, new string[LANGUAGE_COUNT] { UPDATING_FIRMWARE_FAILED, S, G, P, C, F , H } },
            { UPLOAD_FILE, new string[LANGUAGE_COUNT] { UPLOAD_FILE, "Suba el Archivo", "Datei hochladen", P, C, F , H } },
            #endregion U

            #region V
            { VALIDATE, new string[LANGUAGE_COUNT] { VALIDATE, "Valide", "Validieren", P, C, F , H } },
            { VALID_PATH, new string[LANGUAGE_COUNT] { "VALID_PATH", "Validando la ruta de acceso del registro", "Pfad gültig", P, C, F , H } },
            { VARIABLE, new string[LANGUAGE_COUNT] { VARIABLE, "Variable", "Variable", "Variável", C, "Variable" , H } },
            { VELOCITY, new string[LANGUAGE_COUNT] { VELOCITY, "Velocidad", "Geschwindigkeit", "Velocidade", C, "Rapidité" , H } },
            { VELOCITY_ACTUAL_VALUE, new string[LANGUAGE_COUNT] { VELOCITY_ACTUAL_VALUE, "Valor Real de Velocidad", "Aktueller Geschwindigkeitswert", P, C, F , H } },
            { VELOCITY_DEMAND, new string[LANGUAGE_COUNT] { VELOCITY_DEMAND, "Demanda de Velocidad", "Geschwindigkeitsbedarf", P, C, F , H } },
            { VELOCITY_LIMIT, new string[LANGUAGE_COUNT] { VELOCITY_LIMIT, S, "", P, C, F , H } },
            { VELOCITY_LEVEL, new string[LANGUAGE_COUNT]
            {
                VELOCITY_LEVEL,
                "Nivel de velocidad seleccionado",
                "Geschwindigkeitsstufe ausgewählt",
                "Nível de velocidade selecionado",
                C,
                "Niveau de vélocité sélectionné"
            , H } },
            { VELOCITY_THRESHOLD, new string[LANGUAGE_COUNT] { VELOCITY_THRESHOLD, "Umbral de Velocidad", "Geschwindigkeitsschwelle", P, C, F , H } },
            { VELOCITY_THRESHOLD_TIME, new string[LANGUAGE_COUNT] { VELOCITY_THRESHOLD_TIME, "Tiempo de Umbral de Velocidad", "Geschwindigkeitsschwellenzeit", P, C, F , H } },
            { VELOCITY_UPDATED, new string[LANGUAGE_COUNT]
            {
                VELOCITY_UPDATED,
                "Velocidad actualizado a {0}",
                "Geschwindigkeit aktualisiert auf {0}",
                "Velocidade atualizada para {0}",
                C,
                "Rapidité mis à jour à {0}"
            , H } },
            { VELOCITY_WINDOW, new string[LANGUAGE_COUNT] { VELOCITY_WINDOW, "Ventana de Velocidad", "Geschwindigkeitsfenster", P, C, F , H } },
            { VELOCITY_WINDOW_TIME, new string[LANGUAGE_COUNT] { VELOCITY_WINDOW_TIME, "Tiempo de Ventana de Velocidad", "Zeit des Geschwindigkeitsfensters", P, C, F , H } },
            { VERBOSE, new string[LANGUAGE_COUNT] { VERBOSE, "Verboso", "Wortreich", "Verboso", C, "Verbeux" , H } },
            { VERSION, new string[LANGUAGE_COUNT] { VERSION, "Versión", "Version", P, C, F , H } },
            { VF_MODE_FREQUENCY, new string[LANGUAGE_COUNT] { VF_MODE_FREQUENCY, S, "Frequenz für U/f-Modus", P, C, F , H } },
            { VIRTUAL, new string[LANGUAGE_COUNT] { VIRTUAL, "Virtual", "Virtuell", "Virtual", C, "Virtuel" , H } },
            { VIEW, new string[LANGUAGE_COUNT] { VIEW, S, "Ansicht", P, C, F , H } },
            { VISIBLE, new string[LANGUAGE_COUNT] { VISIBLE, "Visible", "Sichtbar", P, C, F , H } },
            { VOLTAGE_DISABLED, new string[LANGUAGE_COUNT] { VOLTAGE_DISABLED, S, G, P, C, F , H } },
            { VOLTAGE_ENABLED, new string[LANGUAGE_COUNT] { VOLTAGE_ENABLED, S, "Spannung freigegeben", P, C, F , H } },
            { VOLTAGE_LIMIT, new string[LANGUAGE_COUNT] { VOLTAGE_LIMIT, "Límite de Voltaje", "Spannungslimit", P, C, F , H } },
            #endregion// V

            #region W
            { WAIT_TARGET_REACHED, new string[LANGUAGE_COUNT] { WAIT_TARGET_REACHED, S, "Auf Zielposition warten", P, C, F , H } },
            { WARNING, new string[LANGUAGE_COUNT] { WARNING, "Aviso", "Warnung", "Aviso", C, "Avertissement" , H } },
            { WARNING_HISTORY, new string[LANGUAGE_COUNT] { WARNING_HISTORY, "Historia de Aviso", "Warnungshistorie", P, C, F , H } },
            { WELCOME, new string[LANGUAGE_COUNT] { WELCOME, "Bienvenido", "Servus", P, C, F , H } },
            { WELCOME_TO_DATAM, new string[LANGUAGE_COUNT] { WELCOME_TO_DATAM, "Bienvenido a Datam", "Willkommen bei Datam!", P, C, F , H } },
            { WINDOWS, new string[LANGUAGE_COUNT] { WINDOWS, "Ventanas", "Fenster", "Janelas", C, "Fenêtres" , H } },
            { WITHIN_SPS, new string[LANGUAGE_COUNT] { WITHIN_SPS, "Dentro de SP1 y SP2", "Mit SPS", P, C, F , H } },
            { WRITING_FLASH_FILE, new string[LANGUAGE_COUNT] { WRITING_FLASH_FILE, S, G, P, C, F , H } },
            #endregion// W

            #region Y
            { YES, new string[LANGUAGE_COUNT] { YES, "Sí", "Ja", P, C, F , H } },
            #endregion Y

            #region Z
            { ZERO_TARGET_PARAMS, new string[LANGUAGE_COUNT]
            {
                ZERO_TARGET_PARAMS,
                "Poner a cero los parámetros de destino\n en la desactivación",
                "Keine Zielparameter vorhanden.",
                P,
                C,
                F
            , H } },
            #endregion /Z

            #endregion General

            #region Special Definition

            #region Dynamic
            { CONNECTED_TO_X_DYNAMIC, new string[LANGUAGE_COUNT] { CONNECTED_TO_X_DYNAMIC, "Conéctese a {0}", "Verbinde zu {0}", P, C, F , H } },
            { DATE_DYNAMIC, new string[LANGUAGE_COUNT] { DATE_DYNAMIC, S, G, P, C, F , H } },
            { DATASHEET_REMOVAL_DYNAMIC, new string[LANGUAGE_COUNT] { DATASHEET_REMOVAL_DYNAMIC, S, G, P, C, F , H } },
            { ENCODER_OFFSET_DYNAMIC, new string[LANGUAGE_COUNT] { ENCODER_OFFSET_DYNAMIC, S, G, P, C, F , H } },
            { EXISTING_BAUDRATE_DYNAMIC, new string[LANGUAGE_COUNT] { EXISTING_BAUDRATE_DYNAMIC, S, G, P, C, F , H } },
            { EXISTING_NODE_ID_DYNAMIC, new string[LANGUAGE_COUNT] { EXISTING_NODE_ID_DYNAMIC, S, G, P, C, F , H } },
            { FAILURE_ADDRESS_DYNAMIC, new string[LANGUAGE_COUNT] { FAILURE_ADDRESS_DYNAMIC, S, G, P, C, F , H } },
            { JSON_FORMAT_ERROR_LINE_DYNAMIC, new string[LANGUAGE_COUNT] { JSON_FORMAT_ERROR_LINE_DYNAMIC, S, G, P, C, F , H } },
            { JSON_FORMAT_ERROR_LINE_COLUMN_DYNAMIC, new string[LANGUAGE_COUNT] { JSON_FORMAT_ERROR_LINE_COLUMN_DYNAMIC, S, G, P, C, F , H } },
            { JSON_FORMAT_ERROR_PATH_LINE_DYNAMIC, new string[LANGUAGE_COUNT] { JSON_FORMAT_ERROR_LINE_DYNAMIC, S, G, P, C, F , H } },
            { JSON_FORMAT_ERROR_PATH_LINE_COLUMN_DYNAMIC, new string[LANGUAGE_COUNT] { JSON_FORMAT_ERROR_LINE_COLUMN_DYNAMIC, S, G, P, C, F , H } },
            { JSON_FORMAT_ERROR_PATH_DYNAMIC, new string[LANGUAGE_COUNT] { JSON_FORMAT_ERROR_LINE_DYNAMIC, S, G, P, C, F , H } },
            { OPERATION_CONFIGMANUFAC_DYNAMIC, new string[LANGUAGE_COUNT] { OPERATION_CONFIGMANUFAC_DYNAMIC, S, G, P, C, F , H } },
            { OUTPUT_FILE_DYNAMIC, new string[LANGUAGE_COUNT] { OUTPUT_FILE_DYNAMIC, S, G, P, C, F , H } },
            { PATH_DYNAMIC, new string[LANGUAGE_COUNT] { PATH_DYNAMIC, S, G, P, C, F , H } },
            { PARAM_RESET_ATTEMPT_DYNAMIC, new string[LANGUAGE_COUNT] { PARAM_RESET_ATTEMPT_DYNAMIC, S, G, P, C, F , H } },
            { NODE_DEFAULT_DYNAMIC, new string[LANGUAGE_COUNT] { PATH_DYNAMIC, S, G, P, C, F , H } },
            { TIME_DYNAMIC, new string[LANGUAGE_COUNT] { TIME_DYNAMIC, S, G, P, C, F , H } },
            { VERSION_DYNAMIC, new string[LANGUAGE_COUNT] { VERSION_DYNAMIC, S, G, P, C, F , H } },
            #endregion Dynamic

            #region Error
            { ERROR_CLOSE_UNEXPECTED, new string[LANGUAGE_COUNT]
            {
                ERROR_CLOSE_UNEXPECTED,
                "Error: La ventana se cerró inesperadamente",
                "Das Fenster wurde unerwartet geschlossen",
                P,
                C,
                F
            , H } },
            { ERROR_ERROR_COUNT, new string[LANGUAGE_COUNT]
            {
                ERROR_ERROR_COUNT,
                "El recuento de errores supera las condiciones de ejecución estables. Se recomienda el apagado. ¿Proceder?",
                "Fehlerzähler überschreitet Betriebsfenster. Herunterfahren wird empfohlen. Fortfahren?",
                P,
                C,
                F
            , H } },
            { ERROR_DEVICE_NOT_SUPPORTED, new string[LANGUAGE_COUNT]
            {
                ERROR_DEVICE_NOT_SUPPORTED,
                S,
                G,
                P,
                C,
                F
            , H } },
            { ERROR_MANUFACT_PARAM_SAVE_FAIL, new string[LANGUAGE_COUNT]
            {
                ERROR_MANUFACT_PARAM_SAVE_FAIL,
                S,
                G,
                P,
                C,
                F
            , H } },
            { ERROR_PANEL_NOT_SUPPORTED, new string[LANGUAGE_COUNT]
            {
                ERROR_PANEL_NOT_SUPPORTED,
                "Error: Esta ventana no es compatible",
                "Fenster wird nicht untersützt",
                P,
                C,
                F
            , H } },
            { ERROR_RUNTIME, new string[LANGUAGE_COUNT]
            {
                ERROR_RUNTIME,
                "Error de Ejecución", //TODO: This technically means "Execution Error"
                "Runtime Error",
                P,
                C,
                F
            , H } },
            #endregion

            #region Instructions
            { INSTRUCTIONS, new string[LANGUAGE_COUNT]
            {
                INSTRUCTIONS,
                "Instrucciones",
                "Anleitung",
                "Instruções",
                C,
                "Instructions"
            , H } },
            { INSTRUCTIONS_CGC, new string[LANGUAGE_COUNT]
            {
                "This application computes the PID current loop gains. Target open loop GH gain and phase margins are 8 dB and 45 degrees. These are the default margins used by Allied products.",
                S,
                "Die application berechnet die PID Stromverstärkungen. Dei angestreben Werte sind für die Verstärkung 8dB und den Phasenversatz 45. Dies entspricht den default werden von Allied Motion Produkten.",
                P,
                C,
                F
            , H } },
            { INSTRUCTIONS_CONFIGURATIONS_SAVE_LOAD_TOOL, new string[LANGUAGE_COUNT]
            {
                "If you would like to load a configuration file, " + Tokens._nl_ + "please use the 'Load From File' button. "
                + Tokens._nl_ + "This launches a window that allows for the selection of the configuration file. " + Tokens._nl_ +
                "The load process will begin automatically once a file is selected, " + Tokens._nl_ + "so please choose carefully." + Tokens._nl_ + Tokens._nl_,
                S,
                G,
                P,
                C,
                F
            , H } },
            { INSTRUCTIONS_CONFIGURATIONS_SAVE_INSTR, new string[LANGUAGE_COUNT]
            {
                "If you would like to save a configuration file, " + Tokens._nl_ + "please use the 'Save To File' button. " + Tokens._nl_ +
                "Then choose a save file location for the configuration file in the dialog. " + Tokens._nl_ + "Once the save directory is selected," +
                "a json formatted configuration file will be generated " + Tokens._nl_ + "in the selected location." + Tokens._nl_,
                S,
                G,
                P,
                C,
                F
            , H } },
            { INSTRUCTIONS_DATASHEET_ADD_UPDATE, new string[LANGUAGE_COUNT]
            {
                "A datasheet can be loaded using the 'Add/Update' button." + Tokens._nl_ + " When slected, this launches a window that allows for the selection of the datasheet file."
                 + Tokens._nl_ + " The process will begin automatically once a file is selected," + Tokens._nl_ + " so please be careful of overwriting needed information."
                 + Tokens._nl_ + Tokens._nl_,
                S,
                G,
                P,
                C,
                F
            , H } },
            { INSTRUCTIONS_DATASHEET_DIALOG, new string[LANGUAGE_COUNT]
            {
                "Select a datasheet version and press 'Remove' to permanently delete it.",
                S,
                "Datenblatt auswählen und mit `Remove' dauerhaft löschen. ",
                P,
                C,
                F
            , H } },
            { INSTRUCTIONS_DATASHEET_REMOVE, new string[LANGUAGE_COUNT]
            {
                "A datasheet can be loaded using the 'Remove' button." + Tokens._nl_ + " When selected, this launches a window that allows for the selection of the datasheet version to be removed."
                + Tokens._nl_ + " The process will happen as soon as the dialog is closed, so please be careful of deleting needed information.",
                S,
                G,
                P,
                C,
                F
            , H } },
            { INSTRUCTIONS_DATASHEET_SCAN, new string[LANGUAGE_COUNT]
            {
                "If you would like to load several datasheet files from a folder," + Tokens._nl_ + " please use the 'Scan' button."
                 + Tokens._nl_ + " This launches a window that allows for the selection of the folder containing the files." + Tokens._nl_ +
                " The load process will begin automatically once a folder is selected." + Tokens._nl_ + " If a new datasheet is found it will be loaded and saved,"
                 + Tokens._nl_ + " otherwise you will be alerted of any already existing datasheets found." + Tokens._nl_ +
                " The process does not check to see if these datasheets match, it only compares their version number." + Tokens._nl_ +
                " If you need to replace a datasheet, please first use the 'Remove' button to remove the old one and" + Tokens._nl_ +
                " then a new one can be loaded using the same process." + Tokens._nl_ + Tokens._nl_,
                S,
                G,
                P,
                C,
                F
            , H } },
            { INSTRUCTIONS_DEV_USER_SETTINGS, new string[LANGUAGE_COUNT]
            {
                "Please select the desired access level. If the desired level isn't available, " +
                "authentication may be required, and may be done using the authorization section. " +
                "To do this fill in the password field then select validate. If authenticated the levels" +
                " available will be added to the drop down selection.",
                "Seleccione el nivel de acceso deseado. Si el nivel deseado no está disponible, " +
                "la autenticación puede ser necesaria y se puede hacer mediante la sección de autorización.",
                "Bitte wählen Sie die gewünschte Zugriffsebene aus. " +
                "Wenn die gewünschte Stufe nicht verfügbar ist, " +
                "ist möglicherweise eine Authentifizierung erforderlich, " +
                "die über den Autorisierungsabschnitt erfolgen kann",
                P,
                C,
                F,
                H
            }},
            { INSTRUCTIONS_FIRMWARE_LOADING, new string[LANGUAGE_COUNT]
            {
                "If you would like to upload firmware to the device, " + Tokens._nl_ + "please use the 'Find File' button. "
                + Tokens._nl_ + "This launches a window that allows for the selection of the firmware file. " + Tokens._nl_ +
                "After selecting the file, the new firmware information will display " + Tokens._nl_ + "and the 'Update' button will be available. "
                + Tokens._nl_ + "Clicking the 'Update' button will begin the firmware flash process. " + Tokens._nl_ +
                "Warning: The firmware update is not reversible, cannot be paused, " + Tokens._nl_ + "and will prevent any other device communications from occurring." + Tokens._nl_,
                S,
                G,
                P,
                C,
                F,
                H
            }},
            { INSTRUCTIONS_LOAD_MANUFACT_LABEL, new string[LANGUAGE_COUNT]
            {
                "If you would like to load a manufacturer label data file, " + Tokens._nl_ + "first please select any options you would like. "
                + Tokens._nl_ + "You have the option to add a serial number and/or " + Tokens._nl_ + "to automatically determine the magnetic encoder offset. "
                + Tokens._nl_ + "Once you have the desired options selected, " + Tokens._nl_ + "please use the 'Load From File' button. "
                + Tokens._nl_ + "This launches a window that allows for the selection of the manufacturer label data file. " + Tokens._nl_ +
                "The load process will begin automatically once a file is selected, " + Tokens._nl_ + "so please choose carefully." + Tokens._nl_ + Tokens._nl_,
                S,
                G,
                P,
                C,
                F,
                H
            }},
            { INSTRUCTIONS_MOVE_AXES, new string[LANGUAGE_COUNT]
            {
                "Moving variables to the corresponding axis boxes will move the corresponding variable to that axis. " +
                "Selecting the checkbox will make the variable visible on the graph.",
                "Movimiendo variables a los cuadros del eje correspondientes moverá la variables a ese eje. " +
                "Seleccionanto la casilla de verificación hará que la variable sea visible en el gráfico.",
                "Durch das Verschieben von Variablen auf die entsprechenden Achsenfelder wird die entsprechende Variable auf diese Achse verschoben. " +
                "Durch Aktivieren des Kontrollkästchens wird die Variable im Graph angezeigt.",
                "Movendo variáveis para os campos do eixo correspondentes, " +
                "a variável correspondente é movida para esse eixo. Marcar a caixa exibe a variável no gráfico.",
                C,
                "Déplacer les variables vers les cases d'axe correspondantes déplacera la variable correspondante sur cet axe sur le graphique. " +
                "Si vous cochez cette case, la variable sera visible sur le graphique."
                , H 
            } },
            { INSTRUCTIONS_PHASING_MODE, new string[LANGUAGE_COUNT]
            {
                "To find the encoder offset the motor must be enabled in this mode. " +
                "After this is done i should be left enaled for at least 30 seconds, " +
                "while it is running it may emit noise. Finally, once " +
                "this time has elapsed, the offset should have been calculated, and muse be saved to flase for it to take effect. " +
                "To do this, press the save button on this window.",
                S,
                "Um den Encoder offset zu ermitteln, muss der Motor in diesem Modus gestartet werden. " +
                "Dabei muss der Motor mindestens 30 Sekunden, dabei können eine Geräuschentwicklung und Bewegungen auftreten. " +
                "Nach dem Zeitraum sollte ein neuer Offset angezeigt werden und muss mit dem 'Save' Button n den Flashspeicher geladen werden.",
                P,
                C,
                F, 
                H 
            } },
            { INSTRUCTIONS_SEL_ADAPT, new string[LANGUAGE_COUNT]
            {
                "Please choose from one of the available network adapters. " +
                "Please note this adapter will be unavailable for the system for any other purpose until control is returned.",
                "Por favor, escoja uno de los adaptadores de red disponibles. " +
                "Tenga en cuenta que este adaptador no estará disponible para el sistema para ninguno otro propósito hasta que se devuelva el control.",
                "Bitte wählen Sie einen der verfügbaren Netzwerkadapter aus. " +
                "Bitte beachten Sie, dass dieser Adapter für das System für keinen anderen Zweck verfügbar ist, " +
                "bis die Kontrolle zurückgegeben wird.",
                "Por favor, escolha um dos adaptadores de rede disponíveis. " +
                "Observe que este adaptador ficará indisponível para o sistema para qualquer outra finalidade até que o controle seja retornado.",
                C,
                "Choisissez parmi l’une des adaptateur réseau disponibles. " +
                "Veuillez noter que cet adaptateur ne sera pas disponible pour le système à d'autres fins jusqu'à ce que le contrôle soit renvoyé." , 
                H } },
            { INSTRUCTIONS_WELCOME_COMMUNICATOR, new string[LANGUAGE_COUNT]
            {
                "To get things going, we need to connect to a communicator. " +
                "To do this you will need to plug the unit it into an open usb port on your PC. " +
                "Once attached Datam will show this as an available device in the tree to the left and " +
                "the communicator can now act as a bridge to connect this PC to the motor control device. " +
                "If the 'Automatic' option is selected " +
                "in the 'Device Scan' options menu located under:\n    Menu → Settings → Network\n" +
                "the communicator will automatically begin scanning for connected devices, " +
                "otherwise you will need to double click the communicator name in the tree to open the scan menu.",
                S,
                "Als erster Schritt muss der Kommunikator über einen freien USB Anschluss mit dem PC verbunden werden. " +
                "War die Verbindung erfolgreich wird der Kommunikator im Geräteregister auf der linken Seite angezeigt. " +
                "Durch einen Doppelklick auf das Kommunikator Symbol können Sie die manuelle Suche nach einen Heidrive Antrieb starten. " +
                "Unter:\n    Menü → Einstellungen → Netzwerk\nkann die Automatische Suchfunktion aktiviert werden.",
                P,
                C,
                F, 
                H 
            } },
            { INSTRUCTION_WELCOME_DEVICE, new string[LANGUAGE_COUNT]
            {
                "After Scanning for devices, the names of found devices will appear in the device tree " +
                "on the left. If the option 'Open Found Devices' is selected " +
                "in the 'Device Scan' options menu located under:\n    Menu → Settings → Network\n" +
                "then each device that is found " +
                "will be opened and the windows for these will appear in this display area. " +
                "If not, you will need to double click on the drive name in the device tree for this to happen.",
                S,
                "Nach der Geräte Suche, wird der Name der gefundenen Geräte im Geräteregister auf der linken Seite Angezeigt. " +
                "Mit einem Doppelklick öffnen Sie den gewünschten Antrieb. " +
                "Wenn die Option „Gefundene Geräte öffen“ unter:\n    Menü → Einstellungen → Netzwerk\naktiviert ist, " +
                "werden die gefunden Geräte automatisch geöffnet.",
                P,
                C,
                F, 
                H 
            } },
            #endregion

            #region Numbers
            { ONE, new string[LANGUAGE_COUNT] { ONE, "Uno", G, P, C, "Un" , H } },
            { TWO, new string[LANGUAGE_COUNT] { TWO, "Dos", G, P, C, "Deux" , H } },
            { THREE, new string[LANGUAGE_COUNT] { THREE, "Tres", G, P, C, "Trois" , H } },
            { FOUR, new string[LANGUAGE_COUNT] { FOUR, "Cuatro", G, P, C, "Quatre" , H } },
            { FIVE, new string[LANGUAGE_COUNT] { FIVE, "Cinco", G, P, C, "Cinq" , H } },
            { SIX, new string[LANGUAGE_COUNT] { SIX, "Seis", G, P, C, "Six" , H } },
            { SEVEN, new string[LANGUAGE_COUNT] { SEVEN, "Siete", G, P, C, "Sept" , H } },
            { EIGHT, new string[LANGUAGE_COUNT] { EIGHT, "Ocho", G, P, C, "Huit" , H } },
            { NINE, new string[LANGUAGE_COUNT] { NINE, "Neuve", G, P, C, "Neuf" , H } },
            { TEN, new string[LANGUAGE_COUNT] { TEN, "Diez", G, P, C, "Dix" , H } },
            { ELEVEN, new string[LANGUAGE_COUNT] { ELEVEN, "Once", G, P, C, "Onze" , H } },
            { TWELVE, new string[LANGUAGE_COUNT] { TWELVE, "Doce", G, P, C, "Douze" , H } },
            { THIRTEEN, new string[LANGUAGE_COUNT] { THIRTEEN, "Trece", G, P, C, "Treize" , H } },
            { FOURTEEN, new string[LANGUAGE_COUNT] { FOURTEEN, "Catorce", G, P, C, "Quatorze" , H } },
            { FIFTEEN, new string[LANGUAGE_COUNT] { FIFTEEN,"Quince", G, P, C, "Quinze" , H } },
            { SIXTEEN, new string[LANGUAGE_COUNT] { SIXTEEN, S, G, P, C, "Seize" , H } },
            { SEVENTEEN, new string[LANGUAGE_COUNT] { SEVENTEEN, "Diecisiete", G, P, C, "Dix-sept" , H } },
            { EIGHTEEN, new string[LANGUAGE_COUNT] { EIGHTEEN, "Dieciocho", G, P, C, "Dix-huit" , H } },
            { NINETEEN, new string[LANGUAGE_COUNT] { NINETEEN, "Diecinueve", G, P, C, "Dix-neuf" , H } },
            { TWENTY, new string[LANGUAGE_COUNT] { TWENTY, "Veinte", G, P, C, "Vingt" , H } },
            #endregion Numbers

            #region MessageBoxes Translations

            #region A
            { MESSAGE_ADD_DEVICE_NODE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_ADD_DEVICE_NODE,
                S,
                G,
                P,
                C,
                F
            , H } },
            { MESSAGE_ADD_NETWORK_NODE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_ADD_NETWORK_NODE,
                S,
                G,
                P,
                C,
                F
            , H } },
            { MESSAGE_ADD_TO_NETWORK_NODE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_ADD_TO_NETWORK_NODE,
                S,
                G,
                P,
                C,
                F
            , H } },
            #endregion /A

            #region B
            { MESSAGE_BEGIN_FACTORY_RESET, new string[LANGUAGE_COUNT]
            {
                MESSAGE_BEGIN_FACTORY_RESET,
                S,
                G,
                P,
                C,
                F
            , H } },
            { MESSAGE_BTN_PROCEED, new string[LANGUAGE_COUNT]
            {
                MESSAGE_BTN_PROCEED,
                S,
                G,
                P,
                C,
                F
            , H } },
            #endregion /B

            #region C
            { MESSAGE_CANT_ADD_CAPTURING, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CANT_ADD_CAPTURING,
                "¡No se puede añadir durante la captura!",
                "Kein Zugriff während er Aufzeichnung!",
                P,
                C,
                F
            , H } },
            { MESSAGE_CLEAR_CURRENT_CAPTURE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CLEAR_CURRENT_CAPTURE,
                "Esto borrará la captura actual. ¿Continuar?",
                "Die bestehende Aufzeichnung wird gelöscht. Fortfahren?",
                P,
                C,
                F
            , H } },
            { MESSAGE_CONFIGURATION_ADDED, new string[LANGUAGE_COUNT] {
                MESSAGE_CONFIGURATION_ADDED,
                S,
                "Die Konfiguration wurde hinzugefügt.",
                P,
                C,
                F
            , H } },
            { MESSAGE_CONFIGURATION_INVALID, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CONFIGURATION_INVALID,
                "El archivo de configuración seleccionado no era válido.",
                "Die geladene Konfiguration ist fehlerhaft.",
                P,
                C,
                F
            , H } },
            { MESSAGE_CONFIGURATION_UPDATED, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CONFIGURATION_UPDATED,
                S,
                "Die Konfiguration wurde upgedated",
                P,
                C,
                F
            , H } },
            { MESSAGE_CONFIGPRESENT, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CONFIGPRESENT,
                "La versión del archivo de configuración seleccionado ya está presente. ¿Quieres reemplazarlo?",
                "Die ausgewählte Konfigurationsdatei ist bereits geladen. Soll diese ersetzt werden?",
                P,
                C,
                F
            , H } },
            { MESSAGE_CONNECTIONLOST, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CONNECTIONLOST,
                "La conexión a {0} fue perdida. ¿Cerrar las ventanas asociadas?",
                "Die Verbindung zu {0} wurde unterbrochen, betreffende Fenster schließen?",
                P,
                C,
                F
            , H } },
            { MESSAGE_CONTROLWORD_DISABLE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CONTROLWORD_DISABLE,
                S,
                G,
                P,
                C,
                F
            , H } },
            { MESSAGE_CONTROLWORD_ENABLE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_CONTROLWORD_ENABLE,
                S,
                G,
                P,
                C,
                F
            , H } },
            #endregion /C

            #region D
            { MESSAGE_DATASHEETERROR, new string[LANGUAGE_COUNT]
            {
                MESSAGE_DATASHEETERROR,
                "Error de Hoja de Datos",
                "Datenblatt Fehler",
                P,
                C,
                F
            , H } },
            { MESSAGE_DATASHEETINVALID, new string[LANGUAGE_COUNT]
            {
                MESSAGE_DATASHEETINVALID,
                "¡Hoja de datos no es válida!",
                "Datenblatt ist fehlerhaft",
                P,
                C,
                F
            , H } },
            { MESSAGE_DEVICE_READY_CONNECT, new string[LANGUAGE_COUNT]
            {
                MESSAGE_DEVICE_READY_CONNECT,
                S,
                G,
                P,
                C,
                F
            , H } },
            { MESSAGE_DISCONNECT_WATCHDAWG, new string[LANGUAGE_COUNT]
            {
                MESSAGE_DISCONNECT_WATCHDAWG,
                S,
                G,
                P,
                C,
                F,
            H }
            },
            #endregion /D

            #region E
            { MESSAGE_EXISTING_CAPTURE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_EXISTING_CAPTURE,
                "La captura estática existente debe borrarse antes de que se puedan modificar las variables de ejes.",
                "Die aktuelles statische Anzeige muss gelöscht werden, bevor die Achsenvaribalen geändert werden können.",
                P,
                C,
                F
            , H } },
            #endregion E

            #region F
            { MESSAGE_FAILED_SAVE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_FAILED_SAVE,
                S,
                G,
                P,
                C,
                F
            , H } },
            { MESSAGE_FAULT_OCCURRED, new string[LANGUAGE_COUNT]
            {
                MESSAGE_FAULT_OCCURRED,
                "Fallo Ocurrido",
                "Fehler aufgetreten",
                P,
                C,
                F
            , H } },
            { MESSAGE_FAULT_ON_DEVICE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_FAULT_ON_DEVICE,
                "¿Se ha detectado un error en {0}. ¿Ir a la ventana de error?",
                "Fehler auf {0} gefunden, zur Fehleranzeige wechseln?",
                P,
                C,
                F
            , H } },
            #endregion /F

            #region I
            { MESSAGE_INVALIDPARAMS, new string[LANGUAGE_COUNT]
            {
                MESSAGE_INVALIDPARAMS,
                "¡Esta gráfica contiene parámetros que no se pueden capturar en el dispositivo conectado! Hay que eliminarlos. ¿Continuar cargando?",
                "Das Diagramm enthält Parameter, die auf dem angeschlossenen Gerät nicht geladen werden können! Diese müssen entfernt werden. Laden fortsetzen?",
                P,
                C,
                F
            , H } },
            #endregion I

            #region L
            { MESSAGE_LEFT_CLICK_SET, new string[LANGUAGE_COUNT]
            {
                MESSAGE_LEFT_CLICK_SET,
                S,
                G,
                P,
                C,
                F
            , H } },
            #endregion /L

            #region M
            { MESSAGE_MAXPARAMS, new string[LANGUAGE_COUNT]
            {
                MESSAGE_MAXPARAMS,
                "¡Máximo de cuatro parámetros por captura!",
                "Maximal vier Parameter pro Aufzeichnung",
                P,
                C,
                F
            , H } },
            { MESSAGE_MISSING_DATASHEET, new string[LANGUAGE_COUNT]
            {
                MESSAGE_MISSING_DATASHEET,
                S,
                G,
                P,
                C,
                F
            , H } },
            #endregion M

            #region N
            { MESSAGE_NEED_DEVICE_SELECT, new string[LANGUAGE_COUNT]
            {
                MESSAGE_NEED_DEVICE_SELECT,
                "Se debe seleccionar un dispositivo del árbol de dispositivos.\n" + "Esto se puede hacer haciendo doble clic en un elemento en el árbol." + " Si ninguno es avalable, conecte uno a un puerto USB disponible.",
                "Aus dem Gerätebaum muss ein Gerät ausgewählt werden.\n" + "Dies ist per Doppelklick auf das Gerät möglich." + " Falls kein Gerät verfügbar ist bitte die USB Verbindung prüfen. \n",
                P,
                C,
                F
            , H } },
            { MESSAGE_NET_CHANGE_REQ, new string[LANGUAGE_COUNT]
            {
                MESSAGE_NET_CHANGE_REQ,
                "Se necesita un cambio de estado de administración de red",
                "NMT-Zustandsänderung notwendig",
                P,
                C,
                F
            , H } },
            { MESSAGE_NET_MAN_STATE_CHANGE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_NET_MAN_STATE_CHANGE,
                "El estado de administración de red tendrá que cambiarse\n" + "a operativo para tener un efecto. ¿Hacer esto?",
                "Der Netzwerkmanagement Stauts muss auf operational geändert\n" + "werden, damit die Änderungen wirksam werden. Fortfahren?",
                P,
                C,
                F
            , H } },
            { MESSAGE_NOCAPPARAMS, new string[LANGUAGE_COUNT]
            {
                MESSAGE_NOCAPPARAMS,
                "¡No hay parámetros establecidos para capturar!",
                "Der Aufzeichnung sind keine Parameter zugewiesen ",
                P,
                C,
                F
            , H } },
            { MESSAGE_NO_OPERATING_MODES, new string[LANGUAGE_COUNT]
            {
                MESSAGE_NO_OPERATING_MODES,
                "Sin Modos de Funcionamiento",
                "Keine Betriebsmodi",
                P,
                C,
                F
            , H } },
            { MESSAGE_NO_OPERATING_MODES_FOUND, new string[LANGUAGE_COUNT]
            {
                MESSAGE_NO_OPERATING_MODES_FOUND,
                "No se han encontrado modos de funcionamiento para este dispositivo.",
                "Keine Betriebsmodi gefunden",
                P,
                C,
                F
            , H } },
            { MESSAGE_NOSAVEDADDRESS, new string[LANGUAGE_COUNT]
            {
                MESSAGE_NOSAVEDADDRESS,
                "Dispositivo {0} no tiene una configuración de dirección guardada",
                "Geräte: {0} hat keine gespeicherte Adresskonfiguration",
                P,
                C,
                F
            , H } },
            #endregion N

            #region P
            { MESSAGE_POWER_CYCLE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_POWER_CYCLE,
                "Ciclo de Energía",
                "KA bitte aus Kontext erschließen",
                P,
                C,
                F
            , H } },
            { MESSAGE_PREPARING_DEVICE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_PREPARING_DEVICE,
                S,
                G,
                P,
                C,
                F
            , H } },

            #endregion /P

            #region R
            { MESSAGE_REJECT_DATASHEET, new string[LANGUAGE_COUNT]
            {
                MESSAGE_REJECT_DATASHEET,
                "¿Está seguro de que desea rechazar la hoja de datos actual?",
                "Sind Sie sicher, das Sie das aktuelle Datenblatt verwerfen wollen?",
                P,
                C,
                F
            , H } },
            { MESSAGE_REMOVE_DATASHEET, new string[LANGUAGE_COUNT]
            {
                MESSAGE_REMOVE_DATASHEET,
                "¿Está seguro de que desea eliminar la hoja de datos seleccionada? Se eliminará permanentemente.",
                "Sind Sie sicher, das Sie das aktuelle Datenblatt löschen wollen?",
                P,
                C,
                F
            , H } },
            { MESSAGE_REPLACE_FAILED, new string[LANGUAGE_COUNT]
            {
                MESSAGE_REPLACE_FAILED,
                "¡Error en la operación de reemplazo!",
                "Der Änderungsvorgang ist fehlgeschlagen!",
                P,
                C,
                F
            , H } },
            { MESSAGE_REQUIRED_ADDRESS, new string[LANGUAGE_COUNT]
            {
                MESSAGE_REQUIRED_ADDRESS,
                "Configuración de Dirección Necesaria",
                "Erforderliche Adresskonfiguration",
                P,
                C,
                F
            , H } },
            { MESSAGE_RESET_DEVICE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_RESET_DEVICE,
                "¿Restablecer {0}?",
                "Reset {0}?",
                P,
                C,
                F
            , H } },
            { MESSAGE_RIGHT_CLICK_SET, new string[LANGUAGE_COUNT]
            {
                MESSAGE_RIGHT_CLICK_SET,
                S,
                G,
                P,
                C,
                F
            , H } },
            #endregion /R

            #region S
            { MESSAGE_SCAN_COMPLETE, new string[LANGUAGE_COUNT]
            {
                MESSAGE_SCAN_COMPLETE,
                S,
                G,
                P,
                C,
                F, 
                H 
            } },
            { MESSAGE_COMMUNICATOR_SCAN_DISABLED, new string[LANGUAGE_COUNT]
            {
                MESSAGE_COMMUNICATOR_SCAN_DISABLED,
                S,
                G,
                P,
                C,
                F
            , H } },
            { MESSAGE_WINDOW_NOT_SUPPORTED, new string[LANGUAGE_COUNT]
            {
                MESSAGE_WINDOW_NOT_SUPPORTED,
                "Este dispositivo no admite esta ventana y se cerrará para evitar problemas. Si cree que se trata de un error, compruebe que la configuración del dispositivo está cargada correctamente.",
                "Das Gerät unterstütz dieses Fenster nicht, und es wird zur Sicherheit geschlossen. Zur Überprüfung dieses Fehlers, prüfen Sie bitte ob die Gerätekonfiguration richtig geladen sit.",
                P,
                C,
                F
            , H } },
            #endregion /S

            #endregion /MessageBoxes Translations

            #endregion Sepcial Definition
        };
        #endregion /Translation Dictionary

        #region Dictionary Helpers
        /// <summary>
        ///  This method will make an entry for the dictionary.
        /// </summary>
        /// <param name=E></param>
        /// <param name=S></param>
        /// <param name=G></param>
        /// <param name=P></param>
        /// <param name=C></param>
        /// <param name=F></param>
        /// <returns></returns>
        private static KeyValuePair<String, String[]> Entry(String E, String S, String G, String P, String C, String F)
        {
            return new KeyValuePair<string, string[]>(E, Extensions_String.MakeStringArray(E, S, G, P, C, F));
        }
        #endregion /Dictionary Helpers

        #region Translation Getters

        #region General

        #region A
        public static string Abort
        {
            get => Find(ABORT);
        }
        public static string About
        {
            get => Find(ABOUT);
        }
        public static string AboveUpper
        {
            get => Find(ABOVE_UPPER);
        }
        public static string Absolute
        {
            get => Find(ABSOLUTE);
        }
        public static string Acceleration
        {
            get => Find(ACCELERATION);
        }
        public static string AccDeltaSpeed
        {
            get => Find(ACCEL_DELTA_SPEED);
        }
        public static string AccDeltaTime
        {
            get => Find(ACCEL_DELTA_TIME);
        }
        public static string AccLimit
        {
            get => Find(ACCEL_LIMIT);
        }
        public static string Accept
        {
            get => Find(ACCEPT);
        }
        public static string AcceptValues
        {
            get => Find(ACCEPT_VALUES);
        }
        public static string AccelerationLevel
        {
            get => Find(ACCEL_LEVEL);
        }
        public static string AccelerationNonNegative
        {
            get => Find(ACCEL_NONNEG);
        }
        public static string AccelerationUpdated
        {
            get => Find(ACCEL_UPDATED);
        }
        public static string Active
        {
            get => Find(ACTIVE);
        }
        public static string ActiveFaultCount
        {
            get => Find(ACTIVE_FAULT_COUNT);
        }
        public static string ActiveWarnCount
        {
            get => Find(ACTIVE_WARN_COUNT);
        }
        public static string ActualPosition
        {
            get => Find(ACTUAL_POSITION);
        }
        public static string ActualVelocity
        {
            get => Find(ACTUAL_VELOCITY);
        }
        public static string Add
        {
            get => Find(ADD);
        }
        public static string AddAxisParam
        {
            get => Find(ADD_AXIS_PARAMETER);
        }
        public static string AddedDatasheets
        {
            get => Find(ADDED_DATASHEETS);
        }
        public static string AddSerialNum
        {
            get => Find(ADD_SERIAL_NUMBER);
        }
        public static string AddUpdate
        {
            get => Find(ADD_UPDATE);
        }
        public static string AddUpdateDatasheet
        {
            get => Find(ADD_UPDATE_DATASHEET);
        }
        public static string AddUpdateDatasheetInstr
        {
            get => Find(ADD_UPDATE_DATASHEET_INSTR);
        }
        public static string Address
        {
            get => Find(ADDRESS);
        }
        public static string Advanced
        {
            get => Find(ADVANCED);
        }
        public static string Alignment
        {
            get => Find(ALIGNMENT);
        }
        public static string Allied
        {
            get => Find(ALLIED);
        }
        public static string Allnet
        {
            get => Find(ALLNET);
        }
        public static string AllowWindows
        {
            get => Find(ALLOW_WINDOWS);
        }
        public static string Analog
        {
            get => Find(ANALOG);
        }
        public static string AnalogInput1
        {
            get => Find(ANALOG_INPUT1);
        }
        public static string AnalogInput2
        {
            get => Find(ANALOG_INPUT2);
        }
        public static string Analyze
        {
            get => Find(ANALYZE);
        }
        public static string Application
        {
            get => Find(APPLICATION);
        }
        public static string AppSett
        {
            get => Find(APP_SETTINGS);
        }
        public static string Apply
        {
            get => Find(APPLY);
        }
        public static string AquiringConfigParams
        {
            get => Find(AQUIRING_CONFIG_PARAMS);
        }
        public static string AquiringManufacParams
        {
            get => Find(AQUIRING_MANUFAC_PARAMS);
        }
        public static string Assert
        {
            get => Find(ASSERT);
        }
        public static string AttachingCommunicator
        {
            get => Find(ATTACHING_A_COMMUNICATOR);
        }
        public static string AttemptConnect
        {
            get => Find(ATTEMPT_CONNECT);
        }
        public static string Authorized
        {
            get => Find(AUTHORIZED);
        }
        public static string Automatic
        {
            get => Find(AUTOMATIC);
        }
        public static string AutoAxis
        {
            get => Find(AUTOMATIC_AXIS);
        }
        public static string AutomaticCorrelationWeight
        {
            get => Find(AUTOMATIC_CORRELATION_WEIGHT);
        }
        public static string AvailDrives
        {
            get => Find(AVAILABLE_DRIVES);
        }
        public static string AverageTime
        {
            get => Find(AVERAGE_TIME);
        }
        public static string AwaitingTrigger
        {
            get => Find(AWAITING_TRIGGER);
        }
        public static string Axis
        {
            get => Find(AXIS);
        }
        public static string AxisScaling
        {
            get => Find(AXIS_SCALING);
        }
        #endregion A

        #region B
        public static string BackgroundColor
        {
            get => Find(BACKGROUND_COLOR);
        }
        public static string BadFileName
        {
            get => Find(BAD_FILE_NAME);
        }
        public static string BaudRate
        {
            get => Find(BAUD_RATE);
        }
        public static string BelowLower
        {
            get => Find(BELOW_LOWER);
        }
        public static string BinFileNotMatchFile
        {
            get => Find(BIN_FILE_NO_MATCH_FILE);
        }
        public static string BitDetection
        {
            get => Find(BIT_DETECTION);
        }
        public static string BitDetector
        {
            get => Find(BIT_DETECTOR);
        }
        public static string Bitfield
        {
            get => Find(BITFIELD);
        }
        public static string BitSelector
        {
            get => Find(BIT_SELECTOR);
        }
        public static string BitSelectorLow
        {
            get => Find(BIT_SELECTOR_LOW);
        }
        public static string Bottom
        {
            get => Find(BOTTOM);
        }
        public static string Brake
        {
            get => Find(BRAKE);
        }
        public static string BrowseConfigurationFiles
        {
            get => Find(BRAKE);
        }
        public static string Buffer
        {
            get => Find(BUFFER);
        }
        public static string Buffering
        {
            get => Find(BUFFERING);
        }
        public static string BuildDateTime
        {
            get => Find(BUILD_DATETIME);
        }
        #endregion B

        #region C
        public static String CalcGains
        {
            get => Find(CALCULATE_GAINS);
        }
        public static String Calculate
        {
            get => Find(CALCULATE);
        }
        public static String Calculating
        {
            get => Find(CALCULATING);
        }
        public static String Cancel
        {
            get => Find(CANCEL);
        }
        public static String CancelCapture
        {
            get => Find(CANCEL_CAPTURE);
        }
        public static String CaptureRange
        {
            get => Find(CAPTURE_RANGE);
        }
        public static String CaptureSize
        {
            get => Find(CAPTURE_SIZE);
        }
        public static String CaptureStatus
        {
            get => Find(CAPTURE_STATUS);
        }
        public static String Capture
        {
            get => Find(CAPTURE);
        }
        public static String Capturing
        {
            get => Find(CAPTURING);
        }
        public static String CaptureOnStart
        {
            get => Find(CAPTURE_ON_START);
        }
        public static String CenterPlot
        {
            get => Find(CENTER_PLOT);
        }
        public static String CheckConnection
        {
            get => Find(CHECK_CONNECTION);
        }
        public static String CheckIP
        {
            get => Find(CHECK_IP);
        }
        public static String Clear
        {
            get => Find(CLEAR);
        }
        public static String ClearingFlash
        {
            get => Find(CLEARING_FLASH);
        }
        public static String Close
        {
            get => Find(CLOSE);
        }
        public static String CloseApp
        {
            get => Find(CLOSE_APP);
        }
        public static String ClosedLoopFreq
        {
            get => Find(CLOSED_LOOP_FREQ);
        }
        public static String Code
        {
            get => Find(CODE);
        }
        public static String CoilInductance
        {
            get => Find(COIL_INDUCTANCE);
        }
        public static String CoilResistance
        {
            get => Find(COIL_RESISTANCE);
        }
        public static String CollatingData
        {
            get => Find(COLLATING_DATA);
        }
        public static String Color
        {
            get => Find(COLOR);
        }
        public static String ComboBoxError
        {
            get => Find(COMBOBOX_ERROR);
        }
        public static String Commands
        {
            get => Find(COMMANDS);
        }
        public static String CommandVal
        {
            get => Find(COMMAND_VAL);
        }
        public static String CommunicationFailed
        {
            get => Find(COMM_FAILED);
        }
        public static String Communicator
        {
            get => Find(COMMUNICATOR);
        }
        public static String CommunicatorDetection
        {
            get => Find(COMMUNICATOR_DETECTION);
        }
        public static String CommunicatorNoLongerDetected
        {
            get => Find(COMMUNICATOR_NOT_DETECTED);
        }
        public static String ConfigParams
        {
            get => Find(CONFIGURATION_PARAMS);
        }
        public static String ConfigParamsFile
        {
            get => Find(CONFIGURATION_PARAMS_FILE);
        }
        public static String ConfigScan
        {
            get => Find(CONFIGURATION_SCAN);
        }
        public static String Configuration
        {
            get => Find(CONFIGURATION);
        }
        public static String Configure
        {
            get => Find(CONFIGURE);
        }
        public static String ConfigSaveLoadTool
        {
            get => Find(CONFIG_SAVE_LOAD_TOOL);
        }
        public static String Connect
        {
            get => Find(CONNECT);
        }
        public static String ConnectAbort
        {
            get => Find(CONNECT_ABORT);
        }
        public static String ConnectFail
        {
            get => Find(CONNECT_FAIL);
        }
        public static String ConnectionDetection
        {
            get => Find(CONNECTION_DETECTION);
        }
        public static String ConnectSuccess
        {
            get => Find(CONNECT_SUCCESS);
        }
        public static String ConnectionUnderway
        {
            get => Find(CONNECT_UNDERWAY);
        }
        public static String ConnectingMotorDevice
        {
            get => Find(CONNECTING_MOTOR_DEVICE);
        }
        public static String Controller
        {
            get => Find(CONTROLLER);
        }
        public static String ControlledStop
        {
            get => Find(CONTROLLED_STOP);
        }
        public static String Controlword
        {
            get => Find(CONTROLWORD);
        }
        public static String CompareWithFile
        {
            get => Find(COMPARE_FILE);
        }
        public static String CompoundScaleWarning
        {
            get => Find(COMPOUND_WARNING);
        }
        public static String Completed
        {
            get => Find(COMPLETED);
        }
        public static String CompletedManufacData
        {
            get => Find(COMPLETED_MANUFAC_DATA);
        }
        public static String CompletedSerialUpdate
        {
            get => Find(COMPLETED_SERIAL_UPDATE);
        }
        public static String Copyright
        {
            get => Find(COPYRIGHT);
        }
        public static String Corr
        {
            get => Find(CORRELATION);
        }
        public static String Correspondance
        {
            get => Find(CORRESPONDANCE);
        }
        public static String CurrentActualValue
        {
            get => Find(CURRENT_ACTUAL_VALUE);
        }
        public static String CurrentActualValueInfo
        {
            get => Find(CURRENT_ACTUAL_VALUE_INFO);
        }
        public static String CurrentFirmwareInfo
        {
            get => Find(CURRENT_FIRMWARE_INFO);
        }
        public static String CurrentGainsCalculator
        {
            get => Find(CURRENT_GAINS_CALCULATOR);
        }
        public static String CurrentLim
        {
            get => Find(CURRENT_LIMIT);
        }
        public static String CurrState
        {
            get => Find(CURRENT_STATE);
        }
        public static String Cwu
        {
            get => Find(CWU);
        }
        public static String CycleTime
        {
            get => Find(CYCLE_TIME);
        }
        public static String CyclicSyncProfile
        {
            get => Find(CYCLIC_SYNC_PROFILE);
        }
        public static String CyclicSyncTorque
        {
            get => Find(CYCLIC_SYNC_TORQUE);
        }
        public static String CyclicSyncTorqueAngle
        {
            get => Find(CYCLIC_SYNC_TORQUE_ANGLE);
        }
        public static String CyclicSyncVel
        {
            get => Find(CYCLIC_SYNC_VELOCITY);
        }
        #endregion C

        #region D
        public static string DataCapture
        {
            get => Find(DATA_CAPTURE);
        }
        public static string DataDown
        {
            get => Find(DATA_DOWN);
        }
        public static string DataLink
        {
            get => Find(DATA_LINK);
        }
        public static string Datasheet
        {
            get => Find(DATASHEET);
        }
        public static string DatasheetAddUpdate
        {
            get => Find(DATASHEET_ADD_UPDATE);
        }
        public static string DatasheetVersion
        {
            get => Find(DATASHEET_VERSION);
        }
        public static string DatasheetDialog
        {
            get => Find(DATASHEET_DIALOG);
        }
        public static string DatasheetLoadingTool
        {
            get => Find(DATASHEET_LOADING_TOOL);
        }
        public static string DatasheetScan
        {
            get => Find(DATASHEET_SCAN);
        }
        public static string DatasheetScanComplete
        {
            get => Find(DATASHEET_SCAN_COMPLETE);
        }
        public static string DatasheetScanNoCandidates
        {
            get => Find(DATASHEET_SCAN_NO_CANDIDATES);
        }
        public static string DatasheetRemoval
        {
            get => Find(DATASHEET_REMOVAL);
        }
        public static string DatasheetScanInstr
        {
            get => Find(DATASHEET_SCAN_INSTR);
        }
        public static string DatasheetViewer
        {
            get => Find(DATASHEET_VIEWER);
        }
        public static string DCLinkVoltage
        {
            get => Find(DC_LINK_VOLTAGE);
        }
        public static string DCLinkVoltageInfo
        {
            get => Find(DC_LINK_VOLTAGE_INFO);
        }
        public static string Debug
        {
            get => Find(DEBUG);
        }
        public static string Deceleration
        {
            get => Find(DECEL);
        }
        public static string DecelLevel
        {
            get => Find(DECEL_LEVEL);
        }
        public static string DecelLimit
        {
            get => Find(DECEL_LIMIT);
        }
        public static string DecelerationNonNegative
        {
            get => Find(DECEL_NON_NEG);
        }
        public static string DecelerationUpdated
        {
            get => Find(DECEL_UPDATED);
        }
        public static string DecDeltaSpeed
        {
            get => Find(DECEL_DELTA_SPEED);
        }
        public static string DecDeltaTime
        {
            get => Find(DECEL_DELTA_TIME);
        }
        public static string Default
        {
            get => Find(DEFAULT);
        }
        public static string DefaultLayout
        {
            get => Find(DEFAULT_LAYOUT);
        }
        public static string DefaultWindows
        {
            get => Find(DEFAULT_WINDOWS);
        }
        public static string DelayTime
        {
            get => Find(DELAY_TIME);
        }
        public static string Delete
        {
            get => Find(DELETE);
        }
        public static string DeleteDatasheet
        {
            get => Find(DELETE_DATASHEET);
        }
        public static string Demand
        {
            get => Find(DEMAND);
        }
        public static string Device
        {
            get => Find(DEVICE);
        }
        public static string DeviceConfiguration
        {
            get => Find(DEVICE_CONFIG);
        }
        public static string DeviceDirect
        {
            get => Find(DEVICE_DIRECT);
        }
        public static string DeviceFound
        {
            get => Find(DEVICE_FOUND);
        }
        public static string DeviceScan
        {
            get => Find(DEVICE_SCAN);
        }
        public static string DeviceScanStart
        {
            get => Find(DEVICE_SCAN_START);
        }
        public static string DeviceUserSettings
        {
            get => Find(DEVICE_USER_SETTINGS);
        }
        public static string Diagnostics
        {
            get => Find(DIAGNOSTICS);
        }
        public static string Dialogue
        {
            get => Find(DIALOGUE);
        }
        public static string Digital
        {
            get => Find(DIGITAL);
        }
        public static string DigitalInputs
        {
            get => Find(DIGITAL_INPUTS);
        }
        public static string DigitaltoAnalogMapping
        {
            get => Find(DIGITAL_ANALOG_MAPPING);
        }
        public static string DigitalOutputs
        {
            get => Find(DIGITAL_OUTPUTS);
        }
        public static string Digitization
        {
            get => Find(DIGITIZATION);
        }
        public static string DigitizationSetPoint1Input
        {
            get => Find(DIGITIZATION_SET_POINT_1);
        }
        public static string DigitizationSetPoint2Input
        {
            get => Find(DIGITIZATION_SET_POINT_2);
        }
        public static string Dipl1
        {
            get => Find(DIPL1);
        }
        public static string Dipl2
        {
            get => Find(DIPL2);
        }
        public static string DirectCommand
        {
            get => Find(DIRECT_COMMAND);
        }
        public static string Disable
        {
            get => Find(DISABLE);
        }
        public static string DisableDrive
        {
            get => Find(DISABLE_DRIVE);
        }
        public static string Disabled
        {
            get => Find(DISABLED);
        }
        public static string DisabledMotorOp
        {
            get => Find(DISABLED_MOTOR_OP);
        }
        public static string Disconnect
        {
            get => Find(DISCONNECT);
        }
        public static string DisconnectWatchdawg
        {
            get => Find(DISCONNECT_WATCHDAWG);
        }
        public static string DisconnectionSuccess
        {
            get => Find(DISCONNECT_SUCCESS);
        }
        public static string Discovering
        {
            get => Find(DISCOVERING);
        }
        public static string Description
        {
            get => Find(DESCRIPTION);
        }
        public static string Displacement
        {
            get => Find(DISPLACEMENT);
        }
        public static string DisplacementUpdated
        {
            get => Find(DISPLACE_UPDATED);
        }
        public static string DockingArea
        {
            get => Find(DOCKING_AREA);
        }
        public static string Document
        {
            get => Find(DOCUMENT);
        }
        public static string DriveCommands
        {
            get => Find(DRIVE_COMMANDS);
        }
        public static string DriveTempLimit
        {
            get => Find(DRIVE_TEMP_NEAR_LIMIT);
        }
        #endregion D

        #region E
        public static string Edit
        {
            get => Find(EDIT);
        }
        public static string Elevated
        {
            get => Find(ELEVATED);
        }
        public static string Enabled
        {
            get => Find(ENABLED);
        }
        public static string EnableDebug
        {
            get => Find(ENABLE_DEBUG);
        }
        public static string EnableDisconTimer
        {
            get => Find(ENABLE_DISCON_TIMER);
        }
        public static string EnabledMotorOp
        {
            get => Find(ENABLED_MOTOR_OP);
        }
        public static string EnableDrive
        {
            get => Find(ENABLE_DRIVE);
        }
        public static string EnableOperation
        {
            get => Find(ENABLE_OPERATION);
        }
        public static string EnableRamp
        {
            get => Find(ENABLE_RAMP);
        }
        public static string EnableVoltage
        {
            get => Find(ENABLE_VOLTAGE);
        }
        public static string EncoderAlignmentStarted
        {
            get => Find(ENCODER_ALIGNMENT_STARTED);
        }
        public static string EncoderOrientationInfo
        {
            get => Find(ENCODER_ORIENTATION_INFO);
        }
        public static string EnteredPhasingMode
        {
            get => Find(ENTERED_PHASING_MODE);
        }
        public static string Emergency
        {
            get => Find(EMERGENCY);
        }
        public static string EmergencyMessages
        {
            get => Find(EMERGENCY_MESSAGES);
        }
        public static string EmergencyStop
        {
            get => Find(EMERGENCY_STOP);
        }
        public static string Error
        {
            get => Find(ERROR);
        }
        public static string ErrorMessages
        {
            get => Find(ERROR_MESSAGES);
        }
        public static string Err_ClosedUnex
        {
            get => Find(ERROR_CLOSE_UNEXPECTED);
        }
        public static string Err_ErrorCount
        {
            get => Find(ERROR_ERROR_COUNT);
        }
        public static string Err_Manufac_Param_Save_Fail
        {
            get => Find(ERROR_MANUFACT_PARAM_SAVE_FAIL);
        }
        public static string Error_DeviceNotSupported
        {
            get => Find(ERROR_DEVICE_NOT_SUPPORTED);
        }
        public static string Err_PanelNotSupported
        {
            get => Find(ERROR_PANEL_NOT_SUPPORTED);
        }
        public static string Err_Runtime
        {
            get => Find(ERROR_RUNTIME);
        }
        public static string ExistingDatasheets
        {
            get => Find(EXISTING_DATASHEETS);
        }
        public static string Exit
        {
            get => Find(EXIT);
        }
        public static string ExportCapture
        {
            get => Find(EXPORT_CAPTURE);
        }
        #endregion E

        #region F
        public static string FactoryReset
        {
            get => Find(FACTORY_RESET);
        }
        public static string Failure
        {
            get => Find(FAILURE);
        }
        public static string Fault
        {
            get => Find(FAULT);
        }
        public static string Faulted
        {
            get => Find(FAULTED);
        }
        public static string FaultHistory
        {
            get => Find(FAULT_HISTORY);
        }
        public static string FaultInj
        {
            get => Find(FAULT_INJECT);
        }
        public static string FaultOnAddress
        {
            get => Find(FAULT_ON_ADDRESS2);
        }
        public static string FaultReactionActive
        {
            get => Find(FAULT_REACTION_ACTIVE);
        }
        public static string FaultReset
        {
            get => Find(FAULT_RESET);
        }
        public static string FaultStatus
        {
            get => Find(FAULT_STATUS);
        }
        public static string File
        {
            get => Find(FILE);
        }
        public static string FileTrajectory
        {
            get => Find(FILE_TRAJECTORY);
        }
        public static string FillArea
        {
            get => Find(FILL_AREA);
        }
        public static string FilterText
        {
            get => Find(FILTER_TEXT);
        }
        public static string FinalS
        {
            get => Find(FINAL_S);
        }
        public static string FindFile
        {
            get => Find(FIND_FILE);
        }
        public static string FindMagEncOffset
        {
            get => Find(FIND_MAG_ENC_OFFSET);
        }
        public static string FirmwareLoadingTool
        {
            get => Find(FIRMWARE_LOADING_TOOL);
        }
        public static string FirmwareNoUpdate
        {
            get => Find(FIRMWARE_NO_UPDATE);
        }
        public static string FirmwareUpload
        {
            get => Find(FIRMWARE_UPLOAD);
        }
        public static string FirmwareUpdateSuccess
        {
            get => Find(FIRMWARE_UPDATE_SUCCESS);
        }
        public static string FirmwareVersion
        {
            get => Find(FIRMWARE_VERSION);
        }
        public static string Font
        {
            get => Find(FONT);
        }
        public static string FontSize
        {
            get => Find(FONT_SIZE);
        }
        public static string ForcePowerReset
        {
            get => Find(FORCE_POWER_RESET);
        }
        public static string ForegroundColor
        {
            get => Find(FOREGROUND_COLOR);
        }

        #endregion F

        #region G

        public static string GaugePanel
        {
            get => Find(GAUGE_PANEL);
        }
        public static string Gauge
        {
            get => Find(GAUGE);
        }
        public static string Gauges
        {
            get => Find(GAUGES);
        }
        public static string GaugeListSettings
        {
            get => Find(GAUGE_LIST_SETTINGS);
        }
        public static string GaugeSettings
        {
            get => Find(GAUGE_SETT);
        }
        public static string General
        {
            get => Find(GENERAL);
        }
        public static string GetCapture
        {
            get => Find(GET_CAPTURE);
        }
        public static string GlobalUserLevel
        {
            get => Find(GLOBAL_USER_LEVEL);
        }
        public static string PanelGPIO
        {
            get => Find(GPIO_PANEL);
        }
        public static string GraphColors
        {
            get => Find(GRAPH_COLORS);
        }
        public static string GraphSett
        {
            get => Find(GRAPH_SETTINGS);
        }
        public static string GridColor
        {
            get => Find(GRID_COLOR);
        }
        public static string Gridline
        {
            get => Find(GRIDLINE);
        }
        public static string Gridlines
        {
            get => Find(GRIDLINES);
        }
        public static string Group
        {
            get => Find(GROUP);
        }
        #endregion G

        #region H
        public static string Halt
        {
            get => Find(HALT);
        }
        public static string HaltOption
        {
            get => Find(HALT_OPTION);
        }
        public static string Help
        {
            get => Find(HELP);
        }
        public static string HideDeviceTree
        {
            get => Find(HIDE_DEVICE_TREE);
        }
        public static string HideStatusBar
        {
            get => Find(HIDE_STATUS_BAR);
        }
        public static string High
        {
            get => Find(HIGH);
        }
        public static string History
        {
            get => Find(HISTORY);
        }
        public static string Homing
        {
            get => Find(HOMING);
        }
        #endregion H

        #region I
        public static string IconsProvidedBy
        {
            get => Find(ICONS_PROVIDED_BY);
        }
        public static string Idle
        {
            get => Find(IDLE);
        }
        public static string IgnoreConnectionLoss
        {
            get => Find(IGNORE_CONNECTION_LOSS);
        }
        public static string Immediate
        {
            get => Find(IMMEDIATE);
        }
        public static string InertialObserver
        {
            get => Find(INERTIAL_OBSERVER);
        }
        public static string Info
        {
            get => Find(INFO);
        }
        public static string Information
        {
            get => Find(INFORMATION);
        }
        public static string Initialization
        {
            get => Find(INITIALIZATION);
        }
        public static string InitialS
        {
            get => Find(INITIAL_S);
        }
        public static string InputSettings
        {
            get => Find(INPUT_SETTINGS);
        }
        public static string InputSource
        {
            get => Find(INPUT_SOURCE);
        }
        public static string InputValue
        {
            get => Find(INPUT_VALUE);
        }
        public static string Input1
        {
            get => Find(INPUT_1);
        }
        public static string Input2
        {
            get => Find(INPUT_2);
        }
        public static string Input3
        {
            get => Find(INPUT_3);
        }
        public static string Input4
        {
            get => Find(INPUT_4);
        }
        public static string Input5
        {
            get => Find(INPUT5);
        }
        public static string Instr
        {
            get => Find(INSTRUCTIONS);
        }
        public static string InstrCGC
        {
            get => Find(INSTRUCTIONS_CGC);
        }
        public static string InstrConfigurationsSaveLoadTool
        {
            get => Find(INSTRUCTIONS_CONFIGURATIONS_SAVE_LOAD_TOOL);
        }
        public static string InstrConfigurationsSaveInstr
        {
            get => Find(INSTRUCTIONS_CONFIGURATIONS_SAVE_INSTR);
        }
        public static string InstrDatasheetAddUpdate
        {
            get => Find(INSTRUCTIONS_DATASHEET_ADD_UPDATE);
        }
        public static string InstrDatasheetDialog
        {
            get => Find(INSTRUCTIONS_DATASHEET_DIALOG);
        }
        public static string InstrDatasheetRemove
        {
            get => Find(INSTRUCTIONS_DATASHEET_REMOVE);
        }
        public static string InstrDatasheetScan
        {
            get => Find(INSTRUCTIONS_DATASHEET_SCAN);
        }
        public static string InstrDevUserSettings
        {
            get => Find(INSTRUCTIONS_DEV_USER_SETTINGS);
        }
        public static string InstrFirmwareLoading
        {
            get => Find(INSTRUCTIONS_FIRMWARE_LOADING);
        }
        public static string InstrLoadManufactLabel
        {
            get => Find(INSTRUCTIONS_LOAD_MANUFACT_LABEL);
        }
        public static string InstrMoveAxes
        {
            get => Find(INSTRUCTIONS_MOVE_AXES);
        }
        public static string InstrPhasingMode
        {
            get => Find(INSTRUCTIONS_PHASING_MODE);
        }
        public static string InstrSelAdapt
        {
            get => Find(INSTRUCTIONS_SEL_ADAPT);
        }
        public static string InstrWelcomeCommunicator
        {
            get => Find(INSTRUCTIONS_WELCOME_COMMUNICATOR);
        }
        public static string InstrWelcomeDevice
        {
            get => Find(INSTRUCTION_WELCOME_DEVICE);
        }
        public static string InterpolatedPos
        {
            get => Find(INTERPOLATED_POS);
        }
        public static string Interval
        {
            get => Find(INTERVAL);
        }
        public static string Invalid
        {
            get => Find(INVALID);
        }
        public static string Io
        {
            get => Find(IO);
        }
        public static string IOLinker
        {
            get => Find(IO_LINKER);
        }
        public static string AddIP
        {
            get => Find(IP_ADD);
        }
        public static string IPScan
        {
            get => Find(IP_SCAN);
        }
        public static string Input
        {
            get => Find(INPUT);
        }
        public static string Inputs
        {
            get => Find(INPUTS);
        }
        #endregion I

        #region J
        public static string JerkLevel
        {
            get => Find(JERK_LEVEL);
        }
        public static string NegativeJerk
        {
            get => Find(JERK_NEG);
        }
        public static string JerkNegUpdated
        {
            get => Find(JERK_NEG_UPDATED);
        }
        public static string JerkNonNegative
        {
            get => Find(JERK_NON_NEG);
        }
        public static string PositiveJerk
        {
            get => Find(JERK_POS);
        }
        public static string JerkPosUpdated
        {
            get => Find(JERK_POS_UPDATED);
        }
        public static string Jog
        {
            get => Find(JOG);
        }

        public static string JogDemand
        {
            get => Find(JOG_DEMAND);
        }

        public static string JogLeftButton
        {
            get => Find(JOG_LEFT_BUTTON);
        }

        public static string JogRightButton
        {
            get => Find(JOG_RIGHT_BUTTON);
        }

        public static string JogTime
        {
            get => Find(JOG_TIME);
        }

        public static string JogTimeCheck
        {
            get => Find(JOG_TIME_CHECK);
        }
        public static string JsonFormatError
        {
            get => Find(JSON_FORMAT_ERROR);
        }
        #endregion J

        #region L
        public static string Language
        {
            get => Find(LANGUAGE);
        }
        public static string LastCommands
        {
            get => Find(LAST_COMMANDS);
        }
        public static string Left
        {
            get => Find(LEFT);
        }
        public static string LeftAxis
        {
            get => Find(LEFT_AXIS);
        }
        public static string LeftAxisVar
        {
            get => Find(LEFT_AXIS_VAR);
        }
        public static string Level
        {
            get => Find(LEVEL);
        }
        public static string LinearRamp
        {
            get => Find(LINEAR_RAMP);
        }
        public static string LivePosition
        {
            get => Find(LIVE_POSITION);
        }
        public static string LiveUpdate
        {
            get => Find(LIVE_UPDATE);
        }
        public static string LimitActive
        {
            get => Find(LIMIT_ACTIVE);
        }
        public static string Link
        {
            get => Find(LINK);
        }
        public static string Load
        {
            get => Find(LOAD);
        }
        public static string LoadingComplete
        {
            get => Find(LOADING_COMPLETE);
        }
        public static string LoadingDataCapture
        {
            get => Find(LOADING_DATA_CAPTURE);
        }

        public static string LoadingParameterPanel
        {
            get => Find(LOADING_PARAMETER_PANEL);
        }
        public static string LoadCapture
        {
            get => Find(LOAD_CAPTURE);
        }
        public static string LoadConfig
        {
            get => Find(LOAD_CONFIG);
        }
        public static string LoadConfigInstr
        {
            get => Find(LOAD_CONFIG_INSTR);
        }
        public static string LoadFirmwareInstr
        {
            get => Find(LOAD_FIRMWARE_INSTR);
        }
        public static string LoadFromFile
        {
            get => Find(LOAD_FROM_FILE);
        }
        public static string Loading
        {
            get => Find(LOADING);
        }
        public static string LoadingDatasheets
        {
            get => Find(LOADING_DATASHEETS);
        }
        public static string LoadingManufacConfigParam
        {
            get => Find(LOADING_MANUFAC_CONFIG_PARAM);
        }
        public static string LoadLimit
        {
            get => Find(I2T_LOAD_LIMITING_WARN);
        }
        public static string LoadingYourData
        {
            get => Find(LOADING_YOUR_DATA);
        }
        public static string LoadLabelData
        {
            get => Find(LOAD_LABEL_DATA);
        }
        public static string LoadingState
        {
            get => Find(LOADING_STATE);
        }
        public static string LoadMotions
        {
            get => Find(LOAD_MOTIONS);
        }
        public static string LoadParamAddress
        {
            get => Find(LOAD_PARAM_ADDRESS);
        }
        public static string LoadPanel
        {
            get => Find(LOAD_PANEL);
        }
        public static string Locked
        {
            get => Find(LOCKED);
        }
        public static string LockLeftAxis
        {
            get => Find(LOCK_LEFT_AXIS);
        }
        public static string LockOpen
        {
            get => Find(LOCK_OPEN);
        }
        public static string LockRightAxis
        {
            get => Find(LOCK_RIGHT_AXIS);
        }
        public static string LockTimeAxis
        {
            get => Find(LOCK_TIME_AXIS);
        }
        public static string Logs
        {
            get => Find(LOGS);
        }
        public static string LogDisplay
        {
            get => Find(LOG_DISPLAY);
        }
        public static string LogFileName
        {
            get => Find(LOG_FILE_NAME);
        }
        public static string LogFilePath
        {
            get => Find(LOG_FILE_PATH);
        }
        public static string LogFileType
        {
            get => Find(LOG_FILE_TYPE);
        }
        public static string LogMessage
        {
            get => Find(LOG_MESSAGE);
        }
        public static string LogSett
        {
            get => Find(LOG_SETTINGS);
        }
        public static string LogSize
        {
            get => Find(LOG_SIZE);
        }
        public static string Logging
        {
            get => Find(LOGGING);
        }
        public static string Low
        {
            get => Find(LOW);
        }
        #endregion L

        #region M
        public static string MagAlignOffest
        {
            get => Find(MAGNETIC_ALIGNMENT_OFFSET);
        }
        public static string Magnitude
        {
            get => Find(MAGNITUDE);
        }
        public static string Maintain
        {
            get => Find(MAINTAIN);
        }
        public static string Major
        {
            get => Find(MAJOR);
        }
        public static string ManufacLabel
        {
            get => Find(MANUFACTURER_LABEL);
        }
        public static string ManufacLabelData
        {
            get => Find(MANUFACTURER_LABEL_DATA);
        }
        public static string ManufacLabelParams
        {
            get => Find(MANUFACTURER_LABEL_PARAMS);
        }
        public static string Manufacturer
        {
            get => Find(MANUFACTURER);
        }
        public static string ManufacParamSaved
        {
            get => Find(MANUFACTURER_PARAM_SAVED);
        }
        public static string ManufacturerSpecific
        {
            get => Find(MANUFACTURER_SPECIFIC);
        }
        public static string MappingI0
        {
            get => Find(MAPPING_I0);
        }
        public static string MarqeeScroll
        {
            get => Find(MARQEE_SCROLL);
        }
        public static string MatchCase
        {
            get => Find(MATCHCASE);
        }
        public static string Maximum
        {
            get => Find(MAXIMUM);
        }
        public static string MaximumAcceleration
        {
            get => Find(MAXIMUM_ACCELERATION);
        }
        public static string MaxCurrent
        {
            get => Find(MAX_CURRENT);
        }
        public static string MaxCurrentInfo
        {
            get => Find(MAX_CURRENT_INFO);
        }
        public static string MaimumxDeceleration
        {
            get => Find(MAX_DECEL);
        }
        public static string MaxMotorSpeed
        {
            get => Find(MAX_MOTOR_SPEED);
        }
        public static string MaxProfileVel
        {
            get => Find(MAX_PROFILE_VELOCITY);
        }
        public static string MaxTorque
        {
            get => Find(MAX_TORQUE);
        }
        public static string MaxTorqueInfo
        {
            get => Find(MAX_TORQUE_INFO);
        }
        public static string MaxVel
        {
            get => Find(MAX_VELOCITY);
        }
        public static string Menu
        {
            get => Find(MENU);
        }
        public static string Messages
        {
            get => Find(MESSAGES);
        }
        public static string Minimum
        {
            get => Find(MINIMUM);
        }
        public static string MinimumLogLevel
        {
            get => Find(MINIMUM_LOG_LEVEL);
        }
        public static string Minor
        {
            get => Find(MINOR);
        }
        public static string MinVel
        {
            get => Find(MIN_VELOCITY);
        }
        public static string ModAxisParams
        {
            get => Find(MODIFY_AXIS_PARAMS);
        }
        public static string Mode
        {
            get => Find(MODE);
        }
        public static string ModeOfOp
        {
            get => Find(MODE_OF_OPERATION);
        }
        public static string Modified
        {
            get => Find(MODIFIED);
        }
        public static string MoreArea
        {
            get => Find(MORE_AREA);
        }
        public static string Motion
        {
            get => Find(MOTION);
        }
        public static string Motions
        {
            get => Find(MOTIONS);
        }
        public static string MotionControl
        {
            get => Find(MOTION_CONTROL);
        }
        public static string MotionController
        {
            get => Find(MOTION_CONTROLLER);
        }
        public static string MotionControllerWelcome
        {
            get => Find(MOTION_CONTROLLER_WELCOME);
        }
        public static string MotionCreation
        {
            get => Find(MOTION_CREATION);
        }
        public static string MotionPlot
        {
            get => Find(MOTION_PLOT);
        }
        public static string MotionProfileType
        {
            get => Find(MOTION_PROFILE_TYPE);
        }
        public static string MotionSchedule
        {
            get => Find(MOTION_SCHEDULE);
        }
        public static string MotionType
        {
            get => Find(MOTION_TYPE);
        }
        public static string Motor
        {
            get => Find(MOTOR);
        }
        public static string MotorController
        {
            get => Find(MOTOR_CONTROLLER);
        }
        public static string MotorInMotion
        {
            get => Find(MOTOR_IN_MOTION);
        }
        public static string MotorRatedCurrent
        {
            get => Find(MOTOR_RATED_CURRENT);
        }
        public static string MotorRatedCurrentInfo
        {
            get => Find(MOTOR_RATED_CURRENT_INFO);
        }
        public static string MotorRatedTorque
        {
            get => Find(MOTOR_RATED_TORQUE);
        }
        public static string MotorRatedTorqueInfo
        {
            get => Find(MOTOR_RATED_TORQUE_INFO);
        }
        public static string MotorTempLimit
        {
            get => Find(MOTOR_TEMP_NEAR_LIMIT);
        }
        public static string Move
        {
            get => Find(MOVE);
        }
        public static string MoveAxes
        {
            get => Find(MOVE_AXES);
        }
        public static string MoveImmediate
        {
            get => Find(MOVE_IMMEDIATE);
        }
        public static string MoveScheduled
        {
            get => Find(MOVE_SCHEDULED);
        }
        public static string MoveToLeft
        {
            get => Find(MOVE_TO_LEFT);
        }
        public static string MoveToRight
        {
            get => Find(MOVE_TO_RIGHT);
        }
        public static string Multiplier
        {
            get => Find(MULTIPLIER);
        }
        #endregion M

        #region N
        public static string Name
        {
            get => Find(NAME);
        }
        public static string NamedStimPanel
        {
            get => Find(NAMED_STIM_PANEL);
        }
        public static string Negative
        {
            get => Find(NEG);
        }
        public static string NegTorqueInfo
        {
            get => Find(NEG_TORQUE_INFO);
        }
        public static string NegTorqueLim
        {
            get => Find(NEG_TORQUE_LIMIT);
        }
        public static string NetManagementState
        {
            get => Find(NET_MANAGEMENT_STATE);
        }
        public static string Network
        {
            get => Find(NETWORK);
        }
        public static string NetworkAdapt
        {
            get => Find(NETWORK_ADAPT);
        }
        public static string New
        {
            get => Find(NEW);
        }
        public static string NextFile
        {
            get => Find(NEXT_FILE);
        }
        public static string No
        {
            get => Find(NO);
        }
        public static string NoCapture
        {
            get => Find(NO_CAPTURE);
        }
        public static string NodeID
        {
            get => Find(NODE_ID);
        }
        public static string NoDevices
        {
            get => Find(NO_DEVICES);
        }
        public static string NoModeSel
        {
            get => Find(NO_MODE_SELECTED);
        }
        public static string NonCapturableParams
        {
            get => Find(NONCAPTURABLE_PARAMS);
        }
        public static string None
        {
            get => Find(NONE);
        }
        public static string NotConnected
        {
            get => Find(NOT_CONNECTED);
        }
        public static string Notes
        {
            get => Find(NOTES);
        }
        public static string NotFaulted
        {
            get => Find(NOT_FAULTED);
        }
        public static string NoDeviceFound
        {
            get => Find(NO_DEVICE_FOUND);
        }

        public static string NotReadySwitchon
        {
            get => Find(NOT_READY_SWITCH_ON);
        }
        public static string NoVariables
        {
            get => Find(NO_VARIABLES);
        }
        public static string NumGauges
        {
            get => Find(NUMBER_GAUGES);
        }
        public static string NumLogs
        {
            get => Find(NUMBER_LOGS);
        }
        #endregion N

        #region O
        public static string Off
        {
            get => Find(OFF);
        }
        public static string Offset
        {
            get => Find(OFFSET);
        }
        public static string Ok
        {
            get => Find(OK);
        }
        public static string On
        {
            get => Find(ON);
        }
        public static string OpenAlliedDevices
        {
            get => Find(OPEN_ALLIED_DEVICES);
        }
        public static string OpenFoundDevices
        {
            get => Find(OPEN_FOUND_DEVICES);
        }
        public static string OpenClosedLoop
        {
            get => Find(OPEN_CLOSED_LOOP);
        }
        public static string OpenLoopGain
        {
            get => Find(OPEN_LOOP_GAIN);
        }
        public static string OpenLoopPhase
        {
            get => Find(OPEN_LOOP_PHASE);
        }
        public static string OpenLoopVoltage
        {
            get => Find(OPEN_LOOP_VOLTAGE);
        }
        public static string OpeningLog
        {
            get => Find(OPENING_LOG);
        }
        public static string Operational
        {
            get => Find(OPERATIONAL);
        }
        public static string OperationModeSpecific
        {
            get => Find(OPERATION_MODE_SPECIFIC);
        }
        public static string OperationDisabled
        {
            get => Find(OPERATION_DISABLED);
        }
        public static string OperationEnabled
        {
            get => Find(OPERATION_ENABLED);
        }
        public static string Options
        {
            get => Find(OPTIONS);
        }
        public static string OutputBitLogicHigh
        {
            get => Find(OUTPUT_BIT_LOGIC_HIGH);
        }
        public static string OutputBitLogicLow
        {
            get => Find(OUTPUT_BIT_LOGIC_LOW);
        }
        public static string OutputSettings
        {
            get => Find(OUTPUT_SETTINGS);
        }
        public static string OutputSource
        {
            get => Find(OUTPUT_SOURCE);
        }
        public static string OutputValue
        {
            get => Find(OUTPUT_VALUE);
        }
        public static string Output1
        {
            get => Find(OUTPUT1);
        }
        public static string Output2
        {
            get => Find(OUTPUT2);
        }
        public static string Output3
        {
            get => Find(OUTPUT3);
        }
        public static string OutsideSPs
        {
            get => Find(OUTSIDE_SPS);
        }

        public static string Override
        {
            get => Find(OVERRIDE);
        }

        public static string Overvoltage
        {
            get => Find(OVER_VOLTAGE);
        }
        public static string Output
        {
            get => Find(OUTPUT);
        }

        #endregion O

        #region P
        public static string Parameter
        {
            get => Find(PARAMETER);
        }
        public static string Param1
        {
            get => Find(PARAM1);
        }
        public static string Param2
        {
            get => Find(PARAM2);
        }
        public static string Param3
        {
            get => Find(PARAM3);
        }
        public static string Param4
        {
            get => Find(PARAM4);
        }
        public static string ParameterPanel
        {
            get => Find(PARAMETER_PANEL);
        }
        public static string ParamDir
        {
            get => Find(PARAM_DIRECTOR);
        }
        public static string Password
        {
            get => Find(PASSWORD);
        }
        public static string Pdol1
        {
            get => Find(PDOL1);
        }
        public static string Pdol2
        {
            get => Find(PDOL2);
        }
        public static string Pending
        {
            get => Find(PENDING);
        }
        public static string Phasing
        {
            get => Find(PHASING);
        }
        public static string PhasingModeInstr
        {
            get => Find(PHASING_MODE_INSTR);
        }
        public static string PidNotValid
        {
            get => Find(PID_NOT_VALID);
        }
        public static string PleaseRebootDevice
        {
            get => Find(PLEASE_REBOOT_DEVICE);
        }
        public static string Plot
        {
            get => Find(PLOT);
        }
        public static string POBSender
        {
            get => Find(POB_SENDER);
        }
        public static string Point
        {
            get => Find(POINT);
        }
        public static string Points
        {
            get => Find(POINTS);
        }
        public static string PointsToCap
        {
            get => Find(POINTS_TO_CAPTURE);
        }
        public static string Polarity
        {
            get => Find(POLARITY);
        }
        public static string Port
        {
            get => Find(PORT);
        }
        public static string Positive
        {
            get => Find(POS);
        }
        public static string PositionDemand
        {
            get => Find(POSITION_DEMAND);
        }
        public static string PosSpinOrientation
        {
            get => Find(POS_SPIN_ORIENTATION);
        }
        public static string Post
        {
            get => Find(POST);
        }
        public static string PosTorqueInfo
        {
            get => Find(POS_TORQUE_INFO);
        }
        public static string PosTorqueLim
        {
            get => Find(POS_TORQUE_LIMIT);
        }
        public static string PostTrig
        {
            get => Find(POST_TRIGGER);
        }
        public static string PostTrig_25
        {
            get => Find(POST_TRIGGER_25);
        }
        public static string PostTrig_50
        {
            get => Find(POST_TRIGGER_50);
        }
        public static string PostTrig_75
        {
            get => Find(POST_TRIGGER_75);
        }
        public static string Pre
        {
            get => Find(PRE);
        }
        public static string PreOp
        {
            get => Find(PRE_OPERATIONAL);
        }
        public static string PreparingData
        {
            get => Find(PREPARING_DATA);
        }
        public static string PreTrigger
        {
            get => Find(PRE_TRIGGER);
        }
        public static string Preview
        {
            get => Find(PREVIEW);
        }
        public static string Previous
        {
            get => Find(PREVIOUS);
        }
        public static string PreviousFile
        {
            get => Find(PREVIOUS_FILE);
        }
        public static string Proceed
        {
            get => Find(PROCEED);
        }
        public static string ProductName
        {
            get => Find(PRODUCT_NAME);
        }
        public static string ProfileAcc
        {
            get => Find(PROFILE_ACCEL);
        }
        public static string ProfileDataFile
        {
            get => Find(PROFILE_DATA_FILE);
        }
        public static string ProfileDec
        {
            get => Find(PROFILE_DECEL);
        }
        public static string ProfilePos
        {
            get => Find(PROFILE_POSITION);
        }
        public static string ProfileTorque
        {
            get => Find(PROFILE_TORQUE);
        }
        public static string ProfileVelocity
        {
            get => Find(PROFILE_VELOCITY);
        }
        #endregion P

        #region Q
        public static string QueueClear
        {
            get => Find(QUEUE_CLEAR);
        }
        public static string QuickStop
        {
            get => Find(QUICK_STOP);
        }
        public static string QuickStopActive
        {
            get => Find(QUICK_STOP_ACTIVE);
        }
        public static string QuickStopDec
        {
            get => Find(QUICK_STOP_DECEL);
        }
        public static string QuickStopRamp
        {
            get => Find(QUICK_STOP_RAMP);
        }
        #endregion Q

        #region R
        public static string Reactions
        {
            get => Find(REACTIONS);
        }
        public static string ReadingBinFile
        {
            get => Find(READING_BIN_FILE);
        }
        public static string ReadingFlashInfo
        {
            get => Find(READING_FLASH_INFO);
        }
        public static string ReadySwitchOn
        {
            get => Find(READY_SWITCH_ON);
        }
        public static string RebootingDevice
        {
            get => Find(REBOOTING_DEVICE);
        }
        public static string Received
        {
            get => Find(RECEIVED);
        }
        public static string ReceivedMessages
        {
            get => Find(RECEIVED_MESSAGES);
        }
        public static string Redo
        {
            get => Find(REDO);
        }
        public static string ReferenceRamp
        {
            get => Find(REFERENCE_RAMP);
        }
        public static string Reject
        {
            get => Find(REJECT);
        }
        public static string Relative
        {
            get => Find(RELATIVE);
        }
        public static string Remote
        {
            get => Find(REMOTE);
        }
        public static string Remove
        {
            get => Find(REMOVE);
        }
        public static string RemoveDatasheet
        {
            get => Find(REMOVE_DATASHEET);
        }
        public static string RemoveDatasheetInstr
        {
            get => Find(REMOVE_DATASHEET_INSTR);
        }
        public static string ReplaceConfig
        {
            get => Find(REPLACE_CONFIG);
        }
        public static string Reserved
        {
            get => Find(RESERVED);
        }
        public static string Reset
        {
            get => Find(RESET);
        }
        public static string ResetDatam
        {
            get => Find(RESET_DATAM);
        }
        public static string ResetFaults
        {
            get => Find(RESET_FAULTS);
        }
        public static string ResetNeeded
        {
            get => Find(RESET_NEEDED);
        }
        public static string ResetOptions
        {
            get => Find(RESET_OPTIONS);
        }
        public static string ResetUnderway
        {
            get => Find(RESET_UNDERWAY);
        }
        public static string ResetWarnings
        {
            get => Find(RESET_WARNINGS);
        }
        public static string ResponseNotValid
        {
            get => Find(RESPONSE_NOT_VALID);
        }
        public static string Result
        {
            get => Find(RESULT);
        }
        public static string RetrievingData
        {
            get => Find(RETRIEVING_DATA);
        }
        public static string RevisionNumber
        {
            get => Find(REVISION_NUMBER);
        }
        public static string Right
        {
            get => Find(RIGHT);
        }
        public static string RightAxis
        {
            get => Find(RIGHT_AXIS);
        }
        public static string RightAxisVar
        {
            get => Find(RIGHT_AXIS_VAR);
        }
        public static string Rms
        {
            get => Find(RMS);
        }
        public static string Run
        {
            get => Find(RUN);
        }
        public static string Rwu
        {
            get => Find(RWU);
        }
        #endregion R

        #region S
        public static string SafeTorqueOff
        {
            get => Find(SAFE_TORQUE_OFF);
        }
        public static string Safety
        {
            get => Find(SAFETY);// HURRY!!!! FIND SAFETY!!!!!!
        }
        public static string Save
        {
            get => Find(SAVE);
        }
        public static string SaveConfig
        {
            get => Find(SAVE_CONFIG);
        }
        public static string SaveConfigInstr
        {
            get => Find(SAVE_CONFIG_INSTR);
        }
        public static string SavedPass
        {
            get => Find(SAVED_PASS);
        }
        public static string SaveFlash
        {
            get => Find(SAVE_FLASH);
        }
        public static string SaveLayout
        {
            get => Find(SAVE_LAYOUT);
        }
        public static string SaveMotionsFile
        {
            get => Find(SAVE_MOTIONS_FILE);
        }
        public static string SaveResetHistogram
        {
            get => Find(SAVE_RESET_HISTOGRAM);
        }
        public static string SaveParameterState
        {
            get => Find(SAVE_PARAMETER_STATE);
        }
        public static string SaveToFile
        {
            get => Find(SAVE_TO_FILE);
        }
        public static string SavingConfigParams
        {
            get => Find(SAVING_CONFIG_PARAMS);
        }
        public static string Scale
        {
            get => Find(SCALE);
        }
        public static string Scan
        {
            get => Find(SCAN);
        }
        public static string Scanning
        {
            get => Find(SCANNING);
        }
        public static string ScanForCommunicators
        {
            get => Find(SCAN_COMMUNICATORS);
        }
        public static string ScanCompleted
        {
            get => Find(SCAN_COMPLETED);
        }
        public static string ScanForDatasheets
        {
            get => Find(SCAN_FOR_DATASHEETS);
        }
        public static string ScanForDevicesOn
        {
            get => Find(SCAN_FOR_DEVICES_ON);
        }
        public static string ScanningDevicesALLNET
        {
            get => Find(SCANNING_DEVICES_ALLNET);
        }
        public static string ScanningDevicesCommunicator
        {
            get => Find(SCANNING_DEVICES_COMMUNICATOR);
        }
        public static string ScanInit
        {
            get => Find(SCAN_INIT);
        }
        public static string ScanSettings_CAN
        {
            get => Find(SCAN_SETTINGS_CAN);
        }
        public static string ScanPeriod
        {
            get => Find(SCAN_PERIOD);
        }
        public static string ScanProgress
        {
            get => Find(SCAN_PROGRESS);
        }
        public static string Schedule
        {
            get => Find(SCHEDULE);
        }
        public static string SciNot
        {
            get => Find(SCIENTIFIC_NOTATION);
        }
        public static string Search
        {
            get => Find(SEARCH);
        }
        public static string SearchFromNode
        {
            get => Find(SEARCH_FROM_NODE);
        }
        public static string SearchToNode
        {
            get => Find(SEARCH_TO_NODE);
        }
        public static string SecondsAfter
        {
            get => Find(SECONDS_AFTER);
        }
        public static string Select
        {
            get => Find(SELECT);
        }
        public static string SelectAdapt
        {
            get => Find(SELECT_ADAPT);
        }
        public static string SelectLogFileLocation
        {
            get => Find(SELECT_LOG_FILE_LOCATION);
        }
        public static string SelectedFirmInfo
        {
            get => Find(SELECTED_FIRM_INFO);
        }
        public static string SelectedFirmFileLocation
        {
            get => Find(SELECTED_FIRM_FILE_LOCATION);
        }
        public static string SelectedLinker
        {
            get => Find(SELECTED_LINKER);
        }
        public static string SelectDev
        {
            get => Find(SELECT_DEVICE);
        }
        public static string Sent
        {
            get => Find(SENT);
        }
        public static string SentMessages
        {
            get => Find(SENT_MESSAGES);
        }
        public static string Send
        {
            get => Find(SEND);
        }
        public static string SensorSelectionCode
        {
            get => Find(SENSOR_SELECTION_CODE);
        }
        public static string SerialNumber
        {
            get => Find(SERIAL_NUMBER);
        }
        public static string Set
        {
            get => Find(SET);
        }
        public static string SetPoint1
        {
            get => Find(SETPOINT1);
        }
        public static string SetPoint2
        {
            get => Find(SETPOINT2);
        }
        public static string Settings
        {
            get => Find(SETTINGS);
        }
        public static string SettingUp
        {
            get => Find(SETTING_UP);
        }
        public static string Setup
        {
            get => Find(SETUP);
        }
        public static string ShowAll
        {
            get => Find(SHOW_ALL);
        }
        public static string ShowAllText
        {
            get => Find(SHOW_ALL_TEXT);
        }
        public static string ShowDatasheet
        {
            get => Find(SHOW_DATASHEET);
        }
        public static string ShowDeviceTree
        {
            get => Find(SHOW_DEVICE_TREE);
        }
        public static string ShowHistory
        {
            get => Find(SHOW_HISTORY);
        }
        public static string ShowParams
        {
            get => Find(SHOW_HIDDEN_PARAMS);
        }
        public static string ShowStatusBar
        {
            get => Find(SHOW_STATUS_BAR);
        }
        public static string ShowWindows
        {
            get => Find(SHOW_WINDOWS);
        }
        public static string Shutdown
        {
            get => Find(SHUTDOWN);
        }
        public static string ShutdownOnDisconnect
        {
            get => Find(SHUTDOWN_ON_DISCONNECT);
        }
        public static string Signal
        {
            get => Find(SIGNAL);
        }
        public static string Signals
        {
            get => Find(SIGNALS);
        }
        public static string SignalSelection
        {
            get => Find(SIGNAL_SELECTION);
        }
        public static string SlowDownRamp
        {
            get => Find(SLOW_DOWN_RAMP);
        }
        public static string SoftwareIdentificationNotUpdated
        {
            get => Find(SOFTWARE_IDENTIFICATION_NOT_UPDATED);
        }
        public static string SourcePanel
        {
            get => Find(SOURCE_PANEL);
        }
        public static string Spin
        {
            get => Find(SPIN);
        }
        public static string Standard
        {
            get => Find(STANDARD);
        }
        public static string Start
        {
            get => Find(START);
        }
        public static string StartCapture
        {
            get => Find(START_CAPTURE);
        }
        public static string StartupScanningOptions
        {
            get => Find(STARTUP_SCANNING_OPTIONS);
        }
        public static string StartMax
        {
            get => Find(START_MAXIMIZED);
        }
        public static string StartTime
        {
            get => Find(START_TIME);
        }
        public static string Status
        {
            get => Find(STATUS);
        }
        public static string StatusBar
        {
            get => Find(STATUS_BAR);
        }
        public static string StatusComplete
        {
            get => Find(STATUS_COMPLETE);
        }
        public static string StatusInProgress
        {
            get => Find(STATUS_IN_PROGRESS);
        }
        public static string StatusMonitor
        {
            get => Find(STATUS_MONITOR);
        }
        public static string StatusNotRun
        {
            get => Find(STATUS_NOT_RUN);
        }
        public static string StatusPanel
        {
            get => Find(STATUS_PANEL);
        }
        public static string Statusword
        {
            get => Find(STATUSWORD);
        }
        public static string Stimulus
        {
            get => Find(STIMULUS);
        }
        public static string Stop
        {
            get => Find(STOP);
        }
        public static string StopImmediate
        {
            get => Find(STOP_IMMEDIATE);
        }
        public static string Stopped
        {
            get => Find(STOPPED);
        }
        public static string StopScheduled
        {
            get => Find(STOP_SCHEDULED);
        }
        public static string Success
        {
            get => Find(SUCCESS);
        }
        public static string SwitchAxis
        {
            get => Find(SWITCH_AXIS);
        }
        public static string SwitchedOn
        {
            get => Find(SWITCHED_ON);
        }
        public static string SwitchOnEnableOperation
        {
            get => Find(SWITCH_ON_ENABLE_OPERATION);
        }
        public static string SwitchOn
        {
            get => Find(SWITCH_ON);
        }

        public static string SwitchOnDisabled
        {
            get => Find(SWITCH_ON_DISABLED);
        }
        public static string System
        {
            get => Find(SYSTEM);
        }
        #endregion S

        #region T
        public static string TargetPosition
        {
            get => Find(TARGET_POSITION);
        }
        public static string TargetReached
        {
            get => Find(TARGET_REACHED);
        }
        public static string TargetTorque
        {
            get => Find(TARGET_TORQUE);
        }
        public static string TargetTorqueInfo
        {
            get => Find(TARGET_TORQUE_INFO);
        }
        public static string TargetVel
        {
            get => Find(TARGET_VELOCITY);
        }
        public static string Test
        {
            get => Find(TEST);
        }
        public static string TestLogGen
        {
            get => Find(TEST_LOG_GENERATION);
        }
        public static string Theme
        {
            get => Find(THEME);
        }
        public static string Time
        {
            get => Find(TIME);
        }
        public static string TimeBase
        {
            get => Find(TIME_BASE);
        }
        public static string TimedOutWaitingForResponse
        {
            get => Find(TIMED_OUT_WAITING_FOR_RESPONSE);
        }
        public static string TimeWithUnit
        {
            get => Find(TIME_W_UNIT);
        }
        public static string Tools
        {
            get => Find(TOOLS);
        }
        public static string TooManyParams
        {
            get => Find(TOO_MANY_PARAMETERS);
        }
        public static string ToNode
        {
            get => Find(TO_NODE_ID);
        }
        public static string Top
        {
            get => Find(TOP);
        }
        public static string Torque
        {
            get => Find(TORQUE);
        }
        public static string TorqueActualValue
        {
            get => Find(TORQUE_ACTUAL_VALUE);
        }
        public static string TorqueActualValueInfo
        {
            get => Find(TORQUE_ACTUAL_VALUE_INFO);
        }
        public static string TorqueDemand
        {
            get => Find(TORQUE_DEMAND);
        }
        public static string TorqueDemandInfo
        {
            get => Find(TORQUE_DEMAND_INFO);
        }
        public static string TorqueProfile
        {
            get => Find(TORQUE_PROFILE);
        }
        public static string TorqueProfileInfo
        {
            get => Find(TORQUE_PROFILE_INFO);
        }
        public static string TorqueSlope
        {
            get => Find(TORQUE_SLOPE);
        }
        public static string TorqueSlopeInfo
        {
            get => Find(TORQUE_SLOPE_INFO);
        }
        public static string Trajectory
        {
            get => Find(TRAJECTORY);
        }
        public static string TrigBetween
        {
            get => Find(TRIGGER_BETWEEN);
        }
        public static string TrigOutside
        {
            get => Find(TRIGGER_OUTSIDE);
        }
        public static string TriggerPosition
        {
            get => Find(TRIGGER_POSITION);
        }
        public static string TriggerSettings
        {
            get => Find(TRIGGER_SETTINGS);
        }
        public static string Troubleshooting
        {
            get => Find(TROUBLESHOOTING);
        }
        public static string Type
        {
            get => Find(TYPE);
        }
        public static string TypeofMotion
        {
            get => Find(TYPE_OF_MOTION);
        }
        #endregion T

        #region U
        public static string UdpMulticast
        {
            get => Find(UDP_MULTICAST);
        }
        public static string UnableToChangeBaud
        {
            get => Find(UNABLE_TO_CHANGE_BAUD);
        }
        public static string UnableReadBin
        {
            get => Find(UNABLE_READ_BIN);
        }
        public static string Unavailable
        {
            get => Find(UNAVAILABLE);
        }
        public static string UncommutatedCurrent
        {
            get => Find(UNCOMMUTATED_CURR);
        }
        public static string Undo
        {
            get => Find(UNDO);
        }
        public static string UnidentifiedDevice
        {
            get => Find(UNIDENTIFIED_DEVICE);
        }
        public static string Uninitialized
        {
            get => Find(UNINITIALIZED);
        }
        public static string Unit
        {
            get => Find(UNIT);
        }
        public static string Unknown
        {
            get => Find(UNKNOWN);
        }
        public static string UnknownState
        {
            get => Find(UNKNOWN_STATE);
        }
        public static string Unlocked
        {
            get => Find(UNLOCKED);
        }
        public static string Unlink
        {
            get => Find(UNLINK);
        }
        public static string UnlinkAll
        {
            get => Find(UNLINK_ALL);
        }
        public static string UnlinkOnChange
        {
            get => Find(UNLINK_ON_CHANGE);
        }
        public static string UnlockRamp
        {
            get => Find(UNLOCK_RAMP);
        }
        public static string UpdatingFirmware
        {
            get => Find(UPDATING_FIRMWARE);
        }
        public static string UpdatingFirmwareFailed
        {
            get => Find(UPDATING_FIRMWARE_FAILED);
        }
        public static string Use
        {
            get => Find(USE);
        }
        public static string UseMulticast
        {
            get => Find(USE_MULTICAST);
        }
        public static string UserAuth
        {
            get => Find(USER_AUTHORIZATION);
        }
        public static string UserLevel
        {
            get => Find(USER_LEVEL);
        }
        public static string UserLevelInfo
        {
            get => Find(USER_LEVEL_INFO);
        }
        public static string UserOutput
        {
            get => Find(USER_OUTPUT);
        }
        public static string UserSettings
        {
            get => Find(USER_SETTINGS);
        }
        public static string UpdateRate
        {
            get => Find(UPDATE_RATE);
        }
        public static string UploadFile
        {
            get => Find(UPLOAD_FILE);
        }
        #endregion U

        #region V
        public static string Validate
        {
            get => Find(VALIDATE);
        }
        public static string Variable
        {
            get => Find(VARIABLE);
        }
        public static string Value
        {
            get => Find(VALUE);
        }
        public static string Velocity
        {
            get => Find(VELOCITY);
        }
        public static string VelocityActualValue
        {
            get => Find(VELOCITY_ACTUAL_VALUE);
        }
        public static string VelDemand
        {
            get => Find(VELOCITY_DEMAND);
        }
        public static string VelocityLimit
        {
            get => Find(VELOCITY_LIMIT);
        }
        public static string VelocityLevel
        {
            get => Find(VELOCITY_LEVEL);
        }
        public static string VelThreshold
        {
            get => Find(VELOCITY_THRESHOLD);
        }
        public static string VelThresholdTime
        {
            get => Find(VELOCITY_THRESHOLD_TIME);
        }
        public static string VelocityUpdated
        {
            get => Find(VELOCITY_UPDATED);
        }
        public static string VelWindow
        {
            get => Find(VELOCITY_WINDOW);
        }
        public static string VelWindowTime
        {
            get => Find(VELOCITY_WINDOW_TIME);
        }
        public static string Verbose
        {
            get => Find(VERBOSE);
        }
        public static string Version
        {
            get => Find(VERSION);
        }
        public static string VFModeFreq
        {
            get => Find(VF_MODE_FREQUENCY);
        }
        public static string Virtual
        {
            get => Find(VIRTUAL);
        }
        public static string Visible
        {
            get => Find(VISIBLE);
        }
        public static string VoltageDisabled
        {
            get => Find(VOLTAGE_DISABLED);
        }
        public static string VoltageEnabled
        {
            get => Find(VOLTAGE_ENABLED);
        }
        public static string VoltageLim
        {
            get => Find(VOLTAGE_LIMIT);
        }
        #endregion V

        #region W
        public static string WaitTargetReached
        {
            get => Find(WAIT_TARGET_REACHED);
        }
        public static string Warning
        {
            get => Find(WARNING);
        }
        public static string WarningHistory
        {
            get => Find(WARNING_HISTORY);
        }
        public static string Welcome
        {
            get => Find(WELCOME);
        }
        public static string WelcomeToDatam
        {
            get => Find(WELCOME_TO_DATAM);
        }
        public static string Windows
        {
            get => Find(WINDOWS);
        }
        public static string WithinSPs
        {
            get => Find(WITHIN_SPS);
        }
        public static string WritingFlashFile
        {
            get => Find(WRITING_FLASH_FILE);
        }
        #endregion W

        #region Y
        public static string Yes
        {
            get => Find(YES);
        }
        #endregion Y

        #region Z
        public static string ZeroTargetParams
        {
            get => Find(ZERO_TARGET_PARAMS);
        }
        #endregion Z

        #endregion General

        #region Special

        #region Numbers
        public static string One
        {
            get => Find(ONE);
        }
        public static string Two
        {
            get => Find(TWO);
        }
        public static string Three
        {
            get => Find(THREE);
        }
        public static string Four
        {
            get => Find(FOUR);
        }
        public static string Five
        {
            get => Find(FIVE);
        }
        public static string Six
        {
            get => Find(SIX);
        }
        public static string Seven
        {
            get => Find(SEVEN);
        }
        public static string Eight
        {
            get => Find(EIGHT);
        }
        public static string Nine
        {
            get => Find(NINE);
        }
        public static string Ten
        {
            get => Find(TEN);
        }
        public static string Eleven
        {
            get => Find(ELEVEN);
        }
        public static string Twelve
        {
            get => Find(TWELVE);
        }
        public static string Thirteen
        {
            get => Find(THIRTEEN);
        }
        public static string Fourteen
        {
            get => Find(FOURTEEN);
        }
        public static string Fifteen
        {
            get => Find(FIFTEEN);
        }
        public static string Sixteen
        {
            get => Find(SIXTEEN);
        }
        public static string Seventeen
        {
            get => Find(SEVENTEEN);
        }
        public static string Eighteen
        {
            get => Find(EIGHTEEN);
        }
        public static string Nineteen
        {
            get => Find(NINETEEN);
        }
        public static string Twenty
        {
            get => Find(TWENTY);
        }

        #endregion Numbers

        #region Messages

        public static string Message_AddingDeviceNode
        {
            get => Find(MESSAGE_ADD_DEVICE_NODE);
        }
        public static string Message_AddingNetworkNode
        {
            get => Find(MESSAGE_ADD_NETWORK_NODE);
        }
        public static string Message_AddingToNetworkNode
        {
            get => Find(MESSAGE_ADD_TO_NETWORK_NODE);
        }
        public static string Msg_BeginFactoryReset
        {
            get => Find(MESSAGE_BEGIN_FACTORY_RESET);
        }
        public static string Msg_BtnProceed
        {
            get => Find(MESSAGE_BTN_PROCEED);
        }
        public static string Msg_CantAdd
        {
            get => Find(MESSAGE_CANT_ADD_CAPTURING);
        }
        public static string Msg_ClearCurrentCapture
        {
            get => Find(MESSAGE_CLEAR_CURRENT_CAPTURE);
        }
        public static string Msg_CurrentFirmwareInfo
        {
            get => Find(MESSAGE_CURRENT_FIRMWARE_INFO);
        }
        public static string Message_CommunicatorScanDisabled
        {
            get => Find(MESSAGE_COMMUNICATOR_SCAN_DISABLED);
        }
        public static string Msg_ConfigurationAdded
        {
            get => Find(MESSAGE_CONFIGURATION_ADDED);
        }
        public static string Msg_ConfigurationInvalid
        {
            get => Find(MESSAGE_CONFIGURATION_INVALID);
        }
        public static string Msg_ConfigurationUpdated
        {
            get => Find(MESSAGE_CONFIGURATION_UPDATED);
        }
        public static string Msg_ConfigPresent
        {
            get => Find(MESSAGE_CONFIGPRESENT);
        }
        public static string Msg_ConnectionLost
        {
            get => Find(MESSAGE_CONNECTIONLOST);
        }
        public static string Message_ControlwordDisable
        {
            get => Find(MESSAGE_CONTROLWORD_DISABLE);
        }
        public static string Message_ControlwordEnable
        {
            get => Find(MESSAGE_CONTROLWORD_ENABLE);
        }
        public static string Msg_DatasheetError
        {
            get => Find(MESSAGE_DATASHEETERROR);
        }
        public static string Msg_DatasheetInvalid
        {
            get => Find(MESSAGE_DATASHEETINVALID);
        }
        public static string Msg_DeviceInvalid
        {
            get => Find(MESSAGE_DEVICEINVALID);
        }
        public static string Message_DeviceReadyToConnect
        {
            get => Find(MESSAGE_DEVICE_READY_CONNECT);
        }
        public static string Msg_DisconnectWatchdawg
        {
            get => Find(MESSAGE_DISCONNECT_WATCHDAWG);
        }
        public static string Msg_ExistingCapture
        {
            get => Find(MESSAGE_EXISTING_CAPTURE);
        }
        public static string Msg_FailedSave
        {
            get => Find(MESSAGE_FAILED_SAVE);
        }
        public static string Msg_FaultOccurred
        {
            get => Find(MESSAGE_FAULT_OCCURRED);
        }
        public static string Msg_FaultOnDevice
        {
            get => Find(MESSAGE_FAULT_ON_DEVICE);
        }
        public static string Msg_InvalidParams
        {
            get => Find(MESSAGE_INVALIDPARAMS);
        }

        #region L
        public static string Message_LeftClickSet
        {
            get => Find(MESSAGE_LEFT_CLICK_SET);
        }
        #endregion /L

        public static string Msg_MaxParams
        {
            get => Find(MESSAGE_MAXPARAMS);
        }
        public static string Message_MissingDatasheet
        {
            get => Find(MESSAGE_MISSING_DATASHEET);
        }
        public static string Msg_NeedDevSelect
        {
            get => Find(MESSAGE_NEED_DEVICE_SELECT);
        }
        public static string Msg_NetChangeReq
        {
            get => Find(MESSAGE_NET_CHANGE_REQ);
        }
        public static string Msg_NetManStateChange
        {
            get => Find(MESSAGE_NET_MAN_STATE_CHANGE);
        }
        public static string Msg_NoCapParams
        {
            get => Find(MESSAGE_NOCAPPARAMS);
        }
        public static string Msg_NoOperatingModes
        {
            get => Find(MESSAGE_NO_OPERATING_MODES);
        }
        public static string Msg_NoOperatingModesFound
        {
            get => Find(MESSAGE_NO_OPERATING_MODES_FOUND);
        }
        public static string Msg_NoSavedAddress
        {
            get => Find(MESSAGE_NOSAVEDADDRESS);
        }
        public static string Msg_PowerCycle
        {
            get => Find(MESSAGE_POWER_CYCLE);
        }
        public static string Message_PreparingDevice
        {
            get => Find(MESSAGE_PREPARING_DEVICE);
        }

        #region R
        public static string Msg_RejectDatasheet
        {
            get => Find(MESSAGE_REJECT_DATASHEET);
        }
        public static string Msg_RemoveDatasheet
        {
            get => Find(MESSAGE_REMOVE_DATASHEET);
        }
        public static string Msg_ReplaceFailed
        {
            get => Find(MESSAGE_REPLACE_FAILED);
        }
        public static string Msg_RequiredAddress
        {
            get => Find(MESSAGE_REQUIRED_ADDRESS);
        }
        public static string Msg_ResetDevice
        {
            get => Find(MESSAGE_RESET_DEVICE);
        }
        public static string Message_RightClickSet
        {
            get => Find(RIGHT);
        }
        #endregion /R

        #region S
        public static string Message_ScanCompleted
        {
            get => Find(MESSAGE_SCAN_COMPLETE);
        }
        public static string Msg_WindowNotSupported
        {
            get => Find(MESSAGE_WINDOW_NOT_SUPPORTED);
        }
        #endregion /S

        #endregion Messages

        #region Dynamic
        //frmMain dynamic label lblCurrentStatus
        public static string CapturedPoints(uint at, uint outOf, string additionalInfo = "")
        {
            if (String.IsNullOrWhiteSpace(additionalInfo))
            {
                return String.Format(Find(CAPTURED_POINTS), at, outOf);
            }
            else
            {
                return String.Format(Find(CAPTURED_POINTS) + CAPTURED_POINTS_APPEND, at, outOf, additionalInfo);
            }
        }
        public static string ConfigParam_LoadFail_Dynamic(String deviceName)
        {
            return String.Format(Find(CONFIGPARAM_LOADFAIL_DYNAMIC), deviceName);
        }
        public static string ConnectedTo_Dynamic(String deviceName)
        {
            return String.Format(Find(CONNECTED_TO_X_DYNAMIC), deviceName);
        }
        public static string Datasheet_Removal_Dynamic(String deviceName)
        {
            return String.Format(Find(DATASHEET_REMOVAL_DYNAMIC), deviceName);
        }
        public static string Date_Dynamic(String deviceName)
        {
            return String.Format(Find(DATE_DYNAMIC), deviceName);
        }
        public static string Encoder_Offset_Dynamic(String deviceName)
        {
            return String.Format(Find(ENCODER_OFFSET_DYNAMIC), deviceName);
        }
        public static string EstimatedTime(Double remaininingSeconds)
        {
            return String.Format(Find(ESTIMATED_TIME), remaininingSeconds);
        }
        public static string Existing_Baudrate_Dynamic(String deviceName)
        {
            return String.Format(Find(EXISTING_BAUDRATE_DYNAMIC), deviceName);
        }
        public static string Existing_Node_ID_Dynamic(String deviceName)
        {
            return String.Format(Find(EXISTING_NODE_ID_DYNAMIC), deviceName);
        }
        public static string Failure_Address_Dynamic(String deviceName)
        {
            return String.Format(Find(FAILURE_ADDRESS_DYNAMIC), deviceName);
        }
        public static string FoundDevice(int foundCount, String deviceName, uint NodeID, uint serialNumber)
        {
            return String.Format(Find(FOUND_DEVICE), foundCount, deviceName, NodeID, serialNumber);
        }
        public static string Gathered(Double at, Double outOf)
        {
            return String.Format(Find(GATHERED), at, outOf);
        }
        public static string Json_Format_Eror_Line_Dynamic(long line)
        {
            return String.Format(Find(JSON_FORMAT_ERROR_LINE_DYNAMIC), line);
        }
        public static string Json_Format_Eror_Line_Column_Dynamic(long line, long column)
        {
            return String.Format(Find(JSON_FORMAT_ERROR_LINE_COLUMN_DYNAMIC), line, column);
        }
        public static string Json_Format_Eror_Path_Line_Column_Dynamic(String path, long line, long column)
        {
            return String.Format(Find(JSON_FORMAT_ERROR_PATH_LINE_COLUMN_DYNAMIC), path, line, column);
        }
        public static string Json_Format_Eror_Path_Line_Dynamic(String path, long line)
        {
            return String.Format(Find(JSON_FORMAT_ERROR_PATH_LINE_DYNAMIC), path, line);
        }
        public static string Json_Format_Eror_Path_Dynamic(String path)
        {
            return String.Format(Find(JSON_FORMAT_ERROR_PATH_DYNAMIC), path);
        }
        public static string Output_File_Dynamic(String deviceName)
        {
            return String.Format(Find(OUTPUT_FILE_DYNAMIC), deviceName);
        }
        public static string Operation_ConfigManufac_Dynamic(String deviceName, String config)
        {
            return String.Format(Find(OPERATION_CONFIGMANUFAC_DYNAMIC), deviceName, config);
        }
        public static string Path_Dynamic(String deviceName)
        {
            return String.Format(Find(PATH_DYNAMIC), deviceName);
        }
        public static string Param_Reset_Attempt_Dynamic(String deviceName, String valueCast, String valueDefault)
        {
            return String.Format(Find(PARAM_RESET_ATTEMPT_DYNAMIC), deviceName, valueCast, valueDefault);
        }
        public static string Node_Default_Dynamic(String deviceName)
        {
            return String.Format(Find(NODE_DEFAULT_DYNAMIC), deviceName);
        }
        public static string Time_Dynamic(String deviceName)
        {
            return String.Format(Find(TIME_DYNAMIC), deviceName);
        }
        public static string Version_Dynamic(String deviceName)
        {
            return String.Format(Find(VERSION_DYNAMIC), deviceName);
        }
        public static string DeviceStateChange_NMT
        {
            get => Find(DEVICE_NMT_STATE_CHANGE);
        }
        public static string ReticulatingSplines // SC2k
        {
            get => Find(RETICULATING_SPLINES);
        }
        public static string ValidPath
        {
            get => Find(VALID_PATH);
        }
        public static string View
        {
            get => Find(VIEW);
        }
        #endregion Dynamic

        #endregion Special

        #endregion Translation Getters

        #region Utility Functions

        /// <summary>
        /// This method will do a safe lookup for a translation in
        /// the local dictionary. If the value exists it will be returned,
        /// else the word is returned untranslated.
        /// </summary>
        /// <param name="languageSetting"></param>
        /// <param name="word"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public static Boolean Find(Int32 languageSetting, String word, out String translation)
        {
#if DEBUG
            Log_Manager.LogMethodCall("TranslationManager", nameof(Find));
#endif
            if (Words.TryLookup(word, out string[] translations))
            {
                if (languageSetting < translations.Length)
                {
                    translation = translations[languageSetting];
                    return true;
                }
            }
            translation = word;
            return false;
        }
        /// <summary>
        /// This method takes in a word and returns the proper translation based on the value of currentLanguage setting if it exists.
        /// If it doesn't exist, the word is returned untranslated.
        /// </summary>
        /// <param name="word">Word to translate</param>
        /// <returns></returns>
        public static String Find(String word)
        {
#if DEBUG
            Log_Manager.LogMethodCall("TranslationManager", nameof(Find));
#endif
            if (Words.TryLookup(word, out string[] translations))
            {
                if(translations != null)
                {
                    string result = translations[CurrentLanguageIndex];
                    if (result == UnknownTokens[CurrentLanguageIndex])
                    {// If we dont actually have a translation return the english option
                        result = translations[0];
                    }
                    return result;
                }
            }
            return word;
        }
        /// <summary>
        /// This method takes in a word and a specific language integer and returns the proper translation based on the value of the given language integer if it exists.
        /// If it doesn't exist, the word is returned untranslated.
        /// </summary>
        /// <param name="word">Word to translate</param>
        /// <param name="language">Language code of the language to translate to</param>
        /// <returns></returns>
        public static String Find(String word, Int32 language)
        {
#if DEBUG
            Log_Manager.LogMethodCall("TranslationManager", nameof(Find));
#endif
            if (Words.TryLookup(word, out string[] translations))
            {
                if (translations != null)
                {
                    return translations[language];
                }
            }
            return word;
        }

        /// <summary>
        /// This method takes in a word and returns a string array of all the available translations of that word.
        /// If the word can't be translated, an empty string array is returned
        /// </summary>
        /// <param name="word">Word to retrive translations for</param>
        /// <returns>String array with every translation of a word</returns>
        public static String[] FindTranslations(string word)
        {
#if DEBUG
            Log_Manager.LogVerbose("TranslationManager", "The function FindComboBoxTranslations was called");
#endif
            if (Words.TryLookup(word, out string[] translations))
            {
                if (translations != null)
                {
                    return translations;
                }
            }
            // If it can't find translation, returns array of the original word
            string[] result = new string[LANGUAGE_COUNT].Select(item=>word).ToArray();//TODO: This might be where it's failing
            return result;
        }

        public static String FindTranslation(String word)
        {
#if DEBUG
            Log_Manager.LogVerbose("TranslationManager", "The function FindComboBoxTranslations was called");
#endif
            if (Words.TryLookup(word, out string[] translations))
            {
                if (translations != null)
                {
                    return translations[CurrentLanguageIndex];
                }
            }
            // If it can't find translation, returns array of the original word
            return word;
        }

        /// <summary>
        /// This method takes in a string array of key values and an array of values of any type and returns a boolean indiciation if a dictionary of ComboBoxItem Arrays filled
        /// with available language translations can be created. If there was a problem with the input key and value arrays, a default dictionary with an error
        /// message is outputted. Otherwise, the correct dictionary is outputted.
        /// </summary>
        /// <typeparam name="T">Contains the type that the input valueArray is</typeparam>
        /// <param name="keyArray">String array of keys for the dictionary</param>
        /// <param name="valueArray">Any type array of values for the dictionary</param>
        /// /// <param name="tempDict"></param>
        /// <returns>True if dictionary was created; false if dictionary couldn't be created.</returns>
        public static Boolean TryPopulateComboBoxItemDictionary<T>(String[] keyArray, T[] valueArray, out Dictionary<Int32, ComboBoxItem[]> tempDict)
        {
            Log_Manager.LogVerbose("TranslationManager", "The function PopulateComboBoxItemDictionary was called");
            tempDict = new Dictionary<int, ComboBoxItem[]>();
            try
            {
                if (keyArray.Length == valueArray.Length)// Check to see if the number of keys is the same as number of values
                {
                    List<ComboBoxItem> generalList = new List<ComboBoxItem>();
                    string key;
                    for (int i = 0; i < LANGUAGE_COUNT; i++)// cycle thru every available language
                    {
                        for (int k = 0; k < keyArray.Length; k++)// cycle thru every message/item in the key array
                        {
                            key = keyArray[k];
                            generalList.Add(new ComboBoxItem(Find(key, i), valueArray[k]));
                        }
                        tempDict.Add(i, generalList.ToArray()); // Add list to the dictionary
                        generalList.Clear();                    // Clear List for use in next language loop
                    }
                }
                else
                {// Make a default Dictionary with the default error messages for every avilable language
                    for (int i = 0; i < LANGUAGE_COUNT; i++)
                    {// Add item to dictionary for every available language
                        ComboBoxItem[] tempArray = { new ComboBoxItem(Find(COMBOBOX_ERROR, i)) };// Temporary array to hold the message and language
                        tempDict.Add(i, tempArray);
                    }
                    Log_Manager.IssueAlert("Number of keys doesn't match number of values", tempDict);// Added by CAH for debugging purposes. This will tell user if this is the reason the combo box dictionary isn't getting properly populated.
                }
                return true; // Return true in either case, bcuz a dictionary has been created either way. The contents may not match the expectations however (could be an error message)
            }
            catch(Exception ex)
            {
                Log_Manager.IssueAlert(ex);
            }
            return false; // tempDict is always returned. If # of keys doesn't equal # of values, an empty dictionary is called.
        }
        #endregion /Utility Functions
    }
}