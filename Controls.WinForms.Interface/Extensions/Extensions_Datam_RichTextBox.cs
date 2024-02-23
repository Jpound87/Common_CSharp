using Common.Constant;
using Configuration.Data;
using Parameters.Interface;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Datam.WinForms.Interface.Extensions
{
    public static class Extensions_Datam_RichTextBox
    {
        #region Identity
        public const string ClassName = nameof(Extensions_Datam_RichTextBox);
        #endregion

        #region Print

        #region Line
        public static void PrintLine(this RichTextBox outputTextBox, uint lineCount)
        {
            if (outputTextBox != null && !outputTextBox.IsDisposed)
            {
                StringBuilder lines = new StringBuilder(Tokens._nl_);
                outputTextBox.DeselectAll();
                for (; lineCount > 0; lineCount--)
                {
                    lines.Append(Tokens._nl_);
                }
                outputTextBox.SelectedText = lines.ToString();
                outputTextBox.DeselectAll();
                outputTextBox.ScrollToCaret();
            }
        }
        public static void PrintLine(this RichTextBox outputTextBox)
        {
            if (outputTextBox != null && !outputTextBox.IsDisposed)
            {
                outputTextBox.DeselectAll();
                outputTextBox.SelectedText = Tokens._nl_;
                outputTextBox.DeselectAll();
                outputTextBox.ScrollToCaret();
            }
        }
        public static void PrintLine(this RichTextBox outputTextBox, string outputLine = "", FontStyle fontStyle = FontStyle.Regular)
        {
            if (outputTextBox != null && !outputTextBox.IsDisposed)
            {
                outputTextBox.DeselectAll();
                outputTextBox.SelectionFont = new Font(outputTextBox.Font, fontStyle);
                outputTextBox.SelectedText = outputLine;
                outputTextBox.DeselectAll();
                outputTextBox.ScrollToCaret();
            }
        }
        #endregion

        #region Success
        public static void PrintSuccess(this RichTextBox outputTextBox, string outputLine)
        {
            if (outputTextBox != null && !outputTextBox.IsDisposed)
            {
                outputTextBox.DeselectAll();
                outputTextBox.SelectionBullet = true;
                outputTextBox.SelectionBackColor = Color.LightBlue;
                outputTextBox.SelectedText = outputLine;
                outputTextBox.DeselectAll();
                outputTextBox.SelectionBullet = false;
                outputTextBox.AppendText(Tokens._nl_);
            }
        }
        #endregion

        #region Alert
        public static void PrintAlert(this RichTextBox outputTextBox, string outputLine)
        {
            if (outputTextBox != null && !outputTextBox.IsDisposed)
            {
                Font boldFont = new Font(outputTextBox.SelectionFont, FontStyle.Bold);
                outputTextBox.DeselectAll();
                outputTextBox.SelectionBullet = true;
                outputTextBox.SelectionFont = boldFont;
                outputTextBox.SelectionBackColor = Color.LightYellow;
                outputTextBox.SelectedText = outputLine;
                outputTextBox.SelectionBullet = false;
                outputTextBox.DeselectAll();
            }
        }
        #endregion

        #region Error
        public static void PrintError(this RichTextBox outputTextBox, string outputLine)
        {
            if (outputTextBox != null && !outputTextBox.IsDisposed)
            {
                Font boldFont = new Font(outputTextBox.SelectionFont, FontStyle.Bold);
                outputTextBox.DeselectAll();
                outputTextBox.SelectionFont = boldFont;
                outputTextBox.SelectionBackColor = Color.LightPink;
                outputTextBox.SelectedText = outputLine;
                outputTextBox.DeselectAll();
            }
        }
        #endregion

        #region Bullet
        public static void PrintBulletLine(this RichTextBox outputTextBox, string outputLine, FontStyle fontStyle = FontStyle.Regular)
        {
            if (outputTextBox != null && !outputTextBox.IsDisposed)
            {
                outputTextBox.DeselectAll();
                outputTextBox.SelectionBullet = true;
                outputTextBox.SelectionFont = new Font(outputTextBox.Font, fontStyle);
                outputTextBox.SelectedText = outputLine;
                outputTextBox.SelectionBullet = false;
                outputTextBox.DeselectAll();
                outputTextBox.ScrollToCaret();
            }
        }
        #endregion

        #region Retrieve
        public static void PrintRetrieve(this RichTextBox outputTextBox, IParameter paramInfo, string data)
        {
            string outputLine;
            outputLine = string.Format("[Retrieval]\tParameter: {1}\tAddress: {0}\tValue: {2}\n", paramInfo.Address.ToString(), paramInfo.FullName, data);

            outputTextBox.DeselectAll();
            outputTextBox.SelectedText = outputLine;
            outputTextBox.DeselectAll();
            outputTextBox.AppendText(Tokens._nl_);
            outputTextBox.ScrollToCaret();
        }
        #endregion

        #region Compare

        public static void PrintCompare(this RichTextBox outputTextBox, IAddress address, string name, string currentValue, string configValue, HighlightColor highlightColor = HighlightColor.Blue)
        {
            string titleLine = $"Parameter: {name}\tAddress: {address}\n";
            string currentLine = $"Current Value:\t{currentValue}\n";
            string fileLine = $"File Value:\t{configValue}\n";

            Font ogFont = outputTextBox.SelectionFont;
            Font boldFont = new Font(outputTextBox.SelectionFont, FontStyle.Bold);
            outputTextBox.DeselectAll();
            switch (highlightColor)
            {
                case HighlightColor.Blue:
                    outputTextBox.SelectionBackColor = Color.LightBlue;
                    break;
                case HighlightColor.Yellow:
                    outputTextBox.SelectionBackColor = Color.LightYellow;
                    break;
                case HighlightColor.Red:
                    outputTextBox.SelectionBackColor = Color.LightPink;
                    break;
            }
            outputTextBox.SelectionBullet = false;
            outputTextBox.SelectionFont = boldFont;
            outputTextBox.SelectedText = titleLine;
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBullet = true;
            outputTextBox.SelectionFont = ogFont;
            outputTextBox.SelectedText = currentLine;
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBullet = true;
            outputTextBox.SelectedText = fileLine;
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBullet = false;
            outputTextBox.AppendText(Tokens._nl_);
            outputTextBox.ScrollToCaret();
        }
        #endregion

        #region Save
        public static void PrintSave(this RichTextBox outputTextBox, IConfigurationData configData)
        {
            PrintSave(outputTextBox, configData.AddressText, configData.Name, configData.ValueText);
        }

        public static void PrintSave(RichTextBox outputTextBox, string addressText, string name, string configValue)
        {
            string titleLine = $"Parameter: {name}\tAddress: {addressText}\n";
            string fileLine = $"Saved Value:\t{configValue}\n";

            Font ogFont = outputTextBox.SelectionFont;
            Font boldFont = new Font(outputTextBox.SelectionFont, FontStyle.Bold);
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBackColor = Color.LightBlue;
            outputTextBox.SelectionBullet = false;
            outputTextBox.SelectionFont = boldFont;
            outputTextBox.SelectedText = titleLine;
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBackColor = Color.White;
            outputTextBox.SelectionFont = ogFont;
            outputTextBox.SelectionBullet = true;
            outputTextBox.SelectedText = fileLine;
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBullet = false;
            outputTextBox.AppendText(Tokens._nl_);
            outputTextBox.ScrollToCaret();
        }

        public static void PrintSave(RichTextBox outputTextBox, IAddress address, string name, string configValue)
        {
            string titleLine = $"Parameter: {name}\tAddress: {address}\n";
            string fileLine = $"Saved Value:\t{configValue}\n";

            Font ogFont = outputTextBox.SelectionFont;
            Font boldFont = new Font(outputTextBox.SelectionFont, FontStyle.Bold);
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBackColor = Color.LightBlue;
            outputTextBox.SelectionBullet = false;
            outputTextBox.SelectionFont = boldFont;
            outputTextBox.SelectedText = titleLine;
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBackColor = Color.White;
            outputTextBox.SelectionFont = ogFont;
            outputTextBox.SelectionBullet = true;
            outputTextBox.SelectedText = fileLine;
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBullet = false;
            outputTextBox.AppendText(Tokens._nl_);
            outputTextBox.ScrollToCaret();
        }
        #endregion 

        #region Load
        public static void PrintLoadSuccess(this RichTextBox outputTextBox, bool success, IParameter paramInfo)
        {
            string outputLine = string.Empty;
            string titleLine = $"Parameter: {paramInfo.FullName}\tAddress: {paramInfo.Address}\n";


            Font ogFont = outputTextBox.SelectionFont;
            Font boldFont = new Font(outputTextBox.SelectionFont, FontStyle.Bold);
            if (success)
            {
                outputTextBox.SelectionBackColor = Color.LightBlue;
                outputTextBox.SelectionBullet = true;
                outputLine = $"Value: {paramInfo.Value_Cast}\n";
            }
            else
            {
                outputTextBox.SelectionBackColor = Color.LightPink;
            }
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBullet = false;
            outputTextBox.SelectionFont = boldFont;
            outputTextBox.SelectedText = titleLine;
            outputTextBox.DeselectAll();
            if (outputLine != string.Empty)
            {
                outputTextBox.SelectionBullet = true;
                outputTextBox.SelectionFont = ogFont;
                outputTextBox.SelectedText = outputLine;
            }
            outputTextBox.DeselectAll();
            outputTextBox.SelectionBullet = false;
            outputTextBox.AppendText(Tokens._nl_);
            outputTextBox.ScrollToCaret();
        }
        #endregion

        #endregion /Print
    }
}
