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
    
    public partial class Librarian
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Librarian()
        {
            this.Session = new HashSet<Session>();
        }
    
        public int IdLibrarian { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MeddleName { get; set; }
        public int IdRole { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    
        public virtual Role Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Session> Session { get; set; }
    }
}
