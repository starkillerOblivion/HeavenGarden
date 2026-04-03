using HeavenGarden.DB;
using HeavenGarden.Windows;
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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            if (!Session.IsAdmin)
            {
                MessageBox.Show("Нет прав доступа");
                NavigationService?.GoBack();
                return;
            }
            RefreshUsers();
            RefreshPlants();
            RefreshGoods();
            RefreshServices();
            RefreshLogs();
        }
        private void RefreshUsers() => lvUsers.ItemsSource = DBClass.connect.Users.ToList();
        private void RefreshPlants() => lvPlants.ItemsSource = DBClass.connect.DeceasedPlants.ToList();
        private void RefreshGoods() => lvGoods.ItemsSource = DBClass.connect.RitualGoods.ToList();
        private void RefreshServices() => lvServices.ItemsSource = DBClass.connect.Services.ToList();
        private void RefreshLogs() => lvLogs.ItemsSource = DBClass.connect.Logs.OrderByDescending(l => l.Timestamp).ToList();
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            new EditUserWindow().ShowDialog();
            RefreshUsers();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var user = DBClass.connect.Users.Find(id);
            new EditUserWindow(user).ShowDialog();
            RefreshUsers();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var user = DBClass.connect.Users.Find(id);
            if (MessageBox.Show($"Удалить {user.Username}? Все связанные данные будут удалены.", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var orders = DBClass.connect.Orders.Where(o => o.UserId == id).ToList();
                foreach (var order in orders)
                {
                    var checks = DBClass.connect.QualityChecks.Where(q => q.OrderId == order.OrderId).ToList();
                    foreach (var check in checks)
                        DBClass.connect.QualityChecks.Remove(check);
                }
                foreach (var order in orders)
                {
                    var items = DBClass.connect.OrderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                    foreach (var item in items)
                        DBClass.connect.OrderItems.Remove(item);
                    var servOrders = DBClass.connect.ServiceOrders.Where(so => so.OrderId == order.OrderId).ToList();
                    foreach (var so in servOrders)
                        DBClass.connect.ServiceOrders.Remove(so);
                }
                foreach (var order in orders)
                    DBClass.connect.Orders.Remove(order);

                var logs = DBClass.connect.Logs.Where(l => l.UserId == id).ToList();
                foreach (var log in logs)
                    DBClass.connect.Logs.Remove(log);

                var shifts = DBClass.connect.Shifts.Where(s => s.GardenerId == id).ToList();
                foreach (var shift in shifts)
                    DBClass.connect.Shifts.Remove(shift);

                var bf = DBClass.connect.BonusesFines.Where(b => b.GardenerId == id).ToList();
                foreach (var b in bf)
                    DBClass.connect.BonusesFines.Remove(b);

                var plants = DBClass.connect.DeceasedPlants.Where(p => p.OwnerId == id).ToList();
                foreach (var plant in plants)
                    DBClass.connect.DeceasedPlants.Remove(plant);

                DBClass.connect.Users.Remove(user);
                DBClass.connect.SaveChanges();
                RefreshUsers();
            }
        }

        private void btnAddPlant_Click(object sender, RoutedEventArgs e)
        {
            new EditPlantWindow().ShowDialog(); 
            RefreshPlants();
        }

        private void btnEditPlant_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var plant = DBClass.connect.DeceasedPlants.Find(id);
            new EditPlantWindow(plant).ShowDialog();
            RefreshPlants();
        }

        private void btnDeletePlant_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var plant = DBClass.connect.DeceasedPlants.Find(id);
            if (MessageBox.Show($"Удалить {plant.Name}?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DBClass.connect.DeceasedPlants.Remove(plant);
                DBClass.connect.SaveChanges();
                RefreshPlants();
            }
        }

        private void btnAddGood_Click(object sender, RoutedEventArgs e)
        {
            new EditGoodWindow().ShowDialog();
            RefreshGoods();
        }

        private void btnEditGood_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var good = DBClass.connect.RitualGoods.Find(id);
            new EditGoodWindow(good).ShowDialog();
            RefreshGoods();
        }

        private void btnDeleteGood_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var good = DBClass.connect.RitualGoods.Find(id);
            if (MessageBox.Show($"Удалить товар '{good.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var items = DBClass.connect.OrderItems.Where(oi => oi.GoodId == id).ToList();
                foreach (var item in items)
                    DBClass.connect.OrderItems.Remove(item);

                DBClass.connect.RitualGoods.Remove(good);
                DBClass.connect.SaveChanges();
                RefreshGoods();
            }
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            new EditServiceWindow().ShowDialog(); 
            RefreshServices();
        }

        private void btnEditService_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var service = DBClass.connect.Services.Find(id);
            new EditServiceWindow(service).ShowDialog();
            RefreshServices();
        }

        private void btnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var service = DBClass.connect.Services.Find(id);
            if (MessageBox.Show($"Удалить услугу '{service.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var serviceOrders = DBClass.connect.ServiceOrders.Where(so => so.ServiceId == id).ToList();
                foreach (var so in serviceOrders)
                    DBClass.connect.ServiceOrders.Remove(so);

                DBClass.connect.Services.Remove(service);
                DBClass.connect.SaveChanges();
                RefreshServices();
            }
        }
    }
}
