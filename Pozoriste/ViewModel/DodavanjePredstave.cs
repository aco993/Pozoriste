using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pozoriste.Models;

namespace Pozoriste.ViewModel
{
    public class DodavanjePredstave
    {
                public DodavanjePredstave()
        {
            this.Izvedbas = new HashSet<Izvedba>();
            this.Recenzijas = new HashSet<Recenzija>();
            this.Glumacs = new HashSet<Glumac>();
            this.Rediteljs = new HashSet<Reditelj>();
            this.izabraniGlumci = izabraniGlumci;
            this.dostupniGlumci = dostupniGlumci;
            
        }
        public int ID_Predstave { get; set; }
        [Display(Name = "Naziv predstave")]
        public string Naziv_Predstave { get; set; }
        [Display(Name = "Dužina trajanja")]
        public int Duzina_trajanja { get; set; }
        [Display(Name = "Opis predstave")]
        public string Opis_predstave { get; set; }
        [Display(Name = "Organizator")]
        public int Organizator_ID_organizatora { get; set; }
        [Display(Name = "Žanr")]
        public int Zanr_ID_zanra { get; set; }
        public byte[] slika { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
        [NotMapped]
        public String SlikaURL { get { return ID_Predstave.ToString() + ".jpg"; } }

        public List<Glumac> dostupniGlumci { get; set; }
        public List<Glumac> izabraniGlumci { get; set; }

        public int[] selektovaniDostupniGlumci { get; set; }
        public int[] selektovaniIzabraniGlumci { get; set; }
  

        public List<Reditelj> dostupniReditelji { get; set; }
        public List<Reditelj> izabraniReditelji { get; set; }

        public int[] selektovaniDostupniReditelji { get; set; }
        public int[] selektovaniIzabraniReditelji { get; set; }

        public virtual ICollection<Izvedba> Izvedbas { get; set; }
        public virtual Organizator Organizator { get; set; }
        public virtual Zanr Zanr { get; set; }
        public virtual ICollection<Recenzija> Recenzijas { get; set; }
        public virtual ICollection<Glumac> Glumacs { get; set; }
        public virtual ICollection<Reditelj> Rediteljs { get; set; }

    }
}