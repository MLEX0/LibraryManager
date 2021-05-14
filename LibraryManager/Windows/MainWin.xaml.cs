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
using System.Windows.Shapes;
using static LibraryManager.AppData;
using LibraryManager.HelpClass;
using LibraryManager.DataBase;

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        //List<DataBase.Book> BookList = new List<DataBase.Book>();

        List<Session> SessionDel { get; set; }

        private int role = 0;

        public MainWin(Librarian User, Reader reader)// добавление сессии
        {
            InitializeComponent();

            txtNameLib.Text = $"{User.LastName} {User.FirstName}";
            btnNewSession.Visibility = Visibility.Visible;
            ListBook.ItemsSource = context.Book.ToList();
        }

        public MainWin(Librarian User)
        {
            InitializeComponent();
            ListBook.ItemsSource = context.Book.ToList();
            role = User.IdRole;
            try
            {
                if (User.IdRole == 1)
                {
                    txtNameLib.Text = $"{User.LastName} {User.FirstName}";
                    
                }
                if (User.IdRole == 2)
                {
                    txtNameLib.Text = $"{User.LastName} {User.FirstName}";
                }
            }
            catch //убрать из-за ненадобности
            {
                MessageBox.Show("Ваша роль не соответствует возможной! \n " +
                    "Пожалуйста, обратитесь к Администратору!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (ListBook.SelectedItem is DataBase.Book selectbook)
            {
                ShowViewBar();
                txtName.Text = selectbook.BookName;
                txtDescription.Text = selectbook.BookDescription;
                if (txtDescription.Text.Length < 30)
                {
                    scrvDescription.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else
                {
                    scrvDescription.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                }

                txtISBN.Text = selectbook.ISBN;
                txtBBK.Text = selectbook.BBK;
                txtUDK.Text = selectbook.UDK;
                txtDateIssue.Text = Convert.ToString(selectbook.DateOfIssue.Year);

                if (selectbook.BookImage != null)
                {
                    using (MemoryStream stream = new MemoryStream(selectbook.BookImage))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        bitmapImage.StreamSource = stream;
                        bitmapImage.EndInit();
                        imageBook.Source = bitmapImage;
                    }
                }
            }
        }

        private void ShowViewBar()
        {
            grdViewBar.Visibility = Visibility.Visible;
            brdNoSelected.Visibility = Visibility.Hidden;
            txtNoSelected.Visibility = Visibility.Hidden;
        }

        private void HideViewBar()
        {
            grdViewBar.Visibility = Visibility.Hidden;
            brdNoSelected.Visibility = Visibility.Visible;
            txtNoSelected.Visibility = Visibility.Visible;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditBookWin addEditBookWin = new AddEditBookWin();
            this.Opacity = 0.5;
            addEditBookWin.ShowDialog();
            ListBook.ItemsSource = AppData.context.Book.ToList();
            this.Opacity = 1;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (ListBook.SelectedItem is Book selectbook)
            {
                AddEditBookWin addEditBookWin = new AddEditBookWin(selectbook);
                this.Opacity = 0.5;
                addEditBookWin.ShowDialog();
                ListBook.ItemsSource = AppData.context.Book.ToList();
                this.Opacity = 1;
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListBook.SelectedItem is Book book)
            {
                var result = MessageBox.Show("Удалить выбранную книгу?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);


                if (result == MessageBoxResult.Yes)
                {
                    SessionDel = AppData.context.Session.Where(i => i.IdBook == book.IdBook).ToList();

                    if (AppData.context.Session.Where(i => i.IdBook == book.IdBook).FirstOrDefault() != null)
                    {
                        var result2 = MessageBox.Show("Вместе с книгой будут удалены все операции, которые с ним связаны! " +
                            "\n Продолжить удаление?", "Продолжить?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result2 == MessageBoxResult.Yes)
                        {
                            for (int i = 0; i < SessionDel.Count(); i++)
                            {
                                AppData.context.Session.Remove(SessionDel[i]);
                            }
                        }
                        else if (result2 == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    AppData.context.Book.Remove(book);
                    AppData.context.SaveChanges();
                }

            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            HideViewBar();
            ListBook.ItemsSource = AppData.context.Book.ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)// поиск книги
        {
            ListBook.ItemsSource = AppData.context.Book
                .Where(i => i.BookName.Contains(txtSearch.Text)
                || i.BookDescription.Contains(txtSearch.Text) 
                || i.BookAuthor.FirstName.Contains(txtSearch.Text) 
                || i.BookAuthor.LastName.Contains(txtSearch.Text)
                || i.Publisher.PublisherName.Contains(txtSearch.Text)
                || i.ISBN.Contains(txtSearch.Text)
                || i.UDK.Contains(txtSearch.Text)
                || i.BBK.Contains(txtSearch.Text)).ToList();
        }

        private void btnNewSession_Click(object sender, RoutedEventArgs e)// выюор книги для сессии
        {
            if (ListBook.SelectedItem is Book selectbook)
            {
                if (selectbook.CountBooksInLibrary < 1)
                {
                    MessageBox.Show("Книги нет в наличии", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    HelpClass.HelpData.AddedInSessionBooks.Add(selectbook);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
