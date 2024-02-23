using Common.Constant;
using Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Common.Extensions
{
    public static partial class Extensions_Control
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Control);
        #endregion

        #region Designer
        public static readonly LicenseUsageMode m_ctorLMUsageMode = LicenseManager.UsageMode;
        public static readonly bool DesignerMode = Process.GetCurrentProcess().ProcessName == "devenv" || m_ctorLMUsageMode == LicenseUsageMode.Designtime;
        #endregion /Designer

        #region Image

        #region Invert
        public static void InvertColor(this Control control)
        {
            if (control.BackgroundImage != null)
            {
                using (Bitmap bitmap = new Bitmap(control.BackgroundImage))
                {
                    control.BackgroundImage = bitmap.InvertColor();
                }
            }
        }
        #endregion /Invert

        #endregion /Image

        #region Host
        public static Boolean IsDesignerHosted(this Control control)
        {
            if (DesignerMode)
            {
                return true;
            }
            while (control != null)
            {
                if ((control.Site != null) && control.Site.DesignMode)
                    return true;
                control = control.Parent;
            }
            return false;
        }
        #endregion /Host

        #region Type

        #region Constants
        /// <summary>
        /// Common user input controls.
        /// </summary>
        private static readonly Type[] definedUserControlTypes = new Type[] { typeof(ComboBox), typeof(TextBox), typeof(NumericUpDown), typeof(Button), typeof(RichTextBox), typeof(SwitchControl),
            typeof(GlowButton), typeof(PictureBox), typeof(RadioButton), typeof(CheckBox), typeof(DataGridView), typeof(ListBox),typeof(CheckedListBox),
            typeof(MenuStrip),typeof(TabControl)};

        /// <summary>
        /// Common user input controls.
        /// </summary>
        private static readonly Type[] ignoredControlTypes = new Type[] { typeof(UpDownBase) };

        /// <summary>
        /// Returns a list of common user input control types.
        /// </summary>
        public static Type[] DefincedUserControlTypes => definedUserControlTypes;

        /// <summary>
        /// Defined to accelerate repeat lookups.
        /// </summary>
        private static readonly HashSet<Control> knownUserControls = new HashSet<Control>(); //TODO: this should not be in extensions, control manager!!
        #endregion

        #region Is Type
        public static bool IsUserControl(this Control control)
        {
            if(knownUserControls.Contains(control) || definedUserControlTypes.Contains(control.GetType()))
            {
                knownUserControls.Add(control);
                return true;
            }
            return false;
        }
        #endregion

        #endregion /Type

        #region Search

        #region Find
        /// <summary>
        /// This method is used to find controls in the given container and sort them into 
        /// 'user' and 'inert' categoried
        /// </summary>
        /// <param name="container">The container to search through for controls.</param>
        /// <returns></returns>
        public static void FindNestedControls_HashSet(this ControlCollection container, ref HashSet<Control> inertControls, ref HashSet<Control> userControls)
        {
            if (container == null || container.Count == 0)
            {
                return;
            }
            IEnumerable<Control> allControls = container.FindNestedControls_Recursive<Control>();
            allControls.GetControlTypeFromContainer_HashSet(definedUserControlTypes, ref userControls);
            if (inertControls.Any())
            {// We have programatically defined inert controls and so should remove any accidental inclusions. 
                userControls.ExceptWith(inertControls);
            }
            inertControls.UnionWith(allControls.Except(userControls));
        }

        /// <summary>
        /// This method is used to find controls in the given container and sort them into 
        /// 'user' and 'inert' categoried
        /// </summary>
        /// <param name="container">The container to search through for controls.</param>
        /// <param name="originalSet">The set of already found controls</param>
        /// <returns></returns>
        public static void FindNestedControls_HashSet(this ControlCollection container,
            ref HashSet<Control> inertControls, ref HashSet<Control> userControls, in IEnumerable<Control> originalSet)
        {
            if (container == null || container.Count == 0)
            {
                return;
            }
            IEnumerable<Control> allControls = container.FindNestedControls_Recursive(originalSet);
           
            allControls.GetControlTypeFromContainer_HashSet(definedUserControlTypes, ref userControls);
            if (inertControls.Any())
            {// We have programatically defined inert controls and so should remove any accidental inclusions. 
                userControls.ExceptWith(inertControls);
            }
            inertControls.UnionWith(allControls.Except(userControls));
        }

        public static HashSet<Control> FindNestedControls_HashSet(this ControlCollection container, in IEnumerable<Control> originalSet = null)
        {
            if (container == null || container.Count == 0)
            {
                return new HashSet<Control>();
            }
            if (originalSet == null)
            {// This is likely
                return container.FindNestedControls_Recursive<Control>().ToHashSet();
            }
            return container.FindNestedControls_Recursive(originalSet).ToHashSet();
        }

        public static HashSet<T> FindNestedControls_HashSet<T>(this ControlCollection container, in IEnumerable<T> originalSet = null) where T : Control
        {
            if (container == null || container.Count == 0)
            {
                return new HashSet<T>();
            }
            if (originalSet == null)
            {// This is likely
                return container.FindNestedControls_Recursive<T>().ToHashSet();
            }
            return container.FindNestedControls_Recursive(originalSet).ToHashSet();
        }
        #endregion /Find

        #region Recursive Function
        /// <summary>
        /// Recursive search for controls of a type in a collection.
        /// </summary>
        /// <typeparam name="T">Type of control to find.</typeparam>
        /// <param name="container">Container to search through for the specified control type.</param>
        private static IEnumerable<T> FindNestedControls_Recursive<T>(this ControlCollection container) where T : Control
        {
            HashSet<T> foundControlsOfType = new HashSet<T>();
            container.FindNestedControls_Recursive(ref foundControlsOfType);
            return foundControlsOfType;
        }

        /// <summary>
        /// Recursive search for controls of a type in a collection.
        /// </summary>
        /// <typeparam name="T">Type of control to find.</typeparam>
        /// <param name="container">Container to search through for the specified control type.</param>
        /// <param name="originalSet">The set of already found controls</param>
        /// <returns></returns>
        private static IEnumerable<T> FindNestedControls_Recursive<T>(this ControlCollection container, in IEnumerable<T> originalSet) where T : Control
        {
            HashSet<T> foundControlsOfType = new HashSet<T>(originalSet);
            container.FindNestedControls_Recursive(ref foundControlsOfType);
            return foundControlsOfType;
        }

        /// <summary>
        /// Recursive search for controls of a type in a collection.
        /// </summary>
        /// <typeparam name="T">Type of control to find.</typeparam>
        /// <param name="container">Container to search through for the specified control type.</param>
        /// <param name="containers">Containers in which the controls were found.</param>
        private static void FindNestedControls_Recursive<T>(this ControlCollection container, ref HashSet<T> refSet) where T : Control
        {
            if (container != null && container.Count > 0)
            {
                try
                {
                    container.GetControlTypeFromContainer_HashSet(out HashSet<T> controls_HashSet);
                    for(int c = 0; c < controls_HashSet.Count; c++) 
                    {
                        Control control = container[c];
                        if (control != null && control is T controlAsType)
                        {
                            Type controlType = control.GetType();
                            if(definedUserControlTypes.Contains(controlType))
                            {// We have reached the depth requires, no more recursion required.
                                refSet.Add(controlAsType);
                                continue;
                            }
                            else if (ignoredControlTypes.Contains(controlType))
                            {// Composite controls or other nusances.
                                continue;
                            }
                            else if (control is SplitContainer splitContainer)
                            {
                                refSet.Add(controlAsType);
                                splitContainer.Panel1.Controls.FindNestedControls_Recursive(ref refSet);
                                splitContainer.Panel2.Controls.FindNestedControls_Recursive(ref refSet);
                                continue;
                            }
                            else
                            {
                                refSet.Add(controlAsType);
                                control.Controls.FindNestedControls_Recursive(ref refSet);
                                continue;
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion /Recursive Function

        #region Get

        #region General
        public static void GetControlTypeFromContainer_HashSet<T>(this ControlCollection controlCollection, out HashSet<T> resultSet) where T : Control
        {
            resultSet = controlCollection.GetControlTypeFromContainer<T>().ToHashSet();
        }

        public static IEnumerable<T> GetControlTypeFromContainer<T>(this ControlCollection controlCollection) where T : Control
        {
            IEnumerable<T> controls =
            (
                from Control control in controlCollection
                where control is T
                select control
            ).Cast<T>();
            return controls;
        }
        #endregion

        #region Select 
        private static void GetControlTypeFromContainer_HashSet(this IEnumerable<Control> controls, Type[] selectTypes, ref HashSet<Control> resultSet)
        {
            resultSet.UnionWith(controls.GetControlTypeFromContainer(selectTypes));
        }

        private static IEnumerable<Control> GetControlTypeFromContainer(this IEnumerable<Control> controls, Type[] selectTypes)
        {
            IEnumerable<Control> selectedControls =
            (
                from Control control in controls
                where control.IsUserControl()
                select control
            );
            return selectedControls;
        }
        #endregion

        #endregion /Get

        #endregion /Search

        #region Paint
        /// <summary>
        /// Suspends painting for the target control. Do NOT forget to call EndControlUpdate!!!
        /// </summary>
        /// <param name="control">visual control</param>
        public static void BeginControlUpdate(this Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, Tokens.WM_SET_REDRAW, IntPtr.Zero, IntPtr.Zero);
            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        /// <summary>
        /// Resumes painting for the target control. Intended to be called following a call to BeginControlUpdate()
        /// </summary>
        /// <param name="control">visual control</param>
        public static void EndControlUpdate(this Control control)
        {
            // Create a C "true" boolean as an IntPtr
            IntPtr wparam = new IntPtr(1);
            Message msgResumeUpdate = Message.Create(control.Handle, Tokens.WM_SET_REDRAW, wparam, IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgResumeUpdate);
            control.Invalidate();
            control.Refresh();
        }
        #endregion

        #region Mouse
        public static bool IsMouseOverControl(this Control control)
        {
            return control.ClientRectangle.Contains(control.PointToClient(Cursor.Position));
        }

        public static bool IsMouseOverControl(this Control control, Padding margin)
        {
            Rectangle rectangle = control.RectangleToScreen(control.ClientRectangle);
            rectangle.Width -= margin.Horizontal;
            rectangle.Height -= margin.Vertical;
            rectangle.X += margin.Top;
            rectangle.Y += margin.Left;

            return rectangle.Contains(Cursor.Position);
        }

        public static bool IsMouseOverControl(this Control control, out Point cursorPosition)
        {
            cursorPosition = Cursor.Position;
            return control.ClientRectangle.Contains(control.PointToClient(cursorPosition));
        }
        #endregion

        #region Font
        /// <summary>
        /// This method will set the font of all children of the given parent control.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="fontName"></param>
        /// <param name="fontStyle"></param>
        public static void ChangeFont(this Form parent, String fontName = "Arial", FontStyle fontStyle = FontStyle.Regular)
        {
            IEnumerable<Control> children = parent.Descendants<Control>();
            children.ChangeFont(fontName, fontStyle);
        }

        /// <summary>
        /// This method will set the font of all children of the given parent control.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="fontName"></param>
        /// <param name="fontStyle"></param>
        public static void ChangeFont(this IEnumerable<Control> controls, String fontName = "Arial", FontStyle fontStyle = FontStyle.Regular)
        {
            FontFamily fontFamily = new FontFamily(fontName);
            foreach (Control control in controls)
            {
                control.ChangeFont(fontFamily, fontStyle);
            }
        }

        /// <summary>
        /// This method will set the font of all children of the given parent control.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="fontName"></param>
        /// <param name="fontStyle"></param>
        public static void ChangeFont(this Control control, FontFamily fontFamily, FontStyle fontStyle = FontStyle.Regular)
        {
            control.Font = new Font(fontFamily, control.Font.Size, fontStyle);
        }
        #endregion Font

        #region Text
        /// <summary>
        /// This method will determine the control type and then adjust its width as appropriate to the contents.
        /// Note: this should only be used when the type is generic, otherwise use the type specific method to save the
        /// processor the branch processing. 
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="control"></param>
        public static void SetControlSizeFromContents(this Control control)
        { 
            if (control is ComboBox comboBox)
            {
                comboBox.SetSizeFromContents();
                return;
            }
            else if (control is TextBox textBox)
            {
                textBox.SetSizeFromContents();
                return;
            }
        }

        /// <summary>
        /// Wrapper for TextRenderer class MeasureText method.
        /// </summary>
        /// <param name="control">Control containing text to measure.</param>
        /// <returns></returns>
        public static Size MeasureText(this Control control)
        {
            return control.Text.MeasureText(control.Font);
        }

        /// <summary>
        ///  Wrapper for TextRenderer class MeasureText method.
        /// </summary>
        /// <param name="control">Control containing text to measure.</param>
        /// /// <param name="padding">Padding to add to result.</param>
        /// <returns>Height of the text.</returns>
        public static int MeasureText_Height(this Control control, int padding = 0)
        {
            return control.Text.MeasureText_Height(control.Font, padding);
        }

        /// <summary>
        ///  Wrapper for TextRenderer class MeasureText method.
        /// </summary>
        /// <param name="control">Control containing text to measure.</param>
        /// <param name="padding">Padding to add to result.</param>
        /// <returns>Width of the text.</returns>
        public static int MeasureText_Width(this Control control, int padding = 0)
        {
            return control.Text.MeasureText_Width(control.Font, padding);
        }

        public static void SetWidthToLongestLengthTextSequence_SpaceDelimited(this Control control, int padding = 0)
        {
            control.SetWidthToLongestLengthTextSequence_SpaceDelimited(control.Font, padding);
        }

        public static void SetWidthToLongestLengthTextSequence_SpaceDelimited(this Control control, Font font, int padding = 0)
        {
            int width = control.Text.GetLongestWidthSequence_CharDelimited(font, Tokens._s_, padding);
            control.MinimumSize = new Size(width, 0);
            control.Size = new Size(Math.Max(width, control.Size.Width), control.Height);
        }

        public static void SetSquareByLongestLengthTextSequence_SpaceDelimited(this Control control, Font font, int padding = 0)
        {
            int width = control.Text.GetLongestWidthSequence_CharDelimited(font, Tokens._s_, padding);
            control.MinimumSize = new Size(width, 0);
            control.Size = new Size(Math.Max(width, control.Size.Width), control.Height);
            control.EnforceSquare();
        }

        #endregion /Text

        #region Text Measurement
        public static void GetTextSize(this Control control, out int width, out int height)
        {
            Size textSize = control.GetTextSize();
            width =  textSize.Width;
            height = textSize.Height;
        }

        public static Size GetTextSize(this Control control)
        {
            return TextRenderer.MeasureText(control.Text, control.Font);
        }

        public static int GetTextSize_Width(this Control control)
        {
            Size textSize = control.GetTextSize();
            return textSize.Width;
        }

        public static int GetTextSize_Height(this Control control)
        {
            Size textSize = control.GetTextSize();
            return textSize.Height;
        }
        #endregion /Text Measurement

        #region Size
        /// <summary>
        /// This method will provide the width of the control including margins.
        /// </summary>
        /// <param name="control">Control to be measured.</param>
        /// <param name="padding">[Optional] padding to be added to the returned width measuement.</param>
        /// <returns>Width of the control with horizontal margins included, 
        /// plus any provided padding.</returns>
        public static Int32 FullWidth(this Control control, Int32 padding = 0)
        {
            return control.Width + control.Margin.Horizontal + padding;
        }

        /// <summary>
        /// This method will provide the height of the control including margins.
        /// </summary>
        /// <param name="control">Control to be measured.</param>
        /// <param name="padding">[Optional] padding to be added to the returned height measuement.</param>
        /// <returns>Width of the control with vertical margins included, 
        /// plus any provided padding.</returns>
        public static Int32 FullHeight(this Control control, Int32 padding = 0)
        {
            return control.Height + control.Margin.Vertical + padding;
        }
        #endregion /Size

        #region Resize
        public static void AdjustWidthToText(this Control control, int minWidth, int padding)
        {
            AdjustWidthToText(control, control.Text, minWidth, padding);
        }

        public static void AdjustWidthToText(this Control control, string text, int minWidth, int padding)
        {
            control.Width = Math.Max(minWidth, TextRenderer.MeasureText(text, control.Font).Width + padding);
        }

        public static void EnforceSquare(this Control control)
        {
            int sideLength = Math.Max(Math.Min(control.MaximumSize.Width, control.Width), Math.Min(control.MaximumSize.Height, control.Height));
            control.Size = new Size(sideLength, sideLength);
        }
        #endregion

        #region Child Finder
        // I got lost in the mall a lot as a child, so thank goodness for child finders.

        /// <summary>
        /// This recursive method will find all of the nested controls in a given control.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IEnumerable<T> Descendants<T>(this Control control) where T : class
        {
            foreach (Control child in control.Controls)
            {
                if (child is T childOfT)
                {
                    yield return (T)childOfT;
                }

                if (child.HasChildren)
                {
                    foreach (T descendant in Descendants<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
        #endregion /Child Finder

        #region Parent Color
        /// <summary>
        /// This method will return the back color of the first ascendent (parent) that has one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Color GetAscendantColor(this Control control) 
        {
            if(control.Parent != null)
            {
                if(control.Parent.BackColor != Color.Transparent)
                {
                    return control.Parent.BackColor;
                }
                return GetAscendantColor(control.Parent);
            }
            return control.BackColor;
        }
        #endregion /Parent Color

        #region Visibilty Test

        [LibraryImport("user32.dll")]
        public static partial IntPtr WindowFromPoint(POINT Point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X { get; set; }
            public int Y { get; set; }

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }

            public static implicit operator POINT(Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        /// <summary>
        /// This method will rutrun true if the form is not hidden from
        /// the user.
        /// </summary>
        /// <param name="control"></param>
        /// <returns>True if the window is visible to the user, else false.</returns>
        public static bool IsVisibleToUser(this Control control, bool throwException = false)
        {
            bool visible = false;
            try
            {
                var pos = control.PointToScreen(control.Location);
                var pointsToCheck = new POINT[]
                {
                    pos,
                    new Point(pos.X + control.Width - 1, pos.Y),
                    new Point(pos.X, pos.Y + control.Height - 1),
                    new Point(pos.X + control.Width - 1, pos.Y + control.Height - 1),
                    new Point(pos.X + control.Width/2, pos.Y + control.Height/2)
                };
                for(int p = 0; p < pointsToCheck.Length; p++) 
                {
                    try
                    {
                        POINT point = pointsToCheck[p];
                        IntPtr windowHandler = WindowFromPoint(point);
                        Control coveringControl = Control.FromChildHandle(windowHandler);
                        if (coveringControl != null)
                        {
                            if (control == coveringControl || control.Contains(coveringControl) || coveringControl.Contains(control))
                            {
                                visible = true;
                                break;// We are done here, huzzah!
                            }
                        }
                    }
                    catch
                    {
                        if(throwException)
                        {
                            throw;
                        }
                    }
               }
            }
            catch (ObjectDisposedException)
            {
                // No one will ever know this happned. 
            }
            catch
            {
                throw;
            }
            return visible;
        }

        /// <summary>
        /// This method will rutrun true if the form is not fully hidden from the user.
        /// </summary>
        /// <param name="control"></param>
        /// <returns>True if the window is visible to the user, else false.</returns>
        public static bool IsObscured(this Control control)
        {
            bool obscured = true;
            try
            {
                var pos = control.PointToScreen(control.Location);
                var pointsToCheck = new POINT[]
                {
                    pos,
                    new Point(pos.X + control.Width - 1, pos.Y),
                    new Point(pos.X, pos.Y + control.Height - 1),
                    new Point(pos.X + control.Width - 1, pos.Y + control.Height - 1),
                    new Point(pos.X + control.Width/2, pos.Y + control.Height/2)
                };
                Parallel.For(0, pointsToCheck.Length, (pointIndex, loopState) =>
                {
                    POINT point = pointsToCheck[pointIndex];
                    IntPtr windowHandler = WindowFromPoint(point);
                    Control coveringControl = Control.FromChildHandle(windowHandler);
                    if (coveringControl != null)
                    {
                        if (!(control == coveringControl && control.Contains(coveringControl)))
                        {
                            obscured = false;// We have proven we can see it at least partially.
                        }
                    }
                });
            }
            catch (ObjectDisposedException)
            {
                // Pay no heed good citizen!
            }
            catch
            {
                throw;
            }
            return obscured;
        }
        #endregion /Visibilty Test

        #region Invoke
        public static void InvokeOnControl(this Control control, params MethodInvoker[] methods)
        {
            for (int at = 0; at < methods.Length; at++)
            {
                control?.Invoke(methods[at]);
            }
        }
        #endregion
    }
}
