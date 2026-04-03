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
    /// Логика взаимодействия для ShiftAssignmentWindow.xaml
    /// </summary>
    public partial class ShiftAssignmentWindow : Window
    {
        public ShiftAssignmentWindow()
        {
            InitializeComponent();
            cmbGardener.ItemsSource = DBClass.connect.Users.Where(u => u.RoleId == 3).ToList();
            dpShiftDate.SelectedDate = DateTime.Today;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var shift = new Shifts
            {
                GardenerId = (int)cmbGardener.SelectedValue,
                ShiftDate = dpShiftDate.SelectedDate ?? DateTime.Today,
                StartTime = TimeSpan.Parse(tbStartTime.Text),
                EndTime = TimeSpan.Parse(tbEndTime.Text),
                Notes = tbNotes.Text
            };
            DBClass.connect.Shifts.Add(shift);
            DBClass.connect.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
