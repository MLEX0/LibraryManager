﻿using LibraryManager.DataBase;
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
    /// Логика взаимодействия для ReaderSessionsWin.xaml
    /// </summary>
    public partial class ReaderSessionsWin : Window
    {
        Librarian CurrentUser { get; set; }

        Reader SelectReader { get; set; }

        Session EditSession { get; set; }

        Session DelSession { get; set; }

        Book EditBook { get; set; }

        public ReaderSessionsWin(Reader selectedreader, Librarian currentuser)
        {
            InitializeComponent();

            txtNameLib.Text = $"{currentuser.LastName.ToString()} {currentuser.FirstName.ToString()}";
            txtHeaderReaderName.Text = $"Операции пользователя  '{selectedreader.ReaderFIO}' Id - {selectedreader.IdReader}";

            CurrentUser = currentuser;
            SelectReader = selectedreader;

            lvSession.ItemsSource = AppData.context.ViewSessions.Where(i => i.IdReader == selectedreader.IdReader).ToList();
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditSession addEditSession = new AddEditSession(CurrentUser, SelectReader);
            this.Opacity = 0.5;
            addEditSession.ShowDialog();
            this.Opacity = 1;

            lvSession.ItemsSource = AppData.context.ViewSessions.Where(i => i.IdReader == SelectReader.IdReader).ToList();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvSession.SelectedItem is ViewSessions selectsession)
            {
                var result = MessageBox.Show("Удалить запись сессии?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DelSession = AppData.context.Session.Where(i => i.IdSession == selectsession.IdSession).FirstOrDefault(); 
                    if (selectsession.DateOfEnd == null)
                    {
                        MessageBox.Show("Нельзя удалить незавершённую сессию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        AppData.context.Session.Remove(DelSession);
                        AppData.context.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            lvSession.ItemsSource = AppData.context.ViewSessions.Where(i => i.IdReader == SelectReader.IdReader).ToList();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (lvSession.SelectedItem is ViewSessions selectsession)
            {
                var result = MessageBox.Show("Завершить сессию?", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                EditSession = AppData.context.Session.Where(i => i.IdSession == selectsession.IdSession).FirstOrDefault();
                EditBook = AppData.context.Book.Where(i => i.IdBook == selectsession.IdBook).FirstOrDefault();
                if (result == MessageBoxResult.Yes)
                {
                    if (selectsession.DateOfEnd != null)
                    {
                        MessageBox.Show("Сессия уже завершена!");
                    }
                    else
                    {
                        EditBook.CountBooksInLibrary++;
                        EditBook.CountBooksInUse--;
                        EditSession.DateOfEnd = System.DateTime.Now;
                        AppData.context.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            lvSession.ItemsSource = AppData.context.ViewSessions.Where(i => i.IdReader == SelectReader.IdReader).ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvSession.ItemsSource = AppData.context.ViewSessions.Where(i => i.LibrarianFirstName.Contains(txtSearch.Text)
            || i.LibrarianLastName.Contains(txtSearch.Text)
            || i.LibrarianMeddleName.Contains(txtSearch.Text)
            || i.ReaderFirstName.Contains(txtSearch.Text)
            || i.ReaderLastName.Contains(txtSearch.Text)
            || i.ReaderMeddleName.Contains(txtSearch.Text)
            || i.BookName.Contains(txtSearch.Text)
            || i.AuthorFirstName.Contains(txtSearch.Text)
            || i.AuthorLastName.Contains(txtSearch.Text)
            || i.AuthorMeddleName.Contains(txtSearch.Text)).ToList();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lvSession.SelectedItem is ViewSessions selectitem)
            {
                ShowViewBar();

                txtLibrarian.Text = selectitem.LibrarianFIO;
                txtDateOfBegin.Text = Convert.ToString(selectitem.DateOfBegin);
                txtReader.Text = selectitem.ReaderFIO;
                if (selectitem.DateOfEnd == null)
                {
                    txtStat.Text = "Не завершена";
                    btnReturn.Visibility = Visibility.Visible;
                }
                else if (selectitem.DateOfEnd != null)
                {
                    txtStat.Text = "Завершена";
                    btnReturn.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
