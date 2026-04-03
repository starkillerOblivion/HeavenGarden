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
    /// Логика взаимодействия для EditServiceWindow.xaml
    /// </summary>
    public partial class EditServiceWindow : Window
    {
        private Services service;
        private bool isNew;
        public EditServiceWindow(Services existing = null)
        {
            InitializeComponent();
            if (existing == null)
            {
                isNew = true;
                service = new Services();
            }
            else
            {
                isNew = false;
                service = existing;
                tbName.Text = service.Name;
                tbDescription.Text = service.Description;
                tbPrice.Text = service.Price.ToString();
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            service.Name = tbName.Text;
            service.Description = tbDescription.Text;
            if (decimal.TryParse(tbPrice.Text, out decimal price))
                service.Price = price;
            if (isNew)
                DBClass.connect.Services.Add(service);
            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
