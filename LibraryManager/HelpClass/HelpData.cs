using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManager.DataBase;

namespace LibraryManager.HelpClass
{
    class HelpData
    {
        public static List<Book> AddedInSessionBooks = new List<Book>();
        public static Reader AddedInSessionReader { get; set; }
        public static BookAuthor NowAddedAuthor { get; set; }
        public static Publisher NowAddedPublisher { get; set; }
    }
}
