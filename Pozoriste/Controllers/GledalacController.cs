using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pozoriste.Models;

namespace Pozoriste.Controllers
{
    public class GledalacController : Controller
    {
        private bazaPozoriste2Entities5 db = new bazaPozoriste2Entities5();
        //
        // GET: /Gledalac/
        public ActionResult Index()
        {
            Pozoriste.ViewModel.ProfilKorisnika profil = new ViewModel.ProfilKorisnika();
            //osnovni podaci
            var korisnik = db.Korisniks.Find(User.Identity.Name);
            profil.ID_korisnika = korisnik.ID_korisnika;
            profil.Ime = korisnik.Ime;
            profil.Prezime = korisnik.Prezime;
            profil.sifra = korisnik.Sifra;

            //umjesto izvedbi join izvedbe i rezervacije, RezervisanaIzvedba ViewModel srediti itd
            List<Pozoriste.ViewModel.RezervisanaIzvedba> rezervisaneIzvedbe = new List<Pozoriste.ViewModel.RezervisanaIzvedba>();

            var rezervacije = db.Rezervacijas.Where(x => x.Gledalac_ID_korisnika == User.Identity.Name).ToList();
            profil.rezervacije = new List<Rezervacija>();
            foreach (var rez in rezervacije)
            {
                profil.rezervacije.Add(rez);
            }

            foreach (var item in db.Izvedbas.Where(x => x.Repertoar_Datum > System.DateTime.Today))
            {
                if (rezervacije.Exists(x => x.Izvedba_ID_izvedbe == item.ID_izvedbe))
                {
                    Pozoriste.ViewModel.RezervisanaIzvedba rezIzv = new ViewModel.RezervisanaIzvedba();
                    rezIzv.id_izvedbe = item.ID_izvedbe;
                    rezIzv.datum_izvedbe = item.Repertoar.Datum;
                    rezIzv.naziv_predstave = item.Predstava.Naziv_Predstave;
                    rezIzv.vrijeme_izvodjenja = item.Vrijeme_izvodjenja;
                    rezIzv.naziv_sale = item.Sala.Naziv_Sale;
                    rezIzv.mesta = db.Rezervacijas.Where(x => x.Izvedba_ID_izvedbe == item.ID_izvedbe).Select(x => x.Sjediste_ID_sjedista).ToList();
                    rezervisaneIzvedbe.Add(rezIzv);
                }
            }
            profil.rezervisaneIzvedbe = rezervisaneIzvedbe;

            DateTime danas = DateTime.Today;
            DateTime sedamDana = danas.AddDays(7);
            DateTime mjesecDana = danas.AddDays(30);
            //izvedbe na danasnjem repertoaru
            profil.izvedbeD = new List<Izvedba>();
            var izvedbeDLista = db.Izvedbas.Where(x => x.Repertoar_Datum == danas).Select(x => x.ID_izvedbe).ToList();
            if (izvedbeDLista.Count > 0)
            {
                foreach (var izvedba in izvedbeDLista)
                {

                    profil.izvedbeD.Add(db.Izvedbas.Where(x => x.ID_izvedbe == izvedba).Single());
                }
            }
            //izvedbe koje ce se odigrati u narednih 7 dana
            profil.izvedbeS = new List<Izvedba>();
            var izvedbeSLista = db.Izvedbas.Where(x => x.Repertoar_Datum <= sedamDana).Where(x => x.Repertoar_Datum >= danas).Select(x => x.ID_izvedbe).ToList();
            foreach (var izvedba in izvedbeSLista)
            {

                profil.izvedbeS.Add(db.Izvedbas.Where(x => x.ID_izvedbe == izvedba).Single());
            }
            //izvedbe koje ce se odigrati u narednih mjesec dana
            profil.izvedbeM = new List<Izvedba>();
            var izvedbeMLista = db.Izvedbas.Where(x => x.Repertoar_Datum <= mjesecDana).Where(x => x.Repertoar_Datum >= danas).Select(x => x.ID_izvedbe).ToList();
            foreach (var izvedba in izvedbeMLista)
            {

                profil.izvedbeM.Add(db.Izvedbas.Where(x => x.ID_izvedbe == izvedba).Single());
            }


            return View(profil);
        }

