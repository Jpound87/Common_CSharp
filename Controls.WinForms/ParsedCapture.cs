using Common.Constant;
using Common.Extensions;
using Datam.WinForms.Interface;
using Datam.WinForms.Struct;
using Parameters.Interface;
using Runtime;
using System;
using System.Collections.Generic;

namespace Datam.WinForms
{
    /// <summary>
    /// This class is designed to hold all the information required to make a plot
    /// </summary>
    [Serializable]
    public class ParsedCapture : ICaptureObject
    {
        #region Identity
        public const String ClassName = nameof(ParsedCapture);
        public String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Validity
        public bool Valid { get; private set; }
        #endregion

        #region Constants
        private const uint MAXIMUM_SIZE = 4;
        #endregion Contants

        #region Grid Lines
        public bool LeftMajorGridLines { get; set; }
        public bool RightMajorGridLines { get; set; }
        public bool LeftMinorGridLines { get; set; }
        public bool RightMinorGridLines { get; set; }
        #endregion /Grid Lines

        #region Data Variables
        /// <summary>
        /// The number of vars.
        /// </summary>
        public uint Size => timeCorrelatedData.Size;
        /// <summary>
        /// The count of active vars.
        /// </summary>
        public uint ActiveVariables => Convert.ToUInt32(activeIndicies.Count);
        private readonly List<uint> activeIndicies = new();
        public uint[] GetActiveIndicies()
        {
            lock (activeIndicies)
            {
                activeIndicies.Clear();
                //TODO: replace with system that makes this on change instead of lookup.
                for (uint s = 0; s < Size; s++)
                {
                    if (timeCorrelatedData[s].Valid)
                    {
                        activeIndicies.Add(s);
                    }
                }
                return activeIndicies.ToArray();
            }
        }

        private readonly List<uint> visibleIndicies = new();
        public uint[] GetVisibleIndicies()
        {
            lock (visibleIndicies)
            {
                visibleIndicies.Clear();
                //TODO: replace with system that makes this on change instead of lookup.
                for (uint s = 0; s < Size; s++)
                {
                    if (timeCorrelatedData[s].Visible)
                    {
                        visibleIndicies.Add(s);
                    }
                }
                return visibleIndicies.ToArray();
            }
        }

        public uint CaptureDepth
        {
            get => timeCorrelatedData.Depth;
            set => timeCorrelatedData.Depth = value;    
        }

        public IDatamVariableCaptureData this[uint i]
        {
            get
            {
                return timeCorrelatedData[i];
            }
        }

        public void SetRawData(uint index, Double[] rawData)
        {
            timeCorrelatedData.SetRawData(index, rawData);  
        }
        public void SetAxis(uint index, PlotAxis axis) => timeCorrelatedData.SetAxis(index, axis);

        public IDictionary<String, uint> dictVarNameToIndex = new Dictionary<String, uint>();// Translates var names to the index.

        internal VariableRelation variableRelation;// Contains the relationship between variables.

        internal TimeCorrelatedData timeCorrelatedData = new(MAXIMUM_SIZE);

        public Double Range_Min
        {
            get => timeCorrelatedData.RangeMin;
            //set => timeCorrelatedData.RangeMin = value;
        }
        public Double Range_Max
        {
            get => timeCorrelatedData.RangeMax;
            //set => captureTriggerData.SetPoint2 = value;
        }
        #endregion /Data Varibles

        #region Trigger

        private DatamCaptureTriggerData captureTriggerData = new();
        public Double Trigger_SetPoint1 
        {
            get => captureTriggerData.SetPoint1; 
            set => captureTriggerData.SetPoint1 = value; 
        } 
        public Double Trigger_SetPoint2
        {
            get => captureTriggerData.SetPoint2;
            set => captureTriggerData.SetPoint2 = value;
        }

        public string Trigger_Variable
        {
            get => captureTriggerData.Variable_Name;
            set => captureTriggerData.Variable_Name = value;
        }
        public string CaptureCondition
        {
            get => captureTriggerData.CaptureCondition;
            set => captureTriggerData.CaptureCondition = value;
        }
        #endregion /Trigger

