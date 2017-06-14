using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pozoriste.ViewModel
{
    public class Karta
    {

        public int Sjediste_ID_sjedista { get; set; }
        public int Izvedba_ID_izvedbe { get; set; }
        public String Naziv { get; set; }
        public int Sjediste_Red_ID_reda { get; set; }
        public int Sjediste_Red_Zona_ID_zone { get; set; }
        public int Sjediste_Red_Zona_Sala_ID_sale { get; set; }
    }
}