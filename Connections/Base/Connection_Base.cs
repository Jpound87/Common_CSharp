using Common.Base;
using Common.Extensions;
using Connections.Interface;
using Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Connections.Base
{
    public abstract class Connection_Base : Dispose_Base, IConnection
    {
        #region Identity
        new public const String ClassName = nameof(Connection_Base);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion /Identity

        #region Time Constants
        protected readonly TimeSpan inactivityTimeoutSpan = TimeSpan.FromMinutes(5);
        protected readonly TimeSpan waitHandleTimeoutSpan = TimeSpan.FromMilliseconds(50);
        #endregion /Time Constants

        #region Readonly
        private readonly IDictionary<EventWaitHandle, Int32> dictWaitHandle_Index = new Dictionary<EventWaitHandle, Int32>();
        #endregion /Readonly

        #region Events
        public event EventHandler<IEventArgs_Request> RequestSent
        {
            add
            {
                lock (Port)
                {
                    Port.RequestSent += value;
                }
            }
            remove
            {
                lock (Port)
                {
                    Port.RequestSent -= value;
                }
            }
        }
        #endregion /Events

        #region Accessors

        #region Timing
        public TimeSpan InactiveSpan { get; protected set; }
        public DateTime LastActiveTime { get; protected set; }
        public Stopwatch WaitTimer { get; protected set; }

        protected void UpdateActiveTime<T>(Object sender, T _)
        {
            InactiveSpan = DateTime.Now - LastActiveTime;
            LastActiveTime = DateTime.Now;
        }

        /// <summary>
        /// This task will monitor the connection for 
        /// </summary>
        protected void WatchForInactivity()
        {
            try
            {
                if(!Runtime_Manager.Alive)
                {// The program is closed so the connection should be as well.
                    Close();
                }
                else if (!Port.IsOpen || InactiveSpan > inactivityTimeoutSpan)
                {
                    DialogResult result = MessageBox.Show("USB connection inactivity detected, disconnect?", "USB Inactivity", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Log_Manager.IssueMessage($"The COM port has shut down due to inactivity.");
                        Close();
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                Dispose();
            }
        }
        #endregion /Timing

        #region Connection
        public Boolean Valid { get; protected set; }
        public IPort Port { get; protected set; }
        public EventWaitHandle[] EventWaitHandles { get; protected set; }
        public CancellationTokenSource TokenSource { get; protected set; }
        public virtual void Close()
        {
            TokenSource?.Cancel();
            Port?.Close();
        }
        #endregion /Connection

        #region Syncronization
        public CancellationToken Token
        {
            get
            {
                return TokenSource.Token;
            }
        }

        public WaitHandle TokenHandle
        {
            get
            {
                return Token.WaitHandle;
            }
        }

        protected void InitializeWaitHandles()
        {
            for (int wh = 0; wh < EventWaitHandles.Length; wh++)
            {
                dictWaitHandle_Index.Add(EventWaitHandles[wh], wh);
            }
        }

        public Int32 LookupHandleIndex(EventWaitHandle eventWaitHandle)
        {
            if (dictWaitHandle_Index.TryLookup(eventWaitHandle, out int index))
            {
                return index;
            }
            return int.MaxValue;
        }

        public Int32 WaitForExitOrEventWaits()
        {
            return Runtime_Manager.WaitForAnyEvent(waitHandleTimeoutSpan, EventWaitHandles);
        }
        #endregion /Syncronization

        #endregion /Accessors

        #region Constructor
        // I lied!, its just a destructor!
        ~Connection_Base()
        {
            TokenSource?.Cancel();
        
            Port?.Dispose();
        }
        #endregion /Constructor

        #region Read & Write
        public abstract void Write(IPortWriteParams portWriteParams);
        public abstract Int32 Read(IPortReadParams portReadParams);
        #endregion /Read & Write

        #region Dispose
        protected override void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();
                    Port?.Close();
                    Port?.Dispose();
                    Runtime_Manager.Cancel(this);
                }
            }
            finally
            {
                base.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        #endregion /Dispose
    }
}
