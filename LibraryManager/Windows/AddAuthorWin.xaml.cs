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
using LibraryManager.DataBase;
using LibraryManager.HelpClass;

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddAuthorWin.xaml
    /// </summary>
    public partial class AddAuthorWin : Window
    {
        private BookAuthor AddAuthor { get; set; }
        

        public AddAuthorWin()
        {
            InitializeComponent();

            AddAuthor = new BookAuthor();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)// сохранение
        {

            try
            {
                if (string.IsNullOrWhiteSpace(txtAuthorFName.Text) || txtAuthorFName.Text.Length > 50)// проверки полей
                {
                    MessageBox.Show("Поле имя не может быть пустым или содержать в себе больше 50 символов", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAuthorLName.Text) || txtAuthorLName.Text.Length > 50)
                {
                    MessageBox.Show("Поле фамилия не может быть пустым или содержать в себе больше 50 символов", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (txtAuthorMName.Text.Length > 50)
                {
                    MessageBox.Show("Поле отчесвто не может содержать в себе больше 50 символов", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (string.IsNullOrWhiteSpace(txtAuthorSign.Text) || txtAuthorSign.Text.Length > 10)
                {
                    MessageBox.Show("Поле авторский знак не может быть пустым или содержать в себе больше 30 символов", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }



                AddAuthor.FirstName = txtAuthorFName.Text;// заполнение свойства добавления
                AddAuthor.LastName = txtAuthorLName.Text;
                AddAuthor.AuthorSighn = txtAuthorSign.Text;

                if (txtAuthorMName.Text == " " || txtAuthorMName.Text == null)// отчество может быть пустым
                {
                    AddAuthor.MeddleName = null;
                }
                else
                {
                    AddAuthor.MeddleName = txtAuthorMName.Text;
                }

                HelpData.NowAddedAuthor = AddAuthor;
                AppData.context.BookAuthor.Add(AddAuthor); // добавление книги
                AppData.context.SaveChanges();
                MessageBox.Show($"Автор {txtAuthorFName.Text} {txtAuthorLName.Text} {txtAuthorMName.Text} добавлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка добавления в базу данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
