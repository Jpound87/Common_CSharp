using Common;
using Common.AM_Constant;
using Common.Extensions;
using Datam.Utilities;
using Datam.Utilities.Extensions;
using Device;
using Device.Interface;
using Parameter;
using Parameter.Interface;
using Runtime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unit.Interface;
using UpdateScheduler;
using UpdateScheduler.Interface;

namespace Common.AM_Controls
{
    #region Utilities
    internal static class Utilities_PolymorphicControl
    {
        public static readonly Type Type_PolymorphicControl = typeof(PolymorphicControl);
    }
    #endregion

    internal class PolymorphicControl : Addressable_Base, IComparable, IAddressable, IValidate
    {
        #region Identity
        new private const string ClassName = nameof(PolymorphicControl);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Events
        public event Action<string> ValueUpdated;
        #endregion

        #region Constants
        private const int TEXTBOX_MAX_LENGTH = 20;
        #endregion

        #region Readonly

        private readonly ComboBox comboBox_Scale;
        private readonly bool updateScale;
        private readonly GroupBox gboContainer;
        private readonly Control valueContainer;

        private readonly ComboBox comboBox_Main;
        #endregion

        #region Address Mapping

        #region CiA402 Address Structure
        private readonly static Address_CiA402[] addressSchema_PolyControl = new Address_CiA402[]
        {
             Allied_CiA402_V7.MFR_DATA_SAVE_ADDRESS
        };

        public static IAddress[] RequiredAddresses_CiA402
        {
            get
            {
                return addressSchema_PolyControl;
            }
        }

        private readonly static IDictionary<NetworkType, IAddress[]> protocol_RequiredAddresses = new Dictionary<NetworkType, IAddress[]>()
        {
            { NetworkType.CiA402, RequiredAddresses_CiA402 }
        };

        public override IDictionary<NetworkType, IAddress[]> Protocol_RequiredAddresses
        {
            get
            {
                return protocol_RequiredAddresses;
            }
        }
        #endregion

        #region Address Structure Enumeration
        private enum PolyControl_AddressEnum
        {
            MFR_DATA_SAVE
        }
        #endregion

        #region Parameters
        private IParameter SaveManufactureLabel_Parameter
        {
            get
            {
                return LookupParameter(PolyControl_AddressEnum.MFR_DATA_SAVE);
            }
        }
        #endregion// CiA402 Address Structure

        #endregion

        #region Sort Compatrators
        /// <summary>
        /// This method, intened to be used as a sort comparison, compares two
        /// polymorphic control objects by name.
        /// </summary>
        /// <param name="thee"></param>
        /// <param name="thine"></param>
        /// <returns></returns>
        public static int Compare(PolymorphicControl thee, PolymorphicControl thine)
        {
            if (thee == null)
            {
                if (thine == null)
                {// If thee and thine are null, they're equal.
                    return 0;
                }
                else
                {// If thee is null and thine is not null, thine is greater.
                    return -1;
                }
            }
            else
            {// If thee is not null...
                if (thine == null)
                {// ...and thine is null, thee is greater.
                    return 1;
                }
                else
                {// ...and thine is not null, compare the text
                    int retval = thee.Name.CompareTo(thine.Name);
                    return retval;
                }
            }
        }

        public int CompareTo(object other)
        {
            if (other is PolymorphicControl otherPoly)
            {
                return PolymorphicControl.Compare(this, otherPoly);
            }
            else
            {
                throw new ArgumentException("Object is not a Temperature");
            }
        }
        #endregion

        #region Syncronization
        private CancellationToken controlCancellationToken;
        #endregion

        #region IO State
        public bool IsInput { get; private set; }

        public bool IsOutput { get; private set; }

        private IO DetermineState_IO()
        {
            switch (Parameter.AccessRights)
            {
                case AccessRights.RW:
                case AccessRights.RWR:
                case AccessRights.RWW:
                case AccessRights.WO:
                    IsInput = true;
                    return IO.Input;
                case AccessRights.ALLIED:
                case AccessRights.CONST:
                case AccessRights.RO:
                    IsOutput = true;
                    return IO.Output;
                default:
                    return IO.Unknown;
            }
        }

