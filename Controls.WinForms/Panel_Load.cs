using Runtime;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Datam.WinForms
{
    public class Panel_Load : TableLayoutPanel
    {
        #region Constants
        private const int ROW_COUNT = 3;
        private const int COLUMN_COUNT = 3;

        private const float FLOAT_FOURTY = 40f;
        private const float FLOAT_FIFTY = 50f;
        private const float FLOAT_SIXTY = 60f;
        #endregion /Constants

        #region Controls

        #region Label

        private readonly Label LoadLabel = new()
        {
            Text = Translation_Manager.LoadingDataCapture + "...",
            Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Italic),
        };

        new public String Text 
        { 
            get => LoadLabel.Text; 
            set => LoadLabel.Text = value;
        }
        #endregion /Label

        #region Progress Bar
        public ProgressBar LoadProgressBar { get; private set; } = new()
        {
            Minimum = 0,
            Maximum = 4,
            Size = new Size(300, 20),
            Anchor = AnchorStyles.Left,
            Style = ProgressBarStyle.Marquee
        };
        #endregion /Progress Bar

        #endregion /Controls

        #region Constructor
        public Panel_Load()
        {
            RowCount = ROW_COUNT;
            ColumnCount = COLUMN_COUNT;
            
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, FLOAT_FIFTY));
            ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, FLOAT_FIFTY));
            RowStyles.Add(new RowStyle(SizeType.Percent, FLOAT_FOURTY));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            RowStyles.Add(new RowStyle(SizeType.Percent, FLOAT_SIXTY));
            Controls.Add(LoadLabel, 1, 0);
            LoadLabel.Dock = DockStyle.Bottom;
            Controls.Add(LoadProgressBar, 1, 1);
        }
        #endregion /Constructor
    }
}
