using Common.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Controls
{
    public class BorderedGroupBox : GroupBox
    {
        #region Identity
        private const String ControlName = nameof(BorderedGroupBox);
        public String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion

        #region Readonly
        private readonly PaintEventHandler paintEventHandler;
        #endregion

        #region Accessors
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                this.borderColor = value;
                DrawGroupBox();
            }
        }

        public int BorderWidth
        {
            get => borderWidth;
            set
            {
                if (value > 0)
                {
                    borderWidth = Math.Min(value, 10);
                    DrawGroupBox();
                }
            }
        }

        public int BorderRadius
        {
            get => borderRadius;
            set
            {   // Setting a radius of 0 produces square corners...
                if (value >= 0)
                {
                    borderRadius = value;
                    DrawGroupBox();
                }
            }
        }

        public int LabelIndent
        {
            get => textIndent;
            set
            {
                textIndent = value;
                DrawGroupBox();
            }
        }
        #endregion

        #region Globals
        private Color borderColor = Color.Black;
        private int borderWidth = 1;
        private int borderRadius = 3;
        private int textIndent = 10;
        #endregion

        #region Constructor
        public BorderedGroupBox() : base()
        {
            InitializeComponent();
            paintEventHandler = new PaintEventHandler(BorderedGroupBox_Paint);
            Paint += paintEventHandler;
        }

        public BorderedGroupBox(int width, float radius, Color color) : base()
        {
            borderWidth = Math.Max(1, width);
            borderColor = color;
            borderRadius = Math.Max(0, Convert.ToInt32(Math.Floor(radius)));
            InitializeComponent();
            paintEventHandler = new PaintEventHandler(BorderedGroupBox_Paint);
            Paint += paintEventHandler;
        }
        #endregion /Constructor

        #region Draw
        private void BorderedGroupBox_Paint(object sender, PaintEventArgs e) => DrawGroupBox(e.Graphics);

        private void DrawGroupBox() => DrawGroupBox(CreateGraphics());

        private void DrawGroupBox(Graphics g)
        {
            Brush textBrush = new SolidBrush(ForeColor);
            SizeF strSize = g.MeasureString(Text, Font);

            Brush borderBrush = new SolidBrush(BorderColor);
            Pen borderPen = new Pen(borderBrush, Convert.ToSingle(borderWidth));
            Rectangle rect = new Rectangle(ClientRectangle.X,
                                            ClientRectangle.Y + Convert.ToInt32(strSize.Height / 2),
                                            ClientRectangle.Width - 2,
                                            ClientRectangle.Height - Convert.ToInt32(strSize.Height / 2) - 1);

            Brush labelBrush = new SolidBrush(BackColor);

            // Clear text and border
            g.Clear(BackColor);

            // Drawing Border (added "Fix" from Jim Fell, Oct 6, '18)
            int rectX = (0 == borderWidth % 2) ? rect.X + borderWidth / 2 : rect.X + 1 + borderWidth / 2;
            int rectHeight = (0 == borderWidth % 2) ? rect.Height - borderWidth / 2 : rect.Height - 1 - borderWidth / 2;
            // NOTE DIFFERENCE: rectX vs rect.X and rectHeight vs rect.Height
            g.DrawRoundedRectangle(borderPen, rectX, rect.Y, rect.Width, rectHeight, Convert.ToSingle(borderRadius));

            // Draw text
            if (Text.Length > 0)
            {
                // Do some work to ensure we don't put the label outside
                // of the box, regardless of what value is assigned to the Indent:
                int width = Convert.ToInt32(rect.Width), posX;
                posX = (textIndent < 0) ? Math.Max(0 - width, textIndent) : Math.Min(width, textIndent);
                posX = (posX < 0) ? rect.Width + posX - Convert.ToInt32(strSize.Width) : posX;
                if (BackColor != Color.Transparent)
                {
                    g.FillRectangle(labelBrush, posX, 0, strSize.Width, strSize.Height);
                }
                g.DrawString(Text, Font, textBrush, posX, 0);
            }
        }
        #endregion /Draw

        #region Component Designer generated code
        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>Required method for Designer support - Don't modify!</summary>
        private void InitializeComponent() => components = new System.ComponentModel.Container();
        #endregion
    }
}