        #endregion

        #region Value
        private bool awaitingUpdate;

        private string value;
        public string Value
        {
            get
            {
                return value;
            }
            private set
            {
                if(Valid && value != this.value)
                {
                    this.value = value;
                    Parent.Invoke(processValueUpdate);
                }
            }
        }

        private MethodInvoker processValueUpdate;
        private void ProcessValueUpdate()
        {
            switch (ControlType)
            {
                case ControlType.ComboBox:
                    comboBox_Main.SelectValue(value);
                    break;
                case ControlType.TextBox:
                default:
                    valueContainer.Text = value;
                    break;
            }
            if (updateScale)
            {
                if (comboBox_Scale.Text != Parameter.Unit.Abbreviation)
                {
                    comboBox_Scale.SelectValue(Parameter.Unit.Abbreviation);
                }
            }
        }

        public bool SavePassRequired
        {
            get
            {
                return Parameter.SavePassRequired;
            }
        }
        #endregion

        #region Control
        public bool Valid { get; private set; }
        public ControlType ControlType => Parameter.Unit.ControlType;// TODO: expose control type on parameter level.
        public Form Parent { get; private set; }
        public Control HostControl
        {
            get
            {
                return gboContainer;
            }
        }

        private Color foreColor = SettingsManager.Theme_TextColor;
        public Color ForeColor
        {
            get => foreColor;
            set
            {
                if(foreColor != value && value != Color.Transparent)
                {
                    foreColor = value;
                    valueContainer.ForeColor = foreColor;
                }
            }
        }

        private Color backColor = SettingsManager.Theme_Control;
        public Color BackColor
        {
            get => backColor;
            set
            {
                if (backColor != value && value != Color.Transparent)
                {
                    backColor = value;
                    valueContainer.BackColor = backColor;
                }
            }
        }

        public bool Selected { get; private set; }

        #endregion

        #region Parameter
        private IParameter parameter;
        public IParameter Parameter
        {
            get
            {
                return parameter;
            }
            private set
            {
                if (value != parameter)
                {
                    parameter = value;
                    Register();
                }
            }
        }

        public IRegistrationTicket RegistrationTicket { get; set; }

        /// <summary>
        /// This method will register the parameter to be updated from the 
        /// upstream device.
        /// </summary>
        /// <param name="parameter">parameter to register</param>
        private void Register()
        {
            if (parameter != null && parameter != Parameter_CiA402.Null)
            {
                RegistrationTicket = new RegistrationTicket(this, parameter, updateValue);
            }
        }

        public string Name
        {
            get
            {
                return Parameter.Name;
            }
        }

        public string AddressStr
        {
            get
            {
                return Parameter.Address.ToString();
            }
        }

        public string Unit
        {
            get
            {
                return Parameter.Unit.Abbreviation;
            }
        }

        public string GroupName
        {
            get
            {
                return Parameter.Group;
            }
        }

        public string Section
        {
            get
            {
                return Parameter.Section;
            }
        }

        public string Description
        {
            get
            {
                return Parameter.Description;
            }
        }
        #endregion

        #region Size
        public int Width
        {
            get
            {
                return HostControl.Width;
            }
        }

        public int Height
        {
            get
            {
                return HostControl.Height;
            }
        }
        #endregion

