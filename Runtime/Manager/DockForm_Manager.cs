using Common;
using Datam.WinForms.Interface;
using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Runtime
{
    public static class Manager_Form
    {
        #region Identity
        public const string ClassName = nameof(Manager_Form);
        #endregion

        #region Dock Change Event 
        private static readonly EventHandler DockStateChangeHandler = new EventHandler(OnDockStateChanged);

        /// <summary>
        /// This Event Handler handles the resizing of any docked form to allow scrollbars to automatically appear
        /// -=CAH=-
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnDockStateChanged(object sender, EventArgs e)
        {
            IDockForm form = sender as IDockForm;
            // -=CAH=- GetMinWidth and Height have the borders included, so for the docked tableLayoutPanel size comparison, need to subtract borders
            bool smallerThanMinWidth = form.Self.Width < form.MinSize_Float.Width;// True if the form is smaller than its docked windows width
            bool smallerThanMinHeight = form.Self.Height < form.MinSize_Float.Height;// True if the form is smaller than its docked windows height
            if (!smallerThanMinWidth && !smallerThanMinHeight)// 
            {
                form.HostPanel.Size = DockSize; 
            }
            else if (smallerThanMinWidth && !smallerThanMinHeight)
            {
                form.HostPanel.Anchor = AnchorDockLeft;
                form.HostPanel.Width = form.HostPanel.MinimumSize.Width;
                form.HostPanel.Height = DockSize.Height; //form.Self.Height - (MainHeight / 2); // subtract one border so autoscroll bar fits
            }
            else if (!smallerThanMinWidth && smallerThanMinHeight)
            {
                form.HostPanel.Anchor = AnchorDockTop;
                form.HostPanel.Width = DockSize.Width; // subtract borders so autoscroll bar fits
                form.HostPanel.Height = form.HostPanel.MinimumSize.Height;
            }
            else// If both conditions are true
            {
                form.HostPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                form.HostPanel.Size = form.HostPanel.MinimumSize;
            }
        }
        #endregion

        #region Settings Update
        public static void UpdateMainDimensions(Size mainSize, Size dockSize, Size clientSize)
        {
            MainSize = mainSize;
            DockSize = dockSize;
            BorderSize = mainSize - clientSize;
        }
        #endregion

        #region Frame Sizes

        #region Main
        public static Size MainSize { get; private set; }
        public static int MainWidth
        {
            get
            {
                return MainSize.Width;
            }
        }
        public static int MainHeight
        {
            get
            {
                return MainSize.Height;
            }
        }
        #endregion

        #region Dock
        public static Size DockSize { get; private set; }
        public static int DockWidth
        {
            get
            {
                return DockSize.Width;
            }
        }
        public static int DockHeight
        {
            get
            {
                return DockSize.Height;
            }
        }
        #endregion

        #region Border
        public static Size BorderSize { get; private set; }
        public static int BorderWidth
        {
            get
            {
                return BorderSize.Width;
            }
        }
        public static int BorderHeight
        {
            get
            {
                return BorderSize.Height;
            }
        }
        #endregion

        #endregion

        #region Dock Areas 
        public static ComboBoxItem[] DockAreas
        {
            get
            {
                return Translation_Manager.ComboBoxItems_DockAreas;
            }
        }
        #endregion

        #region Anchor Styles
        //Anchor styles for various GetDockStates
        public const AnchorStyles AnchorDockLeft = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom);
        public const AnchorStyles AnchorDockRight = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
        public const AnchorStyles AnchorDockTop = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
        public const AnchorStyles AnchorDockBottom = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
        public const AnchorStyles anchorAll = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
        #endregion

        #region Border Adjust
        /// <summary>
        /// This method handles the default sizing for DockContents when they are moved to a different dock state
        /// -=CAH=-
        /// </summary>
        /// <param name="form"></param>
        public static void BorderAdjust(IDockForm form)
        {
            //form.Self.Resize -= DockStateChangeHandler;//de-register event while default sizing goes into place
            if (form.DockState != DockState.Float)
            {
                //bool smallerThanMinWidth = form.Self.Width < form.MinSize_Float.Width;// True if the form is smaller than its docked windows width
                //bool smallerThanMinHeight = form.Self.Height < form.MinSize_Float.Height;// True if the form is smaller than its docked windows height
                //-=CAH=-Step 0: Register and de-register appropriate events:
                //-=CAH=-Step 1: Get rid of form's min size
                //form.Self.MinimumSize = new Size(0, 0);
                //Step 2: Set table min size equal to float min size minus the borders
                //form.HostPanel.MinimumSize = form.MinSize;// Set the main layout panel to not be able to shrink further than our min size
                //form.Self.Resize += DockStateChangeHandler;//re-register event
                form.Self.MaximumSize = DockSize; 
            }
            else
            {
                // -=CAH=-Step 1: Remove table's minimum size:
                // Form.GetParentTablePanel.MinimumSize = new Size(0, 0);
                // -=CAH=-Step 2: Set float window's minimum size:
                form.GetFloatPane.FloatWindow.MinimumSize = form.MinSize_Float;
            }
        }
        #endregion

        #region Close Form
        /// <summary>
        /// This method should be used to close a window that possibly in an unstable state.
        /// </summary>
        /// <param name="window"></param>
        public static void CloseWindow(Form window)
        {
            try
            {
                lock (window)
                {
                    if (window != null)
                    {
                        if (window.IsHandleCreated)
                        {
                            window.Dispose();
                        }
                    }
                }
            }
            finally
            {
                Application.DoEvents();
            }
        }
        #endregion /Close Form
    }
}
