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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeavenGarden.Pages
{
    /// <summary>
    /// Логика взаимодействия для PlantDetailsPage.xaml
    /// </summary>
    public partial class PlantDetailsPage : Page
    {
        private int plantId;
        public PlantDetailsPage(int id)
        {
            InitializeComponent();
            plantId = id;
            LoadData();
        }

        private void LoadData()
        {
            var plant = DBClass.connect.DeceasedPlants.FirstOrDefault(p => p.PlantId == plantId);
            if (plant == null)
            {
                tbName.Text = "Растение не найдено";
                return;
            }

            tbName.Text = plant.Name;
            tbSpecies.Text = $"Вид: {plant.Species}";
            tbCause.Text = $"Причина смерти: {plant.CauseOfDeath}";
            tbDeathDate.Text = $"Дата смерти: {plant.DeathDate:dd.MM.yyyy}";

            var owner = DBClass.connect.Users.FirstOrDefault(u => u.UserId == plant.OwnerId);
            tbOwner.Text = owner != null ? $"Владелец: {owner.FullName}" : "Владелец не указан";

            var logs = DBClass.connect.Logs
                .Where(l => l.TableName == "DeceasedPlants" && l.RecordId == plantId)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
            lvLogs.ItemsSource = logs;
        }
    }
}