        //
        // GET: /Gledalac/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Gledalac/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Gledalac/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Gledalac/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Gledalac/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult EditProfil()
        {
            Pozoriste.ViewModel.ProfilKorisnika profil = new ViewModel.ProfilKorisnika();
            //osnovni podaci
            var korisnik = db.Korisniks.Find(User.Identity.Name);

            return View(korisnik);
        }

        [HttpPost]
        public ActionResult EditProfil(Korisnik korisnik)
        {
            ModelState.Remove(korisnik.ID_korisnika);
            if (ModelState.IsValid)
            {
                db.Entry(korisnik).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Gledalac/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Gledalac/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult RezervisiIzvedba(int id)
        {
            Izvedba izvedba = db.Izvedbas.Find(id);

            Sala sala = db.Salas.Where(x => x.ID_sale == izvedba.Sala_ID_sale).Single();
            //improvizovani dio, vazi samo za jednu zonu, ako ima vise zona ne moze se primijeniti
            ViewBag.brojSjedista = db.Sjedistes.Where(x => x.Red_Zona_Sala_ID_sale == sala.ID_sale).Count();
            ViewBag.brojRedova = db.Reds.Where(x => x.Zona_Sala_ID_sale == sala.ID_sale).Count();
            List<Zona> listaZona = db.Zonas.Where(x => x.Sala_ID_sale == sala.ID_sale).ToList();

            var svaRezervisana = (from x in db.Rezervacijas where x.Izvedba_ID_izvedbe == id select x.Sjediste_ID_sjedista);
            var svaSjedista = (from x in db.Sjedistes where x.Red_Zona_Sala_ID_sale == sala.ID_sale select x.ID_sjedista);
            var svaSlobodna = svaSjedista.Except(svaRezervisana);

            List<int> listaSlobodnih = new List<int>();

            foreach (int num in svaSlobodna)
            {
                listaSlobodnih.Add(num);
            }
            ViewBag.ListaSlobodnih = listaSlobodnih;
            ViewBag.izvedba = izvedba.ID_izvedbe;
            Session["idizvedbe"] = id;
            return View(listaZona);
        }

        public ActionResult RezervisiIzvedbaPost(int id, int id_izvedbe)
        {
            //sredi funkcionalnost rezervacije sjedista
            Rezervacija rez = new Rezervacija();
            int red = db.Sjedistes.Where(x => x.ID_sjedista == id).Select(x => x.Red_ID_reda).FirstOrDefault();
            int zona = db.Sjedistes.Where(x => x.ID_sjedista == id).Select(x => x.Red_Zona_ID_zone).FirstOrDefault();
            int sala = db.Sjedistes.Where(x => x.ID_sjedista == id).Select(x => x.Red_Zona_Sala_ID_sale).FirstOrDefault();

            rez.Sjediste_ID_sjedista = id;
            rez.Sjediste_Red_ID_reda = red;
            rez.Sjediste_Red_Zona_ID_zone = zona;
            rez.Sjediste_Red_Zona_Sala_ID_sale = sala;
            if (db.Gledalacs.Any(x => x.ID_korisnika == User.Identity.Name))
            {
                rez.Gledalac_ID_korisnika = User.Identity.Name;
            }
            rez.Izvedba_ID_izvedbe = id_izvedbe;
            rez.naziv_rezervacije = User.Identity.Name;
            db.Rezervacijas.Add(rez);
            db.SaveChanges();
            if (db.Gledalacs.Any(x => x.ID_korisnika == User.Identity.Name))
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index", "Biletar"); //new idbiletara
        }
        /*       [HttpGet]
               public ActionResult OtkaziRezervaciju(int id) {
                String korisnik= User.Identity.Name;
                   Rezervacija rez=db.Rezervacijas.Where(x=>x.Izvedba_ID_izvedbe==id).Where(x=>x.Gledalac_ID_korisnika==korisnik).Single();
                   return View(rez);
               }
              [ActionName("OtkaziRezervaciju")]
               [HttpPost]
               public ActionResult OtkaziRezervacijuPost(Rezervacija rez)
               {
                   try
                   {
    
                       db.Rezervacijas.Remove(rez);

                       return RedirectToAction("Index");
                   }
                   catch
                   {
                       return View();
                   }
               }
        */

        [HttpGet]
        public ActionResult RecenzujIzvedba(int id)
        {
            TempData["izvedba"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult RecenzujIzvedba(Recenzija recenz)
        {
            recenz.datum_ocjenjivanja = System.DateTime.Now;
            recenz.Gledalac_ID_korisnika = User.Identity.Name;
            int id_izvedbe = int.Parse(TempData["izvedba"].ToString());
            recenz.Predstava_ID_Predstave = db.Izvedbas.Where(x => x.ID_izvedbe == id_izvedbe).Select(x => x.Predstava_ID_Predstave).Single();
            db.Recenzijas.Add(recenz);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult IzmeniRecenzijuIzvedba(int id)
        {
            TempData["izvedba"] = id;
            int pred = db.Izvedbas.Where(x => x.ID_izvedbe == (int)id).Select(x => x.Predstava_ID_Predstave).FirstOrDefault();
            Recenzija rec = db.Recenzijas.Where(x => x.Gledalac_ID_korisnika == User.Identity.Name).Where(x => x.Predstava_ID_Predstave == pred).Single();
            return View(rec);
        }

        [HttpPost]
        public ActionResult IzmeniRecenzijuIzvedba(Recenzija recenz)
        {
            recenz.datum_ocjenjivanja = System.DateTime.Now;
            recenz.Gledalac_ID_korisnika = User.Identity.Name;
            int id_izvedbe = int.Parse(TempData["izvedba"].ToString());
            recenz.Predstava_ID_Predstave = db.Izvedbas.Where(x => x.ID_izvedbe == id_izvedbe).Select(x => x.Predstava_ID_Predstave).Single();
            db.Entry(recenz).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PregledIzvedba(int id)
        {
            Izvedba izvedba = db.Izvedbas.Find(id);
            Predstava predstava = db.Predstavas.Where(x => x.ID_Predstave == izvedba.Predstava_ID_Predstave).Single();
            List<Recenzija> listaRecenzija = db.Recenzijas.Where(x => x.Predstava_ID_Predstave == izvedba.Predstava_ID_Predstave).ToList();
            double prosjecna_ocjena = 0;
            int brojac = 0;
            int ukupno = 0;
            foreach (var item in listaRecenzija)
            {
                ukupno += item.ocjena;
                brojac++;
            }
            if (ukupno > 0)
            {
                prosjecna_ocjena = ukupno / (double)brojac;
            }
            ViewBag.prosjecna = prosjecna_ocjena;
            ViewBag.listaRecenzija = listaRecenzija;

            return View(predstava);
        }
        [HttpGet]
        public ActionResult PonistiRezervacija(int id)
        {
            var rezervacije = db.Rezervacijas.Where(x => x.Izvedba_ID_izvedbe == id).Where(x => x.Gledalac_ID_korisnika == User.Identity.Name).ToList();

            foreach (var rez in rezervacije)
            {
                db.Rezervacijas.Remove(rez);
                db.SaveChanges();
            }
            return RedirectToAction("PregledIzvedba", new { id = id });
        }
        public ActionResult Obrada(int[] chk, string submitButton)
        {
            int sesija = (int)Session["idizvedbe"];
            if (submitButton == "Rezerviši")
            {
                for (int i = 0; i < chk.Length; i++)
                {
                    Rezervacija rez = new Rezervacija();
                    int id = chk[i];
                    int red = db.Sjedistes.Where(x => x.ID_sjedista == id).Select(x => x.Red_ID_reda).FirstOrDefault();
                    int zona = db.Sjedistes.Where(x => x.ID_sjedista == id).Select(x => x.Red_Zona_ID_zone).FirstOrDefault();
                    int sala = db.Sjedistes.Where(x => x.ID_sjedista == id).Select(x => x.Red_Zona_Sala_ID_sale).FirstOrDefault();

                    rez.Sjediste_ID_sjedista = id;
                    rez.Sjediste_Red_ID_reda = red;
                    rez.Sjediste_Red_Zona_ID_zone = zona;
                    rez.Sjediste_Red_Zona_Sala_ID_sale = sala;
                    rez.Gledalac_ID_korisnika = User.Identity.Name;
                    rez.Izvedba_ID_izvedbe = sesija;
                    rez.naziv_rezervacije = User.Identity.Name;
                    db.Rezervacijas.Add(rez);
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index");
        }
    }
}