        #region Constructor
        public PolymorphicControl(IParameter parameter, Form parent, ICommunicatorDevice device) : base (device)
        {
            //updateValue = parent.MakeInvocation(UpdateValue);
            updateValue = new Action(UpdateValue);
            processValueUpdate = new MethodInvoker(ProcessValueUpdate);
            Parent = parent;
            Device = device;
            Parameter = parameter;
            DetermineState_IO();
            toolTip = new ToolTip()
            {
                InitialDelay = 250,
                AutoPopDelay = Constants.MAX_TOOLTIP_TIME,
                ShowAlways = true,
                ReshowDelay = 50,
                ToolTipTitle = parameter.Name,
                UseAnimation = true
            };
            updateScale = false;
            controlCancellationToken = Manager_Runtime.GetToken(parent);
            gboContainer = new GroupBox
            {
                AutoSize = false,
                Name = Name,
                BackColor = Color.Transparent,
                Text = SavePassRequired ? String.Format("{0} {1}", Settings_UI.Floppy, Name) :
                    Parameter.SavedToFlash ? String.Format("{0} {1}", Settings_UI.File, Name) : Name,
                ForeColor = Parameter.SavedToFlash || SavePassRequired ? Settings_UI.SavedParamTextColor : Settings_UI.GroupBoxForeColor,
                Height = 42
            };
            gboContainer.AdjustWidthToText(200, 24);
            //end temp
            switch (Parameter.Unit.ControlType)
            {// Determine how to display the control
                case ControlType.None:
                    break;
                case ControlType.TextBox:
                    TextBox textBox = new TextBox()
                    {
                        MaxLength = TEXTBOX_MAX_LENGTH
                    };
                    gboContainer.Controls.Add(textBox);
                    valueContainer = textBox;
                    textBox.Dock = DockStyle.Fill;
                    break;
                case ControlType.ComboBox:
                    comboBox_Main = new ComboBox();
                    gboContainer.Controls.Add(comboBox_Main);
                    valueContainer = comboBox_Main;
                    comboBox_Main.Items.AddRange(Parameter.Unit.ValueComboBoxChoices);
                    comboBox_Main.Dock = DockStyle.Fill;
                    break;
                case ControlType.Button:
                    Button button = new Button
                    {
                        Text = Parameter.Name
                    };
                    gboContainer.Controls.Add(button);
                    button.Dock = DockStyle.Fill;
                    break;
                case ControlType.ScaledTextBox:
                    gboContainer.Height = 45; // Needs a bit more room
                    TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
                    {
                        RowCount = 1,
                        ColumnCount = 2
                    };
                    tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
                    TextBox textBoxS = new TextBox()
                    {
                        MaxLength = TEXTBOX_MAX_LENGTH
                    };
                    valueContainer = textBoxS;
                    tableLayoutPanel.Controls.Add(textBoxS, 0, 0);
                    textBoxS.Dock = DockStyle.Fill;
                    comboBox_Scale = new ComboBox();
                    comboBox_Scale.Items.AddRange(Parameter.Unit.MinSetEnumerationChoices);
                    comboBox_Scale.Text = Unit;
                    if (Parameter.Unit.MinSetEnumerationChoices.Length <= 1)
                    {// Don't give the illusion of choice
                        comboBox_Scale.Enabled = false;
                    }
                    comboBox_Scale.TrySelect_Name(Parameter.Unit.Abbreviation);
                    comboBox_Scale.SelectedIndexChanged += new EventHandler(ScaleComboBoxValueChanged);
                    updateScale = true;
                    tableLayoutPanel.Controls.Add(comboBox_Scale, 1, 0);
                    comboBox_Scale.Dock = DockStyle.Fill;
                    gboContainer.Controls.Add(tableLayoutPanel);
                    tableLayoutPanel.Dock = DockStyle.Fill;
                    break;
                    //case ControlTypes.ControlWordBox:
                    //    Utility.ControlWord controlword = new Utility.ControlWord(_device);
                    //    bool[] controlwordArray = controlword.GetWord();
                    //    TableLayoutPanel tableLayoutPanelCW = new TableLayoutPanel();
                    //    tableLayoutPanelCW.RowCount = 2;
                    //    tableLayoutPanelCW.ColumnCount = 8;
                    //    tableLayoutPanelCW.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));

                    //    for (byte b = 0; b < 8; b++)
                    //    {
                    //        tableLayoutPanelCW.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
                    //        RadioButton radioButton = new RadioButton();
                    //        radioButton.Appearance = Appearance.Button;
                    //        radioButton.Text = controlwordArray[b] ? "1" : "0";// If true 1 else 0.
                    //        radioButton.Checked = controlwordArray[b];
                    //        radioButton.Padding = Padding.Empty;
                    //        tableLayoutPanelCW.Controls.Add(radioButton, b, 0);
                    //        radioButton.Dock = DockStyle.Fill;
                    //    }
                    //    for (byte b = 8; b < 16; b++)
                    //    {
                    //        tableLayoutPanelCW.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
                    //        RadioButton radioButton = new RadioButton();
                    //        radioButton.Appearance = Appearance.Button;
                    //        radioButton.Text = controlwordArray[b] ? "1" : "0";// If true 1 else 0.
                    //        radioButton.Checked = controlwordArray[b];
                    //        radioButton.Padding = Padding.Empty;
                    //        tableLayoutPanelCW.Controls.Add(radioButton, b-8, 1);
                    //        radioButton.Dock = DockStyle.Fill;
                    //    }

                    //    gboContainer.Controls.Add(tableLayoutPanelCW);
                    //    tableLayoutPanelCW.Dock = DockStyle.Fill;
                    //    break;
            }
            Valid = valueContainer != null;
            if (IsOutput)
            {
                if (Valid)
                {
                    if (valueContainer is TextBox)
                    {
                        TextBox valueContainerTextBox = valueContainer as TextBox;
                        valueContainerTextBox.ReadOnly = true;
                    }
                    else
                    {
                        valueContainer.Enabled = false;
                    }
                }
            }
            if (Parameter is Parameter_CiA402_BitWord parameterBitWord && valueContainer is TextBox textBo)
            {
                textBo.MaxLength = parameterBitWord.BitLength;
            }
            awaitingUpdate = false;
            IRegistrationTicket ticket = new RegistrationTicket(this, parameter);
            InitToolTip();
            if(Valid)
            {
                Device.ScheduleHighPriorityDataUpdate(Parameter, updateValue);
                valueContainer.GotFocus += new EventHandler(FocusObtained);
                valueContainer.LostFocus += new EventHandler(FocusLost);
                valueContainer.ForeColor = ForeColor;
                valueContainer.BackColor = BackColor;
            }
            ToggleVisibility(false);
            return;
        }
        #endregion

