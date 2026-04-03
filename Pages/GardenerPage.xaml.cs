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
    /// Логика взаимодействия для GardenerPage.xaml
    /// </summary>
    public partial class GardenerPage : Page
    {
        public GardenerPage()
        {
            InitializeComponent();
            if (!Session.IsGardener)
            {
                MessageBox.Show("Нет прав доступа");
                NavigationService?.GoBack();
                return;
            }
            cmbOrderStatus.ItemsSource = new[] { "Все", "Pending", "Processing", "Completed" };
            cmbOrderStatus.SelectedIndex = 0;
            RefreshOrders();
            RefreshShifts();
            RefreshBonusesFines();
        }
        private void RefreshOrders()
        {
            var all = DBClass.connect.Orders.ToList();
            string filter = cmbOrderStatus.SelectedItem as string;
            if (filter == null || filter == "Все")
                lvOrders.ItemsSource = all;
            else
                lvOrders.ItemsSource = all.Where(o => o.Status == filter).ToList();
        }
        private void RefreshShifts() => lvShifts.ItemsSource = DBClass.connect.Shifts.ToList();
        private void RefreshBonusesFines() => lvBonusesFines.ItemsSource = DBClass.connect.BonusesFines.ToList();

        private void cmbOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e) => RefreshOrders();

        private void BtnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var order = DBClass.connect.Orders.Find(id);
            var newStatus = "";
            if (order.Status == "Pending") newStatus = "Processing";
            else if (order.Status == "Processing") newStatus = "Completed";
            else return;
            order.Status = newStatus;
            DBClass.connect.SaveChanges();
            RefreshOrders();
        }

        private void BtnQualityCheck_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var order = DBClass.connect.Orders.Find(id);
            new QualityCheckWindow(order).ShowDialog();
            RefreshOrders();
        }

        private void BtnAddShift_Click(object sender, RoutedEventArgs e)
        {
            new ShiftAssignmentWindow().ShowDialog();
            RefreshShifts();
        }

        private void BtnAddBonusFine_Click(object sender, RoutedEventArgs e)
        {
            new BonusFineWindow().ShowDialog();
            RefreshBonusesFines();
        }
    }
}
