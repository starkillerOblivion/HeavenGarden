using HeavenGarden.DB;
using HeavenGarden.Pages;
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

namespace HeavenGarden
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (Session.IsAuthenticated)
                MainFrame.Navigate(new CatalogPage());
            else
                MainFrame.Navigate(new LoginPage());
        }

        private void btnCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CatalogPage());
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage());
        }

        private void btnMemorial_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MemorialWallPage());
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (Session.IsAdmin)
                MainFrame.Navigate(new AdminPage());
            else
                MessageBox.Show("Доступ только для администратора");
        }

        private void btnGardener_Click(object sender, RoutedEventArgs e)
        {
            if (Session.IsGardener||Session.IsAdmin)
                MainFrame.Navigate(new GardenerPage());
            else
                MessageBox.Show("Доступ только для садовника-некрологиста");
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Session.Logout();
            MainFrame.Navigate(new LoginPage());
        }
    }
}
