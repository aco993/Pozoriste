using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pozoriste.Models;

namespace Pozoriste.ViewModel
{
    public class ProfilKorisnika
    {
        public String ID_korisnika { get; set; }

        public String sifra { get; set; }

        public String Ime { get; set; }

        public String Prezime { get; set; }

        public List<Repertoar> repertoari { get; set; }
        public List<Izvedba> izvedbeP { get; set; }
        public List<Izvedba> izvedbeD { get; set; }
        public List<Izvedba> izvedbeS { get; set; }
        public List<Izvedba> izvedbeM { get; set; }

        public List<Predstava> predstave { get; set; }

        public List<Rezervacija> rezervacije { get; set; }

        public List<RezervisanaIzvedba> rezervisaneIzvedbe { get; set; }

        //public List<Karta> karte { get; set; }
    }
}