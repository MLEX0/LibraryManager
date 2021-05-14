using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager.DataBase
{
    public partial class Reader
    {
        public string ReaderFIO { get { return $"{LastName} {FirstName} {MeddleName}"; } set { LastName = value; FirstName = value; MeddleName = value; } }
    }

    public partial class Reader
    {
        public string ReaderPassport { get { return $"{PassportOfReader.Series} {PassportOfReader.Number}"; } set { PassportOfReader.Series = Convert.ToInt32(value); PassportOfReader.Number = Convert.ToInt32(value);} }
    }

    public partial class ViewSessions
    {
        public string ReaderFIO { get { return $"{ReaderLastName} {ReaderFirstName} {ReaderMeddleName}"; } set { ReaderLastName = value; ReaderFirstName = value; ReaderMeddleName = value; } }
    }
}
