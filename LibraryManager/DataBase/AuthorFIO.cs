using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager.DataBase
{
    public partial class BookAuthor
    {
        public string AuthorFIO { get { return $"{LastName} {FirstName} {MeddleName}"; } set { LastName = value; FirstName = value; MeddleName = value; } }
    }

    public partial class ViewSessions
    {
        public string AuthorFIO { get { return $"{AuthorLastName} {AuthorFirstName} {AuthorMeddleName}"; } set { AuthorLastName = value; AuthorFirstName = value; AuthorMeddleName = value; } }
    }
}