        #region Time Axis
        public Double TimeMax => timeCorrelatedData.TimeMax;
        public Double TimeMin => timeCorrelatedData.TimeMin;
        public Double[] Time_Axis => timeCorrelatedData.Time;
        public TimeScale TimeScale
        {
            get => timeCorrelatedData.TimeScale;
            set => timeCorrelatedData.TimeScale = value;
        }
        #endregion /Time Axis

        #region Constructor
        public ParsedCapture()
        {
            Log_Manager.LogMethodCall(nameof(ParsedCapture), nameof(ParsedCapture) + " constructor called");
            Valid = false;
            LeftMajorGridLines = false;//Settings.Default.RightMajorGridLines;
            LeftMinorGridLines = false;// Settings.Default.RightMinorGridLines;
            RightMajorGridLines = false;// Settings.Default.RightMinorGridLines;
            RightMinorGridLines = false;// Settings.Default.RightMinorGridLines;
        }

        public ParsedCapture(bool leftMajorGridLines, bool leftMinorGridLines, bool rightMajorGridLines, bool rightMinorGridLines)
        {
            Log_Manager.LogMethodCall(nameof(ParsedCapture), nameof(ParsedCapture) + " constructor called");
            Valid = true;
            LeftMajorGridLines = leftMajorGridLines;//Settings.Default.RightMajorGridLines;
            LeftMinorGridLines = leftMinorGridLines;// Settings.Default.RightMinorGridLines;
            RightMajorGridLines = rightMajorGridLines;// Settings.Default.RightMinorGridLines;
            RightMinorGridLines = rightMinorGridLines;// Settings.Default.RightMinorGridLines;
        }
        #endregion /Constructor

        #region Methods

        #region Remove 
        /// <summary>
        /// This method will locate the parameter with the given 
        /// name, remove it, and rearrange the remaining 
        /// parameter information so as to make it readable in the 
        /// expected way.
        /// </summary>
        /// <param name="name">Name of the paramater to be removed</param>
        public bool TryRemoveParameterByName(String fullName)
        {
            Log_Manager.LogMethodCall(nameof(ParsedCapture), nameof(TryRemoveParameterByName));
            if (dictVarNameToIndex.ContainsKey(fullName))
            {
                // If the dictionary contains the name.
                uint index = dictVarNameToIndex[fullName];
                return TryRemoveParameterByIndex(index);
            }
            // We have no such name, so we can't remove it
            string strErr = String.Format("ParsedCapture does not contain the name: {0}", fullName);
            Log_Manager.IssueAlert(strErr, this);
            return false;
        }

        /// <summary>
        /// This method will remove the parameter located at 
        /// the given index, remove it, and rearrange the remaining 
        /// parameter information so as to make it readable in the 
        /// expected way.
        /// </summary>
        /// <param name="index">Index at which the parameter to remove is stored</param>
        public bool TryRemoveParameterByIndex(uint index)
        {
            Log_Manager.LogMethodCall(nameof(ParsedCapture), nameof(TryRemoveParameterByIndex));
            try // -EF ADDED TRY CATCH
            {
                if (index < Size)
                {// If the index is possible given the num of vars
                 //rawDataByVar[index] = new double[captureDepth];
                    timeCorrelatedData[index].Deactivate();
                    dictVarNameToIndex.Remove(timeCorrelatedData[index].FullName);
                    timeCorrelatedData.UpdateExtrema();
                    activeIndicies.Remove(index);
                    visibleIndicies.Remove(index);
                    return true;
                }
                else
                {// We have no such index, so we can't remove it
                    string strErr = String.Format("ParsedCapture does not contain the index: {0}", index);
                    Log_Manager.IssueAlert(strErr, this);
                }
            }
            catch(Exception ex)
            {
                Log_Manager.IssueAlert(ex);
            }
            return false;
        }
        #endregion /Remove

