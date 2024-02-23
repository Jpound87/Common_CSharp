using System;
using System.Windows.Forms;

namespace Common.Controls
{
    public partial class ComboBox_Items_Enumeration<T> : ComboBox where T : Enum
    {
        new public T SelectedValue { get; set; }
        new public ComboBoxItem SelectedItem //Addt type combobox item! TODO: all this here 
        { 
            get; set; }
        new public ComboBoxItem[] Items { get; set; }

        public ComboBox_Items_Enumeration()
        {
            InitializeComponent();
        }
    }
}
