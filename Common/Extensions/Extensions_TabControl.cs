using Common.Struct;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_TabControl
    {
        #region Constants
        private const int TODD_HOWARD_CONSTANT = 24;// It just works.
        #endregion

        #region Find
        public static int GetTabIndex(this TabControl tabControl, String id)
        {
            return tabControl.TabPages.IndexOfKey(id);
        }
        #endregion

        #region Size
        public static void UpdateSize(this TabControl tabControl, Control sizeControl, ControlBorderRegion borderRegion, Boolean includeTabText = true)
        {
            try
            {
                sizeControl.SuspendLayout();
                TabPage tabPage = tabControl.SelectedTab;
                Padding tabPagePadding = tabPage.Padding;
                Padding tabControlPadding = tabControl.Margin;

                // Width
                int minWidth = 0;
                if (includeTabText)
                {
                    foreach (TabPage tab in tabControl.TabPages)
                    {
                        minWidth += tab.MeasureText_Width(8);
                    }
                }
                minWidth = Math.Max(borderRegion.MinimumSize.Width, minWidth);
                int horizontalPadding = sizeControl.Margin.Horizontal + tabControlPadding.Horizontal;
                int width = sizeControl.Size.Width + horizontalPadding + tabPagePadding.Horizontal;
                width = Math.Max(width, minWidth);

                // Height
                int verticalPadding = sizeControl.Margin.Vertical + tabControlPadding.Vertical + tabPagePadding.Vertical;
                int height = sizeControl.Size.Height + verticalPadding + tabControl.ItemSize.Height + TODD_HOWARD_CONSTANT;
                height = Math.Max(height, borderRegion.MinimumSize.Height);

                width -= borderRegion.Width;
                height -= borderRegion.Height;

                Size finalSize = new Size(width, height);
                tabControl.Size = finalSize;
            }
            finally
            {
                sizeControl.ResumeLayout();
            }
        }

        public static void UpdateSize(this TabControl tabControl, Control sizeControl, Boolean includeTabText = true)
        {
            try
            {
                sizeControl.Visible = false;
                sizeControl.SuspendLayout();
                TabPage tabPage = tabControl.SelectedTab;
                Padding tabPagePadding = tabPage.Padding;
                Padding tabControlPadding = tabControl.Margin;

                // Width
                int minWidth = 0;
                if (includeTabText)
                {
                    foreach (TabPage tab in tabControl.TabPages)
                    {
                        minWidth += tab.MeasureText_Width() + 8;
                    }
                }
                minWidth = Math.Max(sizeControl.MinimumSize.Width, minWidth);
                int horizontalPadding = sizeControl.Margin.Horizontal + tabControlPadding.Horizontal;
                int width = sizeControl.Size.Width + horizontalPadding + tabPagePadding.Horizontal;
                width = Math.Max(width, minWidth);

                // Height
                int verticalPadding = sizeControl.Margin.Vertical + tabControlPadding.Vertical + tabPagePadding.Vertical;
                int height = sizeControl.Size.Height + verticalPadding + tabControl.ItemSize.Height + TODD_HOWARD_CONSTANT;
                height = Math.Max(height, sizeControl.MinimumSize.Height);

                Size finalSize = new Size(width, height);
                tabControl.Size = finalSize;
            }
            finally
            {
                sizeControl.ResumeLayout();
                sizeControl.Visible = true;
            }
        }
        #endregion /Size

        #region TabPage
        public static TabPage CreateTabPage(this TabControl tabControl, String key, String text)
        {
            lock (tabControl)
            {
                tabControl.TabPages.Add(key, text);
                return tabControl.TabPages[key];
            }
        }
        #endregion /TabPage
    }
}