        #region Add
        /// <summary>
        /// This method is intended to take the name, raw data, and a bool
        /// representing the final visibility of a paramaeter to add to the
        /// parsed capture, evaluate it matches the contained schema, and 
        /// then add the parameter if it does match. It will return -1 if 
        /// the schema doesnt match or it it is over the paramter limit.
        /// </summary>
        /// <param name="name">Name of parameter being added to the capture</param>
        /// <param name="rawData">Array containing the paramaters raw data</param>
        /// <param name="visible">Bool that represents if we want the added 
        /// paramater to start in a visible state</param>
        /// <returns>Integer number of parameters indexed, -1 if failed</returns>
        public int AddParameter(IParameter parameter, PlotAxis axis, bool visible, Double plotAxisWeightFactor)
        {
            Log_Manager.LogMethodCall(nameof(ParsedCapture), nameof(AddParameter));
            try// -EF ADDED TRY CATCH
            {
                if (!dictVarNameToIndex.ContainsKey(parameter.FullName))
                {
                    if (ActiveVariables < MAXIMUM_SIZE)// Artificial limitation to 4 vars, hypothetically can be infinite, realisticially may be increased to perhaps 6. 
                    {
                        timeCorrelatedData.SetVariableCaptureData(ActiveVariables, new DatamVariableCaptureData()
                        {
                            Index = ActiveVariables,
                            Visible = visible,
                            Valid = true,
                            Variable = parameter,
                            Axis = axis
                        });
                        timeCorrelatedData[ActiveVariables].SetRawData(new double[CaptureDepth]);
                        dictVarNameToIndex.TryAddOrUpdate(parameter.FullName , timeCorrelatedData[ActiveVariables].Index);
                        if (ActiveVariables > 2)
                        {// We need two to relate them (this one isn't counted in active vars yet, so if we are at one, we have two)
                            if (CaptureDepth > 0)
                            {// We need data to correlate
                                variableRelation = new VariableRelation(this, plotAxisWeightFactor);
                            }
                        }
                        GetActiveIndicies();
                        return Convert.ToInt32(ActiveVariables) - 1;
                    }
                }
                return -1;// Fail token.
            }
            catch(Exception ex)
            {
                Log_Manager.IssueAlert(ex);
                return -1;
            }
        }

        /// <summary>
        /// This method is intended to take the name, raw data, and a bool
        /// representing the final visibility of a parameter to add to the
        /// parsed capture, evaluate it matches the contained schema, and 
        /// then add the parameter if it does match. It will return -1 if 
        /// the schema doesnt match or it it is over the paramter limit.
        /// </summary>
        /// <param name="evariabl">Name of parameter being added to the capture</param>
        /// <param name="rawData">Array containing the paramaters raw data</param>
        /// <param name="startTime">Start time of the range of captures in seconds</param>
        /// <param name="interval">Interval between points</param>
        /// <returns>True if sucessful, else false.</returns>
        public bool TryAddParameterData_Name(IParameter varible, Double[] rawData, Double plotAxisWeightFactor)
        {
            Log_Manager.LogMethodCall(nameof(ParsedCapture), nameof(TryAddParameterData_Name));
            try// -EF ADDED TRY CATCH
            {
                if (dictVarNameToIndex.TryLookup(varible.FullName, out uint resultIndex))
                {
                    return TryAddParameterData_Index(resultIndex, rawData, plotAxisWeightFactor);
                }
                string strErr = $"The name {varible.FullName} was not found in the name to index dictionary";
                Log_Manager.IssueAlert(strErr, strErr);
                return false;// Fail token.
            }
            catch (IndexOutOfRangeException ioore)// Pronounced Eeyore, the sad donkey of our childhood, now a passable exception
            {
                string strInfo = String.Format("Index out of range exception in ParsedCapture - AddParameterData" +
                    "likely the ParsedCapture data array size was adjusted asynchronously, and this is passable. Message: {0}", ioore.Message);
                Log_Manager.LogWarning(nameof(TryAddParameterData_Name), strInfo);// Warning since this could potentially cause issues
                return false;// Not sure if this needs to be handled (fail seems to be good enough)
            }
            catch (ArgumentNullException ane)// Pronounced Anne, this comment is completely unessicary. 
            {
                string strInfo = String.Format("Index out of range exception in ParsedCapture - AddParameterData" +
                    "likely the ParsedCapture data array size was adjusted asynchronously, and this is passable. Message: {0}", ane.Message);
                Log_Manager.LogWarning(nameof(TryAddParameterData_Name), strInfo);// Warning since this could potentially cause issues
                return false;// Not sure if this needs to be handled (fail seems to be good enough)
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);// This case may not be passable
                return false;
            }
        }

