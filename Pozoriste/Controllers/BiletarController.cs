using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pozoriste.Models;
using System.Web.Helpers;
using Pozoriste.ViewModel;

namespace Pozoriste.Controllers
{
    public class BiletarController : Controller
    {
        private bazaPozoriste2Entities5 db = new bazaPozoriste2Entities5();
        //
        // GET: /Biletar/
        public ActionResult Index(String id)
        {
            Pozoriste.ViewModel.ProfilBiletara profil = new ViewModel.ProfilBiletara();
            //osnovni podaci
            var korisnik = db.Korisniks.Find(id);
            if (korisnik == null)
                korisnik = db.Korisniks.Find(User.Identity.Name);
            profil.ID_korisnika = korisnik.ID_korisnika;
            profil.Ime = korisnik.Ime;
            profil.Prezime = korisnik.Prezime;
            profil.sifra = korisnik.Sifra;
            profil.datum_zaposlenja = korisnik.Biletar.datum_zaposlenja;

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
        // GET: /Biletar/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Biletar/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Biletar/Create
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
        // GET: /Biletar/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Biletar/Edit/5
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
            if (ModelState.IsValid)
            {
                db.Entry(korisnik).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Biletar/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Biletar/Delete/5
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
        public ActionResult PregledIzvedba(int id)
        {
            Izvedba izvedba = db.Izvedbas.Find(id);
            Session["idizvedbe"] = id;
            Sala sala = db.Salas.Where(x => x.ID_sale == izvedba.Sala_ID_sale).Single();
            //improvizovani dio, vazi samo za jednu zonu, ako ima vise zona ne moze se primijeniti
            ViewBag.brojSjedista = db.Sjedistes.Where(x => x.Red_Zona_Sala_ID_sale == sala.ID_sale).Count();
            ViewBag.brojRedova = db.Reds.Where(x => x.Zona_Sala_ID_sale == sala.ID_sale).Count();
            ViewBag.idizvedbe = id;
            List<Zona> listaZona = db.Zonas.Where(x => x.Sala_ID_sale == sala.ID_sale).ToList();
            List<Rezervacija> listaRez = db.Rezervacijas.Where(x => x.Izvedba_ID_izvedbe == id).ToList();
            //List<Karta> listaKar = db.Kartas.Where(x => x.Izvedba_ID_izvedbe == id).ToList();

            List<Sjediste> listaKar = db.Izvedbas.FirstOrDefault(x => x.ID_izvedbe == izvedba.ID_izvedbe).Sjedistes.ToList();

            var svaRezervisana = (from x in db.Rezervacijas where x.Izvedba_ID_izvedbe == id select x.Sjediste_ID_sjedista);
            var svaSjedista = (from x in db.Sjedistes where x.Red_Zona_Sala_ID_sale == sala.ID_sale select x.ID_sjedista);
            var svaSlobodna = svaSjedista.Except(svaRezervisana);

            var svaProdata = (from x in db.Izvedbas.FirstOrDefault(x => x.ID_izvedbe == izvedba.ID_izvedbe).Sjedistes select x.ID_sjedista);
            var svaNeprodata = svaSjedista.Except(svaProdata);

            List<int> listaSlobodnih = new List<int>();

            foreach (int num in svaSlobodna)
            {
                listaSlobodnih.Add(num);
            }

            List<int> listaNeprodatih = new List<int>();

            foreach (int num in svaNeprodata)
            {
                listaNeprodatih.Add(num);
            }
            ViewBag.ListaSlobodnih = listaSlobodnih;
            ViewBag.ListaNeprodatih = listaNeprodatih;
            ViewBag.izvedba = izvedba.ID_izvedbe;



            var torka = new Tuple<List<Zona>, List<Rezervacija>, List<Sjediste>>(listaZona, listaRez, listaKar);

            return View(torka);
        }

        public ActionResult ObradaRezervacije(int[] chk, string submitButton)
        {
            int sesija = (int)Session["idizvedbe"];

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
                    
                    rez.Izvedba_ID_izvedbe = sesija;
                    rez.naziv_rezervacije = User.Identity.Name;
                    db.Rezervacijas.Add(rez);
                    db.SaveChanges();

                }
                return RedirectToAction("PregledIzvedba", new {id=sesija});

        }


        public ActionResult ObradaProdaje(int[] chk, string submitButton) {
            int sesija = (int)Session["idizvedbe"];
            for (int i = 0; i < chk.Length; i++)
            {
                int id = chk[i];
                Sjediste sjed = db.Sjedistes.Where(x => x.ID_sjedista == id).Single();
                db.Izvedbas.FirstOrDefault(x => x.ID_izvedbe == sesija).Sjedistes.Add(sjed);
                db.SaveChanges();

            }

            return RedirectToAction("PregledIzvedba", new { id = sesija });
        }

        public ActionResult ProdajaIzvedba(int id, int id2)
        {
            //var rez = db.Rezervacijas.Where(x => x.Izvedba_ID_izvedbe == id).Where(x => x.Sjediste_ID_sjedista == id2).Single();
            
            Sjediste sjed = db.Sjedistes.Where(x => x.ID_sjedista == id2).Single();
            db.Izvedbas.FirstOrDefault(x => x.ID_izvedbe == id).Sjedistes.Add(sjed);
            db.SaveChanges();
            return RedirectToAction("PregledIzvedba", new { id = id });
        }

        public ActionResult PonistiRezervacija(int id)
        {
            //improvizovano - razdvoji 2 nule za ovaj drugi id
            String idstring = id.ToString();
            String idizvedbe = null;
            String idsjedista = null;
            int counter = 0;
            int rezim = 0;
            foreach (char c in idstring)
            {
                if (rezim == 0)
                {

                    if (counter == 2)
                    {
                        if (c == '0')
                        {
                            idizvedbe += c;
                            rezim++;
                        }
                        idsjedista += c;
                        rezim++;
                    }
                    else
                    {
                        if (c == '0')
                        {
                            counter++;
                        }
                        else idizvedbe += c;
                    }
                }
                else
                    if (idsjedista == null && c == '0')
                    {
                        idizvedbe += c;
                    }
                    else
                        idsjedista += c;
            }
            var a = Int32.Parse(idizvedbe);
            var b = Int32.Parse(idsjedista);
            var rez = db.Rezervacijas.Where(x => x.Izvedba_ID_izvedbe == a).Where(x => x.Sjediste_ID_sjedista == b).Single();
            db.Rezervacijas.Remove(rez);
            db.SaveChanges();
            if (db.Gledalacs.Where(x => x.ID_korisnika == User.Identity.Name).Count() > 0)
                return RedirectToAction("Index", "Gledalac");
            else return RedirectToAction("PregledIzvedba", new { id = a });
        }


        public ActionResult IstorijaRez()
        {
            List<Rezervacija> rezervacije = db.Rezervacijas.ToList();
            List<Pozoriste.ViewModel.Karta> listaKar = new List<ViewModel.Karta>();
            List<Izvedba> izvedbe = db.Izvedbas.ToList();
            
        
            foreach (var item in izvedbe)
            {
                List<Sjediste> listaSjed = db.Izvedbas.FirstOrDefault(x => x.ID_izvedbe == item.ID_izvedbe).Sjedistes.ToList();
                foreach (var item2 in listaSjed) {
                    Pozoriste.ViewModel.Karta karta = new Pozoriste.ViewModel.Karta();
                    karta.Izvedba_ID_izvedbe = item.ID_izvedbe;
                    karta.Naziv = item.Predstava.Naziv_Predstave;
                    karta.Sjediste_ID_sjedista = item2.ID_sjedista;
                    karta.Sjediste_Red_ID_reda = item2.Red_ID_reda;
                    karta.Sjediste_Red_Zona_ID_zone = item2.Red_Zona_ID_zone;
                    karta.Sjediste_Red_Zona_Sala_ID_sale = item2.Red_Zona_Sala_ID_sale;
                    listaKar.Add(karta);
                }
                
            }
            var torka = new Tuple<List<Pozoriste.ViewModel.Karta>, List<Rezervacija>>(listaKar, rezervacije);
            return View(torka);
        }

        private Chart GetChart()
        {
            List<String> nazivi = new List<String>();
            List<int> ucestalosti = new List<int>();

            List<Predstava> predstave = db.Predstavas.ToList();
            int ucestalost=0;
            foreach (var item in predstave)
            {
                if (Session["stanje"] as string == "rezervacija")
                {
                   ucestalost = db.Rezervacijas.Where(x => x.Izvedba.Predstava_ID_Predstave == item.ID_Predstave).Count();
                }
                else {
                    ucestalost = 0;
                    foreach (var item2 in db.Izvedbas.ToList())
                    {
                        if (item2.Predstava_ID_Predstave == item.ID_Predstave)
                            ucestalost += db.Izvedbas.FirstOrDefault(x => x.ID_izvedbe == item2.ID_izvedbe).Sjedistes.Count();
                    }
                    
                }
                if (ucestalost > 0)
                {
                    nazivi.Add(item.Naziv_Predstave);
                    ucestalosti.Add(ucestalost);
                }
            }
            nazivi.ToArray();
            ucestalosti.ToArray();
            String naziv;
            if (Session["stanje"] == "rezervacija") naziv = "Broj rezervacija po predstavi";
            else naziv = "Broj prodatih karata po predstavi";

            return new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
             .AddTitle(naziv)
            .AddSeries(
                    chartType: "column",
                    name: "StudyType",
                    xValue: nazivi,
                        yValues: ucestalosti);
             
        }

        public ActionResult GrafikRezervacija() {
            Session["stanje"] = "rezervacija";
            var model = new ChartViewModel
            {
                Chart = GetChart()
            };
            return View(model);
        }

        public ActionResult GrafikProdaja()
        {
            Session["stanje"] = "prodaja";
            var model = new ChartViewModel
            {
                Chart = GetChart()
            };
            return View(model);
        }

        /*    public ActionResult PregledIzvedbaPost(int id, int id_izvedbe)
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
        rez.Gledalac_ID_korisnika = User.Identity.Name;
        rez.Izvedba_ID_izvedbe = id_izvedbe;
        rez.naziv_rezervacije = User.Identity.Name;
        db.Rezervacijas.Add(rez);
        db.SaveChanges();
        return RedirectToAction("Index");
    }*/
    }
}