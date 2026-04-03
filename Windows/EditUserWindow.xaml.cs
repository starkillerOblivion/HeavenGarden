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
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private Users user;
        private bool isNew;
        public EditUserWindow(Users existing = null)
        {
            InitializeComponent();
            cmbRole.ItemsSource = DBClass.connect.Roles.ToList();
            if (existing == null)
            {
                isNew = true;
                user = new Users();
            }
            else
            {
                isNew = false;
                user = existing;
                tbUsername.Text = user.Username;
                tbEmail.Text = user.Email;
                tbFullName.Text = user.FullName;
                cmbRole.SelectedValue = user.RoleId;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            user.Username = tbUsername.Text;
            user.Email = tbEmail.Text;
            user.FullName = tbFullName.Text;
            user.RoleId = (int)cmbRole.SelectedValue;
            if (!string.IsNullOrEmpty(pbPassword.Password))
                user.PasswordHash = pbPassword.Password;
            if (isNew)
                DBClass.connect.Users.Add(user);
            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
