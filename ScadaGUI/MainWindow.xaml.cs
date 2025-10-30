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
            //dc.StartScanning();

            var loadedTags = configManager.LoadConfiguration();
            foreach (var tag in loadedTags)
            {
                dc.AddTag(tag);
                if (tag.TagName == "AI1")
                {
                    ((AnalogInput)tag).AddAlarm(new Alarm("Alarm1", tag.TagName, 50, AlarmType.High, "majmune"));
                }
            }
                

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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            configManager.SaveConfiguration(dc.GetAllTags());
            Application.Current.Shutdown();
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTagWindow();
            if (addWindow.ShowDialog() == true)
            {
                string name = addWindow.TagName;
                string desc = addWindow.Description;
                string address = addWindow.Address;
                string type = addWindow.TagType;
                // Tip taga
                switch (type)
                {
                    case "AnalogInput":
                        dc.AddTag(new AnalogInput(
                            name,
                            desc,
                            address,
                            addWindow.LowLimit,
                            addWindow.HighLimit,
                            addWindow.Units,
                            addWindow.ScanTime,
                            addWindow.OnOffScan
                        ));
                        break;

                    case "AnalogOutput":
                        dc.AddTag(new AnalogOutput(
                            name,
                            desc,
                            address,
                            addWindow.InitialValue,
                            addWindow.LowLimit,
                            addWindow.HighLimit,
                            addWindow.Units
                        ));
                        break;

                    case "DigitalInput":
                        dc.AddTag(new DigitalInput(
                            name,
                            desc,
                            address,
                            addWindow.ScanTime,
                            addWindow.OnOffScan
                        ));
                        break;

                    case "DigitalOutput":
                        dc.AddTag(new DigitalOutput(
                            name,
                            desc,
                            address,
                            addWindow.InitialValue != 0
                        ));
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

        private void AddAlarm_Click(object sender, RoutedEventArgs e)
        {
            // Filter samo analogne input tagove
            var analogInputs = dc.GetAllTags().OfType<AnalogInput>().ToList();
            if (analogInputs.Count == 0)
            {
                MessageBox.Show("No Analog Inputs available to add alarms.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var addAlarmWindow = new AddAlarmWindow(analogInputs);
            if (addAlarmWindow.ShowDialog() == true)
            {
                var selectedTag = addAlarmWindow.SelectedTag;
                if (selectedTag != null)
                {
                    var newAlarm = new Alarm(
                        addAlarmWindow.AlarmId,
                        selectedTag.TagName,
                        addAlarmWindow.LimitValue,
                        addAlarmWindow.IsHigh ? AlarmType.High : AlarmType.Low,
                        addAlarmWindow.MessageText
                    );

                    selectedTag.AddAlarm(newAlarm);

                    // Snimi sve tagove u XML
                    configManager.SaveConfiguration(dc.GetAllTags());

                    RefreshTables();
                }
            }
        }

        private void RemoveAlarm_Click(object sender, RoutedEventArgs e)
        {
            // Prikupi sve alarme iz svih analognih inputa
            var allAlarms = dc.GetAllTags()
                              .OfType<AnalogInput>()
                              .SelectMany(ai => ai.Alarms)
                              .ToList();

            if (allAlarms.Count == 0)
            {
                MessageBox.Show("No alarms to remove.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var removeWindow = new RemoveAlarmWindow(allAlarms);
            if (removeWindow.ShowDialog() == true)
            {
                var alarmToRemove = removeWindow.SelectedAlarm;
                if (alarmToRemove != null)
                {
                    // Pronađi tag kojem pripada i ukloni alarm
                    var tag = dc.GetAllTags()
                                .OfType<AnalogInput>()
                                .FirstOrDefault(ai => ai.TagName == alarmToRemove.TagName);

                    tag?.RemoveAlarm(alarmToRemove.Id);

                    // Snimi sve tagove u XML
                    configManager.SaveConfiguration(dc.GetAllTags());

                    RefreshTables();
                }
            }
        }
    }
}
