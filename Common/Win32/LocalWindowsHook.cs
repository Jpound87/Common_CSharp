using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Win32
{
    public class LocalWindowsHook : IDisposable
    {
        // Internal properties
        private IntPtr m_hHook = IntPtr.Zero;
        private NativeMethods.HookProc m_filterFunc = null;
        private HookType m_hookType;

        // Event delegate
        public delegate void HookEventHandler(object sender, HookEventArgs e);

        // Event: HookInvoked 
        public event HookEventHandler HookInvoked;
        protected void OnHookInvoked(HookEventArgs e)
        {
            HookInvoked?.Invoke(this, e);
        }

        public LocalWindowsHook(HookType hook)
        {
            m_hookType = hook;
            m_filterFunc = new NativeMethods.HookProc(this.CoreHookProc);
        }

        // Default filter function
        public IntPtr CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
                return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);

            // Let clients determine what to do
            HookEventArgs e = new HookEventArgs();
            e.HookCode = code;
            e.wParam = wParam;
            e.lParam = lParam;
            OnHookInvoked(e);

            // Yield to the next hook in the chain
            return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);
        }

        // Install the hook
        public void Install()
        {
            if (m_hHook != IntPtr.Zero)
                Uninstall();

            int threadId = NativeMethods.GetCurrentThreadId();
            m_hHook = NativeMethods.SetWindowsHookEx(m_hookType, m_filterFunc, IntPtr.Zero, threadId);
        }

        // Uninstall the hook
        public void Uninstall()
        {
            if (m_hHook != IntPtr.Zero)
            {
                NativeMethods.UnhookWindowsHookEx(m_hHook);
                m_hHook = IntPtr.Zero;
            }
        }

        ~LocalWindowsHook()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Uninstall();
        }
    }
}
