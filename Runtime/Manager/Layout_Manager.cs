using Common.Constant;
using Common.Extensions;
using Datam.WinForms.Interface;
using Devices.Interface;
using Runtime.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Runtime
{
    /// <summary>
    /// This class is intended to save the users window layout for devices
    /// </summary>
    public static class Manager_Layout
    {
        #region Identity
        public static string ClassName = nameof(Manager_Layout);
        #endregion

        #region Static Accessors
        private static LayoutData layoutData;
        private static Dictionary<String, HashSet<String>> DictDeviceName_FormNames
        {
            get
            {
                return layoutData.DictDeviceName_FormNames;
            }
            set
            {
                layoutData.DictDeviceName_FormNames = value;
            }
        }

        private static Dictionary<String, Dictionary<DockAreas, Double>> DictDeviceName_DockArea_AreaSize
        {
            get
            {
                return layoutData.DictDeviceName_DockArea_AreaSize;
            }
            set
            {
                layoutData.DictDeviceName_DockArea_AreaSize = value;
            }
        }

        private static Dictionary<String, Dictionary<DockState, Double>> DictDeviceName_DockState_AreaSize
        {
            get
            {
                return layoutData.DictDeviceName_DockState_AreaSize;
            }
            set
            {
                layoutData.DictDeviceName_DockState_AreaSize = value;
            }
        }
        #endregion /Static Accessors

        #region Constants
        private static readonly String dockStateLayoutFileName = Path.Combine(Tokens.BINARIES_PATH, "L.bin");
        private static readonly Size MinFormSize = new(300, 300);
        #endregion /Constants

        #region Time Constants
        private static readonly TimeSpan saveTimeout = TimeSpan.FromSeconds(5);
        #endregion

        #region Dock Style
        public static DockState GetDockStateFromDockStyleInt(int dockStyle)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(GetDockStateFromDockStyleInt));
            if (dockStyle > 0 && dockStyle < 6)
            {
                return GetDockStateFromDockStyle((DockStyle)dockStyle);
            }
            return DockState.Document;
        }

        public static DockState GetDockStateFromDockStyle(DockStyle dockStyle)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(GetDockStateFromDockStyle));
            switch (dockStyle)
            {
                case DockStyle.Fill:
                    return DockState.Document;
                case DockStyle.Left:
                    return DockState.DockLeft;
                case DockStyle.Top:
                    return DockState.DockTop;
                case DockStyle.Bottom:
                    return DockState.DockBottom;
                case DockStyle.Right:
                    return DockState.DockRight;
                default:
                    Log_Manager.IssueDebugAlert("DockState could not be inferred from DockStyle.");
                    return DockState.Document;
            }
        }
        #endregion /Dock Style

        #region Main Form
        private static DockPanel MainDockPanel;//TODO: accessor

        public static event EventHandler DockSizeChanged
        {
            add
            {
                lock (MainDockPanel)
                {
                    MainDockPanel.DockChanged += value;
                }
            }
            remove
            {
                lock (MainDockPanel)
                {
                    MainDockPanel.DockChanged -= value;
                }
            }
        }
        #endregion

        #region Open Forms
        public static string[] GetOpenFormNames(ICommunicatorDevice device)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(GetOpenFormNames));
            if (DictDeviceName_FormNames.TryLookup(device.DisplayName, out HashSet<string> formNames))
            {
                return formNames.ToArray();
            }
            return new string[0];
        }

        private static void AddOpenForm(ICommunicatorDevice device, IDockForm form)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(AddOpenForm));
            if (DictDeviceName_FormNames.TryLookup(device.DisplayName, out HashSet<string> formNames))
            {
                formNames.Add(form.GetType().Name);
            }
            else if(!DictDeviceName_FormNames.TryAddOrUpdate(device.DisplayName, 
                new HashSet<string>() { form.GetType().Name }))
            {
                Log_Manager.IssueDebugAlert($"Set Dockstate failed on {device.DisplayName} for form {form.GetType().Name}");
            }
        }

        public static void AddOpenForms(ICommunicatorDevice device, DockPanel panel)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(AddOpenForms));
            try
            {
                foreach (DockContent content in panel.Contents)
                {
                    if (content is IDockForm form)
                    {
                        AddOpenForm(device, form);
                    }
                }
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);
            }
        }
        #endregion

        #region Area Size

        public static bool TryGetAreaSize_Factor(ICommunicatorDevice device, DockAreas area, out double factor)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(TryGetAreaSize_Factor));
            if (DictDeviceName_DockArea_AreaSize.TryLookup(device.DisplayName, out Dictionary<DockAreas, double> dictAreaSize))
            {
                if (dictAreaSize.TryLookup(area, out factor))
                {
                    return true;
                }
            }
            factor = 0;
            return false;
        }

        public static bool TryGetAreaSize_Factor(ICommunicatorDevice device, DockState area, out double factor)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(TryGetAreaSize_Factor));
            if (DictDeviceName_DockState_AreaSize.TryLookup(device.DisplayName, out Dictionary<DockState, double> dictAreaSize))
            {
                if (dictAreaSize.TryLookup(area, out factor))
                {
                    return true;
                }
            }
            factor = 0;
            return false;
        }

        public static bool TryLookupAreaSize(ICommunicatorDevice device, DockState area, out Size size)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(TryLookupAreaSize));
            switch(area)
            {
                case DockState.Document:
                    return TryLookupAreaSize_Document(device, out size);
                case DockState.DockTop:
                    return TryLookupAreaSize_Top(device, out size);
                case DockState.DockBottom:
                    return TryLookupAreaSize_Bottom(device, out size);
                case DockState.DockLeft:
                    return TryLookupAreaSize_Left(device, out size);
                case DockState.DockRight:
                    return TryLookupAreaSize_Right(device, out size);
                default:
                    size = MinFormSize;
                    return true;
            }
        }

        public static Size DetermineAreaSize(this DockState area)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(DetermineAreaSize));
            if (MainDockPanel != null)
            {
                double top = MainDockPanel.DockTopPortion;
                double bottom = MainDockPanel.DockBottomPortion;
                double left = MainDockPanel.DockLeftPortion;
                double right = MainDockPanel.DockRightPortion;
                switch (area)
                {
                    case DockState.Document:
                        return DetermineAreaSize_Document(left, right, top, bottom);
                    case DockState.DockTop:
                        return DetermineAreaSize_Height(left, right, top);
                    case DockState.DockBottom:
                        return DetermineAreaSize_Height(left, right, bottom);
                    case DockState.DockLeft:
                        return DetermineAreaSize_Width(left, top, bottom);
                    case DockState.DockRight:
                        return DetermineAreaSize_Width(right, top, bottom);
                }
            }
            return MinFormSize;
        }

        #region Height

        #region Top
        public static bool TryLookupAreaSize_Top(ICommunicatorDevice device, out Size size)
        {
            if (DictDeviceName_DockArea_AreaSize.TryLookup(device.DisplayName, out Dictionary<DockAreas, Double> dictAreaSize))
            {
                if (dictAreaSize.TryLookup(DockAreas.DockTop, out double height_dbl) &&
                    dictAreaSize.TryLookup(DockAreas.DockLeft, out double width_left_dbl) &&
                     dictAreaSize.TryLookup(DockAreas.DockRight, out double width_right_dbl))
                {
                    size = DetermineAreaSize_Height(width_left_dbl, width_right_dbl, height_dbl);
                    return true;
                }
            }
            size = new Size();
            return false;
        }
        #endregion /Top

        #region Bottom
        public static bool TryLookupAreaSize_Bottom(ICommunicatorDevice device, out Size size)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(TryGetAreaSize_Factor));
            if (DictDeviceName_DockArea_AreaSize.TryLookup(device.DisplayName, out Dictionary<DockAreas, double> dictAreaSize))
            {
                if (dictAreaSize.TryLookup(DockAreas.DockBottom, out double height_dbl) &&
                    dictAreaSize.TryLookup(DockAreas.DockLeft, out double width_left_dbl) &&
                     dictAreaSize.TryLookup(DockAreas.DockRight, out double width_right_dbl))
                {

                    size = DetermineAreaSize_Height(width_left_dbl, width_right_dbl, height_dbl);
                    return true;
                }
            }
            size = new Size();
            return false;
        }
        #endregion /Bottom

        #region Common
        private static Size DetermineAreaSize_Height(double width_left_dbl, double width_right_dbl, double height_dbl)
        {
            int height_int;
            int width_int = 0;
            IsPanelPresent(out bool top, out bool bottom, out bool left, out bool right);
            if (height_dbl < 1)
            {// Its a percentage
                double height_actual = MainDockPanel.Size.Height * height_dbl;
                height_int = Convert.ToInt32(Math.Ceiling(height_actual));
            }
            else
            {// In pixels
                height_int = Convert.ToInt32(Math.Ceiling(height_dbl));
            }
            if (left)
            {// Remove left portion.
                if (width_left_dbl < 1)
                {// Its a percentage
                    double width_actual = MainDockPanel.Size.Width * width_left_dbl;
                    width_int += Convert.ToInt32(Math.Floor(width_actual));
                }
                else
                {// In pixels
                    width_int += Convert.ToInt32(Math.Floor(width_left_dbl));
                }
            }
            if (right)
            {// Remove right portion.
                if (width_right_dbl < 1)
                {// Its a percentage
                    double width_actual = MainDockPanel.Size.Width * width_right_dbl;
                    width_int += Convert.ToInt32(Math.Floor(width_actual));
                }
                else
                {// In pixels
                    width_int += Convert.ToInt32(Math.Floor(width_right_dbl));
                }
            }
            return new Size(MainDockPanel.Size.Width - width_int, height_int);
        }
        #endregion

        #endregion Height

        #region Width

        #region Left
        public static bool TryLookupAreaSize_Left(ICommunicatorDevice device, out Size size)
        {
            if (DictDeviceName_DockArea_AreaSize.TryLookup(device.DisplayName, out Dictionary<DockAreas, double> dictAreaSize))
            {
                if (dictAreaSize.TryLookup(DockAreas.DockLeft, out double width_dbl) &&
                    dictAreaSize.TryLookup(DockAreas.DockTop, out double height_top_dbl) &&
                     dictAreaSize.TryLookup(DockAreas.DockBottom, out double height_bottom_dbl))
                {
                    size = DetermineAreaSize_Width(width_dbl, height_top_dbl, height_bottom_dbl);
                    return true;
                }
            }
            size = new Size();
            return false;
        }

        #endregion /Left

        #region Right
        public static bool TryLookupAreaSize_Right(ICommunicatorDevice device, out Size size)
        {
            if (DictDeviceName_DockArea_AreaSize.TryLookup(device.DisplayName, out Dictionary<DockAreas, double> dictAreaSize))
            {
                if (dictAreaSize.TryLookup(DockAreas.DockRight, out double width_dbl) &&
                    dictAreaSize.TryLookup(DockAreas.DockTop, out double height_top_dbl) &&
                     dictAreaSize.TryLookup(DockAreas.DockBottom, out double height_bottom_dbl))
                {
                    size = DetermineAreaSize_Width(width_dbl, height_top_dbl, height_bottom_dbl);
                    return true;
                }
            }
            size = new Size();
            return false;
        }
        #endregion

        #region Common
        private static Size DetermineAreaSize_Width(double width_dbl, double height_top_dbl, double height_bottom_dbl)
        {
            int width_int;
            int height_int = 0;
            IsPanelPresent(out bool top, out bool bottom, out _, out _);
            if (width_dbl < 1)
            {// Its a percentage.
                double height_actual = MainDockPanel.Size.Width * width_dbl;
                width_int = Convert.ToInt32(Math.Ceiling(height_actual));
            }
            else
            {// In pixels.
                width_int = Convert.ToInt32(Math.Ceiling(width_dbl));
            }
            if (top)
            {// Remove top section
                if (height_top_dbl < 1)
                {// Its a percentage.
                    double height_actual = MainDockPanel.Size.Height * height_top_dbl;
                    height_int += Convert.ToInt32(Math.Floor(height_actual));
                }
                else
                {// In pixels.
                    height_int += Convert.ToInt32(Math.Floor(height_top_dbl));
                }
            }
            if (bottom)
            {// Remove bottom section
                if (height_bottom_dbl < 1)
                {// Its a percentage.
                    double height_actual = MainDockPanel.Size.Height * height_bottom_dbl;
                    height_int += Convert.ToInt32(Math.Floor(height_actual));
                }
                else
                {// In pixels.
                    height_int += Convert.ToInt32(Math.Floor(height_bottom_dbl));
                }
            }
            return new Size(width_int, MainDockPanel.Size.Height - height_int);
        }
        #endregion

        #endregion /Width

        #region Document
        public static bool TryLookupAreaSize_Document(ICommunicatorDevice device, out Size size)
        {
            if (DictDeviceName_DockArea_AreaSize.TryLookup(device.DisplayName, out Dictionary<DockAreas, double> dictAreaSize))
            {
                if (dictAreaSize.TryLookup(DockAreas.DockLeft, out double width_left_dbl) &&
                    dictAreaSize.TryLookup(DockAreas.DockLeft, out double width_right_dbl) &&
                    dictAreaSize.TryLookup(DockAreas.DockTop, out double height_top_dbl) &&
                    dictAreaSize.TryLookup(DockAreas.DockBottom, out double height_bottom_dbl))
                {
                    size = DetermineAreaSize_Document(width_left_dbl, width_right_dbl, height_top_dbl, height_bottom_dbl);
                    return true;
                }
            }
            size = new Size();
            return false;
        }

        private static Size DetermineAreaSize_Document(double width_left_dbl, double width_right_dbl, double height_top_dbl, double height_bottom_dbl)
        {
            int width_int = 0;
            int height_int = 0;
            double width_actual;
            double height_actual;
            IsPanelPresent(out bool top, out bool bottom, out bool left, out bool right);
            if (left)
            {
                if (width_left_dbl < 1)
                {// Its a percentage
                    width_actual = MainDockPanel.Size.Width * width_left_dbl;
                    width_int += Convert.ToInt32(Math.Floor(width_actual));
                }
                else
                {// In pixels
                    width_int += Convert.ToInt32(Math.Floor(width_left_dbl));
                }
            }
            if (right)
            {
                if (width_right_dbl < 1)
                {// Its a percentage
                    width_actual = MainDockPanel.Size.Width * width_right_dbl;
                    width_int += Convert.ToInt32(Math.Floor(width_actual));
                }
                else
                {// In pixels
                    width_int += Convert.ToInt32(Math.Floor(width_right_dbl));
                }
            }
            if (top)
            {
                if (height_top_dbl < 1)
                {// Its a percentage
                    height_actual = MainDockPanel.Size.Height * height_top_dbl;
                    height_int += Convert.ToInt32(Math.Floor(height_actual));
                }
                else
                {// In pixels
                    height_int += Convert.ToInt32(Math.Floor(height_top_dbl));
                }
            }
            if (bottom)
            {
                if (height_bottom_dbl < 1)
                {// Its a percentage
                    height_actual = MainDockPanel.Size.Height * height_bottom_dbl;
                    height_int += Convert.ToInt32(Math.Floor(height_actual));
                }
                else
                {// In pixels
                    height_int += Convert.ToInt32(Math.Floor(height_bottom_dbl));
                }
            }
            return new Size(MainDockPanel.Size.Width - width_int, MainDockPanel.Size.Height - height_int);
        }
        #endregion /Document

        private static void IsPanelPresent(out bool top, out bool bottom, out bool left, out bool right)// TODO run on form add and store statically
        {
            top = bottom = left = right = false;
            foreach(DockPane pane in MainDockPanel.Panes)
            {
                switch(pane.DockState)
                {
                    case DockState.DockTop:
                        top = true;
                        continue;
                    case DockState.DockBottom:
                        bottom = true;
                        continue;
                    case DockState.DockLeft:
                        left = true;
                        continue;
                    case DockState.DockRight:
                        right = true;
                        continue;
                }
                if(top && bottom && left && right)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// This method will set the area size for individual regions for the current device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="panel"></param>
        private static void SetAreaSize(ICommunicatorDevice device, DockPanel panel)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(SetAreaSize));
            Dictionary<DockAreas, double> dictAreaSize = new Dictionary<DockAreas, double>
            {
                [DockAreas.DockTop] = panel.DockTopPortion,
                [DockAreas.DockBottom] = panel.DockBottomPortion,
                [DockAreas.DockLeft] = panel.DockLeftPortion,
                [DockAreas.DockRight] = panel.DockRightPortion
            };
            if (!DictDeviceName_DockArea_AreaSize.TryAddOrUpdate(device.DisplayName, dictAreaSize))
            {
                Log_Manager.IssueDebugAlert($"Set area size failed on {device.DisplayName}");
            }
        }
        #endregion

        #region Save/Load
        private static readonly Mutex updateSavedLayoutLock = new Mutex();
        private static Task[] tasks_UpdateSavedLayout;
        private static CancellationTokenSource tokenSource_UpdateSavedLayout;
        public static async void UpdateSavedLayout(ICommunicatorDevice device, DockPanel panel)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(UpdateSavedLayout));

            DictDeviceName_FormNames.Clear();// The form names of open forms need to be removed, the states do not.
            try
            {
                if (tasks_UpdateSavedLayout != null)
                {
                    updateSavedLayoutLock.WaitOne();
                    tokenSource_UpdateSavedLayout?.Cancel();
                    tokenSource_UpdateSavedLayout = Runtime_Manager.GetTimedTokenSource(saveTimeout);
                    tasks_UpdateSavedLayout = new Task[2]
                    {
                        Task.Run(() => AddOpenForms(device, panel), tokenSource_UpdateSavedLayout.Token),
                        Task.Run(() => SetAreaSize(device, panel), tokenSource_UpdateSavedLayout.Token),
                        //Task.Run(() => SetDockStates(device, panel), tokenSource_UpdateSavedLayout.Token)
                    };
                    await Runtime_Manager.AwaitAll(tasks_UpdateSavedLayout).ConfigureAwait(true);
                    SaveLayout();
                    updateSavedLayoutLock.ReleaseMutex();
                }
            }
            catch (TaskCanceledException)
            {
                Log_Manager.LogDebug(ClassName, "Update of saved layout failed due to class cancellation");
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);
            }
            finally
            {
                tasks_UpdateSavedLayout = null;// Used to ensure only one operation runs at a time.
            }
        }

        public static void SaveAndClearLayout(ICommunicatorDevice device, DockPanel panel)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(UpdateSavedLayout));
            UpdateSavedLayout(device, panel);
            ClearAllForms(device, panel);
        }

        private static void SaveLayout()
        {
            Log_Manager.LogMethodCall(ClassName, nameof(SaveLayout));
            Manager_File.TryBinarySave_BIN(layoutData, dockStateLayoutFileName);
        }
        #endregion Save/Load

        #region Start
        public static void Start(DockPanel dockPanel)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(Start));
            MainDockPanel = dockPanel;
            try
            {
                if (Manager_File.TryBinaryLoad_BIN(dockStateLayoutFileName, out LayoutData layout))
                {
                    Log_Manager.LogDebug(ClassName, "Found prior layout");
                    layoutData = layout;
                }
                else
                {
                    layout = new LayoutData();
                    SaveLayout();
                }
            }
            catch (Exception ex)
            {
                // Previous layout state definition probablly changed
                Log_Manager.IssueDebugAlert(ex.Message);
            }
        }
        #endregion

        #region Clear
        public static void ClearAllForms(ICommunicatorDevice device, DockPanel panel)
        {
            Log_Manager.LogMethodCall(ClassName, nameof(AddOpenForms));
            try
            {
                IDockContent[] contentArray = panel.Contents.ToArray();
                for (int c = 0; c< contentArray.Length; c++)
                {
                    if (contentArray[c] is DockContent content)
                    {
                        content.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);
            }
        }

        /// <summary>
        /// This method will close all forms open in the dock
        /// </summary>
        public static void ClearDock(DockPanel panel)
        {
            int exempt = 0;//counts the number of float windows open
            while (panel.Contents.Count > exempt && Runtime_Manager.Alive)
            {
                if (panel.Contents[exempt] is DockContent)
                {// Just in case some other type else is stored in this container
                    if (panel.Contents[exempt].DockHandler.DockState != DockState.Float)
                    {
                        panel.Contents[exempt].DockHandler.Dispose();//TODO: make method that checks if we still want this there
                    }
                    else
                    {// TODO: TEMP, we are closing everything rn, but can make this for particular forms
                        panel.Contents[exempt].DockHandler.Dispose();
                        //exempt++;//skip this one, it's not docked
                    }
                }
            }
            panel.Refresh();
        }
        #endregion
    }
}
