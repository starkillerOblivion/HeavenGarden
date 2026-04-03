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
    /// Логика взаимодействия для ManageStatusWindow.xaml
    /// </summary>
    public partial class ManageStatusWindow : Window
    {
        private DeceasedPlants plant;
        public ManageStatusWindow(DeceasedPlants plant)
        {
            InitializeComponent();
            this.plant = plant;
            tbPlantName.Text = plant.Name;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var log = new Logs
            {
                UserId = Session.CurrentUser.UserId,
                Action = "CHANGE_STATUS",
                TableName = "DeceasedPlants",
                RecordId = plant.PlantId,
                Timestamp = DateTime.Now,
                Details = $"Статус изменён на: {tbStatus.Text}"
            };
            DBClass.connect.Logs.Add(log);
            DBClass.connect.SaveChanges();
            MessageBox.Show("Статус обновлён (добавлена запись в лог)");
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
