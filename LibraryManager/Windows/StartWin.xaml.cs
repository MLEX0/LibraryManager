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
    /// Логика взаимодействия для StartWin.xaml
    /// </summary>
    public partial class StartWin : Window
    {
        private static int _ErrorCounter;

        private void Login()//Метод входа в приложение
        {
            DataBase.Librarian user = context.Librarian
.ToList().Where(i => i.Login == txtLogin.Text && i.Password == pswPassword.Password).FirstOrDefault(); // поиск записи в БД с логином и паролем введенным пользователем

            if (user != null)//проверка найденого пароля и логина
            {

                if (user.IdRole == 1)//Вход по роли пользователя --<Допилить>--
                {
                    MainWin MainWin = new MainWin(user);
                    this.Hide();
                    MainWin.ShowDialog();
                    this.Show();
                }
                else if (user.IdRole == 2)
                {
                    AdminWin AdminWin = new AdminWin();
                    AdminWin.Show();
                    this.Close();
                }

            }
            else
            {
                _ErrorCounter++;
                if (_ErrorCounter > 20)//Неправильный ввод пароля
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
            }
        }

        private void CloseWin()
        {
            Application.Current.Shutdown();//Закрытие окна при нажатии
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseWin();
        }



        private void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void pswPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void txtLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CloseWin();
            }
        }
    }
}