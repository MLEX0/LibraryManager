using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager.DataBase
{
    public partial class ViewSessions
    {
        public string LibrarianFIO { get { return $"{LibrarianLastName} {LibrarianFirstName} {LibrarianMeddleName}"; } set { LibrarianLastName = value; LibrarianFirstName = value; LibrarianMeddleName = value; } }
    }
}
