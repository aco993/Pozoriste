﻿//------------------------------------------------------------------------------
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
    
    public partial class Izvedba
    {
        public Izvedba()
        {
            this.Cjenovniks = new HashSet<Cjenovnik>();
            this.Rezervacijas = new HashSet<Rezervacija>();
            this.Sjedistes = new HashSet<Sjediste>();
        }
    
        public int ID_izvedbe { get; set; }
        [Display(Name = "Vrijeme izvođenja")]
        [DataType(DataType.Time)]
        public System.TimeSpan Vrijeme_izvodjenja { get; set; }
        public int Sala_ID_sale { get; set; }
        public int Predstava_ID_Predstave { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Datum")]
        public System.DateTime Repertoar_Datum { get; set; }
    
        public virtual ICollection<Cjenovnik> Cjenovniks { get; set; }
        public virtual Predstava Predstava { get; set; }
        public virtual Repertoar Repertoar { get; set; }
        public virtual Sala Sala { get; set; }
        public virtual ICollection<Rezervacija> Rezervacijas { get; set; }
        public virtual ICollection<Sjediste> Sjedistes { get; set; }
    }
}
