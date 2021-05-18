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
    /// Логика взаимодействия для LibrarianWin.xaml
    /// </summary>
    public partial class LibrarianWin : Window
    {
        Librarian currentUser { get; set; }
        List<Session> SessionDel { get; set; }

        public LibrarianWin(Librarian User)
        {
            InitializeComponent();
            currentUser = User;
            txtNameLib.Text = $"{User.LastName} {User.FirstName}";
            lvLibrarian.ItemsSource = AppData.context.Librarian.ToList();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvLibrarian.SelectedItem is Librarian librarian)
            {
                var result = MessageBox.Show("Удалить выбранного пользователя?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (currentUser != librarian)
                    {
                        SessionDel = AppData.context.Session.Where(i => i.IdLibrarian == librarian.IdLibrarian).ToList();

                        if (AppData.context.Session.Where(i => i.IdLibrarian == librarian.IdLibrarian).FirstOrDefault() != null)
                        {
                            var result2 = MessageBox.Show("Вместе с этим пользователем будут удалены все операции, которые с ним связаны! " +
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

                        AppData.context.Librarian.Remove(librarian);
                        AppData.context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Вы не можете удалить текущего пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            lvLibrarian.ItemsSource = AppData.context.Librarian.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)// добавление библиотекаря
        {
            AddEditLibrarian addEditWin = new AddEditLibrarian();
            this.Opacity = 0.5;
            addEditWin.ShowDialog();
            lvLibrarian.ItemsSource = AppData.context.Librarian.ToList();
            this.Opacity = 1;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (lvLibrarian.SelectedItem is Librarian selectuser)// проверка приведения к типу данных
            {
                AddEditLibrarian addEditWin = new AddEditLibrarian(selectuser);
                this.Opacity = 0.5;
                addEditWin.ShowDialog();
                lvLibrarian.ItemsSource = AppData.context.Librarian.ToList();
                this.Opacity = 1;
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);// если не привёлся, значит запись не выбрана
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)// поиск по значению
        {
            lvLibrarian.ItemsSource = AppData.context.Librarian.Where(i => i.FirstName.Contains(txtSearch.Text)
            || i.LastName.Contains(txtSearch.Text)
            || i.MeddleName.Contains(txtSearch.Text)
            || i.Login.Contains(txtSearch.Text)
            || i.Password.Contains(txtSearch.Text)).ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
