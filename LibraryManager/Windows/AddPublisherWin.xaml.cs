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
using LibraryManager.HelpClass;
using LibraryManager.DataBase;

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddPublisherWin.xaml
    /// </summary>
    public partial class AddPublisherWin : Window
    {
        Publisher AddPublisher { get; set; }

        public AddPublisherWin()
        {
            InitializeComponent();

            AddPublisher = new Publisher();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPublisher.Text) || txtPublisher.Text.Length > 150)
                {
                    MessageBox.Show("Поле название издателя не может быть пустым \nи вмещать в себя более 150 символов!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCityName.Text) || txtCityName.Text.Length > 150)
                {
                    MessageBox.Show("Поле название города не может быть пустым \nи вмещать в себя более 150 символов!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                AddPublisher.PublisherName = txtPublisher.Text;
                AddPublisher.CityName = txtCityName.Text;

                HelpData.NowAddedPublisher = AddPublisher;
                AppData.context.Publisher.Add(AddPublisher); // добавление книги
                AppData.context.SaveChanges();
                MessageBox.Show($"Издатель {AddPublisher.PublisherName} добавлен");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }
        }
    }
}
