using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Pozoriste.ViewModel
{
    public class RezervisanaIzvedba
    {
        public int id_izvedbe { get; set; }
        public String naziv_predstave { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public String idkorisnika { get; set; }
        public String naziv_rezervacije { get; set; }
        public DateTime datum_izvedbe { get; set; }
        public TimeSpan vrijeme_izvodjenja { get; set; }
        public String naziv_sale { get; set; }
        public List<int> mesta { get; set; }

    }
}