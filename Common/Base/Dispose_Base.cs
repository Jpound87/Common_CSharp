using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Common.Base
{
    #region Interface
    public interface IDisposeBase : IDisposable
    {
        void RegisterDisposables(params IDisposable[] disposables);
    }
    #endregion

    public class Dispose_Base : IDisposeBase, IIdentifiable
    {
        #region Identity
        public const string ClassName = nameof(Dispose_Base);
        public virtual String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Globals
        private readonly SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        protected bool disposed = false;
        #endregion

        #region Disposal Registration
        private IDisposable[] disposables;

        protected readonly ICollection<IDisposable> disposablesList = new List<IDisposable>();

        public void RegisterDisposables(params IDisposable[] disposables)
        {
            for (int d = 0; d < disposables.Length; d++)
            {
                disposablesList.Add(disposables[d]);
            }
            this.disposables = disposablesList.ToArray();
        }
        #endregion

        #region Dispose
        ~Dispose_Base()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            try
            {
                if (disposing)
                {
                    if (disposables != null)
                    {
                        for (int d = 0; d < disposables.Length; d++)
                        {
                            try
                            {
                                disposables[d].Dispose();
                            }
                            catch (NullReferenceException)
                            {
                                // This is good.
                            }
                            catch (ObjectDisposedException)
                            {
                                // This is good.
                            }
                            catch (Exception ex)
                            {
#if DEBUG
                                MessageBox.Show($"Exception in dispose_base: {ex.Message}");
#endif
                            }
                        }
                        safeHandle.Dispose();
                    }
                    Dispose();
                    // TODO: dispose managed state (managed objects).
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
            }
            finally
            {
                disposed = true;
            }
        }
       

        public virtual void Dispose()
        {
            // Suppress finalization.
            GC.SuppressFinalize(this);
            Dispose(disposed);
        }
        #endregion
    }
}
