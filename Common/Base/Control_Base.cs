using Common.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Base
{
    public class Control_Base : UserControl, IIdentifiable
    {
        #region Identity
        public const String ControlName = nameof(Control_Base);
        public virtual String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion

        #region Readonly

        #region Designer
        public readonly bool DesignerMode = true;
        #endregion /Designer

        #region Events
        private readonly EventHandler sizeChanged_Handler;
        #endregion

        #region Syncronization
        //protected readonly SemaphoreSlim readySemaphore = Utility_Semaphore.Create_Slim_Single(true);
        #endregion

        #endregion /Readonly

        #region Globals

        #region Latch
        private bool handleActions_Latch = true;
        protected bool postHandleActions_Latch = true;
        protected bool attachEvents_Latch = true;
        protected bool settings_Latch = true;
        protected bool image_Latch = true;
        #endregion

        #region Border Size
        //public constant size that gives the size of the borders
        public Size BorderThickness { get; private set; }

        public int BorderWidth
        {
            get
            {
                int value = BorderThickness.Width;
#if DEBUG
                //Debug.WriteLine($"{nameof(BorderWidth)} getter called returning value: {value}");
#endif
                return value;
            }
        }

        public int BorderHeight
        {
            get
            {
                int value = BorderThickness.Height;
#if DEBUG
                //Debug.WriteLine($"{nameof(BorderHeight)} getter called returning value: {value}");
#endif
                return value;
            }
        }
        #endregion /Border Size

        #endregion /Globals

        #region Constructor
        public Control_Base() : base()
        {
#if DEBUG
            DesignerMode = this.IsDesignerHosted();
#else
            DesignerMode = false;
#endif
            if (!DesignerMode)
            {
                sizeChanged_Handler = new EventHandler(OnSizeChanged);
                CreateControl();
            }
        }

        public Control_Base(bool createHandle = true) : base()
        {
#if DEBUG
            DesignerMode = this.IsDesignerHosted();
#else
            DesignerMode = false;
#endif
            if (!DesignerMode)
            {
                sizeChanged_Handler = new EventHandler(OnSizeChanged);
                if (createHandle)
                {
                    CreateControl();
                }
            }
        }
        #endregion

        #region On Handle Created
        protected sealed override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                OnHandleCreated();
            }
            catch (NotImplementedException)
            {

            }
            HandleActions();
        }

        protected virtual void OnHandleCreated()
        {
            throw new NotImplementedException($"Form: {Name} Called {ControlName} '{nameof(PostHandleActions)}' Method");
        }

        private void HandleActions()
        {
            try
            {
                if (handleActions_Latch)
                {
                    //lock (readySemaphore)
                    //{
                        try
                        {
                            AttachEvents();
                        }
                        catch (NotImplementedException)
                        {

                        }
                        try
                        {
                            AttachDelegates();
                        }
                        catch (NotImplementedException)
                        {

                        }
                        try
                        {
                            RegisterSettings();
                        }
                        catch (NotImplementedException)
                        {

                        }
                        try
                        {
                            InitializeImages();
                        }
                        catch (NotImplementedException)
                        {

                        }
                        SizeChanged += sizeChanged_Handler;
                    //}
                    handleActions_Latch = false;
                }
            }
            finally
            {
                Invalidate();
                //readySemaphore.Release();
                try
                {
                    if (postHandleActions_Latch)
                    {
                        postHandleActions_Latch = false;
                        PostHandleActions();
                    }
                }
                catch (NotImplementedException)
                {

                }
            }
        }

        protected virtual void PostHandleActions()
        {
#if DEBUG
            throw new NotImplementedException($"Form: {Name} Called {ControlName} '{nameof(PostHandleActions)}' Method");
#endif
        }

        #endregion

        #region On Load
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                //await readySemaphore.WaitAsync();
                OnBaseLoad();
            }
            catch (NotImplementedException)
            {

            }
        }

        protected virtual void OnBaseLoad()
        {
#if DEBUG
            throw new NotImplementedException($"Form: {Identity} Called Control_Base '{nameof(OnBaseLoad)}' Method");
#endif
        }
        #endregion

        #region Virtual Methods

        #region Delegates
        protected virtual void AttachDelegates()
        {
#if DEBUG
            throw new NotImplementedException($"Form: {Identity} Called Control_Base '{nameof(AttachDelegates)}' Method");
#endif
        }
        #endregion

        #region Events
        protected virtual void AttachEvents()
        {
#if DEBUG
            throw new NotImplementedException($"Form: {Identity} Called Control_Base '{nameof(AttachEvents)}' Method");
#endif
        }
        #endregion

        #region Images
        protected virtual void InitializeImages()
        {
#if DEBUG
            throw new NotImplementedException($"Form: {Identity} Called Control_Base '{nameof(InitializeImages)}' Method");
#endif
        }
        #endregion

        #region Settings
        protected virtual void RegisterSettings()
        {
#if DEBUG
            throw new NotImplementedException($"Form: {Identity} Called Control_Base '{nameof(RegisterSettings)}' Method");
#endif
        }
        #endregion

        #region Size
        protected virtual void OnSizeChanged(object _, EventArgs e)
        {
            BorderThickness = Size - ClientSize;
        }
        #endregion

        #endregion /Virtual Methods

        #region Dispose
        readonly Dispose_Base dispose_Base = new Dispose_Base();

        public void RegisterDisposables(params IDisposable[] disposables) => dispose_Base.RegisterDisposables(disposables);

        new public virtual void Dispose()
        {
            try
            {
                SizeChanged -= sizeChanged_Handler;
                dispose_Base.Dispose();
            }
            finally
            {
                base.Dispose();
            }
        }
        #endregion
    }
}
