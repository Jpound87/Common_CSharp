namespace Core
{

    public enum LoadResult
    {
        EmptyFileName,
        FileDoesNotExist,
        FileCurrupt,
        Success
    }

    public enum SaveResult
    {
        EmptyFileName,
        Failure,
        Success
    }

    /// <summary>
    /// Enumeration of the log priority levels inteneded to be used with the 
    /// Logger class or with the logEntryData struct
    /// </summary>
    public enum LogPriorityLevel
    {
        V,
        D,
        I,
        W,
        E,
        A
    };

    /// <summary>
    /// Graph axes enumeration.
    /// </summary>
    public enum AXIS : byte
    {
        NONE,
        LEFT,
        RIGHT
    }
}