        #region Focus
        private void FocusObtained(object _, EventArgs e)
        {
            lock (valueContainer)
            {
                Selected = true;
            }
        }

        private void FocusLost(object _, EventArgs e)
        {
            lock (valueContainer)
            {
                Selected = false;
            }
        }

        #endregion

        #region Tool Tip
        private readonly ToolTip toolTip;
        /// <summary>
        /// This method adds a tooltip message to the polymorphic control containing
        /// the parameters description and other relevant information. 
        /// </summary>
        private void InitToolTip()
        {
            string toolTipMessage;
            string addy = Parameter.Address.ToString();
            string desc = Parameter.Description;
            if(SavePassRequired)
            {
                toolTipMessage = $"[{Settings_UI.Floppy}] This parameter is saved to flash memory.\nAddress: {addy}\n{desc}";
            }
            else if (Parameter.SavedToFlash)
            {
                toolTipMessage = $"[{Settings_UI.File}] This parameter is saved to flash memory.\nAddress: {addy}\n{desc}";
            }
            else
            {
                toolTipMessage = $"Address: {addy}\n{desc}";
            }
            if (gboContainer != null)
            {
                toolTip.SetToolTip(gboContainer, toolTipMessage);
            }
            if (valueContainer != null)
            {
                toolTip.SetToolTip(valueContainer, toolTipMessage);
            }
            if(comboBox_Scale!=null)
            {
                toolTip.SetToolTip(comboBox_Scale, toolTipMessage);
            }
        }
        #endregion

