//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarRental.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Uprawnienia
    {
        public int id_uprawnienia { get; set; }
        public int id_konta { get; set; }
        public string uprawnienie { get; set; }
    
        public virtual Konta Konta { get; set; }
    }
}
