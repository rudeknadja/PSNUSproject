using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataConcentrator;
using System.Windows.Threading;

namespace ScadaGUI
{
    public partial class MainWindow : Window
    {
        private DataConcentrator.DataConcentrator dc;
        private ConfigurationManager configManager;

        public MainWindow()
        {
            InitializeComponent();

            dc = new DataConcentrator.DataConcentrator();
            configManager = new ConfigurationManager();
            dc.StartScanning();

            var loadedTags = configManager.LoadConfiguration();
            foreach (var tag in loadedTags)
                dc.AddTag(tag);

            RefreshTables();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) => RefreshTables();
            timer.Start();
        }

        private void RefreshTables()
        {
            TagDataGrid.ItemsSource = dc.GetAllTags();
            AlarmDataGrid.ItemsSource = dc.GetActiveAlarms();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => RefreshTables();

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTagWindow();
            if (addWindow.ShowDialog() == true)
            {
                string name = addWindow.TagName;
                string address = addWindow.Address;
                string type = addWindow.TagType;
                bool scan = addWindow.OnOffScan;

                switch (type)
                {
                    case "AnalogInput":
                        dc.AddTag(new AnalogInput(name, "User added AI", address, 0, 100, "°C", 1000, scan));
                        break;
                    case "DigitalInput":
                        dc.AddTag(new DigitalInput(name, "User added DI", address, 1000, scan));
                        break;
                    case "AnalogOutput":
                        dc.AddTag(new AnalogOutput(name, "User added AO", address, 0, 100, 0, "°C"));
                        break;
                    case "DigitalOutput":
                        dc.AddTag(new DigitalOutput(name, "User added DO", address, false));
                        break;
                }

                RefreshTables();
            }
        }

        private void RemoveTag_Click(object sender, RoutedEventArgs e)
        {
            if (TagDataGrid.SelectedItem is Tag selected)
                dc.RemoveTag(selected.TagName);
            else
                MessageBox.Show("Select a tag to remove.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            RefreshTables();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            dc.StartScanning();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            dc.StopScanning();
        }
    }
}
