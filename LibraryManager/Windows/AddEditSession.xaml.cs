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

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditSession.xaml
    /// </summary>
    public partial class AddEditSession : Window
    {
        Librarian CurrentUser { get; set; }
        Reader SelectReader { get; set; }
        Session AddSession { get; set; }
        
        Book EditBook { get; set; }

        public AddEditSession(Librarian librarian, Reader reader)
        {
            InitializeComponent();

            txtHeader.Text = "Выдача книг";
            txtLibrarian.Text = $"{librarian.FirstName} {librarian.LastName} {librarian.MeddleName}";
            txtReader.Text = $"{reader.FirstName} {reader.LastName} {reader.MeddleName}";
            lvAddBook.ItemsSource = HelpClass.HelpData.AddedInSessionBooks.ToList();

            CurrentUser = librarian;
            SelectReader = reader;
            AddSession = new Session();
        }

        private void btnAddBookInList_Click(object sender, RoutedEventArgs e)// добавление книги в лист
        {
            MainWin mainWin = new MainWin(CurrentUser, SelectReader);
            this.Opacity = 0.5;
            mainWin.ShowDialog();
            this.Opacity = 1;

            lvAddBook.ItemsSource = HelpClass.HelpData.AddedInSessionBooks.ToList();
        }

        private void btnDeleteFromList_Click(object sender, RoutedEventArgs e)// удаление книги из листа
        {
            if (lvAddBook.SelectedItem is Book book)
            {
                var result = MessageBox.Show("Отменить выбор?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    HelpClass.HelpData.AddedInSessionBooks.Remove(book);
                    lvAddBook.ItemsSource = HelpClass.HelpData.AddedInSessionBooks.ToList();
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            HelpClass.HelpData.AddedInSessionBooks.Clear();
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (HelpClass.HelpData.AddedInSessionBooks.Count == 0)
            {
                MessageBox.Show("Книги не выбраны", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                for (int i = 0; i < HelpClass.HelpData.AddedInSessionBooks.Count; i++)// изменение количества всех книг
                {
                    EditBook = HelpClass.HelpData.AddedInSessionBooks[i];

                    EditBook.CountBooksInLibrary--;
                    EditBook.CountBooksInUse++;

                    AppData.context.SaveChanges();
                }

                for (int i = 0; i < HelpClass.HelpData.AddedInSessionBooks.Count; i++)// добавление сессии 
                {

                    AddSession.IdBook = HelpClass.HelpData.AddedInSessionBooks[i].IdBook;
                    AddSession.IdLibrarian = CurrentUser.IdLibrarian;
                    AddSession.IdReader = SelectReader.IdReader;
                    AddSession.DateOfBegin = System.DateTime.Now;
                    AddSession.DateOfEnd = null;

                    AppData.context.Session.Add(AddSession);
                    AppData.context.SaveChanges();
                    AddSession = new Session();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            HelpClass.HelpData.AddedInSessionBooks.Clear();
            this.Close();
        }
    }
}
