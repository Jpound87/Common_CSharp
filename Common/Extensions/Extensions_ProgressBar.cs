using System;
using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_ProgressBar
    {
        #region Identity
        public const String ClassName = nameof(Extensions_ProgressBar);
        #endregion

        #region Update
        public static void Update(this ProgressBar progressBar, int maximum = 1, int value = 0, int minimum = 0)
        {
            lock (progressBar)
            {
                bool changed = false;
                try
                {
                    progressBar.SuspendLayout();
                    if (progressBar.Minimum != minimum)
                    {
                        progressBar.Minimum = minimum;
                        changed = true;
                    }
                    if (progressBar.Maximum != maximum)
                    {
                        progressBar.Maximum = maximum;
                        changed = true;
                    }
                    if (progressBar.Value != value)
                    {
                        progressBar.Value = value;
                        changed = true;
                    }
                    if (changed)
                    {
                        progressBar.Update();
                    }
                }
                finally
                {
                    progressBar.ResumeLayout();
                }
            }
        }
        #endregion /Update
    }
}
