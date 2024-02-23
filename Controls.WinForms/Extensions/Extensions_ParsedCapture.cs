using AM_WinForms.Datam.Extensions;
using Common.Constant;
using Common.Exceptions;
using Common.Extensions;
using Common.Struct;
using Connections.Exceptions;
using Datam.WinForms.Constant;
using Datam.WinForms.Interface;
using OxyPlot;
using Parameters.Extensions;
using Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datam.WinForms.Extensions
{
    public static class Extensions_ParsedCapture
    {
        #region Constant
        private const String TRUE = "T";
        private const String FALSE = "F";

        private const int DGD_VISIBLE_COLUMN = Layout_DataMonitor_Datagrid.VISIBLE_COLUMN;
        private const int DGD_NAME_COLUMN_INDEX = Layout_DataMonitor_Datagrid.NAME_COLUMN_INDEX;
        private const int DGD_COLOR_COLUMN = Layout_DataMonitor_Datagrid.COLOR_COLUMN;
        private const int DGD_TYPE_COLUMN = Layout_DataMonitor_Datagrid.TYPE_COLUMN;
        private const int DGD_AXIS_COLUMN = Layout_DataMonitor_Datagrid.AXIS_COLUMN;
        #endregion /Constant

        #region DataGridView 
        /// <summary>
        /// This method updates the DataGridViews in frmAnalyze to match the variable and axis information given by the passed in ParsedCapture
        /// </summary>
        /// <param name="pc">ParsedCapture to read the data from</param>
        public static void MakeDataGridViewFromPlot(this ICaptureObject pc, DataGridView dataGridView, OxyColor[] variableOxyColors)
        {
            //Clear the old DataGridViews
            dataGridView.Rows.Clear();
            
            //Read the active vars of pc
            foreach (uint activeIndex in pc.GetActiveIndicies())
            {
                if(!pc.TryAddVariableGridViewRow(activeIndex, variableOxyColors[activeIndex], dataGridView))
                {
                    //TODO: alert error
                    break;// No use continuing unless we fix w/e
                }
            }
        }

        /// <summary>
        /// This method takes in an axis, OxyColor, variable Name, and variable type and populates the DataGridView with a new row containing this information.
        /// The DataGridView that gets populated depends on what axis is entered. If the row already exists on the opposite DataGridView, it is moved to the new
        /// DataGridView that corresponds to the input axis.
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="varColor"></param>
        /// <param name="varName"></param>
        /// <param name="varType"></param>
        public static bool TryAddVariableGridViewRow(this ICaptureObject pc, uint activeIndex, OxyColor varColor, DataGridView dataGridView)
        {
            if (pc.ActiveVariables < 5)//Check if there's already 4 vars (this number has to be 5 bcuz we check this AFTER params have been added to the axis.
            {
                string varName = pc[activeIndex].Name;
                string varType = pc[activeIndex].Type;
                PlotAxis axis = pc[activeIndex].Axis;
                if (dataGridView.TryGetRow(DGD_NAME_COLUMN_INDEX, varName, out DataGridViewRow row))
                {// If the data grid view already contains the parameter:
                    SwitchPlotAxis(row, axis);
                }
                else
                {// If the data grid view doesn't already contain the parameter:
                    dataGridView.Rows.Add(TRUE, varName, String.Empty, varType);
                    int end = dataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible);//Get index of last visible row
                    dataGridView.Rows[end].Cells[DGD_COLOR_COLUMN].Style.BackColor = varColor.ColorFromOxyColor();//Change the background of the second cell to reflect the correct color
                    dataGridView.Rows[end].Cells[DGD_AXIS_COLUMN].Value = axis == PlotAxis.Left ? Translation_Manager.Left : Translation_Manager.Right;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method takes in an axis, OxyColor, variable Name, and variable type and populates the DataGridView with a new row containing this information.
        /// The DataGridView that gets populated depends on what axis is entered. If the row already exists on the opposite DataGridView, it is moved to the new
        /// DataGridView that corresponds to the input axis.
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="varColor"></param>
        /// <param name="variableName"></param>
        /// <param name="variableType"></param>
        public static bool TryAddDataGridViewRow(this ParsedCapture pc, PlotAxis axis, String variableName, String variableType, OxyColor varColor, DataGridView dataGridView)
        {
            if (pc.ActiveVariables < 5)//Check if there's already 4 vars (this number has to be 5 bcuz we check this AFTER params have been added to the axis.
            {
                if (dataGridView.TryGetRow(DGD_NAME_COLUMN_INDEX, variableName, out DataGridViewRow row))
                {// If the data grid view already contains the parameter:
                    SwitchPlotAxis(row, axis);
                }
                else
                {// If the data grid view doesn't already contain the parameter:
                    dataGridView.Rows.Add(TRUE, variableName, String.Empty, variableType);
                    int end = dataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible);//Get index of last visible row
                    dataGridView.Rows[end].Cells[DGD_COLOR_COLUMN].Style.BackColor = varColor.ColorFromOxyColor();//Change the background of the second cell to reflect the correct color
                    dataGridView.Rows[end].Cells[DGD_AXIS_COLUMN].Value = axis == PlotAxis.Left ? Translation_Manager.Left : Translation_Manager.Right;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method switches a variable from one axis to another. The input axis is the current axis, and the function will switch it to the opposite axis. It also updates the DataGridView accordingly
        /// </summary>
        /// <param name="name">Variable name to switch</param>
        /// <param name="newAxis">Axis to move the variable to</param>
        public static void SwitchAxis(this ParsedCapture pc, DataGridView dataGridView, CaptureState_Datam state, String name, PlotAxis newAxis, CheckBox[] signalCheckBoxes)
        {
            try
            {
                //The rules of this if statement are: If there is not a current capture, AND if the variable name isn't on any axis on the graph, the user can switch the variable back and forth between axes
                //If there's a capture and the variable doesn't already exist on the capture, alert the user that the current capture needs to be cleared before the axes/axex variables can be modified
                if (state != CaptureState_Datam.HasCapture)// || (State == CAPTURE_STATE.HasCapture && ASI.parsedCapture.dictVarNameToIndex.ContainsKey(varName)))
                {
                    if (dataGridView.TryGetRow(DGD_NAME_COLUMN_INDEX, name, out DataGridViewRow row))
                    {
                        SwitchPlotAxis(row, newAxis);
                    }
                    pc.UpdateVisibleAxisParams(dataGridView, signalCheckBoxes);
                }
            }
            catch (Exception ex)
            {
                ex.LogCaughtException();
                //Log_Manager.LogError(FormName, $"An exception was encountered when swapping axes. Exception Message: {e.Message}.");
            }
        }

        public static void SwitchPlotAxis(DataGridViewRow row, PlotAxis newAxis)
        {
            if (row != null) //Check that a row exists in the source DataGridView
            {
                if (row.Cells[DGD_AXIS_COLUMN] is DataGridViewComboBoxCell dgd_cbo)
                {
                    if (dgd_cbo.Items.Count > 0 && dgd_cbo.Value is PlotAxis axis)
                    {
                        if (axis != newAxis)
                        {
                            dgd_cbo.TrySelect_Enumeration(newAxis);
                        }
                    }
                }
            }
        }

        public static void SwitchPlotAxis(this ParsedCapture pc, DataGridView dataGridView, DataGridViewRow row, CheckBox[] signalCheckBoxes)
        {
            if (row != null) //Check that a row exists in the source DataGridView
            {
                if (row.Cells[DGD_AXIS_COLUMN].Value is string axis)
                {
                    string parameterName = row.Cells[DGD_NAME_COLUMN_INDEX].Value.ToString();//Get the parameter name
                    if (String.IsNullOrWhiteSpace(axis) || String.IsNullOrWhiteSpace(parameterName))
                    {// We must delete the outsider, fullstop.
                        dataGridView.Rows.Remove(row);
                    }
                    else if (pc.dictVarNameToIndex.TryGetValue(parameterName, out uint variableIndex))//Double check that parsedCapture contains the param name (check that DataGridView and actual plot aren't out of sync)
                    {
                        switch (axis)
                        {
                            default:
                            case Translation_Manager.LEFT:
                                pc.SetAxis(variableIndex, PlotAxis.Right);// Switch it!
                                row.Cells[DGD_AXIS_COLUMN].Value = Translation_Manager.RIGHT;
                                break;
                            case Translation_Manager.RIGHT:
                                pc.SetAxis(variableIndex, PlotAxis.Left);// Switch it!
                                row.Cells[DGD_AXIS_COLUMN].Value = Translation_Manager.LEFT;
                                break;
                        }
                        pc.UpdateVisibleAxisParams(dataGridView, signalCheckBoxes);
                    }
                    else //TODO: unite with top condition...
                    {// We must delete the outsider, fullstop.
                        dataGridView.Rows.Remove(row);
                    }
                }
                
            }
        }

        /// <summary>
        /// This method updates what parameters are visible on the graph based on the check state of the DataGridView Rows
        /// </summary>
        public static void UpdateVisibleAxisParams(this ParsedCapture pc, DataGridView dataGridView, CheckBox[] signalCheckBoxes)
        {
            lock (dataGridView)
            {
                try
                {
                    dataGridView.Enabled = false;
                    dataGridView.EndEdit();// Need to do this so we can actually read the state of the CheckBox in column 1
                    string parameterName;
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        DataGridViewRow row = dataGridView.Rows[i];//Copy the row so we can read the value
                        parameterName = row.Cells[DGD_NAME_COLUMN_INDEX].Value.ToString();//Get the parameter name
                        if (pc.dictVarNameToIndex.TryGetValue(parameterName, out uint index))//Double check that parsedCapture contains the param name (check that DataGridView and actual plot aren't out of sync)
                        {
                            if (row.Cells[DGD_VISIBLE_COLUMN].Value != null)
                            {
                                bool visible = row.Cells[DGD_VISIBLE_COLUMN].Value.ToString() == TRUE;
                                if (pc[index].Visible != visible)
                                {
                                    pc[index].SetVisibility(visible);
                                    signalCheckBoxes[index].Checked = visible;
                                }
                            }
                        }
                    }
                }
                catch (CommunicationException ex)
                {
                    Log_Manager.CommunicationError.HandleException(ex);
                }
                catch (Exception ex) when ((ex is Exception_ThreadExit) || (ex is TaskCanceledException))
                {
                    Log_Manager.LogCaughtException(ex);// Do nothing as it is expected when closing forms
                }
                finally
                {
                    dataGridView.Enabled = true;
                }
            }
        }

        /// <summary>
        /// This method updates what parameters are visible on the graph based on the check state of the DataGridView Rows
        /// </summary>
        public static void UpdateVisibleAxisParams(this ParsedCapture pc, DataGridView dataGridView, CheckBox[] signalCheckBoxes, int rowIndex)
        {
            lock (dataGridView)
            {
                dataGridView.SuspendLayout();
                try
                {
                    dataGridView.Enabled = false;
                    dataGridView.EndEdit();// Need to do this so we can actually read the state of the CheckBox in column 1
                    string parameterName;
                    DataGridViewRow row = dataGridView.Rows[rowIndex];//Copy the row so we can read the value
                    parameterName = row.Cells[DGD_NAME_COLUMN_INDEX].Value.ToString();//Get the parameter name
                    if (pc.dictVarNameToIndex.TryGetValue(parameterName, out uint index))//Double check that parsedCapture contains the param name (check that DataGridView and actual plot aren't out of sync)
                    {
                        if (row.Cells[DGD_VISIBLE_COLUMN].Value != null)
                        {
                            bool visible = row.Cells[DGD_VISIBLE_COLUMN].Value.ToString() == TRUE;
                            if (pc[index].Visible != visible)
                            {
                                pc[index].SetVisibility(visible);
                                signalCheckBoxes[index].Checked = visible;
                            }
                        }
                    }
                }
                catch (Exception ex) when ((ex is Exception_ThreadExit) || (ex is TaskCanceledException))
                {
                    // Do nothing as it is expected when closing forms
                }
                catch (CommunicationException ex)
                {
                    Log_Manager.CommunicationError.HandleException(ex);
                }
                finally
                {
                    dataGridView.Enabled = true;
                    dataGridView.ResumeLayout();
                }
            }
        }
        #endregion /DataGridView

        #region Parameters
        /// <summary>
        /// This method takes in a ParsedCapture and a parameter name and attempts to return a string giving the type of the parameter. If
        /// a parameter type can be found, it returns true and that paramType. Else it returns false and default.
        /// </summary>
        /// <param name="pc">ParsedCapture to search</param>
        /// <param name="variableName">Parameter name to find type of</param>
        /// <param name="variableType">Type of paramName</param>
        /// <returns>True if type can be found. False if it can't.</returns>
        private static bool TryGetTypeFromVariableName(this ParsedCapture pc, String variableName, out String variableType)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(variableName))
                {
                    uint index = pc.dictVarNameToIndex[variableName];
                    if (Extensions_AddressType.TryExtractType(pc[index].AddressType_String, out variableType))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogCaughtException();
            }
            variableType = default;
            return false;
        }

        
        public static bool CheckForNonCapturableParameters(this ParsedCapture parsedCapture, params String[] capturableNames)
        {
            bool nonCapturableParam = false;
            try
            {
                foreach (string name in parsedCapture.dictVarNameToIndex.Keys)
                {
                    if (nonCapturableParam)
                    {
                        break;
                    }
                    nonCapturableParam = !capturableNames.Contains(name); 
                }
            }
            catch (InvalidOperationException)
            {
                // Intentionally blank, this is caused (at least when I saw) by a modified collection 
                // in the foreach enumeration, but I'm not sure whats being modified. Seems not to matter
                // as operation is still successful. 
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);
            }
            return nonCapturableParam;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parsedCapture"></param>
        public static void RemoveNonCapturableParameters(this ParsedCapture parsedCapture, params String[] capturableNames)
        {
            List<string> removeNames = new List<string>();
            foreach (string fullName in parsedCapture.dictVarNameToIndex.Keys)
            {
                if (!capturableNames.Contains(fullName))
                {
                    removeNames.Add(fullName);
                }
            }
            foreach (string fullName in removeNames)
            {// Here so as to not modify the collection as we iterate over it TODO: effieciency increase?
                parsedCapture.TryRemoveParameterByName(fullName);
            }
        }
        #endregion /Parameters

        #region Axis
        /// <summary>
        /// This method updates the graph and DataGridView based on AutoAxis being true
        /// </summary>
        /// <param name="pc">ParsedCapture to update</param>
        public static void UpdateAutoAxes(this ParsedCapture pc, DataGridView dataGridView, OxyColor[] variableOxyColors)
        {
            pc.DecideAxes();
            pc.MakeDataGridViewFromPlot(dataGridView, variableOxyColors);
        }

        /// <summary>
        /// This method is responsible for determining which axis to
        /// place individual parameters onto when a user chooses 
        /// automatic placement
        /// </summary>
        /// <param name="parsedCapture">The PlotCapture object containing the parameters which 
        /// need to be correlated and placed on axes</param>
        private static void DecideAxes(this ParsedCapture parsedCapture)
        {
            Log_Manager.LogMethodCall("ParsedCapture.cs", nameof(DecideAxes));
            uint[] visibleIndicies = parsedCapture.GetVisibleIndicies().ToArray(); ;// We will use this to know how many variables we will need to assign
            if (visibleIndicies.Any())
            {
                // Double variables
      
                double bestRelation;
                Stack<double> relations;

                // Misc variables
                IPair<uint> bestPair;
                bool forceOneAxis = false;// Puts all on one axis
                bool forceSplitAxes = true;// Forces the split of axes where possible
                double corrBreakLimit = .7;// If below this point, it will place the plots on sperate axes, TODO make appConfig
     
                if (forceOneAxis)
                {
                    foreach (uint varIndex in visibleIndicies)
                    {
                        parsedCapture[varIndex].SetAxis(PlotAxis.Left);
                    }
                }
                switch (visibleIndicies.Length)
                {
                    #region One
                    case 1:
                        parsedCapture[visibleIndicies[0]].SetAxis(PlotAxis.Left);
                        break;
                    #endregion /One

                    #region Two
                    case 2:
                        if (!forceSplitAxes && parsedCapture.variableRelation.At(visibleIndicies[0], visibleIndicies[1]) >= corrBreakLimit)
                        {//split axes is not set and these are close enough to be on the same axes
                            parsedCapture[visibleIndicies[1]].SetAxis(PlotAxis.Right);
                        }
                        else
                        {
                            parsedCapture[visibleIndicies[1]].SetAxis(PlotAxis.Left);
                        }
                        goto case 1;
                    #endregion /Two

                    #region More
                    default://in this case the two best related vars must be on one axis
                        uint at = 0;
                        uint[] leftVariables = new uint[visibleIndicies.Length /2];
                        uint[] rightVariables = new uint[visibleIndicies.Length - leftVariables.Length];
                        HashSet<uint> remaningIndicies = new HashSet<uint>(visibleIndicies);

                        Array.Copy(visibleIndicies, leftVariables, visibleIndicies.Length);

                        relations = parsedCapture.PollRelations(visibleIndicies); //list of the relations between active vars
                        
                        do
                        {
                            bestRelation = relations.Pop();
                            bestPair = parsedCapture.variableRelation.relationToPair[bestRelation];// This pair will correspond to the indicies due to relation being unique
                            if (!(leftVariables.Contains(bestPair.P1) && leftVariables.Contains(bestPair.P2)))
                            {
                                if (at < leftVariables.Length)
                                {
                                    leftVariables[at++] = bestPair.P1;
                                    leftVariables[at++] = bestPair.P2;
                                    remaningIndicies.RemoveWhere(x => leftVariables.Contains(x));
                                }
                                else
                                {
                                    rightVariables[at++] = bestPair.P1;
                                    rightVariables[at++] = bestPair.P2;
                                    remaningIndicies.RemoveWhere(x => leftVariables.Contains(x));
                                }
                                if (remaningIndicies.Count == 1)
                                {
                                    leftVariables[at] = remaningIndicies.First();
                                }
                            }
                        } while (remaningIndicies.Any());
                        
                        //if (!forceSplitAxes)
                        //{//split axes is not set 
                        //    if (parsedCapture.variableRelation.At(leftVariables[0], leftVariables[2]) >= corrBreakLimit && parsedCapture.variableRelation.At(leftVariables[1], leftVariables[2]) >= corrBreakLimit)
                        //    {//if we're closely related, we go on the same axis
                        //        parsedCapture.leftAxesVarIndicies.Add(leftVariables[2]);
                        //        break;//we're done here
                        //    }
                        //}
                        ////here if force split or didn't make the cut
                        //parsedCapture.rightAxesVarIndicies.Add(leftVariables[2]);
                        break;
                    #endregion /Many
                }
            }
            
        }

        /// <summary>
        /// This method will determine the relations between all indicies in the provided array and 
        /// return them as a sorted list, and is intended as a helper for the DecideAxes method
        /// </summary>
        /// <param name="activeIndicies">An array containg all active indicies</param>
        /// <param name="parsedCapture">The object containing the capture information</param>
        /// <returns></returns>
        private static Stack<Double> PollRelations(this ParsedCapture parsedCapture, params uint[] activeIndicies)
        {
            Log_Manager.LogMethodCall("ParsedCapture.cs", nameof(PollRelations));
            List<double> relations = new List<double>();// List to store the needed correlations and for sort.
            for (int i1 = 0; i1 < activeIndicies.Length; i1++)
            {
                for (int i2 = i1 + 1; i2 < activeIndicies.Length; i2++)
                {// The relation statistic is unique, and keys a dictionary for lookup.
                    relations.Add(parsedCapture.variableRelation.At(activeIndicies[i1], activeIndicies[i2]));
                }
            }
            relations.Sort();// Puts the highest number at the end.
            relations.Reverse();// Makes the highest relation first.
            return new Stack<double>(relations);
        }
        #endregion /Axis
    }
}
