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
    
    public partial class PassportOfReader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PassportOfReader()
        {
            this.Reader = new HashSet<Reader>();
        }
    
        public int IdPassport { get; set; }
        public int Series { get; set; }
        public int Number { get; set; }
        public string PlaceOfIssue { get; set; }
        public System.DateTime DataOfIssue { get; set; }
        public string DepartmentСode { get; set; }
        public string PlaceOfBirth { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reader> Reader { get; set; }
    }
}
