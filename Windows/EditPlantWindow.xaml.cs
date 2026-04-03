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
    /// Логика взаимодействия для EditPlantWindow.xaml
    /// </summary>
    public partial class EditPlantWindow : Window
    {
        private DeceasedPlants plant;
        private bool isNew;
        public EditPlantWindow(DeceasedPlants existingPlant = null)
        {
            InitializeComponent();
            if (existingPlant == null)
            {
                isNew = true;
                plant = new DeceasedPlants();
            }
            else
            {
                isNew = false;
                plant = existingPlant;
                tbName.Text = plant.Name;
                tbSpecies.Text = plant.Species;
                tbCause.Text = plant.CauseOfDeath;
                dpDeathDate.SelectedDate = plant.DeathDate;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                plant.Name = tbName.Text;
                plant.Species = tbSpecies.Text;
                plant.CauseOfDeath = tbCause.Text;
                plant.DeathDate = dpDeathDate.SelectedDate ?? DateTime.Now;
                plant.OwnerId = 1;
                plant.CreatedAt = DateTime.Now;

                DBClass.connect.DeceasedPlants.Add(plant);
            }
            else
            {
                var plantInDb = DBClass.connect.DeceasedPlants.Find(plant.PlantId);
                if (plantInDb != null)
                {
                    plantInDb.Name = tbName.Text;
                    plantInDb.Species = tbSpecies.Text;
                    plantInDb.CauseOfDeath = tbCause.Text;
                    plantInDb.DeathDate = dpDeathDate.SelectedDate ?? DateTime.Now;
                }
            }

            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
