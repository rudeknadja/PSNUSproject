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

namespace ScadaGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private DataConcentrator.DataConcentrator dc;
        private ConfigurationManager configManager;

        public MainWindow()
        {
            InitializeComponent();

            // 🟩 Inicijalizuj DataConcentrator i ConfigurationManager
            dc = new DataConcentrator.DataConcentrator();
            configManager = new ConfigurationManager();

            // 🟩 (opciono) učitaj konfiguraciju
            var loadedTags = configManager.LoadConfiguration();
            foreach (var tag in loadedTags)
            {
                dc.AddTag(tag);
            }

            RefreshTables();
        }

        private void RefreshTables()
        {
            List<Tag> tags = dc.GetAllTags();
            List<Alarm> alarms = dc.GetActiveAlarms();

            TagDataGrid.ItemsSource = null;
            TagDataGrid.ItemsSource = tags;

            AlarmDataGrid.ItemsSource = null;
            AlarmDataGrid.ItemsSource = alarms;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTables();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
