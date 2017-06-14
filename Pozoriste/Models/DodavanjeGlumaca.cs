using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pozoriste.Models
{
    public class DodavanjeGlumaca
    {
        public IEnumerable<SelectListItem> glumci { get; set; }
        public string[] selektovaniGlumci { get; set; }
    }
}