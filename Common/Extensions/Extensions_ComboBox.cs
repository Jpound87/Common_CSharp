using Common.Constant;
using Common.Utility;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_ComboBox
    {
        #region Identity
        public const String ClassName = nameof(Extensions_ComboBox);
        #endregion

        #region ComboBox

        #region Text
        /// <summary>
        /// This method takes a combobox and adjusts its drop down
        /// box to be wide enough to display the elements in it
        /// </summary>
        /// <param name="comboBox">The ComboBox to resize</param>
        public static void SetSizeFromContents(this ComboBox comboBox, bool shrink = false)
        {
            try
            {
                int currItemWidth;
                int dropWidth;
                int boxWidth;
                comboBox.SuspendLayout();
                for (int comboBoxItemIndex = 0; comboBoxItemIndex < comboBox.Items.Count; comboBoxItemIndex++)
                {
                    if (comboBox.Items[comboBoxItemIndex] is ComboBoxItem comboBoxItem)
                    {
                        currItemWidth = Extensions_String.MeasureText(comboBoxItem.Text, comboBox.Font).Width;
                        dropWidth = currItemWidth + 7;
                        boxWidth = currItemWidth + 17;
                    }
                    else
                    {
                        currItemWidth = Extensions_String.MeasureText(comboBox.Items[comboBoxItemIndex].ToString(), comboBox.Font).Width;
                        dropWidth = currItemWidth + 7;
                        boxWidth = currItemWidth + 17;
                    }
                    if (dropWidth > comboBox.DropDownWidth)
                    {// If we are trying to display more digits than the minimum size window will allow.
                        comboBox.DropDownWidth = dropWidth;// 7 for some padding, why 7 specifically? Cuz.
                    }
                    if (shrink || boxWidth > comboBox.Width)
                    {
                        comboBox.Width = boxWidth;
                    }
                }
            }
            finally
            {
                comboBox.ResumeLayout();
            }
        }
        #endregion

        #region Resize

        public static void ResizeToContents(this ComboBox[] comboBoxes)
        {
            int maxWidth = 0;
            foreach (ComboBox comboBox in comboBoxes)
            {
                foreach (ComboBoxItem comboBoxItem in comboBox.Items)
                {
                    maxWidth = Math.Max(TextRenderer.MeasureText(comboBoxItem.Text, comboBox.Font).Width, maxWidth);
                }
            }
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.Width = maxWidth;
            }
        }

        public static void ResizeToContents(this ComboBox comboBox, int minWidth = 30)
        {
            int maxWidth = 0;
            foreach (ComboBoxItem comboBoxItem in comboBox.Items)
            {
                if (TextRenderer.MeasureText(comboBoxItem.Text, comboBox.Font).Width >= minWidth)
                {
                    maxWidth = Math.Max(TextRenderer.MeasureText(comboBoxItem.Text, comboBox.Font).Width, maxWidth);
                }
            }
            comboBox.Width = maxWidth + 30;// 30 for the drop down arrow.
        }
        #endregion /Resize

        #region Add Items
        public static void TryAddItemRange<T>(this ComboBox comboBox, T selectItem, params ComboBoxItem[] comboBoxItems) where T : Enum
        {
            comboBox.Items.Clear();
            comboBox.TryAddItemRange(comboBoxItems);
            if (comboBox.Items.Count > 0)
            {
                if (!comboBox.TrySelect_Enumeration(selectItem))
                {
                    comboBox.SelectedIndex = 0;
                }
            }
            else
            {
                comboBox.Enabled = false;
            }
        }

        public static void TryAddItemRange(this ComboBox comboBox, params ComboBoxItem[] comboBoxItems)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(comboBoxItems);
        }
        #endregion / Add Items

        #region Select

        #region First
        public static void SelectFirst(this ComboBox cboBox)
        {
            if (cboBox.Items.Count > 0)
            {
                cboBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region Enumeration

        #region Add
        public static bool TryAddItemsFromEnum<T>(this ComboBox cboEnum, bool throwException = false) where T : Enum
        {
            try
            {
                cboEnum.Items.AddRange(Utility_ComboBoxItem.GetComboBoxItemsFromEnumAsArray<Themes>());
                cboEnum.SelectedIndex = 0;
                return true;
            }
            catch 
            {
                if (throwException)
                {
                    throw;
                }
            }
            return false;
        }

        public static bool TryAddItemsFromEnum_SelectItem<T>(this ComboBox cboEnum, T selectEnum, bool throwException = false) where T : Enum
        {
            try
            {
                cboEnum.Items.AddRange(Utility_ComboBoxItem.GetComboBoxItemsFromEnumAsArray<T>());
                if (!cboEnum.TrySelect_Enumeration(selectEnum))
                {
                    cboEnum.SelectedIndex = 0;
                    return false;
                }
                return true;
            }
            catch
            {
                if(throwException)
                {
                    throw;
                }
            }
            return false;
        }
        #endregion /Add

        #region Select
        /// <summary>
        /// This method retrieves the enumeration code from the referenced device and then
        /// sets the referenced combobox to the corresponding item
        /// </summary>
        /// <param name="cboEnum">The combobox containing the enumeration choices</param>
        /// <param name="enumInt">The value to be selected</param>
        public static bool Select_Enumeration(this ComboBox cboEnum, int enumInt)
        {
            return TrySelect_Enumeration(cboEnum, enumInt.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This method retrieves the enumeration code from the referenced device and then
        /// sets the referenced combobox to the corresponding item
        /// </summary>
        /// <param name="cboEnum">The combobox containing the enumeration choices</param>
        /// <param name="enumStr">String containing the value to be selected</param>
        public static bool TrySelect_Enumeration(this ComboBox cboEnum, String enumStr, bool throwException = false)
        {
            try
            {
                for (int i = 0; i < cboEnum.Items.Count; i++)
                {
                    if (cboEnum.Items[i] is ComboBoxItem comboBoxItem)
                    {
                        if (((int)comboBoxItem.Value).ToString() == enumStr)
                        {
                            cboEnum.SelectedIndex = i;
                            return true;
                        }
                    }
                    else
                    {
                        if (cboEnum.Items[i].ToString() == enumStr)
                        {
                            cboEnum.SelectedIndex = i;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static bool TrySelect_Enumeration<T>(this ComboBox cboEnum, T desiredEnum, bool throwException = false) where T : Enum
        {
            try
            {
                for (int i = 0; i < cboEnum.Items.Count; i++)
                {
                    if (cboEnum.Items[i] is ComboBoxItem comboBoxItem)
                    {
                        if (comboBoxItem.Value is T enumerationValue)
                        {
                            if (enumerationValue.Equals(desiredEnum))
                            {
                                if (cboEnum.SelectedIndex != i)
                                {// Already selected.
                                    cboEnum.SelectedIndex = i;
                                }
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (cboEnum.Items[i].ToString() == desiredEnum.ToString())
                        {
                            if (cboEnum.SelectedIndex != i)
                            {// Already selected.
                                cboEnum.SelectedIndex = i;
                            }
                            return true;
                        }
                    }
                }
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static bool TrySelect_Enumeration<T>(this DataGridViewComboBoxCell cboEnum, T desiredEnum, bool throwException = false) where T : Enum
        {
            try
            {
                for (int i = 0; i < cboEnum.Items.Count; i++)
                {
                    if (cboEnum.Items[i] is ComboBoxItem comboBoxItem)
                    {
                        if (comboBoxItem.Value is T enumerationValue)
                        {
                            if (enumerationValue.Equals(desiredEnum))
                            {
                                cboEnum.Value = cboEnum.Items[i];
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (cboEnum.Items[i].ToString() == desiredEnum.ToString())
                        {
                            cboEnum.Value = desiredEnum;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        #endregion /Select

        #endregion /Enumeration

        #region Name
        public static bool TrySelect_Name(this ComboBox comboBox, string nameToFind, bool throwException = false)
        {
            try
            {
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    if (comboBox.Items[i] is ComboBoxItem comboBoxItem)
                    {
                        string itemName = comboBoxItem.Text;
                        if (nameToFind == itemName)
                        {
                            comboBox.SelectedIndex = i;
                            return true;
                        }
                    }
                    else if (comboBox.Items[i].ToString() == nameToFind)
                    {
                        comboBox.SelectedIndex = i;
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion /Name

        #region Value
        public static bool TrySelect_Value_AsNumeric(this ComboBox comboBox, string valueToFindAsHexString)
        {
            if (Utility_General.TryConvertFromHexStringToInt(valueToFindAsHexString, out int valueAsInt))
            {
                return TrySelect_Value_AsNumeric(comboBox, (double)valueAsInt);
            }
            return false;
        }

        public static bool TrySelect_Value_AsNumeric(this ComboBox comboBox, double valueToFind, bool throwException = false)
        {
            try
            {
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    ComboBoxItem comboBoxItem = comboBox.Items[i] as ComboBoxItem;
                    double itemValue = Double.Parse(comboBoxItem.Value as string);
                    if (valueToFind == itemValue)
                    {
                        comboBox.SelectedIndex = i;
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void SelectValue<T>(this ComboBox comboBox, T valueToFind) where T : IEquatable<T>
        {
            if (!(comboBox == null || valueToFind == null))
            {
#if DEBUG
                string type = typeof(T).FullName;
#endif
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    if (comboBox.Items[i] is ComboBoxItem comboBoxItem)
                    {
                        if (comboBoxItem.Value is T compareValue)
                        {
                            if (compareValue.Equals(valueToFind))
                            {
                                comboBox.SelectedIndex = i;
                                return; // We are done;
                            }
                        }
                        else if (comboBoxItem.Value is Enum enumCompareValue)
                        {
#if DEBUG
                            //object enumCompareValue_Obj = Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode());
                            //TypeCode enumCompareValue_TypeCode = enumCompareValue.GetTypeCode();
                            //object enumCompareValue_ObjAsType = Convert.ChangeType(Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode()), typeof(T));
#endif
                            if (Convert.ChangeType(Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode()), typeof(T)).Equals(valueToFind))
                            {
                                comboBox.SelectedIndex = i;
                                return; // We are done;
                            }
                        }
                    }
                    else if (valueToFind is string nameToFind && comboBox.Items[i].ToString() == nameToFind)
                    {
                        comboBox.SelectedIndex = i;
                    }
                }
            }
        }

        public static void SelectValue<T>(this ComboBox comboBox, T valueToFind, int defaultIndex) where T : IEquatable<T>
        {
            if (!(comboBox == null || valueToFind == null))
            {
#if DEBUG
                string type = typeof(T).FullName;
#endif
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    if (comboBox.Items[i] is ComboBoxItem comboBoxItem)
                    {
                        if (comboBoxItem.Value is T compareValue)
                        {
                            if (compareValue.Equals(valueToFind))
                            {
                                comboBox.SelectedIndex = i;
                                return; // We are done;
                            }
                        }
                        else if (comboBoxItem.Value is Enum enumCompareValue)
                        {
#if DEBUG
                            //object enumCompareValue_Obj = Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode());
                            //TypeCode enumCompareValue_TypeCode = enumCompareValue.GetTypeCode();
                            //object enumCompareValue_ObjAsType = Convert.ChangeType(Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode()), typeof(T));
#endif
                            if (Convert.ChangeType(Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode()), typeof(T)).Equals(valueToFind))
                            {
                                comboBox.SelectedIndex = i;
                                return; // We are done;
                            }
                        }
                    }
                    else if (valueToFind is string nameToFind && comboBox.Items[i].ToString() == nameToFind)
                    {
                        comboBox.SelectedIndex = i;
                        return; // We are done;
                    }
                }
            }
            comboBox.SelectedIndex = defaultIndex;
        }

        public static bool TrySelectValue<T>(this ComboBox comboBox, T valueToFind) where T : IEquatable<T>
        {
            if (!(comboBox == null || valueToFind == null))
            {
                comboBox.Enabled = false;
                try
                {
                    if (comboBox.SelectedIndex > -1)
                    {
                        if (comboBox.SelectedItem is ComboBoxItem comboBoxItem)
                        {
                            if (comboBoxItem.Value is T compareValue)
                            {
                                if (compareValue.Equals(valueToFind))
                                {// Already at the right location
                                    return true;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        if (comboBox.SelectedIndex != i)
                        {// Already here
                            if (comboBox.Items[i] is ComboBoxItem comboBoxItem)
                            {

                                if (comboBoxItem.Value is T compareValue)
                                {

                                    if (compareValue.Equals(valueToFind))
                                    {
                                        comboBox.SelectedIndex = i;
                                        return true;
                                    }
                                }
                            }
                            else if (valueToFind is string nameToFind && comboBox.Items[i].ToString() == nameToFind)
                            {// Not combobox item
                                comboBox.SelectedIndex = i;
                                return true;
                            }
                        }
                    }
                }
                catch (Exception) { }// Don't even care at all... okay maybe later a loggable offence
                finally
                {
                    comboBox.Enabled = true;
                }
            }
            return false;
        }

        public static bool TrySelect_Value(this ComboBox comboBox, string valueToFind, bool throwException = false)
        {
            if (String.IsNullOrEmpty(valueToFind))
            {
                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }
                return false;
            }
            else
            {
                try
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        ComboBoxItem comboBoxItem = comboBox.Items[i] as ComboBoxItem;
                        string itemValue = comboBoxItem.Value.ToString();
                        if (valueToFind == itemValue)
                        {
                            comboBox.SelectedIndex = i;
                            return true;
                        }
                    }
                    return false;
                }
                catch
                {
                    if (throwException)
                    {
                        throw;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        #endregion /Value

        #endregion /Select

        #region Get Value
        public static bool TryGetSelectedValue_AsType<T>(this ComboBox comboBox, out T result, bool throwException = false)
        {
            Type typeT;
            string nameT;
            try
            {
                if (comboBox.SelectedItem != null)
                {
                    typeT = typeof(T);
                    nameT = typeT.Name;
                    if (comboBox.SelectedItem is ComboBoxItem comboboxItem)
                    {
                        if (comboboxItem.Value is T _result)
                        {
                            result = _result;
                            return true;
                        }
                    }
                }
                result = default;
                return false;
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
                else
                {
                    result = default;
                    return false;
                }
            }
        }

        public static String GetSelectedValue_AsString(this ComboBox comboBox, bool throwException = false)
        {
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                try
                {
                    if (comboBox.SelectedItem.TryConvertToComboBoxItem(out ComboBoxItem comboBoxItem))
                    {
                        return comboBoxItem.Value as String;
                    }
                    else
                    {
                        return comboBox.SelectedItem.ToString();
                    }
                }
                catch
                {
                    if (throwException)
                    {
                        throw;
                    }
                }
            }
            return Tokens.Alert;
        }

        public static String GetSelectedText_AsString(this ComboBox comboBox, bool throwException = false)
        {
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                try
                {
                    if (comboBox.SelectedItem.TryConvertToComboBoxItem(out ComboBoxItem comboBoxItem))
                    {
                        return comboBoxItem.Text as String;
                    }
                    else
                    {
                        return comboBox.SelectedItem.ToString();
                    }
                }
                catch
                {
                    if (throwException)
                    {
                        throw;
                    }
                }
            }
            return Tokens.Alert;
        }

        public static Object GetSelectedValue_AsObject(this ComboBox comboBox, bool throwException = false)
        {
            try
            {
                if (comboBox.SelectedItem.TryConvertToComboBoxItem(out ComboBoxItem comboBoxItem))
                {
                    return comboBoxItem.Value;
                }
                else
                {
                    return comboBox.SelectedItem;
                }
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
            }
            return default;
        }
        #endregion /Get Value

        #endregion /ComboBox

        #region ToolStripComboBox

        #region Select
        public static void SelectValue<T>(this ToolStripComboBox comboBox, T valueToFind) where T : IEquatable<T>
        {
            if (!(comboBox == null || valueToFind == null))
            {
#if DEBUG
                string type = typeof(T).FullName;
#endif
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    if (comboBox.Items[i] is ComboBoxItem comboBoxItem)
                    {
                        if (comboBoxItem.Value is T compareValue)
                        {
                            if (compareValue.Equals(valueToFind))
                            {
                                comboBox.SelectedIndex = i;
                                return; // We are done;
                            }
                        }
                        else if (comboBoxItem.Value is Enum enumCompareValue)
                        {
#if DEBUG
                            //object enumCompareValue_Obj = Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode());
                            //TypeCode enumCompareValue_TypeCode = enumCompareValue.GetTypeCode();
                            //object enumCompareValue_ObjAsType = Convert.ChangeType(Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode()), typeof(T));
#endif
                            if (Convert.ChangeType(Convert.ChangeType(enumCompareValue, enumCompareValue.GetTypeCode()), typeof(T)).Equals(valueToFind))
                            {
                                comboBox.SelectedIndex = i;
                                return; // We are done;
                            }
                        }
                    }
                    else if (valueToFind is string nameToFind && comboBox.Items[i].ToString() == nameToFind)
                    {
                        comboBox.SelectedIndex = i;
                    }
                }
            }
        }
        #endregion /Select

        #endregion

        #region DataGridViewComboBoxCell

        #region Select
        public static void SelectValue_DataGridViewComboBoxCell<T>(this DataGridViewComboBoxCell comboBox, T valueToFind) where T : IEquatable<T>
        {
            if (!(comboBox == null || valueToFind == null))
            {
                try
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        if (comboBox.Items[i] is ComboBoxItem comboBoxItem)
                        {
                            if (comboBoxItem.Value is T compareValue)
                            {
                                if (compareValue.Equals(valueToFind))
                                {
                                    comboBox.Value = valueToFind;
                                    return; // We are done;
                                }
                            }
                        }
                        else if (valueToFind is string nameToFind && comboBox.Items[i].ToString() == nameToFind)
                        {
                            comboBox.Value = valueToFind;
                        }
                    }
                }
                catch (Exception) { };
            }
        }
        #endregion /Select

        #endregion
    }
}
