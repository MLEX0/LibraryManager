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
using LibraryManager.DataBase;
using LibraryManager.HelpClass;
using Microsoft.Win32;

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditBookWin.xaml
    /// </summary>
    public partial class AddEditBookWin : Window
    {
        string pathImage = null;
        private Book AddBook { get; set; }
        private Book EditBook { get; set; }

        private bool isEdit;

        public AddEditBookWin() // Сценарий добавления
        {
            InitializeComponent();

            cmbPublisher.ItemsSource = AppData.context.Publisher.ToList(); // Заполнение по базе данных
            cmbPublisher.DisplayMemberPath = "PublisherName";
            cmbPublisher.SelectedIndex = 0;

            cmbAuthor.ItemsSource = AppData.context.BookAuthor.ToList(); // Заполнение по базе данных
            cmbAuthor.DisplayMemberPath = "AuthorFIO";
            cmbAuthor.SelectedIndex = 0;

            cmbAgeRaiting.ItemsSource = AppData.context.AgeRating.ToList(); // Заполнение по базе данных
            cmbAgeRaiting.DisplayMemberPath = "NameRaiting";
            cmbAgeRaiting.SelectedIndex = 0;

            txtHeader.Text = "Добавление книги";
            isEdit = false;
            AddBook = new Book();
        }

        public AddEditBookWin(Book book)// Сценарий изменения
        {
            InitializeComponent();

            cmbPublisher.ItemsSource = AppData.context.Publisher.ToList(); // Заполнение по базе данных
            cmbPublisher.DisplayMemberPath = "PublisherName";
            cmbPublisher.SelectedIndex = 0;

            cmbAuthor.ItemsSource = AppData.context.BookAuthor.ToList(); // Заполнение по базе данных
            cmbAuthor.DisplayMemberPath = "AuthorFIO";
            cmbAuthor.SelectedIndex = 0;

            cmbAgeRaiting.ItemsSource = AppData.context.AgeRating.ToList(); // Заполнение по базе данных
            cmbAgeRaiting.DisplayMemberPath = "NameRaiting";
            cmbAgeRaiting.SelectedIndex = 0;

            txtHeader.Text = "Изменение книги";
            isEdit = true;
            AddBook = new Book();
            EditBook = book;

            if (book.BookImage != null)
            {
                using (MemoryStream stream = new MemoryStream(book.BookImage))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    imgBook.Source = bitmapImage;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEdit == true)
            {
                txtBookName.Text = EditBook.BookName;
                txtBookDescription.Text = EditBook.BookDescription;
                txtAudithoryText.Text = EditBook.Audithory;
                txtBBK.Text = EditBook.BBK;
                txtCountInLibrary.Text = Convert.ToString(EditBook.CountBooksInLibrary);
                txtISBN.Text = Convert.ToString(EditBook.ISBN);
                txtUDK.Text = EditBook.UDK;
                txtPlace.Text = EditBook.PlaceNumber;
                cmbPublisher.SelectedIndex = Convert.ToInt32(EditBook.IdPublisher) - 1;
                cmbAuthor.SelectedIndex = Convert.ToInt32(EditBook.IdBookAuthor) - 1;
                dpkDateOfIssue.SelectedDate = EditBook.DateOfIssue;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                imgBook.Source = new BitmapImage(new Uri(openFile.FileName));
                pathImage = openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBookName.Text))
            {
                MessageBox.Show("Поле название не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBookDescription.Text))
            {
                MessageBox.Show("Поле описание не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("Поле ISBN не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUDK.Text))
            {
                MessageBox.Show("Поле UDK не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBBK.Text))
            {
                MessageBox.Show("Поле BBK не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPlace.Text))
            {
                MessageBox.Show("Поле место на полке не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dpkDateOfIssue.SelectedDate == null)
            {
                MessageBox.Show("Поле дата издания не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCountInLibrary.Text))
            {
                MessageBox.Show("Поле количество книг не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAudithoryText.Text))
            {
                MessageBox.Show("Поле аудитория не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (isEdit == false)
                {
                    // если нет фото передаём null
                    if (pathImage != null)
                    {
                        AddBook.BookImage = File.ReadAllBytes(pathImage);
                    }
                    else
                    {
                        AddBook.BookImage = null;
                    }

                    // если не выбрали фото, выводим вопрос с подтверждением
                    if (pathImage == null)
                    {
                        var resMess = MessageBox.Show("Фото не выбрано. Сохранить книгу без фото?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (resMess == MessageBoxResult.No)
                        {
                            return;
                        }
                    }



                    AddBook.BookName = txtBookName.Text;
                    AddBook.BookDescription = txtBookDescription.Text;
                    AddBook.ISBN = txtISBN.Text;
                    AddBook.UDK = txtUDK.Text;
                    AddBook.BBK = txtBBK.Text;
                    AddBook.PlaceNumber = txtPlace.Text;
                    AddBook.DateOfIssue = dpkDateOfIssue.SelectedDate.Value;
                    AddBook.DateOfReceipt = System.DateTime.Now;
                    AddBook.CountBooksInLibrary = Convert.ToInt32(txtCountInLibrary.Text);
                    AddBook.Audithory = txtAudithoryText.Text;


                    AddBook.IdPublisher = AppData.context.Publisher
                    .Where(i => i.PublisherName == cmbPublisher.Text)
                    .Select(i => i.IdPublisher).FirstOrDefault();

                    AddBook.IdBookAuthor = AppData.context.BookAuthor.Where(i => i.IdBookAuthor == cmbAuthor.SelectedIndex + 1).Select(i => i.IdBookAuthor).FirstOrDefault();

                    AddBook.IdBibliographicDecsription = 1;

                    AddBook.IdAgeReating = AppData.context.AgeRating
                    .Where(i => i.NameRaiting == cmbAgeRaiting.Text)
                    .Select(i => i.IdAgeRating).FirstOrDefault();

                    AppData.context.Book.Add(AddBook); // добавление книги
                    AppData.context.SaveChanges();
                    MessageBox.Show($"Книга {txtBookName.Text} добавлена");
                }

                if (isEdit == true)
                {
                    // изм. товара

                    // если нет фото передаём null
                    if (pathImage != null)
                    {
                        EditBook.BookImage = File.ReadAllBytes(pathImage); // переделать!!!!!!!!!!!!
                    }

                    // если не выбрали фото, выводим вопрос с подтверждением


                    EditBook.BookName = txtBookName.Text;
                    EditBook.BookDescription = txtBookDescription.Text;
                    EditBook.ISBN = txtISBN.Text;
                    EditBook.UDK = txtUDK.Text;
                    EditBook.BBK = txtBBK.Text;
                    EditBook.PlaceNumber = txtPlace.Text;
                    EditBook.CountBooksInLibrary = Convert.ToInt32(txtCountInLibrary.Text);
                    EditBook.Audithory = txtAudithoryText.Text;


                    EditBook.IdPublisher = AppData.context.Publisher
                    .Where(i => i.PublisherName == cmbPublisher.Text)
                    .Select(i => i.IdPublisher).FirstOrDefault();

                    EditBook.IdAgeReating = AppData.context.AgeRating
                    .Where(i => i.NameRaiting == cmbAgeRaiting.Text)
                    .Select(i => i.IdAgeRating).FirstOrDefault();

                    EditBook.IdAgeReating = AppData.context.AgeRating
                    .Where(i => i.NameRaiting == cmbAgeRaiting.Text)
                    .Select(i => i.IdAgeRating).FirstOrDefault();

                    AppData.context.SaveChanges();

                    MessageBox.Show("Данные успешно сохранены");

                }
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка добавления в базу данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void btnAddPublisher_Click(object sender, RoutedEventArgs e)
        {
            AddPublisherWin addPublisherWin = new AddPublisherWin();
            this.Opacity = 0.5;
            addPublisherWin.ShowDialog();
            this.Opacity = 1;


            if (HelpData.NowAddedPublisher != null)
            {
                cmbPublisher.ItemsSource = AppData.context.Publisher.ToList(); // Заполнение по базе данных
                cmbPublisher.DisplayMemberPath = "PublisherName";
                int nowAddedPublisherId = HelpData.NowAddedPublisher.IdPublisher;
                cmbPublisher.SelectedIndex = AppData.context.Publisher
                    .Where(i => i.IdPublisher == nowAddedPublisherId)
                    .Select(i => i.IdPublisher).FirstOrDefault() - 1;

                HelpData.NowAddedPublisher = null;
            }
        }

        private void btnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            AddAuthorWin addAuthorWin = new AddAuthorWin();
            this.Opacity = 0.5;
            addAuthorWin.ShowDialog();
            this.Opacity = 1;

            if (HelpData.NowAddedAuthor != null)
            {
                cmbAuthor.ItemsSource = AppData.context.BookAuthor.ToList(); // Заполнение по базе данных
                cmbAuthor.DisplayMemberPath = "AuthorFIO";
                int nowAddedAuthorId = HelpData.NowAddedAuthor.IdBookAuthor;
                cmbAuthor.SelectedIndex = AppData.context.BookAuthor
                    .Where(i => i.IdBookAuthor == nowAddedAuthorId)
                    .Select(i => i.IdBookAuthor).FirstOrDefault() - 1;

                HelpData.NowAddedAuthor = null;
            }
        }

        private void TextBlock_GotFocus(object sender, RoutedEventArgs e)// Доделать
        {

        }

        private void TextBlock_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
