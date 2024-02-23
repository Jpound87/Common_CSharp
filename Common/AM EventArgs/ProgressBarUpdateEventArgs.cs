using System;
namespace Common.AM_EventArgs
{
    public class ProgressBarUpdateEventArgs : EventArgs, IProgressBarUpdateEventArgs
    {
        #region Globals
        public int Minimum { get; private set; }
        public int Value { get; private set; }
        public int Maximum { get; private set; }
        #endregion

        #region Constructor
        public ProgressBarUpdateEventArgs(int max, int value, int min)
        {
            Maximum = max;
            Value = value;
            Minimum = min;
        }
        #endregion
    }
}
