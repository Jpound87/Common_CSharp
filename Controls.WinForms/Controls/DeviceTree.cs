using Common;
using Common.Constant;
using Common.Controls;
using Common.Extensions;
using Datam.WinForms.Constants;
using Datam.WinForms.Interface;
using Datam.WinForms.Struct;
using Datasheets.Interface;
using Devices;
using Devices.Interface.CiA402;
using Devices.Interface.EventArgs;
using Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using UI.Interface;

namespace Datam.WinForms.Controls
{
    public partial class MainSidePanel : UserControl
    {
        #region Identity
        private const String ControlName = nameof(MainSidePanel);
        public String Identity
        {
            get
            {
                return ControlName;
            }
        }
        private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
        #endregion

        #region Constants
     
        private const int NODE_WIDTH_MIN = 150;
        private const int NODE_HEIGHT_MIN = 17;
        private const int TOP_BOTTOM_TREE_BUMPER_LOGO_HEIGHT= 50;
        private const int NODE_INDENT = 7;

        private const int TVW_PADDING = 35;

        public const int EXPANDED_WIDTH = 400;
        public const int COLLAPSED_WIDTH = 40;

        private static readonly Object NO_DEVICES_TAG = new Object();
        private static readonly Object ALLNET_SCAN_TAG = new Object(); //Better get those scan tages checked out, they could be cancerous.
        #endregion

