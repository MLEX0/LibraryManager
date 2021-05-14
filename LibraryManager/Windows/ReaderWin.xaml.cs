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
using LibraryManager.DataBase;

namespace LibraryManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReaderWin.xaml
    /// </summary>
    public partial class ReaderWin : Window
    {

        PassportOfReader PassDel { get; set; }
        List<Session> SessionDel { get; set; }
        Librarian CurrentUser { get; set; }

        public ReaderWin(Librarian User)
        {
            InitializeComponent();

            CurrentUser = User;

            txtNameLib.Text = $"{User.LastName.ToString()} {User.FirstName.ToString()}";

            ListReader.ItemsSource = context.Reader.ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ListReader.SelectedItem is DataBase.Reader selectreader)
            {
                ShowViewBar();

                txtLName.Text = selectreader.LastName;
                txtFName.Text = selectreader.FirstName;
                txtMName.Text = selectreader.MeddleName;
                txtGender.Text = selectreader.Gender.GenderName;
                txtBirthday.Text = Convert.ToString(selectreader.Birthday);
                if (Convert.ToInt32(System.DateTime.Now.Month) < Convert.ToInt32(selectreader.Birthday.Month))
                {
                    txtAge.Text = Convert.ToString(Convert.ToInt32(System.DateTime.Now.Year) - Convert.ToInt32(selectreader.Birthday.Year) + 1);
                }
                else if (Convert.ToInt32(System.DateTime.Now.Month) > Convert.ToInt32(selectreader.Birthday.Month))
                {
                    txtAge.Text = Convert.ToString(Convert.ToInt32(System.DateTime.Now.Year) - Convert.ToInt32(selectreader.Birthday.Year));
                }
                else if (Convert.ToInt32(System.DateTime.Now.Month) == Convert.ToInt32(selectreader.Birthday.Month) && Convert.ToInt32(System.DateTime.Now.Day) < Convert.ToInt32(selectreader.Birthday.Day))
                {
                    txtAge.Text = Convert.ToString(Convert.ToInt32(System.DateTime.Now.Year) - Convert.ToInt32(selectreader.Birthday.Year));
                }
                else
                {
                    txtAge.Text = Convert.ToString(Convert.ToInt32(System.DateTime.Now.Year) - Convert.ToInt32(selectreader.Birthday.Year) + 1);
                }
                txtPassportSeries.Text = Convert.ToString(selectreader.PassportOfReader.Series);
                txtPassportNumber.Text = Convert.ToString(selectreader.PassportOfReader.Number);
                txtReaderId.Text = Convert.ToString(selectreader.IdReader);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListReader.ItemsSource = context.Reader
                .Where(i => i.LastName.Contains(txtSearch.Text) 
                || i.FirstName.Contains(txtSearch.Text)
                || i.MeddleName.Contains(txtSearch.Text)).ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditReaderWin addEditReaderWin = new AddEditReaderWin();
            this.Opacity = 0.5;
            addEditReaderWin.ShowDialog();
            this.Opacity = 1;
            ListReader.ItemsSource = context.Reader.ToList();
            HideViewBar();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (ListReader.SelectedItem is Reader selectedreader)
            {
                AddEditReaderWin addEditReaderWin = new AddEditReaderWin(selectedreader);
                this.Opacity = 0.5;
                addEditReaderWin.ShowDialog();
                this.Opacity = 1;
                HideViewBar();
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ListReader.ItemsSource = context.Reader.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListReader.SelectedItem is Reader reader)
            {
                var result = MessageBox.Show("Удалить выбранного читателя?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    PassDel = AppData.context.PassportOfReader.Where(i => i.IdPassport == reader.IdPassport).ToList().FirstOrDefault();

                    SessionDel = AppData.context.Session.Where(i => i.IdReader == reader.IdReader).ToList();

                    if (AppData.context.Session.Where(i => i.IdReader == reader.IdReader).FirstOrDefault() != null)
                    {
                        var result2 = MessageBox.Show("Вместе с читателем будут удалены все операции, которые с ним связаны! " +
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

                    AppData.context.Reader.Remove(reader);
                    AppData.context.PassportOfReader.Remove(PassDel);

                    AppData.context.SaveChanges();
                    HideViewBar();
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ListReader.ItemsSource = context.Reader.ToList();
        }

        private void btnReaderSessions_Click(object sender, RoutedEventArgs e)
        {
            if (ListReader.SelectedItem is Reader selectedreader)
            {
                ReaderSessionsWin readerSessionsWin = new ReaderSessionsWin(selectedreader, CurrentUser);
                this.Opacity = 0.5;
                readerSessionsWin.ShowDialog();
                this.Opacity = 1;
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
