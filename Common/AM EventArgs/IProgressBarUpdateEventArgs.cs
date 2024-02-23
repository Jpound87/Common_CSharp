namespace Common.AM_EventArgs
{
    #region Progress Update Event Delegate 
    public delegate void ProgressUpdateEvent_Delegate(IProgressBarUpdateEventArgs e);
    #endregion

    public interface IProgressBarUpdateEventArgs
    {
        #region Accessors
        int Minimum { get; }
        int Value { get; }
        int Maximum { get; }
        #endregion
    }
}
