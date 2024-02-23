using Common.Constant;
using Common.Interface;
using System;
using System.Windows.Forms;

namespace Common.Struct
{
    public struct ExceptionData : IExceptionData
    {
        #region Accessors
        public String Name { get; private set; }
        public String Source { get; private set; }
        public String Message { get; private set; }
        #endregion /Accessors

        #region Constructor
        public ExceptionData(Exception ex)
        {
            Name = ex.GetType().Name;
            Source = ex.Source;
            Message = $"Exception '{Name}' occured in '{Source}'. Message: '{ex.Message}'.";
        }
        public ExceptionData(String message, Form caller)
        {
            Name = Tokens.RUNTIME_ERROR;
            Source = caller.Text;
            Message = $"Exception '{Name}' occured in '{Source}'. Message: '{message}'.";
        }
        public ExceptionData(String message, Object caller)
        {
            Name = Tokens.RUNTIME_ERROR;
            Source = caller.GetType().Name;
            Message = $"Exception '{Name}' occured in '{Source}'. Message: '{message}'.";
        }
        public ExceptionData(String title, String message)
        {
            Name = title;
            Source = Tokens.DATAM;
            Message = $"Exception '{Name}'. Message: '{message}'.";
        }
        #endregion /Constructor
    }
}
