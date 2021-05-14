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
    /// Логика взаимодействия для AdminWin.xaml
    /// </summary>
    public partial class AdminWin : Window
    {
        private Librarian userData;

        public AdminWin(Librarian User)
        {
            InitializeComponent();
            userData = User;

            txtWelcome.Text = $"Пользователь: {userData.LastName} {userData.FirstName} {userData.MeddleName}";
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            MainWin mainWin = new MainWin(userData);
            this.Hide();
            mainWin.ShowDialog();
            this.Show();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            ReaderWin readerWin = new ReaderWin(userData);
            this.Hide();
            readerWin.ShowDialog();
            this.Show();
        }

        private void btnLibrarian_Click(object sender, RoutedEventArgs e)
        {
            LibrarianWin librarianWin = new LibrarianWin(userData);
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
