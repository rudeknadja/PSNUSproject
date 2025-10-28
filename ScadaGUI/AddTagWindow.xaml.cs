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
    public partial class AddTagWindow : Window
    {
        public string TagName { get; private set; }
        public string Address { get; private set; }
        public string TagType { get; private set; }
        public bool OnOffScan { get; private set; }

        public AddTagWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            TagName = TagNameBox.Text;
            Address = AddressBox.Text;
            TagType = (TypeBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();
            OnOffScan = ScanBox.IsChecked == true;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

