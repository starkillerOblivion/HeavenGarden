using HeavenGarden.DB;
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

namespace HeavenGarden.Windows
{
    /// <summary>
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        private DeceasedPlants plant;
        public StatisticsWindow(DeceasedPlants plant)
        {
            InitializeComponent();
            this.plant = plant;
            LoadStats();
        }

        private void LoadStats()
        {
            tbPlantName.Text = plant.Name;
            var logs = DBClass.connect.Logs
                .Where(l => l.TableName == "DeceasedPlants" && l.RecordId == plant.PlantId)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
            tbLogCount.Text = logs.Count.ToString();
            lvStatusLogs.ItemsSource = logs; 
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}
