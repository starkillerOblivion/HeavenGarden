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
    /// Логика взаимодействия для BonusFineWindow.xaml
    /// </summary>
    public partial class BonusFineWindow : Window
    {
        public BonusFineWindow()
        {
            InitializeComponent();
            cmbGardener.ItemsSource = DBClass.connect.Users.Where(u => u.RoleId == 3).ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var bf = new BonusesFines
            {
                GardenerId = (int)cmbGardener.SelectedValue,
                Amount = decimal.Parse(tbAmount.Text),
                Reason = tbReason.Text,
                Date = DateTime.Now
            };
            DBClass.connect.BonusesFines.Add(bf);
            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
