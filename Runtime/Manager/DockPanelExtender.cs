using Common.Base;
using DockThemes.Factory;
using DockSuite.Themes.Interface;
using System.Windows.Forms;
using DockSuite.Themes.Interface.Constants;

namespace DockThemes.Extensions
{
    public sealed class DockPanelExtender
    {
        public interface IDockPaneSplitterControlFactory
        {
            ISplitterControlBase CreateSplitterControl(IDockPane pane);
        }
        
        public interface IWindowSplitterControlFactory
        {
            SplitterBase CreateSplitterControl(ISplitterHost host);
        }

        public interface IDockWindowFactory
        {
            IDockWindow CreateDockWindow(IDockPanel dockPanel, DockState dockState);
        }

        public interface IDockPaneCaptionFactory
        {
            IDockPaneCaptionBase CreateDockPaneCaption(IDockPane pane);
        }

        public interface IDockPaneStripFactory
        {
            IDockPaneStripBase CreateDockPaneStrip(IDockPane pane);
        }

        public interface IAutoHideStripFactory
        {
            IAutoHideStripBase CreateAutoHideStrip(IDockPanel panel);
        }

        public interface IAutoHideWindowFactory
        {
            IAutoHideWindowControl CreateAutoHideWindow(IDockPanel panel);
        }

        public interface IPaneIndicatorFactory
        {
            IPaneIndicator CreatePaneIndicator(IThemeBase theme);
        }

        public interface IPanelIndicatorFactory
        {
            IPanelIndicator CreatePanelIndicator(DockStyle style, IThemeBase theme);
        }

        public interface IDockOutlineFactory
        {
            IDockOutlineBase CreateDockOutline();
        }

        public interface IDockIndicatorFactory
        {
            IDockIndicator CreateDockIndicator(IDockDragHandler dockDragHandler);
        }

        #region DefaultDockPaneFactory



        #endregion

        #region DefaultFloatWindowFactory


        #endregion

        private IDockPaneFactory m_dockPaneFactory = null;

        public IDockPaneFactory DockPaneFactory
        {
            get
            {
                if (m_dockPaneFactory == null)
                {
                    m_dockPaneFactory = new DefaultDockPaneFactory();
                }

                return m_dockPaneFactory;
            }
            set
            {
                m_dockPaneFactory = value;
            }
        }

        public IDockPaneSplitterControlFactory DockPaneSplitterControlFactory { get; set; }

        public IWindowSplitterControlFactory WindowSplitterControlFactory { get; set; }

        private IFloatWindowFactory m_floatWindowFactory = null;

        public IFloatWindowFactory FloatWindowFactory
        {
            get
            {
                if (m_floatWindowFactory == null)
                {
                    m_floatWindowFactory = new DefaultFloatWindowFactory();
                }

                return m_floatWindowFactory;
            }
            set
            {
                m_floatWindowFactory = value;
            }
        }

        public IDockWindowFactory DockWindowFactory { get; set; }

        public IDockPaneCaptionFactory DockPaneCaptionFactory { get; set; }

        public IDockPaneStripFactory DockPaneStripFactory { get; set; }

        private IAutoHideStripFactory m_autoHideStripFactory = null;

        public IAutoHideStripFactory AutoHideStripFactory
        {
            get
            {
                return m_autoHideStripFactory;
            }
            set
            {
                if (m_autoHideStripFactory == value)
                {
                    return;
                }

                m_autoHideStripFactory = value;
            }
        }

        private IAutoHideWindowFactory m_autoHideWindowFactory;
        
        public IAutoHideWindowFactory AutoHideWindowFactory
        {
            get { return m_autoHideWindowFactory; }
            set
            {
                if (m_autoHideWindowFactory == value)
                {
                    return;
                }

                m_autoHideWindowFactory = value;
            }
        }

        public IPaneIndicatorFactory PaneIndicatorFactory { get; set; }

        public IPanelIndicatorFactory PanelIndicatorFactory { get; set; }

        public IDockOutlineFactory DockOutlineFactory { get; set; }

        public IDockIndicatorFactory DockIndicatorFactory { get; set; }
    }
}
