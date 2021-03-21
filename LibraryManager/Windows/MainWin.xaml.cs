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
using static LibraryManager.AppData;

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        List<DataBase.Book> BookList = new List<DataBase.Book>();

        private int _role = 0;

        public MainWin()
        {
            InitializeComponent();
        }

        public MainWin(DataBase.Librarian User)
        {
            InitializeComponent();
            BookList = new List<DataBase.Book>(context.Book);
            ListBook.ItemsSource = BookList;
            _role = User.IdRole;
            try
            {
                if (User.IdRole == 1)
                {
                    txtNameEmpl.Text = $"{User.LastName.ToString()} {User.FirstName.ToString()}";
                    //Показать кнопки --<Допилить>--
                }
                if (User.IdRole == 2)
                {
                    txtNameEmpl.Text = $"{User.LastName.ToString()} {User.FirstName.ToString()}";
                    btnBack.Visibility = Visibility.Hidden;
                    //Скрыть кнопки --<Допилить>--
                }
            }
            catch //убрать к хренам из-за ненадобности
            {
                MessageBox.Show("Ваша роль не соответствует возможной! \n " +
                    "Пожалуйста, обратитесь к Администратору!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
