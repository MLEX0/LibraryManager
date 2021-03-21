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

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWin.xaml
    /// </summary>
    public partial class AdminWin : Window
    {
        private DataBase.Librarian _userData;
        public AdminWin()
        {
            InitializeComponent();
        }

        public AdminWin(DataBase.Librarian User)
        {
            InitializeComponent();
            _userData = User;
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            MainWin mainWin = new MainWin(_userData);
            this.Hide();
            mainWin.ShowDialog();
            this.Show();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            ReaderWin readerWin = new ReaderWin(_userData);
            this.Hide();
            readerWin.ShowDialog();
            this.Show();
        }

        private void btnLibrarian_Click(object sender, RoutedEventArgs e)
        {
            LibrarianWin librarianWin = new LibrarianWin(_userData);
            this.Hide();
            librarianWin.ShowDialog();
            this.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
