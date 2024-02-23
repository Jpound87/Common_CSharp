using Common;
using Common.Constant;
using Datam.WinForms.Interface;
using Parameters.Extensions;
using Parameters.Interface;
using Runtime;
using System;
using System.Linq;

namespace Datam.WinForms.Struct
{
    public struct DatamVariableCaptureData : IDatamVariableCaptureData, IValidate
    {
        #region Valid
        /// <summary>
        /// Used to set this as active.
        /// </summary>
        public bool Valid { get; set; }
        #endregion /Valid

        #region Plot
        public bool Visible { get; set; }
        public uint Index { get; set; }
        public PlotAxis Axis { get; set; }
        public void SetAxis(PlotAxis axis)
        {
            Axis = axis;
        }
        #endregion /Plot

        #region Variable
        private IParameter variable;
        public IParameter Variable 
        {
            readonly get
            {
                return variable;
            }
            set
            {
                variable = value;
                if (Extensions_AddressType.TryExtractType(AddressType_String, out string type))
                {
                    Type = type;
                }
                else
                {
                    Type = Translation_Manager.Unavailable;
                }
            }
        }
        public readonly String Name => variable.Name;
        public readonly String FullName => variable.FullName;
        public String Type { get; private set; }
        internal readonly IAddressType AddressType => variable.AddressType;
        public readonly String AddressType_String => variable.AddressType_String_Allied;
        #endregion /Variable

        #region Data
        public readonly int Length => data?.Length ?? 0;
        private Double[] data;
        public Double this[uint i]
        {
            get
            {
                if (data != null)
                {
                    return data[i];
                }
                else
                {
                    return 0;
                }
            }
        }
        
        public void SetRawData(Double[] rawData)
        {
            if (rawData.Any())
            {
                if (Length != rawData.Length)
                {
                    if (data == null)
                    {
                        data = new double[rawData.Length];
                        if (data.Last() != 0 && data[0..(data.Length - 2)].Average() == 0)
                        {
                            Array.Fill(rawData, data.Last());
                        }
                    }
                    else
                    {
                        Array.Resize(ref data, rawData.Length);
                    }
                }
                rawData.CopyTo(data, 0);// = rawData;
            }
        }

        public readonly Double Minimum => data?.Min() ?? Double.MaxValue;
        public readonly Double Maximum => data?.Max() ?? Double.MinValue;
        public readonly Double Average => data?.Average() ?? 0;
        #endregion /Data

        #region Control
        public void Deactivate()
        {
            Visible = false;
            Valid = false;
            Axis = PlotAxis.None;
        }

        public void SetVisibility(bool visible = true)
        {
            Visible = visible;
        }
        #endregion /Control
    }
}
