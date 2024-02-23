using Common.Extensions;
using Common.Constant;
using System;
using System.Collections.Generic;
using WeifenLuo.WinFormsUI.Docking;

namespace Runtime.Structs
{
    [Serializable]
    internal struct LayoutData
    {
        #region Identity
        public const String StructName = nameof(LayoutData);
        #endregion /Identity

        #region Side Bar
        public OpenState SideBarState { get; private set; }
        #endregion /Side Bar

        #region Open Panels
        private static readonly Dictionary<String, HashSet<String>> dictDeviceName_FormNames = new();
        public Dictionary<String, HashSet<String>> DictDeviceName_FormNames
        {
            get
            {
                return dictDeviceName_FormNames;
            }
            set
            {
                foreach (var entry in value)
                {
                    dictDeviceName_FormNames.TryAddOrUpdate(entry.Key, entry.Value);
                }
            }
        }
        #endregion /Open Panels

        #region Area Size
        private static readonly Dictionary<String, Dictionary<DockAreas, Double>> dictDeviceName_DockArea_AreaSize = new();
        public Dictionary<string, Dictionary<DockAreas, double>> DictDeviceName_DockArea_AreaSize
        {
            get
            {
                return dictDeviceName_DockArea_AreaSize;
            }
            set
            {
                foreach (var entry in value)
                {
                    dictDeviceName_DockArea_AreaSize.TryAddOrUpdate(entry.Key, entry.Value);
                    AddAreaSize_DockState(entry.Key, entry.Value);
                }
            }
        }
        private static readonly Dictionary<string, Dictionary<DockState, double>> dictDeviceName_DockState_AreaSize =
            new Dictionary<string, Dictionary<DockState, double>>();
        public Dictionary<string, Dictionary<DockState, double>> DictDeviceName_DockState_AreaSize
        {
            get
            {
                return dictDeviceName_DockState_AreaSize;
            }
            set
            {
                foreach (var entry in value)
                {
                    dictDeviceName_DockState_AreaSize.TryAddOrUpdate(entry.Key, entry.Value);
                    AddAreaSize_DockArea(entry.Key, entry.Value);
                }
            }
        }
        #endregion /Area Size

        #region Constructor
        public LayoutData(Dictionary<String, HashSet<String>> dictDeviceName_FormNames,
            Dictionary<string, Dictionary<DockAreas, double>> dictDeviceName_AreaSize,
            Dictionary<string, Dictionary<string, DockState>> dictDeviceName_FormNameDockState,
            OpenState openState = OpenState.Open)
        {
            SideBarState = openState;

            DictDeviceName_FormNames = dictDeviceName_FormNames;
            DictDeviceName_DockArea_AreaSize = dictDeviceName_AreaSize;
        }
        #endregion /Constructor

        #region Add Area
        private static void AddAreaSize_DockState(String name, Dictionary<DockAreas, double> dictAreaSize)
        {
            Dictionary<DockState, double> dictDockState_Size = new Dictionary<DockState, double>
            {
                { DockState.Float, dictAreaSize[DockAreas.Float] },
                { DockState.Hidden, dictAreaSize[DockAreas.Float] },
                { DockState.Unknown, dictAreaSize[DockAreas.Float] },
                { DockState.Document, dictAreaSize[DockAreas.Document] },
                { DockState.DockLeft, dictAreaSize[DockAreas.DockLeft] },
                { DockState.DockRight, dictAreaSize[DockAreas.DockRight] },
                { DockState.DockTop, dictAreaSize[DockAreas.DockTop] },
                { DockState.DockBottom, dictAreaSize[DockAreas.DockBottom] }
            };
            dictDeviceName_DockState_AreaSize.Add(name, dictDockState_Size);
        }

        private static void AddAreaSize_DockArea(String name, Dictionary<DockState, double> dictAreaSize)
        {
            Dictionary<DockAreas, double> dictDockArea_Size = new Dictionary<DockAreas, double>
            {
                { DockAreas.Float, dictAreaSize[DockState.Float] },
                { DockAreas.Document, dictAreaSize[DockState.Document] },
                { DockAreas.DockLeft, dictAreaSize[DockState.DockLeft] },
                { DockAreas.DockRight, dictAreaSize[DockState.DockRight] },
                { DockAreas.DockTop, dictAreaSize[DockState.DockTop] },
                { DockAreas.DockBottom, dictAreaSize[DockState.DockBottom] }
            };
            dictDeviceName_DockArea_AreaSize.Add(name, dictDockArea_Size);
        }
        #endregion /Add Area
    }
}
