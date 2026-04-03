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
    /// Логика взаимодействия для QualityCheckWindow.xaml
    /// </summary>
    public partial class QualityCheckWindow : Window
    {
        private Orders order;

        public QualityCheckWindow(Orders currentOrder)
        {
            InitializeComponent();
            order = currentOrder;
            tbOrderId.Text = order.OrderId.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var quality = new QualityChecks
            {
                OrderId = order.OrderId,
                CheckDate = DateTime.Now,
                Rating = int.Parse(((ComboBoxItem)cmbRating.SelectedItem).Content.ToString()),
                Comments = tbComment.Text
            };
            DBClass.connect.QualityChecks.Add(quality);
            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
