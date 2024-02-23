using System;
using System.Collections.Generic;

namespace Common.Utility
{
    public static class Utility_ComboBoxItem
    {
        #region Create 

        #endregion /Create

        #region Get
        public static List<ComboBoxItem> GetComboBoxItemsFromEnumAsList<T>() where T : Enum
        {
            List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem>();
            foreach (T enumeration in Utility_Enums.GetEnumValuesAsArray<T>())
            {
                comboBoxItems.Add(new ComboBoxItem(enumeration.ToString(), enumeration));
            }
            return comboBoxItems;
        }

        public static ComboBoxItem[] GetComboBoxItemsFromEnumAsArray<T>() where T : Enum
        {
            return GetComboBoxItemsFromEnumAsList<T>().ToArray();
        }

        public static List<ComboBoxItem> GetComboBoxItemsFromEnumAsList_Aplhabetical<T>() where T : Enum
        {
            List<ComboBoxItem> comboBoxItems = GetComboBoxItemsFromEnumAsList<T>();
            comboBoxItems.Sort((a, b) => (a.Text.CompareTo(b.Text)));
            return comboBoxItems;
        }

        public static ComboBoxItem[] GetComboBoxItemsFromEnumAsArray_Aplhabetical<T>() where T : Enum
        {
            return GetComboBoxItemsFromEnumAsList_Aplhabetical<T>().ToArray();
        }
        #endregion /Get
    }
}