        /// <summary>
        /// This method is intended to take the name, raw data, and a bool
        /// representing the final visibility of a parameter to add to the
        /// parsed capture, evaluate it matches the contained schema, and 
        /// then add the parameter if it does match. It will return -1 if 
        /// the schema doesnt match or it it is over the paramter limit.
        /// </summary>
        /// <param name="evariabl">Name of parameter being added to the capture</param>
        /// <param name="rawData">Array containing the paramaters raw data</param>
        /// <param name="startTime">Start time of the range of captures in seconds</param>
        /// <param name="interval">Interval between points</param>
        /// <returns>True if sucessful, else false.</returns>
        public bool TryAddParameterData_Index(uint index, Double[] rawData, Double plotAxisWeightFactor)
        {
            Log_Manager.LogMethodCall(nameof(ParsedCapture), nameof(TryAddParameterData_Name));
            try// -EF ADDED TRY CATCH
            {
                if (rawData.Length == CaptureDepth && index < timeCorrelatedData.Size)
                {// Make sure the data fits the current model
                    timeCorrelatedData.SetRawData(index, rawData);
                    if (ActiveVariables > 2)
                    {// We need two to realte them 
                        if (CaptureDepth > 0)
                        { // We need data to correlate.
                            variableRelation = new VariableRelation(this, plotAxisWeightFactor);
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (IndexOutOfRangeException ioore)// Pronounced Eeyore, the sad donkey of our childhood, now a passable exception
            {
                string strInfo = String.Format("Index out of range exception in ParsedCapture - AddParameterData" +
                    "likely the ParsedCapture data array size was adjusted asynchronously, and this is passable. Message: {0}", ioore.Message);
                Log_Manager.LogWarning(nameof(TryAddParameterData_Name), strInfo);// Warning since this could potentially cause issues
                return false;// Not sure if this needs to be handled (fail seems to be good enough)
            }
            catch (ArgumentNullException ane)// Pronounced Anne, this comment is completely unessicary. 
            {
                string strInfo = String.Format("Index out of range exception in ParsedCapture - AddParameterData" +
                    "likely the ParsedCapture data array size was adjusted asynchronously, and this is passable. Message: {0}", ane.Message);
                Log_Manager.LogWarning(nameof(TryAddParameterData_Name), strInfo);// Warning since this could potentially cause issues
                return false;// Not sure if this needs to be handled (fail seems to be good enough)
            }
            catch (Exception ex)
            {
                Log_Manager.IssueAlert(ex);// This case may not be passable
                return false;
            }
        }
        #endregion /Add

        #region Variable Relationship
        public void UpdateVariableRelationship(Double plotAxisWeightFactor)
        {
            if (ActiveVariables > 0)
            {// We need two to relate them (this one isn't counted in active vars yet, so if we are at one, we have two)
                variableRelation = new VariableRelation(this, plotAxisWeightFactor);
            }
        }
        #endregion /Variable Relationship

        #region Time

        #region Set
        /// <summary>
        /// This method sets the time for the plot when it's created (I think)
        /// </summary>
        /// <param name="startTimeMs"></param>
        /// <param name="intervalMs"></param>
        /// <returns></returns>
        public void SetTime(Double startTimeMs, Double intervalMs) => timeCorrelatedData.SetTime_Interval(startTimeMs, intervalMs);

        public Double SetTime(uint index, Double intervalMs) => timeCorrelatedData.SetTime_Interval(index, intervalMs);

        public void SetTime(Double[] times) => timeCorrelatedData.SetTime_Array(times);

        public void SetTime_Value(uint index, Double value) => timeCorrelatedData.SetTime_Value(index, value);
        #endregion /Set

        #endregion /Time

        #endregion /Methods
    }
}
