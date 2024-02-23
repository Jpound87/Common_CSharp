using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    public static class Extensions_ComboBoxItem
    {
        #region Identity
        public const String ClassName = nameof(Extensions_ComboBoxItem);
        #endregion

        #region Create 
        public static bool TryMakeComboBoxItemArray<T1, T2>(this Dictionary<T1, T2> dictionary, out ComboBoxItem[] comboBoxItems)
        {
            try
            {
                if (dictionary != null && dictionary.Count > 0)
                {
                    lock (dictionary)
                    {// Can't lock on null so check first....
                        comboBoxItems = new ComboBoxItem[dictionary.Count];
                        for (int d = 0; d < dictionary.Count; d++)
                        {
                            comboBoxItems[d] = new ComboBoxItem(dictionary.Keys.ElementAt(d).ToString(), dictionary.Values.ElementAt(d));
                        }
                        return true;
                    }
                }
            }
            catch
            {
                
            }
            comboBoxItems = new ComboBoxItem[0];
            return false;
        }
        #endregion /Create

        #region Conversion
        public static bool TryConvertToComboBoxItem(this Object objComboBoxItem, out ComboBoxItem comboBoxItem)
        {
            try
            {
                comboBoxItem = objComboBoxItem as ComboBoxItem;
                return true;
            }
            catch
            {
                comboBoxItem = new ComboBoxItem();
                return false;
            }
        }
        #endregion /Conversion
    }
}
