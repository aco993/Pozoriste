using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pozoriste.Models;
using System.Web.Mvc;

namespace Pozoriste.ViewModel
{
    public class ProfilAdmina
    {
        public String ID_korisnika { get; set; }

        public List<Biletar> biletari { get; set; }

        public List<Predstava> predstave { get; set; }

        public List<Repertoar> repertoari { get; set; }
        public List<Izvedba> izvedbeP { get; set; }
        public List<Izvedba> izvedbeD { get; set; }
        public List<Izvedba> izvedbeS { get; set; }
        public List<Izvedba> izvedbeM { get; set; }

        public List<Sala> sale { get; set; }

        public List<Zanr> zanrovi  { get; set; }

        public List<Organizator> organizatori { get; set; }

        public List<Glumac> glumci { get; set; }

        public List<Reditelj> reditelji { get; set; }
        //public List<Karta> karte { get; set; }

       


    }
}