        #region Color Coding
        /// <summary>
        /// This method is used to color code the authorization levels for Allied level users.
        /// </summary>
        public void ColorCode()
        {
            switch(Parameter.AuthorizationLevel)
            {
                case AuthorizationLevel.Allied:
                    gboContainer.BackColor = Settings_UI.AlliedAuthColor;
                    return;
                case AuthorizationLevel.Authorized:
                    gboContainer.BackColor = Settings_UI.AuthorizedAuthColor;
                    return;
                case AuthorizationLevel.Advanced:
                    gboContainer.BackColor = Settings_UI.ElevatedAuthColor;
                    return;
                case AuthorizationLevel.Standard:
                    gboContainer.BackColor = Settings_UI.StandardAuthColor;
                    return;
                case AuthorizationLevel.Safety:
                    gboContainer.BackColor = Settings_UI.SafetyAuthColor;
                    return;
                case AuthorizationLevel.All:
                    gboContainer.BackColor = Settings_UI.AllAuthColor;
                    return;
            }
        }
        #endregion

        #region Value Update
        private readonly Action updateValue;
        public void UpdateValue()
        {
            if (Valid && !Selected)
            {
                // If the control has a value container and is not curently selected
                UnlatchEventTrigger();
                Value = Parameter.Value_Display;
                ValueUpdated?.Invoke(Parameter.Value_Display);
                LatchEventTrigger();
            }
        }
        #endregion

        #region Event Trigger Latching

        public void LatchEventTrigger()
        {
            if (Parameter.AccessRights != AccessRights.RO)
            {
                if (valueContainer is TextBox textBox)
                {
                    textBox.Leave += new EventHandler(ValueContainerTextChanged);
                    textBox.KeyUp += new KeyEventHandler(ValueContainerEnterPressed);
                }
                else if (valueContainer is ComboBox comboBox)
                {
                    comboBox.Leave += new EventHandler(ValueContainerIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ValueContainerIndexChanged);
                }
            }
        }

        public void UnlatchEventTrigger()
        {
            if (Parameter.AccessRights != AccessRights.RO)
            {
                if (valueContainer is TextBox)
                {
                    valueContainer.Leave -= new EventHandler(ValueContainerTextChanged);
                    valueContainer.KeyUp -= new KeyEventHandler(ValueContainerEnterPressed);
                }
                else if (valueContainer is ComboBox comboBox)
                {
                    comboBox.Leave -= new EventHandler(ValueContainerIndexChanged);
                    comboBox.SelectedIndexChanged -= new EventHandler(ValueContainerIndexChanged);
                }
            }
        }
        #endregion

