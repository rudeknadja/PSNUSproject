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
    public partial class RemoveAlarmWindow : Window
    {
        public Alarm SelectedAlarm { get; private set; }

        public RemoveAlarmWindow(List<Alarm> alarms)
        {
            InitializeComponent();
            AlarmComboBox.ItemsSource = alarms;
            AlarmComboBox.SelectedIndex = 0;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            SelectedAlarm = AlarmComboBox.SelectedItem as Alarm;
            DialogResult = true;
            Close();
        }
    }
}
