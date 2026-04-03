using HeavenGarden.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private Users currentUser;

        public ProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (Session.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован");
                NavigationService?.Navigate(new LoginPage());
                return;
            }

            currentUser = DBClass.connect.Users.Find(Session.CurrentUser.UserId);
            if (currentUser == null)
            {
                MessageBox.Show("Пользователь не найден");
                NavigationService?.Navigate(new LoginPage());
                return;
            }

            Session.CurrentUser = currentUser;

            tbUsername.Text = currentUser.Username;
            tbEmail.Text = currentUser.Email;
            tbFullName.Text = currentUser.FullName;

            if (!string.IsNullOrEmpty(currentUser.AvatarPath) && File.Exists(currentUser.AvatarPath))
            {
                imgAvatar.Source = new BitmapImage(new Uri(currentUser.AvatarPath));
            }
            else
            {
                imgAvatar.Source = new BitmapImage(new Uri("pack://application:,,,/Images/iconProfile.png"));
            }

            var orders = DBClass.connect.Orders
                .Where(o => o.UserId == currentUser.UserId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            lvOrders.ItemsSource = orders;
        }

        private void btnSelectAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null)
            {
                MessageBox.Show("Пользователь не загружен");
                return;
            }

            var dialog = new OpenFileDialog
            {
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Выберите аватар"
            };
            if (dialog.ShowDialog() == true)
            {
                string avatarsDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "avatars");
                if (!Directory.Exists(avatarsDir))
                    Directory.CreateDirectory(avatarsDir);

                string fileName = $"{currentUser.UserId}_{Guid.NewGuid()}{System.IO.Path.GetExtension(dialog.FileName)}";
                string destPath = System.IO.Path.Combine(avatarsDir, fileName);
                File.Copy(dialog.FileName, destPath, true);

                if (!string.IsNullOrEmpty(currentUser.AvatarPath) && File.Exists(currentUser.AvatarPath))
                    File.Delete(currentUser.AvatarPath);

                currentUser.AvatarPath = destPath;
                DBClass.connect.SaveChanges();
                imgAvatar.Source = new BitmapImage(new Uri(destPath));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null) return;

            currentUser.Email = tbEmail.Text.Trim();
            currentUser.FullName = tbFullName.Text.Trim();

            if (!string.IsNullOrEmpty(pbPassword.Password))
                currentUser.PasswordHash = pbPassword.Password;

            DBClass.connect.SaveChanges();
            MessageBox.Show("Профиль успешно обновлён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
