using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pozoriste.Models
{
    public class DodavanjeReditelja
    {
        public IEnumerable<SelectListItem> reditelji { get; set; }
        public string[] selektovaniReditelji { get; set; }
    }
}