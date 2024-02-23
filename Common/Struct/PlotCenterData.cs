using System;

namespace Common.Struct
{
    public struct PlotCenterData
    {
        #region Identity
        public const String StructName = nameof(PlotCenterData);
        #endregion /Identity

        #region Accessors
        public Double TimeMax { get; set; }
        public Double TimeMin { get; set; }
        public Double LeftMax { get; set; }
        public Double LeftMin { get; set; }
        public Double RightMax { get; set; }
        public Double RightMin { get; set; }
        #endregion /Accessors
    }
}
