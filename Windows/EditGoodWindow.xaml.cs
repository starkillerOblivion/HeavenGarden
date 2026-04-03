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
    /// Логика взаимодействия для EditGoodWindow.xaml
    /// </summary>
    public partial class EditGoodWindow : Window
    {
        private RitualGoods good;
        private bool isNew;
        public EditGoodWindow(RitualGoods existing = null)
        {
            InitializeComponent();
            if (existing == null)
            {
                isNew = true;
                good = new RitualGoods();
            }
            else
            {
                isNew = false;
                good = existing;
                tbName.Text = good.Name;
                tbDescription.Text = good.Description;
                tbPrice.Text = good.Price.ToString();
                tbStock.Text = good.StockQuantity.ToString();
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            good.Name = tbName.Text;
            good.Description = tbDescription.Text;
            if (decimal.TryParse(tbPrice.Text, out decimal price))
                good.Price = price;
            if (int.TryParse(tbStock.Text, out int stock))
                good.StockQuantity = stock;
            if (isNew)
                DBClass.connect.RitualGoods.Add(good);
            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
