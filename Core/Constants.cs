namespace Core
{
    public static class Constants
    {
        public static readonly string SysInfo = string.Format("On Start System Information: Processor Count = {0}", Environment.ProcessorCount);

        public const string NONE = "None";

        public const string INFORMATION = "Information";
        public const string UNKNOWN = "Unknown";

        public const string INVALID = "Invalid";
        public const string INITIALIZED = "Initialized";

        public const string INDEX = "Index";
        public const string SUBINDEX = "Subindex";

        public const string TITLE = "Title";
        public const string NAME = "Name";

        public const string SERVICE = "Service";

        public const string GROUP = "Group";

        public const string MIN = "Min";
        public const string MAX = "Max";

        public const string TYPE = "Type";

        public const string PROPERTY = "Property";

        public const string DRIVE = "drive";
        public const string DATAM = "datam";

        public const string ADDRESS = "Address";
        public const string ACCESSRIGHTS = "Access Rights";
        public const string ACCESSLEVEL = "Access Level";

        public const string UNIT = "Unit";
        public const string UNITSCALE = "Unit Scale";
        public const string DISPLAYUNIT = "Display Unit";
        public const string DISPLAYSCALE = "Display Scale";
        public const string DEFAULTVALUE = "Default Value";

        public const string SECTION = "Section";

        public const string TOP = "TOP";

        public const string ITEMS = "Items";

        public const string VALUE = "Value";

        public const string ENUMERATION = "Enumeration";

        //Groups
        public const string GENERAL = "General";

        public const string MANUFACTURER = "Manufacturer";

        public const string TEMPERATURE = "Temperature";

        public const string VERSION = "Version";

        public const string VELOCITY_LOOP = "Velocity Loop";
        public const string DEVICE_TIME = "Device Time";
        public const string ACCESS_LEVEL = "Access Level";

        public const string FAULT = "Fault";
        public const string WARNING = "Warning";

        public const string DATA_PACKET = "Data Packet";

        public const string POSITION = "Position";
        public const string VELOCITY = "Velocity";

        public static readonly string USER_APP_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //public static readonly string USER_DATAM_DIRECTORY = Path.Combine(USER_APP_DIRECTORY, "Datam");
        //public static readonly string DATAM_CONFIG_DIRECTORY = Path.Combine(USER_DATAM_DIRECTORY, "Configuration");
        //public static readonly string DATAM_CONFIG_PREFIX = "Datam_";
        public const string JSON_EXT = ".json";
        //public const string DATAM_CONFIG_EXT = ".dad";


        #region Hex Numbers
        public const string Ox = "0x";
        #endregion
    }
}
