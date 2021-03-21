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
    /// Логика взаимодействия для LibrarianWin.xaml
    /// </summary>
    public partial class LibrarianWin : Window
    {
        public LibrarianWin(DataBase.Librarian User)
        {
            InitializeComponent();
        }
    }
}
