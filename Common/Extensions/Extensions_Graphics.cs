using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Common.Extensions
{
    public static class Extensions_Graphics
    {
        #region Constants
        // adjust disabled image alpha level
        public const Single FADE_LEVEL = .7F;
        // adjust mirror image alpha level
        public const Single MIRROR_LEVEL = .15F;
        public static readonly ColorMatrix FadedMatrix = new()
        {
            Matrix00 = 1f,           //r
            Matrix11 = 1f,           //g
            Matrix22 = 1f,           //b
            Matrix33 = FADE_LEVEL,   //a
            Matrix44 = 1f           //w
        };

        public static readonly ColorMatrix MirrorMatrix = new()
        {
            Matrix00 = 1f,           //r
            Matrix11 = 1f,           //g
            Matrix22 = 1f,           //b
            Matrix33 = MIRROR_LEVEL, //a
            Matrix44 = 1f           //w
        };
        #endregion /Constants

        #region Draw
        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, int x, int y, int width, int height, int radius)
        {
            graphics.DrawRoundedRectangle(pen, Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(width), Convert.ToSingle(height), Convert.ToSingle(radius));
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = old;
        }

        public static void DrawMask(this Graphics g, Rectangle bounds, Color primaryFill, Color secondaryFill)
        {
            bounds.Inflate(-1, -1);
            // create an interior path
            using (GraphicsPath gp = g.GenerateRoundedRectanglePath(bounds, 4))
            {
                // fill the button with a subtle glow
                LinearGradientBrush fillBrush;

                using (fillBrush = new LinearGradientBrush(bounds, primaryFill, secondaryFill, 75f))
                {
                    Blend blend = new Blend
                    {
                        Positions = new float[] { 0f, .1f, .2f, .3f, .4f, .5f, 1f },
                        Factors = new float[] { 0f, .1f, .2f, .4f, .7f, .8f, 1f }
                    };
                    fillBrush.Blend = blend;
                    g.FillPath(fillBrush, gp);
                }
            }
        }

        /// <summary>Draw a colored bitmap</summary>
        public static void DrawColoredImage(this Graphics g, Image img, Rectangle bounds, Color clr)
        {
            using (ImageAttributes ia = new ImageAttributes())
            {
                ia.SetColorMatrix(clr.ToColorMatrix());
                g.DrawImage(img, bounds, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            }
        }

        public static void DrawFadedImage(this Graphics g, Image img, Rectangle bounds)
        {
            using (ImageAttributes ia = new ImageAttributes())
            {
                ia.SetColorMatrix(FadedMatrix);
                g.DrawImage(img, bounds, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            }
        }
        #endregion /Draw

        #region Generate
        public static GraphicsPath GenerateRoundedRectangle(this Graphics graphics, RectangleF rectangle, float radius)
        {
            float diameter;
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.0F)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }
            else
            {
                if (radius >= (Math.Min(rectangle.Width, rectangle.Height)) / 2.0)
                {
                    return graphics.GenerateCapsule(rectangle);
                }
                diameter = radius * 2.0F;
                SizeF sizeF = new SizeF(diameter, diameter);
                RectangleF arc = new RectangleF(rectangle.Location, sizeF);
                path.AddArc(arc, 180, 90);
                arc.X = rectangle.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = rectangle.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = rectangle.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();
            }
            return path;
        }

        /// <summary>
        /// Create a rounded rectangle GraphicsPath
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath GenerateRoundedRectanglePath(this Graphics g, Rectangle bounds, Single radius)
        {
            // create a path
            GraphicsPath pathBounds = new GraphicsPath();
            // arc top left
            pathBounds.AddArc(bounds.Left, bounds.Top, radius, radius, 180, 90);
            // line top
            pathBounds.AddLine(bounds.Left + radius, bounds.Top, bounds.Right - radius, bounds.Top);
            // arc top right
            pathBounds.AddArc(bounds.Right - radius, bounds.Top, radius, radius, 270, 90);
            // line right
            pathBounds.AddLine(bounds.Right, bounds.Top + radius, bounds.Right, bounds.Bottom - radius);
            // arc bottom right
            pathBounds.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            // line bottom
            pathBounds.AddLine(bounds.Right - radius, bounds.Bottom, bounds.Left + radius, bounds.Bottom);
            // arc bottom left
            pathBounds.AddArc(bounds.Left, bounds.Bottom - radius, radius, radius, 90, 90);
            // line left
            pathBounds.AddLine(bounds.Left, bounds.Bottom - radius, bounds.Left, bounds.Top + radius);
            pathBounds.CloseFigure();
            return pathBounds;
        }

        private static GraphicsPath GenerateCapsule(this Graphics graphics, RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else path.AddEllipse(baseRect);
            }
            catch { path.AddEllipse(baseRect); }
            finally { path.CloseFigure(); }
            return path;
        }
        #endregion /Generate
    }
}
