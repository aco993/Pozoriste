using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pozoriste.Models;
using System.ComponentModel.DataAnnotations;

namespace Pozoriste.ViewModel
{
    public class ProfilBiletara
    {
        public String ID_korisnika { get; set; }

        public String sifra { get; set; }

        public String Ime { get; set; }

        public String Prezime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime datum_zaposlenja { get; set; }

        public List<Repertoar> repertoari { get; set; }

        public List<Izvedba> izvedbeP { get; set; }
        public List<Izvedba> izvedbeD { get; set; }
        public List<Izvedba> izvedbeS { get; set; }
        public List<Izvedba> izvedbeM { get; set; }

        public List<Predstava> predstave { get; set; }

       


    }
}