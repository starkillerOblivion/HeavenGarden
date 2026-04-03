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
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        private List<DeceasedPlants> allPlants;

        public CatalogPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            allPlants = DBClass.connect.DeceasedPlants.ToList();
            lvPlants.ItemsSource = allPlants;

            var causes = allPlants.Select(p => p.CauseOfDeath).Distinct().ToList();
            causes.Insert(0, "Все");
            cmbFilterCause.ItemsSource = causes;
            cmbFilterCause.SelectedIndex = 0;
        }

        private void cmbFilterCause_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = cmbFilterCause.SelectedItem as string;
            if (selected == null || selected == "Все")
                lvPlants.ItemsSource = allPlants;
            else
                lvPlants.ItemsSource = allPlants.Where(p => p.CauseOfDeath == selected).ToList();
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            int plantId = (int)((Button)sender).Tag;
            NavigationService?.Navigate(new PlantDetailsPage(plantId));
        }

        private void BtnEditPlant_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser?.RoleId != 1)
            {
                MessageBox.Show("Доступно только администратору");
                return;
            }
            int plantId = (int)((Button)sender).Tag;
            var plant = DBClass.connect.DeceasedPlants.Find(plantId);
            var editWindow = new Windows.EditPlantWindow(plant);
            editWindow.ShowDialog();
            LoadData();
        }

        private void BtnDeletePlant_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser?.RoleId != 1)
            {
                MessageBox.Show("Доступно только администратору");
                return;
            }
            int plantId = (int)((Button)sender).Tag;
            var plant = DBClass.connect.DeceasedPlants.Find(plantId);
            if (MessageBox.Show($"Удалить растение '{plant.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DBClass.connect.DeceasedPlants.Remove(plant);
                DBClass.connect.SaveChanges();
                LoadData();
            }
        }

        private void BtnManageStatus_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser?.RoleId != 3)
            {
                MessageBox.Show("Доступно только садовнику-некрологисту");
                return;
            }
            int plantId = (int)((Button)sender).Tag;
            var plant = DBClass.connect.DeceasedPlants.Find(plantId);
            new Windows.ManageStatusWindow(plant).ShowDialog();
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser?.RoleId != 3)
            {
                MessageBox.Show("Доступно только садовнику-некрологисту");
                return;
            }
            int plantId = (int)((Button)sender).Tag;
            var plant = DBClass.connect.DeceasedPlants.Find(plantId);
            new Windows.StatisticsWindow(plant).ShowDialog();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = tbSearch.Text.Trim();
            if (search.Length > 0 && search != "Поиск по имени...")
            {
                var filtered = allPlants.Where(p => p.Name.ToLower().Contains(search)).ToList();
                lvPlants.ItemsSource = filtered;
            }
            else
            {
                lvPlants.ItemsSource = allPlants;
            }
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text == "Поиск по имени...")
                tbSearch.Text = "";
        }

        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
                tbSearch.Text = "Поиск по имени...";
        }
    }
}
