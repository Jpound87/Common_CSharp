using System.Drawing;
using System.Drawing.Drawing2D;

namespace Common.Controls
{
    public class Override_Rectangle_Switch
    {
        #region Identity
        public const string ClassName = nameof(Override_Rectangle_Switch);
        #endregion

        #region Readonly
        private readonly float x;
        private readonly float y;
        private readonly float width;
        private readonly float height;
        #endregion

        #region Properties
        private float radius;
        public float Radius
        {
            get => radius;
            set
            {
                if (radius != value)
                {
                    radius = value;
                }
            }
        }

        private readonly GraphicsPath graphicsPath;
        public GraphicsPath Path => graphicsPath;

        public RectangleF Shape => new RectangleF(x, y, width, height);
        #endregion

        #region Constructor
        public Override_Rectangle_Switch(float width, float height, float radius, float x = 0, float y = 0)
        {
            graphicsPath = new GraphicsPath();

            this.radius = radius;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            
            if (this.radius <= 0f)
            {
                graphicsPath.AddRectangle(new RectangleF(this.x, this.y, this.width, this.height));
            }
            else
            {
                RectangleF ef = new RectangleF(this.x, this.y, 2f * this.radius, 2f * this.radius);
                RectangleF ef2 = new RectangleF((this.width - (2f * this.radius)) - 1f, this.x, 2f * this.radius, 2f * this.radius);
                RectangleF ef3 = new RectangleF(this.x, (this.height - (2f * this.radius)) - 1f, 2f * this.radius, 2f * this.radius);
                RectangleF ef4 = new RectangleF((this.width - (2f * this.radius)) - 1f, (this.height - (2f * this.radius)) - 1f, 2f * this.radius, 2f * this.radius);

                graphicsPath.AddArc(ef, 180f, 90f);
                graphicsPath.AddArc(ef2, 270f, 90f);
                graphicsPath.AddArc(ef4, 0f, 90f);
                graphicsPath.AddArc(ef3, 90f, 90f);
                graphicsPath.CloseAllFigures();
            }
        }
        #endregion
    }
}
