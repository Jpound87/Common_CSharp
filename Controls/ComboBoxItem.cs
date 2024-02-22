using System;
using System.Collections.Generic;

namespace Utility.Interface
{
    

    /// <summary>
    /// Allows us to set up combo boxes with an easy to use item construct
    /// </summary>
    public class ComboBoxItem : IComparable
    {
        
        public string Text { get; set; }

        public object Value { get; set; }
        
        public ComboBoxItem(string text)
        {
            Text = text;
            Value = text;
        }
        
        public ComboBoxItem()
        {
            Text = String.Empty;
            Value = new object();
        }
        
        public ComboBoxItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public bool ContainsValue(object value)
        {
            return ContainsValue(this, value);
        }

        public override string ToString()
        {
            return Text;
        }

        #region Sort Comparers
        /// <summary>
        /// This method, intened to be used as a sort comparison, compares two
        /// ComboBoxItems control objects by name.
        /// </summary>
        /// <param name="thee"></param>
        /// <param name="thine"></param>
        /// <returns></returns>
        public static int Compare(ComboBoxItem thee, ComboBoxItem thine)
        {
            if (thee == null)
            {
                if (thine == null)
                {// If thee and thine are null, they're equal.
                    return 0;
                }
                else
                {// If thee is null and thine is not null, thine is greater.
                    return -1;
                }
            }
            else
            {// If thee is not null...
                if (thine == null)
                {// ...and thine is null, thee is greater.
                    return 1;
                }
                else
                {// ...and thine is not null, compare the text
                    int retval = thee.Text.CompareTo(thine.Text);
                    return retval;
                }
            }
        }

        public int CompareTo(object other)
        {
            if (other is ComboBoxItem otherItem)
            {
                return ComboBoxItem.Compare(this, otherItem);
            }
            else
            {
                throw new ArgumentException("Object is not a Temperature");
            }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// This method will check if a comboboxitem contains the given value.
        /// </summary>
        /// <param name="cboItem">The combobox item to check.</param>
        /// <param name="value">The value to check for in the comboboxitem.</param>
        /// <returnsTrue if value is found, else false.></returns>
        public static bool ContainsValue(ComboBoxItem cboItem, object value)
        {
            if (cboItem.Value.Equals(value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method will check if a list of comboboxitems contains the 
        /// given value.
        /// </summary>
        /// <param name="cboItemList">List of combobox items to check.</param>
        /// <param name="value">Value to find in list</param>
        /// <returns>True if value is found, else false.</returns>
        public static bool ListContainsValue(List<ComboBoxItem> cboItemList, object value)
        {
            return ArrayContainsValue(cboItemList.ToArray(), value);
        }

        /// <summary>
        /// This method will check if an array of combo box items contains the 
        /// given value.
        /// </summary>
        /// <param name="cboItemList">List of combobox items to check.</param>
        /// <param name="value">Value to find in list</param>
        /// <returns>True if value is found, else false.</returns>
        public static bool ArrayContainsValue(ComboBoxItem[] cboItemArray, object value)
        {
            for(int index = 0; index < cboItemArray.Length; index++)
            {
                if(ContainsValue(cboItemArray[index], value))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
