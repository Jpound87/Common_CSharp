using Common.Utility;
using System;
using System.Drawing;

namespace Common.Controls
{
    public class RotatingLabel : System.Windows.Forms.Label
    {
        #region Identity
        private const String ControlName = nameof(RotatingLabel);
        public String Identity
        {
            get
            {
                return ControlName;
            }
        }
        #endregion

        #region Accessors
        public int RotateAngle 
        { 
            get 
            { 
                return m_RotateAngle; 
            } 
            set 
            { 
                m_RotateAngle = value; 
                Invalidate(); 
            } 
        }

        public String NewText 
        { 
            get 
            { 
                return m_NewText; 
            } 
            set 
            { 
                m_NewText = value; Invalidate(); 
            } 
        }
        #endregion

        #region Globals
        private int m_RotateAngle = 0;
        private string m_NewText = string.Empty;
        #endregion

        #region Paint
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Brush b = new SolidBrush(ForeColor);
            SizeF size = e.Graphics.MeasureString(NewText, Font, Parent.Width);

            int normalAngle = ((RotateAngle % 360) + 360) % 360;
            double normaleRads = Utility_General.DegToRad(normalAngle);

            int hSinTheta = Convert.ToInt32(Math.Ceiling(size.Height * Math.Sin(normaleRads)));
            int wCosTheta = Convert.ToInt32(Math.Ceiling(size.Width * Math.Cos(normaleRads)));
            int wSinTheta = Convert.ToInt32(Math.Ceiling(size.Width * Math.Sin(normaleRads)));
            int hCosTheta = Convert.ToInt32(Math.Ceiling(size.Height * Math.Cos(normaleRads)));

            int rotatedWidth = Math.Abs(hSinTheta) + Math.Abs(wCosTheta);
            int rotatedHeight = Math.Abs(wSinTheta) + Math.Abs(hCosTheta);

            Width = rotatedWidth;
            Height = rotatedHeight;

            int numQuadrants =
                (normalAngle >= 0 && normalAngle < 90) ? 1 :
                (normalAngle >= 90 && normalAngle < 180) ? 2 :
                (normalAngle >= 180 && normalAngle < 270) ? 3 :
                (normalAngle >= 270 && normalAngle < 360) ? 4 :
                0;

            int horizShift = 0;
            int vertShift = 0;

            if (numQuadrants == 1)
            {
                horizShift = Math.Abs(hSinTheta);
            }
            else if (numQuadrants == 2)
            {
                horizShift = rotatedWidth;
                vertShift = Math.Abs(hCosTheta);
            }
            else if (numQuadrants == 3)
            {
                horizShift = Math.Abs(wCosTheta);
                vertShift = rotatedHeight;
            }
            else if (numQuadrants == 4)
            {
                vertShift = Math.Abs(wSinTheta);
            }

            e.Graphics.TranslateTransform(horizShift, vertShift);
            e.Graphics.RotateTransform(RotateAngle);

            e.Graphics.DrawString(NewText, Font, b, 0f, 0f);
            base.OnPaint(e);
        }
        #endregion /Paint
    }
}