        #region Timing
        public TimeSpan AutoExpandDelay { get; set; } = TimeSpan.FromSeconds(7);
        public TimeSpan ProcessSplitDelay { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan AutoCollapseDelay { get; set; } = TimeSpan.FromSeconds(3);
        public TimeSpan ResizeWait { get; set; } = TimeSpan.FromMilliseconds(250);
        private static readonly TimeSpan REMOVE_SCAN_ICON_TIMEOUT = TimeSpan.FromSeconds(20);
        private static readonly TimeSpan CLEAR_TIMEOUT = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan RESUME_VISIBILITY_RESTORE_SPAN = TimeSpan.FromMilliseconds(250);
        #endregion /Timing

        #region Readonly
        private readonly ConcurrentBag<IFoundDeviceNetworkNode_Struct> newNetworkNodesScanData = new ConcurrentBag<IFoundDeviceNetworkNode_Struct>();
        private readonly Dictionary<TreeNode, Dictionary<ProtocolType, TreeNode>> dictCommunicatorNode_DictProtocol_Node = new Dictionary<TreeNode, Dictionary<ProtocolType, TreeNode>>();
        private readonly CancellationTokenSource visibilityRestore_TokenSource;
        #endregion /Readonly

        #region Events
        public event Action<bool> DeviceTreeVisibilityChanged;
        public event Action<String> RequestRemoveTab;
        public event Action<String> ReportScanningEvent;
        //public event Action<String> ReportScanningFailure;
        public event Action<OpenState> DeviceTreeOpenStateChanged;
        public event Action<ITreeNodeData> TreeNodeClicked;
        public event Action<IDeviceTreeNodeData_CiA402> RequestAddTab;

        new public event Action<Size> OnSizeChanged;
        #endregion /Events

        #region Event Handlers
        private readonly EventHandler deviceTreeDoubleClick_Handler;
        private readonly EventHandler panelButtonColumn_MouseEnter_Handler;
        private readonly EventHandler panelButtonColumn_MouseLeave_Handler;
        private readonly EventHandler PanelViewArea_MouseEnter_Handler;
        private readonly EventHandler PanelViewArea_MouseLeave_Handler;
        private readonly EventHandler btnDeviceWindowClick_EventHandler;
        private readonly EventHandler chkScanToggleCheckedChanged_Handler;
        private readonly EventHandler panelViewMouseEnter_Handler;
        private readonly EventHandler deviceTreeMouseLeave_Handler;
        private readonly EventHandler deviceTreeOpen_Handler;
        private readonly EventHandler deviceTreeClose_Handler;
        private readonly EventHandler deviceTreeRequestScan_Handler;

        private readonly TreeViewEventHandler resizePanelView_Handler;
        #endregion /Event Handlers

        #region Delegates
        private readonly Action communicatorDetector_RemoveScanning_Action;
        private readonly Action visibilityRestore_Action;
        private readonly Action processSplit_Action;
        private readonly Action processExpand_Action;
        private readonly Action processCollapse_Action;

        private readonly Action setVisibile_True_Action;
        private readonly Action<bool> toggleExpand_Action;
        private readonly Action<DeviceFound_Struct> tempThing_Action;

        private readonly MethodInvoker updateDeviceFound_Invoker;
        private readonly MethodInvoker updateText_Invoker;
        private readonly MethodInvoker resize_Invoker;
        #endregion /Delegates

        #region Images
        private static readonly String openSidePanelDir = String.Format("{0}double_chevron_left.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);
        private static readonly String closeSidePanelDir = String.Format("{0}double_chevron_right.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);
        private static readonly String startScanDir = String.Format("{0}search-48.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);
        private static readonly String activeScanDir = String.Format("{0}search-active-48.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);
        private static readonly String treeCollapseDir = String.Format("{0}tree-collapse-80.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);
        private static readonly String treeExpandDir = String.Format("{0}tree-expand-80.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);

        private static readonly String imgDirectoryLogo_Allied = String.Format("{0}Logo_Allied.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);
        private static readonly String imgDirectoryLogo_Hei = String.Format("{0}Logo_Heidrive.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS);


        private static readonly Bitmap imgOpenSidePanel = assembly.GetImage(openSidePanelDir);
        private static readonly Bitmap imgCloseSidePanel = assembly.GetImage(closeSidePanelDir);
        private static readonly Bitmap imgStartScan = assembly.GetImage(startScanDir);
        private static readonly Bitmap imgActiveScan = assembly.GetImage(activeScanDir);
        private static readonly Bitmap imgTreeCollapse = assembly.GetImage(treeCollapseDir);
        private static readonly Bitmap imgTreeExpand = assembly.GetImage(treeExpandDir);

        private static readonly Bitmap imgLogo_Allied = assembly.GetImage(imgDirectoryLogo_Allied);
        private static readonly Bitmap imgLogo_Hei = assembly.GetImage(imgDirectoryLogo_Hei);


        private int themeIndex_CAN;
        private int themeIndex_CAT;
        private int themeIndex_Motor;
        private int themeIndex_Info;

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button Background image")]
        new public Image BackgroundImage
        {
            get
            {
                return picLogo_Bottom.Image;
            }
            set
            {
                picLogo_Bottom.Image = value;
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button image")]
        public Image Image //TODO: if set by user: override!
        {
            get
            {
                return picLogo_Bottom.Image;
            }
            set
            {
                picLogo_Bottom.Image = value;
            }
        }

        public bool BackgroundInverted { get; private set; } = false;
        private IconAlter backgroundAlter = IconAlter.None;
        public IconAlter BackgroundAlter 
        { 
            get
            {
                return backgroundAlter;
            }
            private set
            {
                if (backgroundAlter != value)
                {
                    backgroundAlter = value;
                    InvertSplitButtonBackground();
                }
            }
        } 
        #endregion Images

        #region Tree Image List

        #region Static Image Data

        #region Index Enum
        /// <summary>
        /// Used to maintain order.
        /// </summary>
        private enum TreeImageIndicies : byte
        {
            CloudIndex,
            SearchIndex,

            MotorIndex_All,
            MotorIndex_Hei,
            MotorIndex_Gry,

            StepperIndex,
            ComIndex,

            CatIndex_All,
            CatIndex_Hei,
            CatIndex_Gry,

            CanIndex_All,
            CanIndex_Hei,
            CanIndex_Gry,

            SelectIndex,
            NoneIndex,

            InfoIndex_All,
            InfoIndex_Hei,
            InfoIndex_Gry
        }
        #endregion

        #region Directories
        private static readonly String[] imgDirs = new String[18]
        {
            String.Format("{0}cloud50.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}search32.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),

            String.Format("{0}All_motor_32.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Heidrive_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Gry_motor_32.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),

            String.Format("{0}stepper32.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}slave32.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),

            String.Format("{0}All_etherCAT_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Hei_etherCAT_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Gry_etherCAT_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),

            String.Format("{0}All_rs232_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Hei_rs232_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Gry_rs232_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),

            String.Format("{0}select16.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}none24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),

            String.Format("{0}All_info_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Hei_info_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS),
            String.Format("{0}Gry_info_24.png", Tokens.IMAGE_DIRECTORY_DATAM_WINFORMS)
        };
        #endregion

        #region Indicies
        private static readonly byte imgCloudIndex = Convert.ToByte(TreeImageIndicies.CloudIndex);
        private static readonly byte imgSearchIndex = (byte)TreeImageIndicies.SearchIndex;
        private static readonly byte imgComIndex = (byte)TreeImageIndicies.ComIndex;
        private static readonly byte imgMotorIndex_All = (byte)TreeImageIndicies.MotorIndex_All;
        private static readonly byte imgMotorIndex_Hei = (byte)TreeImageIndicies.MotorIndex_Hei;
        private static readonly byte imgMotorIndex_Gry = (byte)TreeImageIndicies.MotorIndex_Gry;
        private static readonly byte imgStepperIndex = (byte)TreeImageIndicies.StepperIndex;
        private static readonly byte imgCatIndex_All = (byte)TreeImageIndicies.CatIndex_All;
        private static readonly byte imgCatIndex_Hei = (byte)TreeImageIndicies.CatIndex_Hei;
        private static readonly byte imgCatIndex_Gry = (byte)TreeImageIndicies.CatIndex_Gry;
        private static readonly byte imgCanIndex_All = (byte)TreeImageIndicies.CanIndex_All;
        private static readonly byte imgCanIndex_Hei = (byte)TreeImageIndicies.CanIndex_Hei;
        private static readonly byte imgCanIndex_Gry = (byte)TreeImageIndicies.CanIndex_Gry;
        private static readonly byte imgSelectIndex = (byte)TreeImageIndicies.SelectIndex;
        private static readonly byte imgNoneIndex = (byte)TreeImageIndicies.NoneIndex;
        private static readonly byte imgInfoIndex_All = (byte)TreeImageIndicies.InfoIndex_All;
        private static readonly byte imgInfoIndex_Hei = (byte)TreeImageIndicies.InfoIndex_Hei;
        private static readonly byte imgInfoIndex_Gry = (byte)TreeImageIndicies.InfoIndex_Gry;
        #endregion

        #region Images
        private static readonly Bitmap imgCloud = assembly.GetImage(imgDirs[imgCloudIndex]);
        private static readonly Bitmap imgSearch = assembly.GetImage(imgDirs[imgSearchIndex]);
        private static readonly Bitmap imgCom = assembly.GetImage(imgDirs[imgComIndex]);
        private static readonly Bitmap imgMotor_All = assembly.GetImage(imgDirs[imgMotorIndex_All]);
        private static readonly Bitmap imgMotor_Hei = assembly.GetImage(imgDirs[imgMotorIndex_Hei]);
        private static readonly Bitmap imgMotor_Gry = assembly.GetImage(imgDirs[imgMotorIndex_Gry]);
        private static readonly Bitmap imgStepper = assembly.GetImage(imgDirs[imgStepperIndex]);
        private static readonly Bitmap imgCat_All = assembly.GetImage(imgDirs[imgCatIndex_All]);
        private static readonly Bitmap imgCat_Hei = assembly.GetImage(imgDirs[imgCatIndex_Hei]);
        private static readonly Bitmap imgCat_Gry = assembly.GetImage(imgDirs[imgCatIndex_Gry]);
        private static readonly Bitmap imgCan_All = assembly.GetImage(imgDirs[imgCanIndex_All]);
        private static readonly Bitmap imgCan_Hei = assembly.GetImage(imgDirs[imgCanIndex_Hei]);
        private static readonly Bitmap imgCan_Gry = assembly.GetImage(imgDirs[imgCanIndex_Gry]);
        private static readonly Bitmap imgSelect = assembly.GetImage(imgDirs[imgSelectIndex]);
        private static readonly Bitmap imgNone = assembly.GetImage(imgDirs[imgNoneIndex]);
        private static readonly Bitmap imgInfo_All = assembly.GetImage(imgDirs[imgInfoIndex_All]);
        private static readonly Bitmap imgInfo_Hei = assembly.GetImage(imgDirs[imgInfoIndex_Hei]);
        private static readonly Bitmap imgInfo_Gry = assembly.GetImage(imgDirs[imgInfoIndex_Gry]);
        #endregion

        #region Device Tree Image Theme Helpers
        public static void GetTreeThemeImageIndexes(Themes theme, out int selectedThemeIndex_CAN, out int selectedThemeIndex_CAT, out int selectedThemeIndex_Motor, out int selectedThemeIndex_Info)
        {
            GetTreeThemeImageIndexes_Networks(theme, out selectedThemeIndex_CAN, out selectedThemeIndex_CAT);
            GetTreeThemeImageIndexes_MotorInfo(theme, out selectedThemeIndex_Motor, out selectedThemeIndex_Info); 
        }

        public static void GetTreeThemeImageIndexes_Networks(Themes theme, out int selectedThemeIndex_CAN, out int selectedThemeIndex_CAT)
        {
            switch (theme)
            {
                case Themes.Pastel:
                case Themes.Allied:
                    selectedThemeIndex_CAN = imgCanIndex_All;
                    selectedThemeIndex_CAT = imgCatIndex_All;
                    return;
                case Themes.Hei:
                    selectedThemeIndex_CAN = imgCanIndex_Hei;
                    selectedThemeIndex_CAT = imgCatIndex_Hei;
                    return;
                case Themes.Grey:
                    selectedThemeIndex_CAN = imgCanIndex_Gry;
                    selectedThemeIndex_CAT = imgCatIndex_Gry;
                    return;
            }
            selectedThemeIndex_CAN = imgCanIndex_Gry;
            selectedThemeIndex_CAT = imgCatIndex_Gry;
        }

        public static void GetTreeThemeImageIndexes_MotorInfo(Themes theme, out int selectedThemeIndex_Motor, out int selectedThemeIndex_Info)
        {
            switch (theme)
            {
                case Themes.Pastel:
                case Themes.Allied:
                    selectedThemeIndex_Motor = imgMotorIndex_All;
                    selectedThemeIndex_Info = imgInfoIndex_All;
                    return;
                case Themes.Hei:
                    selectedThemeIndex_Motor = imgMotorIndex_Hei;
                    selectedThemeIndex_Info = imgInfoIndex_Hei;
                    return;
                case Themes.Grey:
                    selectedThemeIndex_Motor = imgMotorIndex_Gry;
                    selectedThemeIndex_Info = imgInfoIndex_Gry;
                    return;
            }
            selectedThemeIndex_Motor = imgMotorIndex_Gry;
            selectedThemeIndex_Info = imgInfoIndex_Gry;
        }
        #endregion

        #endregion /Static Image Data

        #region Create Method
        private static ImageList deviceTreeImageList;
        private static void CreateDeviceTreeImageList()
        {
            if (deviceTreeImageList == null)
            {
                deviceTreeImageList = new ImageList();
                deviceTreeImageList.Images.Add(imgCloud);
                deviceTreeImageList.Images.Add(imgSearch);

                deviceTreeImageList.Images.Add(imgMotor_All);
                deviceTreeImageList.Images.Add(imgMotor_Hei);
                deviceTreeImageList.Images.Add(imgMotor_Gry);

                deviceTreeImageList.Images.Add(imgStepper);
                deviceTreeImageList.Images.Add(imgCom);

                deviceTreeImageList.Images.Add(imgCat_All);
                deviceTreeImageList.Images.Add(imgCat_Hei);
                deviceTreeImageList.Images.Add(imgCat_Gry);

                deviceTreeImageList.Images.Add(imgCan_All);
                deviceTreeImageList.Images.Add(imgCan_Hei);
                deviceTreeImageList.Images.Add(imgCan_Gry);

                deviceTreeImageList.Images.Add(imgSelect);
                deviceTreeImageList.Images.Add(imgNone);

                deviceTreeImageList.Images.Add(imgInfo_All);
                deviceTreeImageList.Images.Add(imgInfo_Hei);
                deviceTreeImageList.Images.Add(imgInfo_Gry);
                deviceTreeImageList.ImageSize = new Size(24, 24);
            }
        }
        #endregion

        #endregion Tree Image List.

        #region Context Menus
        private static readonly IIsObject<ContextMenuStrip> deviceTreeContextMenu_Open = new IsObject<ContextMenuStrip>();
        private static readonly IIsObject<ContextMenuStrip> deviceTreeContextMenu_Closed = new IsObject<ContextMenuStrip>();
        #endregion

        #region Tool Tip
        private static readonly ToolTip deviceTreeToolTip = new ToolTip
        {
            InitialDelay = 500
        };
        #endregion /Tool Tip

        #region Color

        #region Control Button
        private Color controlButtonForeColor;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ControlButtonForeColor
        {
            get
            {
                return controlButtonForeColor;
            }
            set
            {
                if(controlButtonForeColor != value)
                {
                    controlButtonForeColor = value;
                    chkScanToggle.ForeColor = value;
                    chkTreeToggle.ForeColor = value;
                }
            }
        }

        private Color controlButtonBackColor;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set control button background color"), DefaultValue(typeof(SystemColors), "Control")]
        public Color ControlButtonBackColor
        {
            get
            {
                return controlButtonBackColor;
            }
            set
            {
                if (controlButtonBackColor != value)
                {
                    controlButtonBackColor = value;
                    btnToggleExpand.BackColor = value;
                    btnToggleExpand_Top.BackColor = value;
                    btnToggleExpand_Bottom.BackColor = value;
                }
            }
        }
        #endregion /Control Button

        #region View Button
        public bool IsOpen { get; private set; } = true;
        private OpenState openState = OpenState.Open;
        public OpenState OpenState 
        { 
            get
            {
                return openState;
            }
            private set
            {
                if(openState != value)
                {
                    openState = value;
                    IsOpen = openState == OpenState.Open;
                }
            }
        }

        private Color viewButtonForeColor;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        public Color ViewButtonForeColor
        {
            get
            {
                return viewButtonForeColor;
            }
            set
            {
                if (viewButtonForeColor != value)
                {
                    viewButtonForeColor = value;
                    btnToggleExpand.ForeColor = value;
                    btnToggleExpand_Top.ForeColor = value;
                    btnToggleExpand_Bottom.ForeColor = value;
                }
            }
        }

        private Color viewButtonBackColor;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set control button background color"), DefaultValue(typeof(SystemColors), "Control")]
        public Color ViewButtonBackColor
        {
            get
            {
                return viewButtonBackColor;
            }
            set
            {
                if (viewButtonBackColor != value)
                {
                    viewButtonBackColor = value;
                    chkScanToggle.BackColor = value;
                    chkTreeToggle.BackColor = value;
                }
            }
        }
        #endregion /View Button

        #region Panel Buttons 
        private Color panelButton_Accent1_ForeColor;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        public Color PanelButton_Accent1_ForeColor
        {
            get
            {
                return panelButton_Accent1_ForeColor;
            }
            set
            {
                if (panelButton_Accent1_ForeColor != value)
                {
                    panelButton_Accent1_ForeColor = value;
                    SetPanelButton_Accent_Fore();
                }
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        private Color panelButton_Accent2_ForeColor;
        public Color PanelButton_Accent2_ForeColor
        {
            get
            {
                return panelButton_Accent2_ForeColor;
            }
            set
            {
                if (panelButton_Accent2_ForeColor != value)
                {
                    panelButton_Accent2_ForeColor = value;
                    SetPanelButton_Accent_Fore();
                }
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set control button background color"), DefaultValue(typeof(SystemColors), "ControlLight")]
        private Color panelButton_Accent1_BackColor;
        public Color PanelButton_Accent1_BackColor
        {
            get
            {
                return panelButton_Accent1_BackColor;
            }
            set
            {
                if (panelButton_Accent1_BackColor != value)
                {
                    panelButton_Accent1_BackColor = value;
                    SetPanelButton_Accent_Back();
                }
            }
        }

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set control button background color"), DefaultValue(typeof(SystemColors), "ControlLightLight")]
        private Color panelButton_Accent2_BackColor;
        public Color PanelButton_Accent2_BackColor
        {
            get
            {
                return panelButton_Accent2_BackColor;
            }
            set
            {
                if (panelButton_Accent2_BackColor != value)
                {
                    panelButton_Accent2_BackColor = value;
                    SetPanelButton_Accent_Back();
                }
            }
        }
        #endregion /Panel Buttons

        #region Back 
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button background color"), DefaultValue(typeof(SystemColors), "Control")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                tlpPanel.BackColor = value;
                tvwDeviceView.BackColor = value;
            }
        }
        #endregion / Back 

        #region Fore
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button forground color"), DefaultValue(typeof(SystemColors), "ControlText")]
        new public Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                tlpPanel.ForeColor = value;
                tvwDeviceView.ForeColor = value;
            }
        }
        #endregion /Fore

        #region Mouse Over
        private Color mouseOverBackColor = SystemColors.ControlLight;
        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlLight")]
        public Color MouseOverBackColor
        {
            get
            {
                return mouseOverBackColor;
            }
            set
            {
                mouseOverBackColor = value;
                SetPanelButton_Accent_MouseOver();
            }
        }

        #endregion /Mouse Over

        #region Mouse Down

        /// <summary>Get/Set button Background image</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(SystemColors), "ControlDark")]
        private Color mouseDownBackColor;
        public Color MouseDownBackColor
        {
            get
            {
                return mouseDownBackColor;
            }
            set
            {
                mouseDownBackColor = value;
                SetPanelButton_Accent_MouseDown();
            }
        }
        #endregion

        #region On
        SplitButton selectedButton;
        private Color onColor = Color.DarkGray;
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(Color), "DarkGray")]
        public Color OnColor
        {
            get
            {
                return onColor;
            }
            set
            {
                onColor = value;
                SetSelectedColor();
            }
        }
        #endregion /On

        #region Off
        private Color offColor = Color.Gray;
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button mouse over color"), DefaultValue(typeof(Color), "Gray")]
        public Color OffColor
        {
            get
            {
                return offColor;
            }
            set
            {
                offColor = value;
                SetSelectedColor();
            }
        }
        #endregion /Off

        #endregion /Color

        #region Communicator Scan
        public event Action<bool> ToggleScan;
        public bool IsScanning
        {
            get
            {
                return chkScanToggle.Checked;
            }
            set
            {
                lock (chkScanToggle)
                {
                    if (chkScanToggle.Checked != value)
                    {
                        chkScanToggle.Checked = value;
                    }
                }
            }
        }

        /// <summary>
        /// The container for the scanning node if present
        /// </summary>
        private static readonly IIsObject<TreeNode> ScanningNode = new IsObject<TreeNode>();
        #endregion /Communicator Scan

        #region Device Tree
        //private readonly int panelRow_TopButton = 0;
        //private readonly int panelRow_Scanning = 1;
        private readonly int panelRow_TopLogo = 2;
        private readonly int panelRow_TreeNodes = 3;

        private readonly int treeToggleControlColumn = 0;
        private readonly int deviceTreeColumn = 1;
        private readonly int expandControlColumn_Split = 2; 
        private readonly int expandControlColumn_Solid = 3;
        #endregion

        #region Panel Buttons
        private readonly SplitButton[] splitButtons;
        private readonly int[] splitButtonRows;

        public event MethodInvoker MotorControlPanelClicked_Invoker;
        /// <summary>Get/Set Motor Control Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool MotorControlPanelVisible
        {
            get
            {
                return sbnMotorControlPanel.Visible;
            }
            set
            {
                sbnMotorControlPanel.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }

        public event MethodInvoker DataMonitorPanelClicked_Invoker;
        /// <summary>Get/Set Data Monitor Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool DataMonitorPanelVisible
        {
            get
            {
                return sbnDataMonitorPanel.Visible;
            }
            set
            {
                sbnDataMonitorPanel.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }

        public event MethodInvoker ParameterPanelClicked_Invoker;
        /// <summary>Get/Set Parameter Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool ParameterPanelVisible
        {
            get
            {
                return sbnParameterPanel.Visible;
            }
            set
            {
                sbnParameterPanel.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }

        public event MethodInvoker DiagnosticsPanelClicked_Invoker;
        /// <summary>Get/Set Diagnostics Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool DiagnosticsPanelVisible
        {
            get
            {
                return sbnDiagnosticPanel.Visible;
            }
            set
            {
                sbnDiagnosticPanel.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }

        public event MethodInvoker GaugePanelClicked_Invoker;
        /// <summary>Get/Set Gauge Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool GaugePanelVisible
        {
            get
            {
                return sbnGaugePanel.Visible;
            }
            set
            {
                sbnGaugePanel.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }

        public event MethodInvoker GPIOPanelClicked_Invoker;
        /// <summary>Get/Set GPIO Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool GPIOPanelVisible
        {
            get
            {
                return sbnGPIO.Visible;
            }
            set
            {
                sbnGPIO.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }

        public event MethodInvoker IOMappingPanelClicked_Invoker;
        /// <summary>Get/Set IO Mapping Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool IOMappingPanelVisible
        {
            get
            {
                return sbnMappingIO.Visible;
            }
            set
            {
                sbnMappingIO.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }

        public event MethodInvoker ConfigurationPanelClicked_Invoker;
        /// <summary>Get/Set Configuration Panel Visible</summary>
        [Browsable(true), Category("Appearance"), RefreshProperties(RefreshProperties.All),
        Description("Get/Set button visibile state"), DefaultValue(false)]
        public bool ConfigurationPanelVisible
        {
            get
            {
                return sbnConfigurationPanel.Visible;
            }
            set
            {
                sbnConfigurationPanel.Visible = value;
                ToggleExpandSplitState(DetermineSplitState(value));
            }
        }
        #endregion /Visibility

        #region Synchronization
        private CancellationTokenSource autoExpand_TokenSource;
        private CancellationTokenSource determineSplit_TokenSource;
        private readonly ManualResetEvent treeClearAwaiter = new ManualResetEvent(true);
        #endregion

        #region Auto Expand
        private bool mouseEventsAttached = false;
        public bool AutoExpand { get; set; }
        #endregion /Auto Expand

        #region Constructor
        public MainSidePanel()
        {
            InitializeComponent();

            CreateDeviceTreeImageList();

            DoubleBuffered = true;

            tvwDeviceView.ImageList = deviceTreeImageList;
            tvwDeviceView.ImageIndex = imgCloudIndex;
            tvwDeviceView.SelectedImageIndex = imgSelectIndex;

            tvwDeviceView.Indent = 7;
            treeToggleControlColumn = tlpPanel.GetColumn(chkTreeToggle);
            deviceTreeColumn = tlpPanel.GetColumn(pnlDeviceTree);
            expandControlColumn_Split = tlpPanel.GetColumn(btnToggleExpand_Top);
            expandControlColumn_Solid = tlpPanel.GetColumn(btnToggleExpand);

            tlpPanel.MaximumSize = new Size(EXPANDED_WIDTH, tlpPanel.MaximumSize.Height);

            ContextMenuStrip = new ContextMenuStrip();

            updateDeviceFound_Invoker = new MethodInvoker(UpdateDeviceFound);

            deviceTreeDoubleClick_Handler = new EventHandler(TvwAvailableDevices_DoubleClick);
            tvwDeviceView.DoubleClick += deviceTreeDoubleClick_Handler;

            panelButtonColumn_MouseEnter_Handler = new EventHandler(BtnToggleDeviceTree_MouseEnter);
            panelButtonColumn_MouseLeave_Handler = new EventHandler(BtnToggleDeviceTree_MouseLeave);

            PanelViewArea_MouseEnter_Handler = new EventHandler(PanelView_MouseEnter);
            tvwDeviceView.MouseEnter += PanelViewArea_MouseEnter_Handler;
            picLogo_Bottom.MouseEnter += PanelViewArea_MouseEnter_Handler;

            PanelViewArea_MouseLeave_Handler = new EventHandler(PanelView_MouseLeave);
            tvwDeviceView.MouseLeave += PanelViewArea_MouseLeave_Handler;
            picLogo_Bottom.MouseLeave += PanelViewArea_MouseLeave_Handler;

            resizePanelView_Handler = new TreeViewEventHandler(ResizeTreeView);
            tvwDeviceView.AfterExpand += resizePanelView_Handler;
            tvwDeviceView.AfterCollapse += resizePanelView_Handler;

            //panelRow_TopButton = tlpPanel.GetRow(chkTreeToggle);
            //panelRow_Scanning = tlpPanel.GetRow(pgbScanning);
            panelRow_TopLogo = tlpPanel.GetRow(pnlTopLogo);
            panelRow_TreeNodes = tlpPanel.GetRow(pnlDeviceTree);

            panelViewMouseEnter_Handler = new EventHandler(DeviceTree_MouseEnter);
            tvwDeviceView.MouseEnter += panelViewMouseEnter_Handler;
            tlpPanel.MouseEnter += panelViewMouseEnter_Handler;
            deviceTreeMouseLeave_Handler = new EventHandler(DeviceTree_MouseLeave);
            tvwDeviceView.MouseLeave += deviceTreeMouseLeave_Handler;
            tlpPanel.MouseLeave += deviceTreeMouseLeave_Handler;

            deviceTreeOpen_Handler = new EventHandler(DeviceTree_Expand);
            deviceTreeClose_Handler = new EventHandler(DeviceTree_Collapse);

            deviceTreeRequestScan_Handler = new EventHandler(RequestScan);

            communicatorDetector_RemoveScanning_Action = new Action(ScanningNode_Remove);

            visibilityRestore_Action = new Action(RestoreVisibility);

            sbnMotorControlPanel.Click += SbnMotorControlPanel_Click; //TODO: replace with handler
            sbnDataMonitorPanel.Click += SbnDataMonitorPanel_Click;
            sbnParameterPanel.Click += SbnParameterPanel_Click;
            sbnDiagnosticPanel.Click += SbnDiagnosticPanel_Click;
            sbnGaugePanel.Click += SbnGaugePanel_Click;
            sbnGPIO.Click += SbnGPIO_Click;
            sbnMappingIO.Click += SbnMappingIO_Click;
            sbnConfigurationPanel.Click += SbnConfigurationPanel_Click;

            processSplit_Action = new Action(ProcessExpandSplitState);
            processExpand_Action = new Action(ProcessExpand);
            processCollapse_Action = new Action(ProcessCollapse);
            toggleExpand_Action = new Action<bool>(ToggleExpand);
            tempThing_Action = new Action<DeviceFound_Struct>(ProcessDeviceNodeData_Add);

            updateText_Invoker = new MethodInvoker(UpdateText);

            resize_Invoker = new MethodInvoker(ResizeTreeView_Common);

            setVisibile_True_Action = new Action(SetVisibleCore_Delayed);

            Translation_Manager.LanguageChanged += updateText_Invoker;

            splitButtons = new SplitButton[8]
            {
                sbnMotorControlPanel,
                sbnDataMonitorPanel,
                sbnParameterPanel,
                sbnDiagnosticPanel,
                sbnGaugePanel,
                sbnGPIO,
                sbnMappingIO,
                sbnConfigurationPanel
            };
            splitButtonRows = new int[8]
            {
                tlpPanel.GetRow(sbnMotorControlPanel),
                tlpPanel.GetRow(sbnDataMonitorPanel),
                tlpPanel.GetRow(sbnParameterPanel),
                tlpPanel.GetRow(sbnDiagnosticPanel),
                tlpPanel.GetRow(sbnGaugePanel),
                tlpPanel.GetRow(sbnGPIO),
                tlpPanel.GetRow(sbnMappingIO),
                tlpPanel.GetRow(sbnConfigurationPanel)
            };
            SetVisibility_PanelOptions(false);
            InitializePanelButton();
            SetupToolip();
            SetFonts();
            PerformLayout();
        }

        private void SetFonts()
        {
            foreach(SplitButton splitButton in splitButtons)
            {
                splitButton.Font = Control_Display.CaptionFont_Large;
                splitButton.ForeColor = Control_Display.LightText_Color;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            visibilityRestore_TokenSource?.Cancel();// We don't want to be visible yet apparantly
            if (IsHandleCreated)
            {
                Visible = false;
                this.ScheduleCancelableAction(RESUME_VISIBILITY_RESTORE_SPAN, visibilityRestore_Action);
            }
            base.OnResize(e);
        }

        private void RestoreVisibility()// TODO: Sytemize this in generic, spread the love (as needed)
        {
            PerformLayout();
            Application.DoEvents();
            Visible = true;
        }

        private void InitializePanelButton()
        {
            for (int sbn = 0; sbn < splitButtons.Length; sbn++)
            {
                splitButtons[sbn].Visible = false;
                splitButtons[sbn].MouseEnter += panelButtonColumn_MouseEnter_Handler;
                splitButtons[sbn].MouseLeave += panelButtonColumn_MouseLeave_Handler;
                splitButtons[sbn].MouseEnter += PanelViewArea_MouseEnter_Handler;
                splitButtons[sbn].MouseLeave += PanelViewArea_MouseLeave_Handler;
                desiredSplitState = desiredSplitState || splitButtons[sbn].Visible;
            }
            ProcessExpandSplitState();
            chkScanToggle.BackColor = ControlButtonBackColor;
            chkTreeToggle.BackColor = ControlButtonBackColor;
            chkScanToggle.ForeColor = ControlButtonForeColor;
            chkTreeToggle.ForeColor = ControlButtonForeColor;
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    //resizeWait_TokenSource = this.ScheduleCancelableAction(ResizeWait, setVisibile_True_Action);
        //}

        //protected override void OnResize(EventArgs e)
        //{
        //    if (IsHandleCreated)
        //    {
        //        base.OnResize(e);
        //        base.SetVisibleCore(false);
        //        resizeWait_TokenSource?.Cancel();
        //        resizeWait_TokenSource = this.ScheduleCancelableAction(ResizeWait, setVisibile_True_Action);
        //    }
        //}

        private void SetVisibleCore_Delayed()
        {
            tlpPanel.Visible = true;
            base.SetVisibleCore(true);
        }
        #endregion

        #region Settings
        private void SetPanelButton_Accent_Fore()
        {
            bool toggle = true;
            for(int sbn = 0; sbn < splitButtons.Length; sbn++)
            {
                if (splitButtons[sbn].Visible)
                {
                    if (toggle)
                    {
                        splitButtons[sbn].ForeColor = panelButton_Accent1_ForeColor;
                    }
                    else
                    {
                        splitButtons[sbn].ForeColor = panelButton_Accent2_ForeColor;
                    }
                    toggle = !toggle;
                }
            }
        }

        private void SetPanelButton_Accent_Back()
        {
            bool toggle = true;
            for (int sbn = 0; sbn < splitButtons.Length; sbn ++)
            {
                if (splitButtons[sbn].Visible)
                {
                    if (toggle)
                    {
                        splitButtons[sbn].ForeColor = panelButton_Accent1_BackColor;
                    }
                    else
                    {
                        splitButtons[sbn].ForeColor = panelButton_Accent2_BackColor;
                    }
                    toggle = !toggle;
                }
            }
        }

        private void SetPanelButton_Accent_MouseOver()
        {
            for (int sbn = 0; sbn < splitButtons.Length; sbn++)
            {
                 splitButtons[sbn].MouseOverBackColor = mouseOverBackColor;
            }
        }
        private void SetPanelButton_Accent_MouseDown()
        {
            for (int sbn = 0; sbn < splitButtons.Length; sbn++)
            {
                splitButtons[sbn].MouseDownBackColor = mouseDownBackColor;
            }
        }

        private void SetSelectedColor()
        {
            foreach (SplitButton splitButton in splitButtons)
            {
                if (selectedButton != null && splitButton == selectedButton)
                {
                    splitButton.BackColor = OnColor;
                }
                else if (splitButton.BackColor != OffColor)
                {
                    splitButton.BackColor = OffColor;
                }
            }
        }
        #endregion

        #region Translation
        public void UpdateText()//todo, link to event translate changed in settings
        {
            chkScanToggle.Text = chkScanToggle.Checked ? Translation_Manager.Scanning : Translation_Manager.Scan;
            sbnMotorControlPanel.Text = Translation_Manager.MotionController;
            sbnDataMonitorPanel.Text = Translation_Manager.DataCapture;
            sbnParameterPanel.Text = Translation_Manager.ParameterPanel;
            sbnDiagnosticPanel.Text = Translation_Manager.Diagnostics;
            sbnGaugePanel.Text = Translation_Manager.GaugePanel;
            sbnGPIO.Text = Translation_Manager.PanelGPIO;
            sbnMappingIO.Text = Translation_Manager.MappingI0;
            sbnConfigurationPanel.Text = Translation_Manager.DeviceConfiguration;

            ContextMenuStrip contextMenu;
            lock (deviceTreeContextMenu_Open)
            {
                contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add(Translation_Manager.Close, null, deviceTreeClose_Handler);
                contextMenu.Items.Add(Translation_Manager.ScanForCommunicators, null, deviceTreeRequestScan_Handler);
                deviceTreeContextMenu_Open.Set(contextMenu);
            }
            lock (deviceTreeContextMenu_Closed)
            {
                contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add(Translation_Manager.LockOpen, null, deviceTreeOpen_Handler);
                deviceTreeContextMenu_Closed.Set(contextMenu);
            }
            
        }
        #endregion /Translation

        #region Theme
        public void UpdateTheme(Themes theme)
        {
            lock (tlpPanel)
            {
                GetTreeThemeImageIndexes
                (
                    theme,
                    out themeIndex_CAN,
                    out themeIndex_CAT,
                    out themeIndex_Motor,
                    out themeIndex_Info
                );

                switch (theme)//TODO: new method
                {
                    case Themes.Allied:
                    case Themes.Pastel:
                    case Themes.Grey:
                        picLogo_Top.BackgroundImage = imgLogo_Allied;
                        backgroundAlter = IconAlter.None;
                        InvertSplitButtonBackground();
                        return;
                    case Themes.Hei:
                        picLogo_Top.BackgroundImage = imgLogo_Hei;
                        backgroundAlter = IconAlter.Invert;
                        InvertSplitButtonBackground();
                        return;
                    case Themes.Ormec:
                        picLogo_Top.BackgroundImage = imgLogo_Allied; //TODO: replace with ORMEC branding
                        backgroundAlter = IconAlter.None;
                        InvertSplitButtonBackground();
                        return;
                }
                //Image = deviceTreeImageList.Images[themeIndex_Motor]; TODO: image appropriate
                tvwDeviceView.UpdateImageIndex(Translation_Manager.CANOPEN, themeIndex_CAN);
                tvwDeviceView.UpdateImageIndex(Translation_Manager.ETHERCAT, themeIndex_CAT);
                tvwDeviceView.UpdateImageIndex(Tokens.MOTOR, themeIndex_Motor);// TODO: change over to use translation manger for common words task
                tvwDeviceView.UpdateImageIndex(Tokens.INFORMATION, themeIndex_Info);
            }
        }

        public void InvertSplitButtonBackground()
        {
            lock (splitButtons)
            {
                if (BackgroundInverted)
                {
                    if (BackgroundAlter == IconAlter.None)
                    {
                        foreach (SplitButton splitButton in splitButtons)
                        {
                            splitButton.InvertColor();
                            BackgroundInverted = false;
                        }
                    }
                }
                else
                {
                    if (BackgroundAlter == IconAlter.Invert)
                    {
                        foreach (SplitButton splitButton in splitButtons)
                        {
                            splitButton.InvertColor();
                            BackgroundInverted = true;
                        }
                    }
                }
            }
        }
        #endregion /Theme

        #region Tool Tip
        protected void SetupToolip()
        {
            Log_Manager.LogMethodCall(ControlName, nameof(SetupToolip));
            deviceTreeToolTip.SetToolTip(btnToggleExpand, Translation_Manager.HideDeviceTree);
            deviceTreeToolTip.SetToolTip(btnToggleExpand_Top, Translation_Manager.HideDeviceTree);
            deviceTreeToolTip.SetToolTip(btnToggleExpand_Bottom, Translation_Manager.HideDeviceTree);

            deviceTreeToolTip.SetToolTip(sbnMotorControlPanel, Translation_Manager.MotionController);
            deviceTreeToolTip.SetToolTip(sbnDataMonitorPanel, Translation_Manager.DataCapture);
            deviceTreeToolTip.SetToolTip(sbnParameterPanel, Translation_Manager.ParameterPanel);
            deviceTreeToolTip.SetToolTip(sbnGaugePanel, Translation_Manager.GaugePanel);
            deviceTreeToolTip.SetToolTip(sbnDiagnosticPanel, Translation_Manager.Diagnostics);
            deviceTreeToolTip.SetToolTip(sbnGPIO, Translation_Manager.PanelGPIO);
            deviceTreeToolTip.SetToolTip(sbnMappingIO, Translation_Manager.MappingI0);
            deviceTreeToolTip.SetToolTip(sbnConfigurationPanel, Translation_Manager.Configuration);
        }
        #endregion

        #region Tree View Control
        public void DisplayDeviceFound()
        {
            if(InvokeRequired)
            {
                Invoke(updateDeviceFound_Invoker);
            }
            else
            {
                UpdateDeviceFound();
            }
        }

        private void UpdateDeviceFound()
        {
            lock (tvwDeviceView)
            {
                if (tvwDeviceView.Nodes.ContainsKey(Translation_Manager.NO_DEVICES))
                {// We haven't yet displayed a device.
                    NoDevicesNode_Remove();
                }
            }
        }
        #endregion

        #region Node Expand
        /// <summary>
        /// 
        /// </summary>
        private void ExpandTree()
        {
            try
            {
                tvwDeviceView.SuspendLayout();
                tvwDeviceView.ExpandTree();
            }
            finally
            {
                tvwDeviceView.ResumeLayout();
            }
        }
        #endregion /Node Expand

        #region On Click
        /// <summary>
        /// This handler will open a new tab with a connection object for the selected drive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvwAvailableDevices_DoubleClick(object sender, EventArgs e)
        {
            Log_Manager.LogMethodCall(ControlName, nameof(TvwAvailableDevices_DoubleClick));
            if(tvwDeviceView.SelectedNode != null && tvwDeviceView.SelectedNode.Tag is ITreeNodeData treeNodeData)
            {
                TreeNodeClicked?.Invoke(treeNodeData);
            }
        }
        #endregion /On Click

        #region Scan Request
        private void RequestScan(object _, EventArgs e)
        {
            chkScanToggle.Checked = true;// The rest is taken care of in its event handler
        }
        #endregion

        #region Visibility Request
        public void SetVisibility_PanelOptions(bool visible)// TODO: version with enums for each menu option
        {
            lock (tlpPanel)
            {
                bool changed = false;
                foreach (SplitButton splitButton in splitButtons)
                {
                    if (splitButton.Visible != visible)
                    {
                        changed = true;
                        splitButton.Visible = visible;
                    }
                }
                if (changed)
                {
                    ToggleExpandSplitState();
                }
            }
        }

        private bool DetermineSplitState(bool split = false)
        {
            lock (splitButtons)
            {
                if (split)
                {
                    return split;
                }
                foreach (SplitButton splitButton in splitButtons)
                {
                    if (splitButton.Visible)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool splitState = false;
        public bool desiredSplitState = false;
        private void ToggleExpandSplitState(bool split = false)
        {
            lock (tlpPanel)
            {
                if (split != splitState)
                {
                    determineSplit_TokenSource?.Cancel();
                    determineSplit_TokenSource = null;
                    desiredSplitState = split;
                    determineSplit_TokenSource = this.ScheduleCancelableAction(ProcessSplitDelay, processSplit_Action);
                }
            }
        }

        private void ProcessExpandSplitState()
        {
            lock (tlpPanel)
            {
                try
                {
                    tlpPanel.SuspendLayout();
                    bool notSplitState = !desiredSplitState;
                    btnToggleExpand.Enabled = notSplitState;
                    btnToggleExpand.Visible = notSplitState;
                    btnToggleExpand_Top.Enabled = desiredSplitState;
                    btnToggleExpand_Top.Visible = desiredSplitState;
                    btnToggleExpand_Bottom.Enabled = desiredSplitState;
                    btnToggleExpand_Bottom.Visible = desiredSplitState;
                    if (desiredSplitState)
                    {
                        for (int sbn = 0; sbn < splitButtons.Length; sbn++)
                        {
                            if (splitButtons[sbn].Visible)
                            {
                                tlpPanel.RowStyles[splitButtonRows[sbn]].SizeType = SizeType.AutoSize;
                            }
                            else
                            {
                                tlpPanel.RowStyles[splitButtonRows[sbn]].SizeType = SizeType.Absolute;
                                tlpPanel.RowStyles[splitButtonRows[sbn]].Height = 0;
                            }
                        }
                    }
                    else
                    {
                        for (int sbn = 0; sbn < splitButtonRows.Length; sbn++)
                        {
                            tlpPanel.RowStyles[splitButtonRows[sbn]].SizeType = SizeType.Absolute;
                            tlpPanel.RowStyles[splitButtonRows[sbn]].Height = 0;
                        }
                    }
                    splitState = desiredSplitState;
                }
                finally
                {
                    tlpPanel.ResumeLayout();
                }
            }
        }
        #endregion

        #region Resize 
        /// <summary>
        /// This method is needed to register the event to tree view events.
        /// 'ResizeTreeView_Common' was made for the method invoker used by threaded methods.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="e"></param>
        private void ResizeTreeView(object _, TreeViewEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(resize_Invoker);
            }
            else
            {
                ResizeTreeView_Common();
            }
        }

        private void ResizeTreeView_Common()
        {
            lock (tvwDeviceView)
            {
                int height = 0;
                int maxWidth = NODE_WIDTH_MIN;
                if (tvwDeviceView.Nodes.Count > 0)
                {
                    height += NODE_HEIGHT_MIN;
                    tvwDeviceView.Nodes.NodeDimension_Recursive(TVW_PADDING, tvwDeviceView.Indent, ref height, ref maxWidth);
                    tlpPanel.RowStyles[panelRow_TopLogo].Height = TOP_BOTTOM_TREE_BUMPER_LOGO_HEIGHT;
                    chkTreeToggle.Enabled = true;
                }
                else
                {
                    tlpPanel.RowStyles[panelRow_TopLogo].Height = 0;
                    chkTreeToggle.Visible = false;
                }
                tlpPanel.RowStyles[panelRow_TreeNodes].Height = height;
                if (IsOpen)
                {
                    OnSizeChanged?.Invoke(new Size(maxWidth + COLLAPSED_WIDTH + NODE_INDENT, Height));
                }
            }
        }
        #endregion /Resize

        #region Scanning Nodes
  
        #region Add
        public void ScanningNode_Add()
        {
            if (!ScanningNode.IsInstance)
            {
                var communicatorTreeNode = tvwDeviceView.Nodes.Add(Translation_Manager.SCAN, Translation_Manager.ScanningDevicesCommunicator, imgSearchIndex, imgSearchIndex);
                ScanningNode.Set(communicatorTreeNode);
                communicatorTreeNode.Tag = ScanningNode;
                ResizeTreeView_Common();
            }
        }
        #endregion /Add

        #region Remove
        public void ScanningNode_Remove()
        {
            lock (tvwDeviceView)
            {
                if (ScanningNode.IsInstance)
                {
                    tvwDeviceView.Nodes.RemoveByKey(Translation_Manager.SCAN);
                    ScanningNode.Clear();
                    ResizeTreeView_Common();
                }
            }
        }
        #endregion /Remove

        #endregion /Scanning Nodes

        #region Virtual
        public void VirtualDevice_Add(IDeviceTreeNodeData treeNodeData)
        {
            if (!tvwDeviceView.Nodes.ContainsKey(Translation_Manager.VIRTUAL))
            {
                TreeNode communicatorTreeNode = tvwDeviceView.Nodes.Add(Translation_Manager.VIRTUAL, Translation_Manager.Virtual, imgCloudIndex, imgCloudIndex);
                communicatorTreeNode.Tag = treeNodeData;
                ResizeTreeView_Common();
            }
        }

        public void VirtualDevice_Remove()
        {
            tvwDeviceView.Nodes.RemoveByKey(Translation_Manager.VIRTUAL);
        }
        #endregion

        #region No Devices
        public void NoDevicesNode_Add()
        {
            TreeNode communicatorTreeNode = tvwDeviceView.Nodes.Add(Translation_Manager.NO_DEVICES, Translation_Manager.NoDevices, imgNoneIndex, imgNoneIndex);
            communicatorTreeNode.Tag = NO_DEVICES_TAG;
            ResizeTreeView_Common();
        }

        public void NoDevicesNode_Remove()
        {
            tvwDeviceView.Nodes.RemoveByKey(Translation_Manager.NO_DEVICES);
            ResizeTreeView_Common();
        }
        #endregion

        #region Communicator Device Tree Events

        #region Communicator Added
        public bool OnCommunicatorAdded(IEventArgs_Communicator e, out ITreeNodeData_Scan communicatorTreeData)
        {
            bool addCommunicator = !tvwDeviceView.Nodes.ContainsKey(e.CommunicatorInfo.ID);
            if (addCommunicator)
            {
                var communicatorNode = tvwDeviceView.Nodes.Add(e.CommunicatorInfo.ID, e.CommunicatorInfo.ToString(), imgComIndex, imgComIndex);
                communicatorTreeData = new TreeNodeData_Communicator_Struct(communicatorNode, e.CommunicatorInfo);
                communicatorNode.Tag = communicatorTreeData;
                NoDevicesNode_Remove();
            }
            else
            {
                communicatorTreeData = null;
            }
            ResizeTreeView_Common();
            return addCommunicator;
        }
        #endregion// Communicator Added

        #region Communicator Removed
        /// <summary>
        /// This method will remove the communicator node represented by the given ID string.
        /// </summary>
        /// <param name="ID">String ID of the communicator to be removed.</param>
        /// <returns>True if the device tree still contains elements, else false.</returns>
        public bool RemoveCommunicatorNode(String ID)
        {
            tvwDeviceView.Nodes.RemoveByKey(ID);
            bool hasRemaining = tvwDeviceView.Nodes.Count > 0;
            if (!hasRemaining)
            {
                NoDevicesNode_Add();
            }
            ResizeTreeView_Common();
            return hasRemaining;
        }
        #endregion

        #region Clear Communicator Node
        public void ClearCommunicatorsNetworkNode(TreeNode communicatorNode, ProtocolType protocolType)
        {
            treeClearAwaiter.Reset();
            lock (dictCommunicatorNode_DictProtocol_Node)
            {
                foreach (TreeNode connectionTreeNode in communicatorNode.Nodes)
                {
                    if (dictCommunicatorNode_DictProtocol_Node.TryRemove(connectionTreeNode, out Dictionary<ProtocolType, TreeNode> protocolType_Node))
                    {// If we have this communicator node.
                        if (protocolType_Node.TryRemove(protocolType, out TreeNode networkNode))
                        {// If we have this network node.
                            try
                            {// Close all existing devices on this connection node, since a new scan is about to start.
                                foreach (TreeNode deviceTreeNode in networkNode.Nodes)// TODO: do we want to close thse? it should be an option probablly...
                                {
                                    if (deviceTreeNode.Tag is DeviceTreeNodeData_CiA402 deviceTreeNodeData)
                                    {
                                        RequestRemoveTab.Invoke(deviceTreeNodeData.ID);
                                    }
                                }
                            }
                            finally
                            {
                                networkNode.Remove();
                            }
                        }
                    }
                }
            }
            treeClearAwaiter.Set();
        }

        #endregion

        #endregion /Communicator Device Tree Events

        #region Network Node

        #region ALLNET 
        public void Scanning_ALLNET_Add()
        {
            string strIP = Translation_Manager.ScanningDevicesALLNET;
            lock (tvwDeviceView)
            {
                var communicatorTreeNode = tvwDeviceView.Nodes.Add(Translation_Manager.ALLNET, strIP, imgSearchIndex, imgSearchIndex);
                communicatorTreeNode.Tag = ALLNET_SCAN_TAG;
            }
            ResizeTreeView_Common();
        }

        public void Scanning_ALLNET_Remove()
        {
            lock (tvwDeviceView)
            {
                tvwDeviceView.Nodes.RemoveByKey(Translation_Manager.ALLNET);
            }
            ResizeTreeView_Common();
        }

        public void ALLNET_Node_Add(IPAddress IP)
        {
            string strIP = IP.ToString();
            lock (tvwDeviceView)
            {
                var communicatorTreeNode = tvwDeviceView.Nodes.Add(strIP, $"ALLNET @ {strIP}", imgStepperIndex, imgStepperIndex);
                communicatorTreeNode.Tag = IP;
            }
            ResizeTreeView_Common();
        }

        public void ALLNET_Node_Remove(IPAddress IP)
        {
            string strIP = IP.ToString();
            tvwDeviceView.Nodes.RemoveByKey(strIP);
            ResizeTreeView_Common();
        }
        #endregion

        #region Create 
        private static Dictionary<ProtocolType, TreeNode> CreateProtocolNodeDictionary()
        {
            return new Dictionary<ProtocolType, TreeNode>();
        }
        #endregion /Create

        #endregion /Network Nodes

        #region Add Node
        public void DeviceNodeData_Add(IDeviceFoundEventArgs dfea, IDatasheet datasheet)//, IDeviceTreeNodeData_CiA402 deviceTreeNodeData_CiA402)
        {
            DeviceFound_Struct tempThing = new DeviceFound_Struct(dfea, datasheet);//, deviceTreeNodeData_CiA402);
            if (InvokeRequired)
            {
                Invoke(tempThing_Action, tempThing);
            }
            else
            {
                ProcessDeviceNodeData_Add(tempThing);
            }
        }

        private void ProcessDeviceNodeData_Add(DeviceFound_Struct tempThing)
        {
            lock (dictCommunicatorNode_DictProtocol_Node)
            {
                IDeviceFoundEventArgs dfea = tempThing.DeviceFoundEventArgs;
                IDatasheet datasheet = tempThing.Datasheet;
                Dictionary<ProtocolType, TreeNode> dict_Protocol_Node = dictCommunicatorNode_DictProtocol_Node.ProvideOrCreate(dfea.CommunicatorTreeNode, CreateProtocolNodeDictionary);

                ITreeNodeData connectionTreeNodeData = new TreeNodeData_Connection_Struct(dfea.ProtocolType);
                int imageIndex;
                string networkName;
                switch (dfea.ProtocolType)
                {
                    case ProtocolType.EtherCAT:
                        imageIndex = themeIndex_CAT;
                        networkName = Translation_Manager.ETHERCAT;
                        break;
                    case ProtocolType.CANopen:
                        imageIndex = themeIndex_CAN;
                        networkName = Translation_Manager.CANOPEN;
                        break;
                    default:
                        imageIndex = imgNoneIndex;
                        networkName = Translation_Manager.None;
                        break;
                }
                FoundDeviceNetworkNode_Struct protocolNodeScanData = new FoundDeviceNetworkNode_Struct(networkName, imageIndex, dfea);

                if (!dictCommunicatorNode_DictProtocol_Node[dfea.CommunicatorTreeNode].TryLookup(dfea.ProtocolType, out TreeNode selectedNetworkTreeNode))
                {
                    selectedNetworkTreeNode = protocolNodeScanData.CreateProtocolDeviceNode();
                    selectedNetworkTreeNode.Tag = protocolNodeScanData;
                    dictCommunicatorNode_DictProtocol_Node[protocolNodeScanData.CommunicatorTreeNode].Add(protocolNodeScanData.ProtocolType, selectedNetworkTreeNode);
                }

                AddFoundDevices(dfea, datasheet, protocolNodeScanData, selectedNetworkTreeNode);//deviceTreeNodeData_CiA402, selectedNetworkTreeNode);
            }
        }
        #endregion /Add Node

        #region Add Found Devices
        /// <summary>
        /// This will add the device found during the scan, it must run on the UI thread
        /// </summary>
        private void AddFoundDevices(IDeviceFoundEventArgs dfea, IDatasheet datasheet, FoundDeviceNetworkNode_Struct protocolNodeScanData, TreeNode selectedNetworkTreeNode)//, IDeviceTreeNodeData_CiA402 deviceTreeNodeData_CiA402, TreeNode selectedNetworkTreeNode)
        {
            treeClearAwaiter.WaitOne(CLEAR_TIMEOUT, true);
            try
            {
                if (dictCommunicatorNode_DictProtocol_Node.ContainsKey(dfea.CommunicatorTreeNode))
                {// It better!

                    ReportScanningEvent?.Invoke(String.Format(Translation_Manager.Message_AddingToNetworkNode, protocolNodeScanData.NetworkName));
                    tvwDeviceView.FindTypeInTreeTags(out HashSet<IDeviceData> existingDeviceData);
                    foreach (IDeviceTreeNodeData deviceData in existingDeviceData)
                    {
                        if (deviceData.DeviceName == dfea.DeviceInformation.Name)
                        {// We already have it.
                            return;
                        }
                    }

                    ICommunicatorDevice_CiA402 device_CiA402 = new CommunicatorDevice_HTX_CiA402(datasheet, dfea);

                    IDeviceTreeNodeData_CiA402 newDeviceData = new DeviceTreeNodeData_CiA402(dfea.CommunicatorInformation, device_CiA402, dfea.DeviceInformation);
                    ReportScanningEvent?.Invoke(String.Format(Translation_Manager.Message_AddingDeviceNode, newDeviceData.DeviceName));
                    if (String.IsNullOrWhiteSpace(newDeviceData.ManufacturerName))
                    {// This shouldn't be the case but you know the drill.
                        TreeNode deviceTreeNodeDrive = selectedNetworkTreeNode.AddNodeWithImage(Tokens.MOTOR, newDeviceData.DeviceName, themeIndex_Info);
                        deviceTreeNodeDrive.Tag = newDeviceData;
                    }
                    else
                    {// We have drive information.
                        TreeNode deviceTreeNodeMotorModel = selectedNetworkTreeNode.AddNodeWithImage(Tokens.INFORMATION, newDeviceData.ManufacturerName, themeIndex_Motor);
                        deviceTreeNodeMotorModel.Tag = newDeviceData;
                        TreeNode deviceTreeNodeDrive = deviceTreeNodeMotorModel.AddNodeWithImage(Tokens.MOTOR, newDeviceData.DeviceName, themeIndex_Info);
                        deviceTreeNodeDrive.Tag = newDeviceData;
                    }
                    RequestAddTab?.Invoke(newDeviceData);
                }
            }
            catch(Exception ex)
            {
                Log_Manager.LogAssert(ControlName, ex.Message);
            }
            finally
            {
                SetDeviceTreeViewVisibility(true);
            }
        }
        #endregion /Add Found Devices

        #region Control Event Handlers

        #region Check Box
        private void ChkScanToggle_CheckedChanged(object _, EventArgs e)
        {
            lock (chkScanToggle)
            {
                ToggleScan?.Invoke(chkScanToggle.Checked);
                chkScanToggle.BackgroundImage = chkScanToggle.Checked ? imgActiveScan : imgStartScan;
                //chkScanToggle.Text = chkScanToggle.Checked ? Translation_Manager.Scanning : Translation_Manager.Scan;
            }
        }

        private void ChkTreeToggle_CheckedChanged(object _, EventArgs e)
        {
            SetDeviceTreeViewVisibility(chkTreeToggle.Checked);
        }

        private void SetDeviceTreeViewVisibility(bool visible = false)
        {
            lock (tvwDeviceView)
            {
                tvwDeviceView.Visible = visible;
                if (visible)
                {// Show device tree (default state)
                    //chkTreeToggle.Text = Translation_Manager.HideDeviceTree;
                    chkTreeToggle.BackgroundImage = imgTreeCollapse;
                    tlpPanel.RowStyles[panelRow_TreeNodes].SizeType = SizeType.AutoSize;
                }
                else
                {
                    //chkTreeToggle.Text = Translation_Manager.ShowDeviceTree;
                    chkTreeToggle.BackgroundImage = imgTreeExpand;
                    tlpPanel.RowStyles[panelRow_TreeNodes].SizeType = SizeType.Absolute;
                    tlpPanel.RowStyles[panelRow_TreeNodes].Height = 0;
                }
                DeviceTreeVisibilityChanged?.Invoke(visible);
            }
        }
        #endregion /Check Box

        #region Button
        private void BtnToggleDeviceTree_Top_Click(object sender, EventArgs e)
        {
            ToggleExpand();
        }

        private void BtnToggleDeviceTree_Bottom_Click(object sender, EventArgs e)
        {
            ToggleExpand();
        }

        private void BtnToggleExpand_Click(object sender, EventArgs e)
        {
            ToggleExpand();
        }

        private void DeviceTree_Expand(object _, EventArgs e)
        {
            Log_Manager.LogMethodCall(ControlName, nameof(DeviceTree_Expand));
            ToggleExpand(true);
        }

        private void DeviceTree_Collapse(object _, EventArgs e)
        {
            Log_Manager.LogMethodCall(ControlName, nameof(DeviceTree_Collapse));
            ToggleExpand(false);
        }

        public void SetOpenState(OpenState openState, bool process = true)
        {
            OpenState = openState;
            if (process)
            {
                ProcessToggleExpand(IsOpen);// IsOpen is set by OpenState accessor.
            }
        }

        private void ProcessExpand()
        {
            ProcessToggleExpand(true);
        }

        private void ProcessCollapse()
        {
            ProcessToggleExpand(false);
        }

        private void ProcessToggleExpand(bool expand)
        {
            if (InvokeRequired)
            {
                Invoke(toggleExpand_Action, expand);
            }
            else
            {
                ToggleExpand(expand);
            }
        }

        /// <summary>
        /// This method will toggle the open/close device view dynamic
        /// </summary>
        /// <param name="expand">If true, will override toggle and open the window or keep it open</param>
        private void ToggleExpand(bool expand = false)
        {
            autoExpand_TokenSource?.Cancel();// If the user clicked we dont wanna keep doing this automatic toggle.
            autoExpand_TokenSource = null;
            try
            {
                bool stateClosed = OpenState == OpenState.Closed;
                picLogo_Top.Visible = stateClosed;// We want these to be the opposite of the current state since we are switching.
                picLogo_Bottom.Visible = stateClosed;
                chkScanToggle.Visible = stateClosed;
                chkScanToggle.Visible = stateClosed;
                tvwDeviceView.Visible = stateClosed;
                if (expand || stateClosed)// Current State: Closed
                {// Expand is the override for the cases we just want to open the window, not toggle
                    Width = EXPANDED_WIDTH;

                    tlpPanel.ColumnStyles[deviceTreeColumn].SizeType = SizeType.Percent;
                    tlpPanel.ColumnStyles[deviceTreeColumn].Width = 100;// Percent.
                  
                    btnToggleExpand.BackgroundImage = imgOpenSidePanel;
                    btnToggleExpand_Top.BackgroundImage = imgOpenSidePanel;
                    btnToggleExpand_Bottom.BackgroundImage = imgOpenSidePanel;

                    deviceTreeToolTip.SetToolTip(btnToggleExpand, Translation_Manager.HideDeviceTree);
                    deviceTreeToolTip.SetToolTip(btnToggleExpand_Top, Translation_Manager.HideDeviceTree);
                    deviceTreeToolTip.SetToolTip(btnToggleExpand_Bottom, Translation_Manager.HideDeviceTree);

                    // Detach mouse over open/close events. Unlock resizing of panel
                    if (mouseEventsAttached)
                    {
                        mouseEventsAttached = false;
                        btnToggleExpand.MouseEnter -= panelButtonColumn_MouseEnter_Handler;
                        btnToggleExpand.MouseLeave -= panelButtonColumn_MouseLeave_Handler;
                        btnToggleExpand_Top.MouseEnter -= panelButtonColumn_MouseEnter_Handler;
                        btnToggleExpand_Top.MouseLeave -= panelButtonColumn_MouseLeave_Handler;
                        btnToggleExpand_Bottom.MouseEnter -= panelButtonColumn_MouseEnter_Handler;
                        btnToggleExpand_Bottom.MouseLeave -= panelButtonColumn_MouseLeave_Handler;
                        tvwDeviceView.MouseEnter += PanelViewArea_MouseEnter_Handler;
                        tvwDeviceView.MouseLeave += PanelViewArea_MouseLeave_Handler;
                    }
                    OpenState = OpenState.Open;
                    SetDeviceTreeViewVisibility(chkTreeToggle.Checked);
                }
                else if (!stateClosed)// Current State: Open
                {// Close Window
                    Width = COLLAPSED_WIDTH;
                    tlpPanel.ColumnStyles[deviceTreeColumn].SizeType = SizeType.Absolute;
                    tlpPanel.ColumnStyles[deviceTreeColumn].Width = 0;

                    btnToggleExpand.BackgroundImage = imgCloseSidePanel;
                    btnToggleExpand_Top.BackgroundImage = imgCloseSidePanel;
                    btnToggleExpand_Bottom.BackgroundImage = imgCloseSidePanel;

                    deviceTreeToolTip.SetToolTip(btnToggleExpand, Translation_Manager.ShowDeviceTree);
                    deviceTreeToolTip.SetToolTip(btnToggleExpand_Top, Translation_Manager.ShowDeviceTree);
                    deviceTreeToolTip.SetToolTip(btnToggleExpand_Bottom, Translation_Manager.ShowDeviceTree);

                    // Attach mouse over open/close events. Lock resizing of panel.
                    if (!mouseEventsAttached)
                    {
                        mouseEventsAttached = true;
                        btnToggleExpand.MouseEnter += panelButtonColumn_MouseEnter_Handler;
                        btnToggleExpand.MouseLeave += panelButtonColumn_MouseLeave_Handler;
                        btnToggleExpand_Top.MouseEnter += panelButtonColumn_MouseEnter_Handler;
                        btnToggleExpand_Top.MouseLeave += panelButtonColumn_MouseLeave_Handler;
                        btnToggleExpand_Bottom.MouseEnter += panelButtonColumn_MouseEnter_Handler;
                        btnToggleExpand_Bottom.MouseLeave += panelButtonColumn_MouseLeave_Handler;
                        tvwDeviceView.MouseEnter -= PanelViewArea_MouseEnter_Handler;
                        tvwDeviceView.MouseLeave -= PanelViewArea_MouseLeave_Handler;
                    }
                    OpenState = OpenState.Closed;
                    SetDeviceTreeViewVisibility(false);
                }
                
            }
            finally
            {
                DeviceTreeOpenStateChanged?.Invoke(OpenState);
            }
        }
        #endregion /Button

        #region Split Button
        private void SbnMotorControlPanel_Click(object _, EventArgs e)
        {
            selectedButton = sbnMotorControlPanel;
            SetSelectedColor();
            MotorControlPanelClicked_Invoker?.Invoke();
        }

        private void SbnDataMonitorPanel_Click(object _, EventArgs e)
        {
            selectedButton = sbnDataMonitorPanel;
            SetSelectedColor();
            DataMonitorPanelClicked_Invoker?.Invoke();
        }

        private void SbnParameterPanel_Click(object _, EventArgs e)
        {
            selectedButton = sbnParameterPanel;
            SetSelectedColor();
            ParameterPanelClicked_Invoker?.Invoke();
        }

        private void SbnGaugePanel_Click(object _, EventArgs e)
        {
            selectedButton = sbnGaugePanel;
            SetSelectedColor();
            GaugePanelClicked_Invoker?.Invoke();
        }

        private void SbnDiagnosticPanel_Click(object _, EventArgs e)
        {
            selectedButton = sbnDiagnosticPanel;
            SetSelectedColor();
            DiagnosticsPanelClicked_Invoker?.Invoke();
        }

        private void SbnGPIO_Click(object _, EventArgs e)
        {
            selectedButton = sbnGPIO;
            SetSelectedColor();
            GPIOPanelClicked_Invoker?.Invoke();
        }

        private void SbnMappingIO_Click(object _, EventArgs e)
        {
            selectedButton = sbnMappingIO;
            SetSelectedColor();
            IOMappingPanelClicked_Invoker?.Invoke();
        }

        private void SbnConfigurationPanel_Click(object _, EventArgs e)
        {
            selectedButton = sbnConfigurationPanel;
            SetSelectedColor();
            ConfigurationPanelClicked_Invoker?.Invoke();
        }


        #endregion /Split Button

        #region Mouse
        private void BtnToggleDeviceTree_MouseEnter(object _, EventArgs e)
        {
            lock (tlpPanel)
            {
                if (AutoExpand)
                {
                    autoExpand_TokenSource?.Cancel();
                    if (OpenState == OpenState.Closed && autoExpand_TokenSource == null)
                    {
                        autoExpand_TokenSource = this.ScheduleCancelableAction(AutoExpandDelay, processExpand_Action);
                    }
                    else
                    {
                        autoExpand_TokenSource = null;
                    }
                }
            }
        }

        private void BtnToggleDeviceTree_MouseLeave(object _, EventArgs e)
        {
            lock (tlpPanel)
            {
                autoExpand_TokenSource?.Cancel();
                autoExpand_TokenSource = null;
            }
        }

        private void PanelView_MouseEnter(object _, EventArgs e)
        {
            lock (tlpPanel)
            {
                autoExpand_TokenSource?.Cancel();
                autoExpand_TokenSource = null;
            }
        }

        private void PanelView_MouseLeave(object _, EventArgs e)
        {
            lock (tlpPanel)
            {
                if (AutoExpand)
                {
                    if (OpenState == OpenState.Open && autoExpand_TokenSource == null)
                    {
                        autoExpand_TokenSource = this.ScheduleCancelableAction(AutoCollapseDelay, processCollapse_Action);
                    }
                    else
                    {
                        autoExpand_TokenSource = null;
                    }
                }
            }
        }

        /// <summary>
        /// Handler that will set up a context menu for a right click on the tabs,
        /// allowing for the user to close a connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceTree_MouseEnter(object _, EventArgs e)
        {// If we are in the Tab area, we want our context menu to display close and on select run the close handler
            lock (tlpPanel)
            {
                ContextMenuStrip contextMenu;
                lock (ContextMenuStrip)
                {
                    switch (OpenState)
                    {
                        case OpenState.Open:
                            if (deviceTreeContextMenu_Open.TryGetInstance(out contextMenu))
                            {
                                ContextMenuStrip = contextMenu;
                            }
                            break;
                        case OpenState.Closed:
                            if (deviceTreeContextMenu_Closed.TryGetInstance(out contextMenu))
                            {
                                ContextMenuStrip = contextMenu;
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Handler that clears the context menu if we leave the tab space
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceTree_MouseLeave(object _, EventArgs e)
        {// If we leave the tab area, we no longer want the menu
            lock (tlpPanel)
            {
                lock (ContextMenuStrip)
                {
                    ContextMenuStrip = new ContextMenuStrip();
                }
            }
        }
        #endregion

        #endregion /Control Event Handler

        #region Dispose
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (components != null))
                {
                    btnToggleExpand_Top.MouseEnter -= panelButtonColumn_MouseEnter_Handler;
                    btnToggleExpand_Bottom.MouseEnter -= panelButtonColumn_MouseEnter_Handler;
                    tvwDeviceView.MouseEnter -= PanelViewArea_MouseEnter_Handler;
                    tvwDeviceView.MouseLeave -= PanelViewArea_MouseLeave_Handler;

                    chkScanToggle.CheckedChanged -= chkScanToggleCheckedChanged_Handler;
                    tvwDeviceView.AfterExpand -= resizePanelView_Handler;
                    tvwDeviceView.AfterCollapse -= resizePanelView_Handler;
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
