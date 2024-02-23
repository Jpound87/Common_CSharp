using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_TableLayout
    {
        #region RowStlye
        public static RowStyle GetRowStyle_Percent(float percent = 50)
        {
            return new RowStyle(SizeType.Percent, percent);
        }
        public static void AddRowStyle_Percent(this TableLayoutPanel tableLayoutPanel, uint count = 1, float percent = 50)
        {
            for (int c = 0; c < count; c++)
            {
                tableLayoutPanel.RowStyles.Add(GetRowStyle_Percent(percent));
            }
        }

        public static RowStyle GetRowStyle_AutoSize()
        {
            return new RowStyle(SizeType.AutoSize);
        }
        public static void AddRowStyle_AutoSize(this TableLayoutPanel tableLayoutPanel, uint count = 1)
        {
            for (int c = 0; c < count; c++)
            {
                tableLayoutPanel.RowStyles.Add(GetRowStyle_AutoSize());
            }
        }
        #endregion /RowStyle

        #region ColumnStyle
        public static ColumnStyle GetColumnStyle_Absolute(int width = 2)// The new vodka Absolut Roe (ewww)
        {
            return new ColumnStyle(SizeType.Absolute, width);
        }
        public static void AddColumnStyle_Absolute(this TableLayoutPanel tableLayoutPanel, uint count = 1, int width = 2)
        {
            for (int c = 0; c < count; c++)
            {
                tableLayoutPanel.ColumnStyles.Add(GetColumnStyle_Absolute(width));
            }
        }

        public static ColumnStyle GetColumnStyle_AutoSize()
        {
            return new ColumnStyle(SizeType.AutoSize);
        }
        public static void AddColumnStyle_AutoSize(this TableLayoutPanel tableLayoutPanel, uint count = 1)
        {
            for (int c = 0; c < count; c++)
            {
                tableLayoutPanel.ColumnStyles.Add(GetColumnStyle_AutoSize());
            }
        }
        #endregion /ColumnStyle
    }
}