        #region Value Container
        private async void ValueContainerTextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                try
                {
                    textBox.RemoveSpaceFromLive();
                    await TextBoxValueChanged(textBox);
                }
                catch (Exception ex) when ((ex is ThreadExitException) || (ex is TaskCanceledException))
                {
                    // Do nothing as it is expected when closing forms
                }
                catch (CommunicationException ex)
                {
                    Manager_Data.CommunicationError.HandleException(ex);
                }
            }
        }

        private async void ValueContainerIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                try
                {
                    await ComboBoxValueChanged(comboBox);
                    gboContainer.Focus();// Used to take focus out of text box
                }
                catch (Exception ex) when ((ex is ThreadExitException) || (ex is TaskCanceledException))
                {
                    // Do nothing as it is expected when closing forms
                }
                catch (CommunicationException ex)
                {
                    Manager_Data.CommunicationError.HandleException(ex);
                }
            }
        }

        private void ValueContainerEnterPressed(object _, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                gboContainer.Focus();// Used to take focus out of text box
            }
        }
        #endregion

        #region TextBox
        private async Task TextBoxValueChanged(TextBox textBox)
        {
            if (Parameter.Value_Display != textBox.Text)
            {
                bool success = false;
                if (Parameter.IsString)
                {
                    success = await Device.SendUpdate_TextBox_Async(textBox, Parameter);
                }
                else if (Parameter.Unit.ControlType == ControlType.ScaledTextBox)
                {
                    if (Double.TryParse(textBox.Text, out double value))
                    {
                        if (Utilities_Parameter.TryGetNumericTypeMaxSize(Parameter.Type, out double maxSize) && value > maxSize)
                        {
                            textBox.Text = Parameter.Type.ReturnValueTypeFormatted(maxSize.ToString());
                        }
                        success = await Device.SendUpdate_TextBox_AsScalar_Async(textBox, comboBox_Scale, Parameter);
                    }
                }
                else if (Parameter.IsNumeric)
                {
                    if(Parameter is Parameter_CiA402_BitWord)
                    {
                        string intValueStr = Convert.ToInt32(textBox.Text, 2).ToString();
                        success = await textBox.UpdateFlashReturnSuccess(textBox.BackColor, Device.VerifiedSendData_Async(Parameter, intValueStr));
                    }
                    else if (Double.TryParse(textBox.Text, out double value))
                    {
                        if (Utilities_Parameter.TryGetNumericTypeMaxSize(Parameter.Type, out double maxSize) && value > maxSize)
                        {
                            textBox.Text = Parameter.Type.ReturnValueTypeFormatted(maxSize.ToString());
                        }
                        success = await Device.SendUpdate_TextBox_AsNumeric_Async(textBox, Parameter);
                    }
                }
                else
                {
                    success = await Device.SendUpdate_TextBox_AsNumeric_Async(textBox, Parameter);
                }
                if (success)
                {
                    if(SavePassRequired)
                    {
                        DialogResult result = MessageBox.Show("Save command must be issued to set this parameter.\nWould you like to send it?",
                            "Manufacturer Save Key", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        switch (result)
                        {
                            case DialogResult.Yes:
                                if(!await Device.VerifiedSendData_Async(SaveManufactureLabel_Parameter, Constants.FLASH_SAVE_KEY))
                                {
                                    Manager_Data.LogMethodCall(ClassName, nameof(TextBoxValueChanged));
                                    Manager_Data.LogError(ClassName, "Manufacturer Save Key send failed.");
                                }
                                break;
                        }
                    }
                }
                else
                {
                    Parent.Invoke(updateValue);
                }
            }
        }
        #endregion

        #region ComboBox
        private async Task ComboBoxValueChanged(ComboBox comboBox)
        {
            if (comboBox.SelectedIndex != -1 && comboBox.SelectedItem is ComboBoxItem comboBoxItem)
            {
                if (Parameter.Value_Display != comboBoxItem.Value.ToString())
                {
                    bool success = await UpdateAsync(Device.SendUpdate_ComboBox_Async(comboBox, Parameter));
                    if (!success)
                    {
                        Manager_Runtime.Context.Post(state =>
                        {
                            UnlatchEventTrigger();
                            Value = Parameter.Value_Display;// TODO: return old value
                            LatchEventTrigger();
                        }, null);
                    }
                }
            }
        }

        /// <summary>
        /// This helper method adds the registered control to a update blocking set
        /// and awaits the update response from the device. Once the response is received
        /// the registrant is removed from the blocking set.
        /// </summary>
        /// <param name="registrant">The control to be registered as awaiting an update</param>
        /// <param name="awaitTask">The boolean returning update task to await</param>
        private async Task<bool> UpdateAsync(Task<bool> awaitTask)
        {
            try
            {
                if (!awaitingUpdate)
                {
                    awaitingUpdate = true;
                }
                await awaitTask;
            }
            finally
            {
                awaitingUpdate = false;
            }
            return awaitTask.Result;
        }

        private void ScaleComboBoxValueChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cboUnitScale)
            {
                Parameter_CiA402.UpdateParameterDisplayScaling(cboUnitScale, Parameter);
            }
            Parent.Invoke(updateValue);
            gboContainer.Focus();// Take focus out of the combobox
        }

        public void EnableUnit(bool enabled)
        {
            if (comboBox_Scale != null)
            {
                if (!enabled)
                {
                    if (comboBox_Scale.Text != Parameter.Unit.Abbreviation)
                    {
                        comboBox_Scale.TrySelect_Name(Parameter.Unit.Abbreviation);
                    }
                }
                comboBox_Scale.Enabled = enabled;
            }
        }
        #endregion

        #region Visibility
        public void ToggleVisibility(bool visible)
        {
            HostControl.Visible = visible;
            if(valueContainer != null)
            {
                valueContainer.Visible = visible;
            }
            if(comboBox_Scale != null)
            {
                comboBox_Scale.Visible = visible;
            }
        }
        #endregion
    }
}

