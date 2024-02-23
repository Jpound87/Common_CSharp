using System;

namespace Common.Win32
{
    public class HookEventArgs : EventArgs
    {
        public int HookCode { get; set; }
        public IntPtr wParam { get; set; }
        public IntPtr lParam { get; set; }
    }
}
