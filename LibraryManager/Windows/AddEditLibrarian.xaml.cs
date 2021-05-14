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
    /// Логика взаимодействия для AddEditLibrarian.xaml
    /// </summary>
    public partial class AddEditLibrarian : Window
    {
        private Librarian AddLibrarian { get; set; }
        private Librarian EditLibrarian { get; set; }

        private bool isEdit;

        public AddEditLibrarian()
        {
            InitializeComponent();

            cmbUserRole.ItemsSource = AppData.context.Role.ToList(); // Заполнение cmbUserRole по базе данных
            cmbUserRole.DisplayMemberPath = "RoleName";
            cmbUserRole.SelectedIndex = 1;

            txtHeader.Text = "Добавление пользователя";
            isEdit = false;
            AddLibrarian = new Librarian();
        }

        public AddEditLibrarian(Librarian librarian)// Сценарий изменения
        {
            InitializeComponent();

            cmbUserRole.ItemsSource = AppData.context.Role.ToList(); // Заполнение cmbUserRole по базе данных
            cmbUserRole.DisplayMemberPath = "RoleName";
            cmbUserRole.SelectedIndex = 1;

            txtHeader.Text = "Изменение пользователя";
            isEdit = true;
            AddLibrarian = new Librarian();
            EditLibrarian = librarian;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    MessageBox.Show("Поле Фамилия не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("Поле Имя не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLogin.Text))
                {
                    MessageBox.Show("Поле Логин не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Поле Логин не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var uniqueUser = AppData.context.Librarian.ToList().Where(i => i.Login == txtLogin.Text).FirstOrDefault();

                if (isEdit == false)
                {
                    if (uniqueUser == null)
                    {
                        var result = MessageBox.Show("Вы уверены?", "Добавление нового сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            AddLibrarian.LastName = txtLastName.Text;
                            AddLibrarian.FirstName = txtFirstName.Text;
                            if (!string.IsNullOrWhiteSpace(txtMiddleName.Text))
                            {
                                AddLibrarian.MeddleName = txtMiddleName.Text;
                            }
                            AddLibrarian.Login = txtLogin.Text;
                            AddLibrarian.Password = txtPassword.Text;
                            AddLibrarian.IdRole = cmbUserRole.SelectedIndex + 1;

                            AppData.context.Librarian.Add(AddLibrarian);
                            AppData.context.SaveChanges();
                            MessageBox.Show("Сотрудник добавлен");
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Логин занят", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                else if (isEdit == true)
                {
                    if (uniqueUser == null || uniqueUser.Login == EditLibrarian.Login)
                    {
                        var result = MessageBox.Show("Вы уверены?", "Изменение данных сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            EditLibrarian.FirstName = txtFirstName.Text;
                            EditLibrarian.LastName = txtLastName.Text;
                            if (!string.IsNullOrWhiteSpace(txtMiddleName.Text))
                            {
                                EditLibrarian.MeddleName = txtMiddleName.Text;
                            }
                            EditLibrarian.Login = txtLogin.Text;
                            EditLibrarian.Password = txtPassword.Text;
                            EditLibrarian.IdRole = cmbUserRole.SelectedIndex + 1;

                            AppData.context.SaveChanges();
                            MessageBox.Show("Изменения сохранены");
                            this.Close();
                        }
                    }

                    else
                    {
                        MessageBox.Show("Логин занят", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления в базу данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEdit == true)
            {
                txtFirstName.Text = EditLibrarian.FirstName;
                txtLastName.Text = EditLibrarian.LastName;
                txtMiddleName.Text = EditLibrarian.MeddleName;
                txtLogin.Text = EditLibrarian.Login;
                txtPassword.Text = EditLibrarian.Password;
                cmbUserRole.SelectedIndex = Convert.ToInt32(EditLibrarian.IdRole) - 1;
            }
        }

        private void txtPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Any(x => ';'.Equals(x));//Запрет ввода ';' в поле Login
        }
    }
}
