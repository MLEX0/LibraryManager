//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManager.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Librarian_Session
    {
        public int IdLibrarionSessiom { get; set; }
        public int IdSession { get; set; }
        public int IdLibrarian { get; set; }
        public int IdTypeOperation { get; set; }
    
        public virtual Librarian Librarian { get; set; }
        public virtual Session Session { get; set; }
        public virtual TypeOperation TypeOperation { get; set; }
    }
}
