using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    /// Логика взаимодействия для StartWin.xaml
    /// </summary>
    public partial class StartWin : Window
    {
        int cpActivate = 0;
        int errorCounter = 0;
        int errorOfRead = 0;
        string SaveLogin { get; set; }
        string SavePassword { get; set; }

        public StartWin()
        {
            InitializeComponent();


            if (File.Exists("SaveUser.txt") == true)// защита от дауна
            {
                if (FileSaveClass.FileRead("SaveUser.txt") != null)
                {
                    try
                    {
                        FileSaveClass.FileRead("SaveUser.txt"); // проверка правильности данных в файле
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка сохранения пользователя, повторите вход!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        errorOfRead = 1;
                        File.Delete("SaveUser.txt");
                    }

                    if (errorOfRead == 0)
                    {
                        using (StreamReader sr = new StreamReader("SaveUser.txt"))
                        {
                            string[] words;
                            string str = sr.ReadLine();
                            words = str.Split(new char[] { ';' });
                            SaveLogin = words[0];
                            SavePassword = words[1];
                        }
                        var saveuser = context.Librarian.ToList().Where(u => u.Login == SaveLogin && u.Password == SavePassword).FirstOrDefault();
                        if (saveuser == null)// Проверка id 
                        {
                            MessageBox.Show("Сохранённый пользователь перестал существовать!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            File.Delete("SaveUser.txt");
                        }
                        else
                        {
                            cbxRemind.IsChecked = true;
                            txtLogin.Text = SaveLogin;
                            pswPassword.Password = SavePassword;
                        }
                    }
                }
            }
            else if (File.Exists("SaveUser.txt") == false)// Если файла не существует, создаёт файл
            {
                File.Create("SaveUser.txt");
            }
        }


        private void CapchaGet()//Получает новую капчу и присваивает её текстовому полю! 
        {
            string CpString = "";
            CpString = CapchaGenClass.CapchaGenerate();
            txbCapchaEnter.Text = CpString;
        }


        private void Login()//Метод входа в приложение
        {
            DataBase.Librarian user = context.Librarian
.ToList().Where(i => i.Login == txtLogin.Text && i.Password == pswPassword.Password).FirstOrDefault(); // поиск записи в БД с логином и паролем введенным пользователем

            if (user != null && txbCapchaEnter.Text.ToLower() == txtCapcha.Text.ToLower())//проверка найденого пароля и логина
            {

                if (File.Exists("SaveUser.txt") == true)// Проверка существования файла!
                {
                    if (cbxRemind.IsChecked == true && File.Exists("SaveUser.txt") == true)
                    {
                        FileSaveClass.FileWrite($"{user.Login};{user.Password}", "SaveUser.txt");// записывает пользователя в файл
                    }
                    if (cbxRemind.IsChecked == false)// удаление файла 
                    {
                        File.Delete("SaveUser.txt");
                    }
                }
                else
                {
                    if (cbxRemind.IsChecked == true && File.Exists("SaveUser.txt") == false)// Полная шляпа, когда пользователь трогает сраный файл!!!
                    {
                        MessageBox.Show("Внимание! \nИсполняемый файл занaят системным процессом! " +
                            "\nПри следующей авторизации вам придётся ещё раз ввести ваши данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                switch (user.IdRole) // Разделение по ролям
                {
                    case 1:
                        AdminWin adminWin = new AdminWin(user); // Переход на окно администратора
                        this.Hide();
                        adminWin.ShowDialog();
                        this.Close();
                        break;

                    case 2:
                        ReaderWin mainWin = new ReaderWin(user); // Переход на рабочее окно пользователя
                        this.Hide();
                        mainWin.ShowDialog();
                        this.Close();
                        break;
                }
                

            }
            else
            {
                errorCounter++;

                if (errorCounter > 2000) //Неправильный ввод пароля
                {
                    MessageBox.Show("Обнаружен слишком низкий IQ " +
                        "\nПриведите к рабочей станции человека \nс " +
                        "самым высоким IQ в комнате и перезапустите программу!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    btnLogin.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Логин или пароль введены неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (txbCapchaEnter.Text.ToLower() != txtCapcha.Text.ToLower() && cpActivate == 1) //неправильно введена капча
                {
                    MessageBox.Show("Неправильно введена капча!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (errorCounter > 2) // Открытие капчи при трёх ошибках
                {
                    CapchaShow();
                    cpActivate = 1;
                }

                if (cpActivate == 1) // Получение новой капчи при первом открытии
                {
                    CapchaGet();
                }
            }
        }

        private void btnCapchaReboot_Click(object sender, RoutedEventArgs e)
        {
            CapchaGet(); //Обновление капчи по нажатию кнопки
        }

        private void CapchaShow() //открывает капчу на окне
        {
            btnClose.Visibility = Visibility.Hidden;
            btnLogin.Visibility = Visibility.Hidden;
            btnClose2.Visibility = Visibility.Visible;
            btnLogin2.Visibility = Visibility.Visible;
            txtCapcha.Visibility = Visibility.Visible;
            txbCapcha.Visibility = Visibility.Visible;
            txbCapchaEnter.Visibility = Visibility.Visible;
            btnCapchaReboot.Visibility = Visibility.Visible;
            brdCapcha.Visibility = Visibility.Visible;
        }

        private void CloseWin()
        {
            Application.Current.Shutdown(); //Закрытие окна при нажатии
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseWin(); // Закрытие по кнопке
        }

        private void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            Login(); // Вход по кнопке
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) //Выход при нажатии Esc
            {
                CloseWin();
            }

            if (e.Key == Key.Enter) // Вход при нажатии Enter
            {
                Login();
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) // При нажатии на текст "Запомнить меня" комбобокс маеняется
            {
                if (cbxRemind.IsChecked == false)
                {
                    cbxRemind.IsChecked = true;
                }
                else
                {
                    cbxRemind.IsChecked = false;
                }
            }
        }
        
        private void txtLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Any(x => ';'.Equals(x));//Запрет ввода ';' в поле Login
        }

        private void pswPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Any(x => ';'.Equals(x));//Запрет ввода ';' в поле Password
        }
    }
}