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
using DataConcentrator;

namespace ScadaGUI
{
    public partial class AddTagWindow : Window
    {
        public string TagName => TagNameTextBox.Text;
        public string Description => DescriptionTextBox.Text;
        public string Address => AddressTextBox.Text;

        public string TagType { get; private set; }
        public bool OnOffScan { get; private set; }
        public int ScanTime { get; private set; }
        public double LowLimit { get; private set; }
        public double HighLimit { get; private set; }
        public string Units { get; private set; }
        public double InitialValue { get; private set; }
        public bool HasAlarms { get; private set; }

        public AddTagWindow()
        {
            InitializeComponent();
            // Nakon inicijalizacije, postavljamo da je default izbor AnalogInput
            AnalogInputRadio.IsChecked = true;
            TagType = "AnalogInput"; // default
            RenderDynamicFields();
        }

        // Proveravamo koji je radio button izabran i ažuriramo TagType
        private void TagTypeRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (AnalogInputRadio.IsChecked == true) TagType = "AnalogInput";
            else if (AnalogOutputRadio.IsChecked == true) TagType = "AnalogOutput";
            else if (DigitalInputRadio.IsChecked == true) TagType = "DigitalInput";
            else if (DigitalOutputRadio.IsChecked == true) TagType = "DigitalOutput";

            // Renderujemo dinamička polja na osnovu izabranog tipa taga
            RenderDynamicFields();
        }

        private void RenderDynamicFields()
        {
            DynamicFieldsPanel.Children.Clear();

            switch (TagType)
            {
                case "DigitalInput":
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Scan Time (ms):" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "ScanTimeTextBox" });
                    DynamicFieldsPanel.Children.Add(new CheckBox { Name = "OnOffScanCheckBox", Content = "On/Off Scan" });
                    break;

                case "DigitalOutput":
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Initial Value:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "InitialValueTextBox" });
                    break;

                case "AnalogInput":
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Scan Time (ms):" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "ScanTimeTextBox" });

                    DynamicFieldsPanel.Children.Add(new CheckBox { Name = "AlarmsCheckBox", Content = "Alarms Enabled" });
                    DynamicFieldsPanel.Children.Add(new CheckBox { Name = "OnOffScanCheckBox", Content = "On/Off Scan" });

                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Low Limit:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "LowLimitTextBox" });
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "High Limit:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "HighLimitTextBox" });
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Units:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "UnitsTextBox" });
                    break;

                case "AnalogOutput":
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Initial Value:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "InitialValueTextBox" });
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Low Limit:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "LowLimitTextBox" });
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "High Limit:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "HighLimitTextBox" });
                    DynamicFieldsPanel.Children.Add(new TextBlock { Text = "Units:" });
                    DynamicFieldsPanel.Children.Add(new TextBox { Name = "UnitsTextBox" });
                    break;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (UIElement element in DynamicFieldsPanel.Children)
                {
                    if (element is TextBox tb)
                    {
                        // Parsiramo vrednosti iz textboxova i castujemo ih na odgovarajuće tipove
                        switch (tb.Name)
                        {
                            case "ScanTimeTextBox": ScanTime = int.Parse(tb.Text); break;
                            case "LowLimitTextBox": LowLimit = double.Parse(tb.Text); break;
                            case "HighLimitTextBox": HighLimit = double.Parse(tb.Text); break;
                            case "UnitsTextBox": Units = tb.Text; break;
                            case "InitialValueTextBox": InitialValue = double.Parse(tb.Text); break;
                        }
                    }
                    else if (element is CheckBox cb)
                    {
                        switch (cb.Name)
                        {
                            case "OnOffScanCheckBox": OnOffScan = cb.IsChecked == true; break;
                            case "AlarmsCheckBox": HasAlarms = cb.IsChecked == true; break;
                        }
                    }
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message);
            }
        }
    }
}

