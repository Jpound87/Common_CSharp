namespace Common
{
    public delegate void Notify();
    public interface INotify
    {
        #region Events
        event Notify OnChanged;
        event Notify OnAdded;
        event Notify OnRemoved;
        #endregion
    }
}
