using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Pozoriste.ViewModel
{
    public class ZonaUSali
    {
        public int ID_zone { get; set; }
        [Display(Name = "Naziv zone")]
        public string Naziv_zone { get; set; }
        public int Sala_ID_sale { get; set; }
        public int brojSjedista { get; set; }
        public int brojRedova { get; set; }
        public int cijena { get; set; }
    }
}