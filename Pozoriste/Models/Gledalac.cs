//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pozoriste.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Gledalac
    {
        public Gledalac()
        {
            this.Recenzijas = new HashSet<Recenzija>();
            this.Rezervacijas = new HashSet<Rezervacija>();
        }
    
        public string ID_korisnika { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Datum registracije")]
        public System.DateTime Datum_registracije { get; set; }
    
        public virtual Korisnik Korisnik { get; set; }
        public virtual Korisnik Korisnik1 { get; set; }
        public virtual ICollection<Recenzija> Recenzijas { get; set; }
        public virtual ICollection<Rezervacija> Rezervacijas { get; set; }
    }
}
