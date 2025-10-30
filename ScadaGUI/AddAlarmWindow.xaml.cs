using DataConcentrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScadaGUI
{
    public partial class AddAlarmWindow : Window
    {
        public AnalogInput SelectedTag { get; private set; }
        public string AlarmId => AlarmIdTextBox.Text;
        public bool IsHigh => HighRadio.IsChecked == true;
        public double LimitValue { get; private set; }
        public string MessageText => MessageTextBox.Text;

        public AddAlarmWindow(List<AnalogInput> availableTags)
        {
            InitializeComponent();
            TagComboBox.ItemsSource = availableTags;
            TagComboBox.SelectedIndex = 0;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            SelectedTag = TagComboBox.SelectedItem as AnalogInput;
            
            // U zavisnosti od izbora da li high limit ili low limit, postavljamo limit koji je definisan za dati AnalogInput
            LimitValue = IsHigh ? SelectedTag.HighLimit : SelectedTag.LowLimit;

            DialogResult = true;
            Close();
        }
    }
}
