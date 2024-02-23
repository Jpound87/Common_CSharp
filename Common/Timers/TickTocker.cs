using Common.Base;
using System;
using System.Windows.Forms;

namespace Common.Timers
{
    public class TickTocker : Dispose_Base// I know youth things!!! (this will be dated before anyone reads it)
    {
        #region Identity
        new public const String ClassName = nameof(TickTocker);
        #endregion

        #region Timer
        private readonly Timer timer = new Timer
        {
            Interval = 500,
            Enabled = false
        };
        #endregion

        #region Events
        public event Action Tick;
        #endregion /Events

        #region Globals
        public bool Running => timer.Enabled;
        public int Interval
        {
            get
            {
                return timer.Interval;
            }
            set
            {
                lock (timer)
                {
                    timer.Interval = value;
                }
            }
        }
        #endregion /Globals

        #region Constructors
        public TickTocker(bool enabled = true)
        {
            timer.Enabled = enabled;
            timer.Tick += new EventHandler(TickHandler);
        }

        public TickTocker(int interval_ms, bool enabled = true)
        {
            timer.Enabled = enabled;
            timer.Tick += new EventHandler(TickHandler);
            Interval = interval_ms;
        }

        public TickTocker(TimeSpan interval, bool enabled = true)
        {
            timer.Enabled = enabled;
            timer.Tick += new EventHandler(TickHandler);
            Interval = Convert.ToInt32(Math.Ceiling(interval.TotalMilliseconds));
        }
        #endregion /Constructors

        #region Control
        public void Start(bool immediate = false)
        {
            lock (timer)
            {
                timer.Enabled = true;
            }
            if(immediate)
            {
                Tick?.Invoke();
            }
        }

        public void Stop()
        {
            lock (timer)
            {
                timer.Enabled = false;
            }
        }

        public void ToggleTimerEnabled(bool start, bool immediate = false)
        {
            lock (timer)
            {
                switch(start)
                {
                    case true:
                        Start(immediate);
                        return;
                    default:
                        Stop();
                        return;
                }
            }
        }

        public void UpdateInterval(int interval)
        {
            lock (timer)
            {
                timer.Interval = interval;
                if (Running)
                {// Resety needed.
                    timer.Stop();
                    timer.Start();
                }
            }
        }
        #endregion /Control

        #region Ticking
        private void TickHandler(object _, EventArgs e)
        {
            Tick?.Invoke();
        }
        #endregion /Ticking

        #region Dispose
        public override void Dispose()
        {
            timer.Enabled = false;
            timer.Dispose();
            base.Dispose();
        }
        #endregion
    }
}
