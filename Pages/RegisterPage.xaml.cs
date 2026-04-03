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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            string pass = pbPassword.Password;
            string passConfirm = pbPasswordConfirm.Password;
            string email = tbEmail.Text.Trim();
            string fullName = tbFullName.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName))
            {
                tbError.Text = "Все поля обязательны";
                return;
            }
            if (pass != passConfirm)
            {
                tbError.Text = "Пароли не совпадают";
                return;
            }
            if (DBClass.connect.Users.Any(u => u.Username == login))
            {
                tbError.Text = "Логин уже занят";
                return;
            }
            if (DBClass.connect.Users.Any(u => u.Email == email))
            {
                tbError.Text = "Email уже используется";
                return;
            }

            var newUser = new Users
            {
                Username = login,
                PasswordHash = pass,
                Email = email,
                FullName = fullName,
                RoleId = 2,
                CreatedAt = System.DateTime.Now,
                AvatarPath = null
            };
            DBClass.connect.Users.Add(newUser);
            DBClass.connect.SaveChanges();

            var savedUser = DBClass.connect.Users.FirstOrDefault(u => u.Username == login);
            Session.CurrentUser = savedUser; 
            MessageBox.Show("Регистрация успешна! Добро пожаловать.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService?.Navigate(new CatalogPage());
        }
    }
}
