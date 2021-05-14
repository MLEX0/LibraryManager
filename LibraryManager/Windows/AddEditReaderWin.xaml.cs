using LibraryManager.DataBase;
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
    /// Логика взаимодействия для AddEditReaderWin.xaml
    /// </summary>
    public partial class AddEditReaderWin : Window
    {
        private Reader AddReader { get; set; }
        private Reader EditReader { get; set; }
        private PassportOfReader AddPassport { get; set; }
        private PassportOfReader EditPassport { get; set; }

        private bool isEdit;

        public AddEditReaderWin()
        {
            InitializeComponent();

            cmbGender.ItemsSource = AppData.context.Gender.ToList(); // Заполнение по базе данных
            cmbGender.DisplayMemberPath = "GenderName";
            cmbGender.SelectedIndex = 0;

            txtHeader.Text = "Добавление читателя";
            isEdit = false;
            AddReader = new Reader();
            AddPassport = new PassportOfReader();
        }

        public AddEditReaderWin(Reader reader)
        {
            InitializeComponent();

            cmbGender.ItemsSource = AppData.context.Gender.ToList(); // Заполнение по базе данных
            cmbGender.DisplayMemberPath = "GenderName";
            cmbGender.SelectedIndex = 0;

            txtHeader.Text = "Изменение читателя";
            isEdit = true;
            AddReader = new Reader();
            EditReader = reader;
            EditPassport = AppData.context.PassportOfReader.Where(i => i.IdPassport == reader.IdPassport).ToList().FirstOrDefault();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFName.Text) && txtFName.Text.Length > 50)// Проверка ввода полей
                {
                    MessageBox.Show("Поле имя не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLName.Text) && txtLName.Text.Length > 50)
                {
                    MessageBox.Show("Поле фамилия не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (txtMName.Text.Length > 50)
                {
                    MessageBox.Show("Поле отчество не может превышать 50 символов", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPDepartmentCode.Text) && txtPDepartmentCode.Text.Length > 20)
                {
                    MessageBox.Show("Поле Код подразделения не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPNumber.Text) && Convert.ToInt32(txtPNumber.Text) > 999999)
                {
                    MessageBox.Show("Поле номер паспорта не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPPlaceOfBirth.Text))
                {
                    MessageBox.Show("Поле место рождения не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPPlaceOfIssue.Text))
                {
                    MessageBox.Show("Поле кем выдан не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (dpcBirthday.SelectedDate == null)
                {
                    MessageBox.Show("Поле дата рождения не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (dpcPDayOfIssue.SelectedDate == null)
                {
                    MessageBox.Show("Поле дата выдачи паспорта не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPSeries.Text) && Convert.ToInt32(txtPNumber.Text) > 9999)
                {
                    MessageBox.Show("Поле серия паспорта не может быть пустым", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (isEdit == false)
                {
                    AddPassport.DataOfIssue = dpcPDayOfIssue.SelectedDate.Value;
                    AddPassport.DepartmentСode = txtPDepartmentCode.Text;
                    AddPassport.Number = Convert.ToInt32(txtPNumber.Text);
                    AddPassport.PlaceOfBirth = txtPPlaceOfBirth.Text;
                    AddPassport.PlaceOfIssue = txtPPlaceOfIssue.Text;
                    AddPassport.Series = Convert.ToInt32(txtPSeries.Text);

                    AppData.context.PassportOfReader.Add(AddPassport);

                    AddReader.Birthday = dpcBirthday.SelectedDate.Value;
                    AddReader.FirstName = txtFName.Text;
                    AddReader.IdGender = cmbGender.SelectedIndex + 1;
                    AddReader.LastName = txtLName.Text;
                    AddReader.IdPassport = AppData.context.PassportOfReader.Where(i => i.Number == AddPassport.Number 
                    && i.Series == AddPassport.Series).Select(i => i.IdPassport).FirstOrDefault();

                    if (txtMName.Text == " " || txtMName.Text == null || txtMName.Text == "")
                    {
                        AddReader.MeddleName = null;
                    }
                    else
                    {
                        AddReader.MeddleName = txtMName.Text;
                    }

                    AppData.context.Reader.Add(AddReader);
                    AppData.context.SaveChanges();
                    MessageBox.Show($"Читатель {AddReader.FirstName} {AddReader.LastName} {txtMName.Text} добавлен");
                }
                if (isEdit == true)
                {
                    EditPassport.DataOfIssue = dpcPDayOfIssue.SelectedDate.Value;
                    EditPassport.DepartmentСode = txtPDepartmentCode.Text;
                    EditPassport.Number = Convert.ToInt32(txtPNumber.Text);
                    EditPassport.PlaceOfBirth = txtPPlaceOfBirth.Text;
                    EditPassport.PlaceOfIssue = txtPPlaceOfIssue.Text;
                    EditPassport.Series = Convert.ToInt32(txtPSeries.Text);

                    AppData.context.SaveChanges();

                    EditReader.Birthday = dpcBirthday.SelectedDate.Value;
                    EditReader.FirstName = txtFName.Text;
                    EditReader.IdGender = cmbGender.SelectedIndex + 1;
                    EditReader.LastName = txtLName.Text;

                    if (txtMName.Text == " " || txtMName.Text == null || txtMName.Text == "")
                    {
                        EditReader.MeddleName = null;
                    }
                    else
                    {
                        EditReader.MeddleName = txtMName.Text;
                    }

                    AppData.context.SaveChanges();
                    MessageBox.Show($"Изменеия сохранены", Title="Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();

            }
            catch
            {
                MessageBox.Show("Ошибка добавления в базу данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEdit == true)
            {
                txtFName.Text = EditReader.FirstName;
                txtLName.Text = EditReader.LastName;
                txtMName.Text = EditReader.MeddleName;
                txtPDepartmentCode.Text = Convert.ToString(EditReader.PassportOfReader.DepartmentСode);
                txtPNumber.Text = Convert.ToString(EditReader.PassportOfReader.Number);
                txtPPlaceOfBirth.Text = Convert.ToString(EditReader.PassportOfReader.PlaceOfBirth);
                txtPPlaceOfIssue.Text = Convert.ToString(EditReader.PassportOfReader.PlaceOfIssue);
                txtPSeries.Text = Convert.ToString(EditReader.PassportOfReader.Series);
                dpcPDayOfIssue.SelectedDate = EditReader.PassportOfReader.DataOfIssue;//"DateOfIssue"
                dpcBirthday.SelectedDate = EditReader.Birthday;
                cmbGender.SelectedIndex = EditReader.IdGender - 1;

            }
        }

        private void txtPSeries_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }
    }
}
