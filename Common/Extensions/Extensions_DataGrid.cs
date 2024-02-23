using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_DataGrid
    {
        #region Identity
        public const String ClassName = nameof(Extensions_DataGrid);
        #endregion

        #region DataGridView

        #region Column
        public static DataGridViewColumn[] GetColumnsFromNames(this DataGridView dataGridView, params string[] columnNames)
        {
            List<DataGridViewColumn> dataGridViewColumnCollection = new List<DataGridViewColumn>();
            foreach (string columnName in columnNames)
            {
                if (TryGetDataGridViewColumn(dataGridView, columnName, out DataGridViewColumn dataGridViewColumn))
                {
                    dataGridViewColumnCollection.Add(dataGridViewColumn);
                }
            }
            return dataGridViewColumnCollection.ToArray();
        }

        public static bool TryGetDataGridViewColumn(this DataGridView dataGridView, string columnName, out DataGridViewColumn dataGridViewColumn)
        {
            if (dataGridView.Columns.Contains(columnName))
            {
                dataGridViewColumn = dataGridView.Columns[columnName];
                return true;
            }
            dataGridViewColumn = default;
            return false;
        }
        #endregion

        #region Row

        #region Create
        public static void CreateRow_Message(this DataGridView gridView, String message)
        {
            int atRow;
            DataRow row;
            lock (gridView)
            {// We can't add more than one row at a time
                atRow = gridView.Rows.Count;
                if (gridView.DataSource is DataTable messageTable)
                {
                    row = messageTable.NewRow();
                    row[0] = message;
                    messageTable.Rows.Add(row);
                }
            }
        }
        #endregion /Create

        #region Get
        public static bool TryGetRow(this DataGridView dataGridView, int lookupColumnIndex, String searchString, out DataGridViewRow row)
        {
            if (dataGridView.Rows != null) //Check that a row exists in the source DataGridView
            {
                foreach (DataGridViewRow r in dataGridView.Rows)
                {//Search every row in source (should be between 0-4)
                    if (r.Cells[lookupColumnIndex].Value.ToString().Equals(searchString))
                    {
                        row = r;
                        return true;
                    }
                }
            }
            row = default;
            return false;
        }
        #endregion /Get

        #endregion /Row

        #endregion /DataGridView
    }
}
