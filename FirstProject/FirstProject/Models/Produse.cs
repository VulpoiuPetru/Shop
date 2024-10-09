//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirstProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Produse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produse()
        {
            this.Recepties = new HashSet<Receptie>();
            this.Vanzaris = new HashSet<Vanzari>();
        }
    
        public int IDProdus { get; set; }
        public Nullable<int> IDAmbalaj { get; set; }
        public Nullable<int> IDPersoana { get; set; }
        public Nullable<int> IDTipProdus { get; set; }
        public Nullable<System.DateTime> DataAmbalare { get; set; }
        public string Status { get; set; }
    
        public virtual Ambalaj Ambalaj { get; set; }
        public virtual Persoana Persoana { get; set; }
        public virtual TipProdu TipProdu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receptie> Recepties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vanzari> Vanzaris { get; set; }
    }
}
