using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Pozoriste.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Globalization;
using System.Web.Helpers;

namespace Pozoriste.Controllers
{
    public class AdminController : Controller
    {
        private bazaPozoriste2Entities5 db = new bazaPozoriste2Entities5();

        public AdminController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        //
        // GET: /Admin/
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public ActionResult Index()
        {
            Pozoriste.ViewModel.ProfilAdmina profil = new ViewModel.ProfilAdmina();

            //sale
            profil.sale = new List<Sala>();
            var saleLsta = db.Salas.Select(x => x.ID_sale).ToList();
            foreach (var item in saleLsta)
            {
                profil.sale.Add(db.Salas.Where(x => x.ID_sale == item).Single());
            }

            //biletari
            profil.biletari = new List<Biletar>();
            var bilLista = db.Biletars.Select(x => x.ID_korisnika).ToList();
            foreach (var item in bilLista)
            {
                profil.biletari.Add(db.Biletars.Where(x => x.ID_korisnika == item).Single());
            }
            //predstave
            profil.predstave = new List<Predstava>();
            var predstLista = db.Predstavas.Select(x => x.ID_Predstave).ToList();
            foreach (var predstava in predstLista)
            {
                profil.predstave.Add(db.Predstavas.Where(x => x.ID_Predstave == predstava).Single());
            }
            //repertoari
            profil.repertoari = new List<Repertoar>();
            var repertoariLista = db.Repertoars.Select(x => x.Datum).ToList();
            foreach (var repertoar in repertoariLista)
            {
                profil.repertoari.Add(db.Repertoars.Where(x => x.Datum == repertoar).Single());
            }

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
            //reditelji
            profil.reditelji = new List<Reditelj>();
            var redLista = db.Rediteljs.Select(x => x.ID_reditelja).ToList();
            foreach (var red in redLista)
            {
                profil.reditelji.Add(db.Rediteljs.Where(x => x.ID_reditelja == red).Single());
            }

            //glumci
            profil.glumci = new List<Glumac>();
            var glumLista = db.Glumacs.Select(x => x.ID_glumca).ToList();
            foreach (var glumac in glumLista)
            {
                profil.glumci.Add(db.Glumacs.Where(x => x.ID_glumca == glumac).Single());
            }
            //organizatori
            profil.organizatori = new List<Organizator>();
            var organLista = db.Organizators.Select(x => x.ID_organizatora).ToList();
            foreach (var organ in organLista)
            {
                profil.organizatori.Add(db.Organizators.Where(x => x.ID_organizatora == organ).Single());
            }

            return View(profil);
        }

        [HttpGet]
        public ActionResult DetaljiSala(int id)
        {
            Session["sala"] = id;
            var sala = db.Salas.Find(id);
            ViewBag.brojSjedista = db.Sjedistes.Where(x => x.Red_Zona_Sala_ID_sale == id).Count();
            ViewBag.brojRedova = db.Reds.Where(x => x.Zona_Sala_ID_sale == id).Count();
            int brojSjedista = db.Sjedistes.Where(x => x.Red_Zona_Sala_ID_sale == id).Count();
            int brojRedova = db.Reds.Where(x => x.Zona_Sala_ID_sale == id).Count();
            List<Zona> listaZona = db.Zonas.Where(x => x.Sala_ID_sale == sala.ID_sale).ToList();
            List<ViewModel.ZonaUSali> listaZonaUSali = new List<ViewModel.ZonaUSali>();
            foreach (var item in listaZona)
            {
                ViewModel.ZonaUSali zona = new ViewModel.ZonaUSali();
                zona.Sala_ID_sale = id;
                zona.brojSjedista = db.Sjedistes.Where(x => x.Red_Zona_Sala_ID_sale == id).Where(x => x.Red_Zona_ID_zone == item.ID_zone).Count();
                zona.brojRedova = db.Reds.Where(x => x.Zona_Sala_ID_sale == id).Where(x=>x.Zona_ID_zone==item.ID_zone).Count();
                zona.Naziv_zone = item.Naziv_zone;
                zona.ID_zone = item.ID_zone;
                listaZonaUSali.Add(zona);
            }
            return View(listaZonaUSali);
        }
        //
        //GET: /Admin/CreateBiletar
        [HttpGet]
        public ActionResult CreateBiletar()
        {
            return View();
        }

        //
        // POST: /Admin/Create
        [HttpPost]
        public async Task<ActionResult> CreateBiletar(Biletar biletar)
        {
            biletar.datum_zaposlenja = System.DateTime.Now;

            try
            {
                Korisnik kor = new Korisnik();
                Biletar bil = new Biletar();

                var user = new ApplicationUser() { UserName = biletar.ID_korisnika };
                var result = await UserManager.CreateAsync(user, biletar.Korisnik.Sifra);

                // TODO: Add insert logic here
                kor.ID_korisnika = biletar.ID_korisnika;
                kor.Sifra = biletar.Korisnik.Sifra;
                kor.Ime = biletar.Korisnik.Ime;
                kor.Prezime = biletar.Korisnik.Prezime;
                bil.ID_korisnika = biletar.ID_korisnika;
                bil.datum_zaposlenja = biletar.datum_zaposlenja;

                ModelState.Remove("datum_zaposlenja");

                if (ModelState.IsValid)
                {
                    db.Korisniks.Add(kor);
                    db.Biletars.Add(bil);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
        }

        //GET: /Create/Sala
        [HttpGet]
        public ActionResult CreateSala()
        {
            return View();
        }

        //POST: /Create/Sala
        [HttpPost]
        public ActionResult CreateSala(Sala sala)
        {
            ModelState.Remove("ID_sale");
            if (ModelState.IsValid)
            {
                sala.ID_sale = db.Salas.Max(x => x.ID_sale)+1;
                db.Salas.Add(sala);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //
        // GET: /Admin/Create
        [HttpGet]
        public ActionResult CreatePredstava()
        {
           var organizatori = db.Organizators.Select(x => x.Naziv_organizatora);
            var zanrovi = db.Zanrs.Select(x => x.Naziv_zanra);
            ViewBag.organizatori = organizatori;
            ViewBag.zanrovi = zanrovi;

            return View();
        }

        //
        // POST: /Admin/Create
        [HttpPost]
        public ActionResult CreatePredstava(Predstava predstava)
        {

            try
            {
                var nazivOrganizatora = predstava.Organizator.Naziv_organizatora;
                var nazivZanra = predstava.Zanr.Naziv_zanra;

                var org = db.Organizators.Where(x => x.Naziv_organizatora == nazivOrganizatora).Single();
                var zanr = db.Zanrs.Where(x => x.Naziv_zanra == nazivZanra).Single();
                // TODO: Add insert logic here
                predstava.Zanr_ID_zanra = zanr.ID_zanra;
                predstava.Organizator_ID_organizatora = org.ID_organizatora;
                predstava.Zanr = zanr;
                predstava.Organizator = org;

                ModelState.Remove("ID_Predstave");

                if (ModelState.IsValid)
                {
                    predstava.ID_Predstave = db.Predstavas.Max(x => x.ID_Predstave)+1; //TODO ovo treba srediti
                    TempData["idpredstave"] = predstava.ID_Predstave;
                    db.Predstavas.Add(predstava);
                    db.SaveChanges();
 
                    predstava.File.SaveAs(Server.MapPath("~/Images/")+predstava.ID_Predstave+".jpg");

                    WebImage img = new WebImage(Server.MapPath("~/Images/")+predstava.ID_Predstave+".jpg");
                    if (img.Width > 250)
                        img.Resize(250,250);
                    img.Save(Server.MapPath("~/Images/") + predstava.ID_Predstave + ".jpg");
                    return RedirectToAction("UbaciGlumce");
                }


                return RedirectToAction("Index");
            }
            catch
            {

                return View();
            }
        }
        [HttpGet]
        public ActionResult UbaciGlumce()
        {

            ViewModel.DodavanjePredstave dp = new ViewModel.DodavanjePredstave();
            dp.dostupniGlumci = db.Glumacs.ToList();
            dp.izabraniGlumci = new List<Glumac>();
            dp.dostupniReditelji = db.Rediteljs.ToList();
            dp.izabraniReditelji = new List<Reditelj>();
            int idpredstave = (int)TempData["idpredstave"];
            Session["idPredstave"] = idpredstave;

            foreach (var item in dp.dostupniGlumci)
            {
                item.Ime_glumca = item.Ime_glumca + " " + item.Prezime_glumca;
            }

            foreach (var item in dp.dostupniReditelji)
            {
                item.Ime_reditelja = item.Ime_reditelja +" "+ item.Prezime_reditelja;
            }

            return View(dp);
        }

        [HttpPost]
        public ActionResult UbaciGlumce(ViewModel.DodavanjePredstave model, string add, string remove,string add2, string remove2)
        {
            int idp = (int)Session["idpredstave"];
         var glumci = db.Predstavas.FirstOrDefault(x => x.ID_Predstave ==idp ).Glumacs.ToList();
         var reditelji = db.Predstavas.FirstOrDefault(x => x.ID_Predstave == idp).Rediteljs.ToList();
            if (glumci.Count()==0)
            {    
               
                model.izabraniGlumci = new List<Glumac>();
            }
          else
          { model.izabraniGlumci = glumci.ToList(); }
            if (reditelji.Count() == 0)
            {

                model.izabraniReditelji = new List<Reditelj>();
            }
            else
            { model.izabraniReditelji = reditelji.ToList(); }


            if (!string.IsNullOrEmpty(add))
            {
                int idg = model.selektovaniDostupniGlumci[0];
                Glumac g = db.Glumacs.Find(idg);
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == idp).Glumacs.Add(g);
                db.SaveChanges();
                model.izabraniGlumci.Add(g);
                foreach (var item in model.izabraniGlumci)
                {
                    item.Ime_glumca = item.Ime_glumca + " " + item.Prezime_glumca;
                }
            }
            else if (!string.IsNullOrEmpty(remove)) {
                int idg = model.selektovaniIzabraniGlumci[0];
                Glumac g = db.Glumacs.Find(idg);
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == idp).Glumacs.Remove(g);
                db.SaveChanges();
                model.izabraniGlumci.Remove(g);
            }
            if (!string.IsNullOrEmpty(add2))
            {
                int idg = model.selektovaniDostupniReditelji[0];
                Reditelj r = db.Rediteljs.Find(idg);
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == idp).Rediteljs.Add(r);
                db.SaveChanges();
                model.izabraniReditelji.Add(r);
                foreach (var item in model.izabraniReditelji)
                {
                    item.Ime_reditelja = item.Ime_reditelja + " " + item.Prezime_reditelja;
                }
            }
            else if (!string.IsNullOrEmpty(remove2))
            {
                int idg = model.selektovaniIzabraniReditelji[0];
                Reditelj r = db.Rediteljs.Find(idg);
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == idp).Rediteljs.Remove(r);
                db.SaveChanges();
                model.izabraniReditelji.Remove(r);
            }
            IEnumerable<Glumac> svi = db.Glumacs.ToList();
            IEnumerable<Glumac> sviOsim = svi.Except(model.izabraniGlumci);
            foreach (var item in sviOsim)
            {
                item.Ime_glumca = item.Ime_glumca + " " + item.Prezime_glumca;
            }
            model.dostupniGlumci = sviOsim.ToList();
            IEnumerable<Reditelj> sviR = db.Rediteljs.ToList();
            IEnumerable<Reditelj> sviOsimR = sviR.Except(model.izabraniReditelji);
            foreach (var item in sviOsimR)
            {
                item.Ime_reditelja = item.Ime_reditelja +" "+ item.Prezime_reditelja;
            }
            model.dostupniReditelji = sviOsimR.ToList();


    return View(model);
        }

        [HttpGet]
        public ActionResult CreateIzvedba()
        {

            var sale = db.Salas.Select(x => x.Naziv_Sale);
            var predstave = db.Predstavas.Select(x => x.Naziv_Predstave);
            ViewBag.sale = sale;
            ViewBag.predstave = predstave;

            return View();
        }

        [HttpPost]
        public ActionResult CreateIzvedba(Izvedba izvedba)
        {
            int rep = db.Repertoars.Where(x => x.Datum == izvedba.Repertoar_Datum).Count();
            var repdat = new Repertoar();

            if (rep == 0)
            {
                repdat.Datum = izvedba.Repertoar_Datum;
                db.Repertoars.Add(repdat);
                db.SaveChanges();
            }
            var predstava = db.Predstavas.Where(x => x.Naziv_Predstave == izvedba.Predstava.Naziv_Predstave).Single();
            var sala = db.Salas.Where(x => x.Naziv_Sale == izvedba.Sala.Naziv_Sale).Single();


            izvedba.Sala_ID_sale = sala.ID_sale;
            izvedba.Predstava_ID_Predstave = predstava.ID_Predstave;
            izvedba.Sala = sala;
            izvedba.Predstava = predstava;

            ModelState.Remove("ID_izvedbe");
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    db.IntervaliIzvedbe(predstava.ID_Predstave, izvedba.Sala_ID_sale, izvedba.Repertoar_Datum, izvedba.Vrijeme_izvodjenja);
                    //db.Izvedbas.Add(izvedba);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View("~/Views/Admin/NeuspeloDodavanjeIzvedbe.cshtml");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DodajCjenovnik(int id)
        {
            TempData["id"] = id;
            Izvedba izvedba = db.Izvedbas.Find(id);
            var sala = izvedba.Sala_ID_sale;
            var zona = db.Zonas.Where(x => x.Sala_ID_sale == sala).ToList();
            ViewBag.broj = zona.Count;
            int cjenovnika = db.Cjenovniks.Count()+1;
            TempData["izvedba"] = id;
            Cjenovnik cjenovnik = new Cjenovnik(cjenovnika, DateTime.Now, izvedba.Repertoar_Datum, izvedba.ID_izvedbe);
            int stari = db.Cjenovniks.Where(x => x.Izvedba_ID_izvedbe == id).Max(x => x.id_cjenovnika).FirstOrDefault();
            db.SaveChanges();
            TempData["cjenovnik"] = cjenovnik;
            TempData["idcjenovnika"] = cjenovnika;
            List<Cijena_zone> cjenZone = new List<Cijena_zone>();
            
            foreach (var item in zona)
            {
                Zona zonica = db.Zonas.Where(x => x.ID_zone == item.ID_zone).Single();
                if (stari > 0)
                {
                    int stara_cijena = db.Cijena_zone.Where(x => x.Cjenovnik_id_cjenovnika == stari).Where(x=>x.Zona_ID_zone==item.ID_zone).Select(x=>x.Cijena).Single();
                    Cijena_zone cjen_zona = new Cijena_zone(stara_cijena, item.ID_zone, item.Sala_ID_sale, cjenovnik.id_cjenovnika);
                    cjen_zona.Zona = zonica;
                    cjenZone.Add(cjen_zona);
                }
                else
                {
                    Cijena_zone cjen_zona = new Cijena_zone(0, item.ID_zone, item.Sala_ID_sale, cjenovnik.id_cjenovnika);
                    cjen_zona.Zona = zonica;
                    cjenZone.Add(cjen_zona);
                }
                
            }
            
            return View(cjenZone);
        }
        [HttpPost]
        public ActionResult DodajCjenovnik(List<Cijena_zone> cjenovnici)
        {
            foreach (Cijena_zone cjenovnik in cjenovnici)
            {
                int idizvedbe = (int)TempData["izvedba"];
                int idsale=db.Izvedbas.Where(x=>x.ID_izvedbe==idizvedbe).Select(x=>x.Sala_ID_sale).Single();
                Cijena_zone cjen = new Cijena_zone();
                cjen.Zona_ID_zone = db.Zonas.Where(x => x.Naziv_zone == cjenovnik.Zona.Naziv_zone).Where(x=>x.Sala_ID_sale==idsale).Select(x => x.ID_zone).Single();
                cjen.Cijena = cjenovnik.Cijena;
                cjen.Zona_Sala_ID_sale = db.Zonas.Where(x => x.ID_zone == cjen.Zona_ID_zone).Select(x => x.Sala_ID_sale).Single();
                //cjen.Zona_Sala_ID_sale = db.Zonas.Where(x => x.ID_zone == cjenovnik.Zona_ID_zone).Select(x => x.Sala_ID_sale).Single();
                cjen.Cjenovnik_id_cjenovnika = (int)TempData["idcjenovnika"];
                Zona zona = db.Zonas.Where(x => x.ID_zone == cjen.Zona_ID_zone).Single();
                Cjenovnik cjeen = TempData["cjenovnik"] as Cjenovnik;
                cjen.Zona = zona;
                cjen.Cjenovnik = cjeen;
                db.Cijena_zone.Add(cjen);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ObrisiBiletar(String id) {
            Biletar bil = db.Biletars.Find(id);
            db.Biletars.Remove(bil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditPredstava(int id)
        {
            var organizatori = db.Organizators.Select(x => x.Naziv_organizatora);
            var zanrovi = db.Zanrs.Select(x => x.Naziv_zanra);
            ViewBag.organizatori = organizatori;
            ViewBag.zanrovi = zanrovi;
            Predstava pred = db.Predstavas.Find(id);

            return View(pred);
        }

        [HttpPost]
        public ActionResult EditPredstava(Predstava pred)
        {
            Zanr zanr = db.Zanrs.Where(x => x.Naziv_zanra == pred.Zanr.Naziv_zanra).Single();
            Organizator org = db.Organizators.Where(x => x.Naziv_organizatora == pred.Organizator.Naziv_organizatora).Single();
            pred.Zanr_ID_zanra = zanr.ID_zanra;
            pred.Organizator_ID_organizatora = org.ID_organizatora;
            pred.Zanr = zanr;
            pred.Organizator = org;
            if (ModelState.IsValid)
            {
                db.Entry(pred).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditIzvedba(int id)
        {
            var sale = db.Salas.Select(x => x.Naziv_Sale);
            var predstave = db.Predstavas.Select(x => x.Naziv_Predstave);
            ViewBag.sale = sale;
            ViewBag.predstave = predstave;
            Izvedba izv = db.Izvedbas.Find(id);

            return View(izv);
        }

        [HttpPost]
        public ActionResult EditIzvedba(Izvedba izvedba)
        {
            int rep = db.Repertoars.Where(x => x.Datum == izvedba.Repertoar.Datum).Count();
            var repdat = new Repertoar();

            if (rep == 0)
            {
                repdat.Datum = izvedba.Repertoar.Datum;
                db.Repertoars.Add(repdat);
                db.SaveChanges();
            }
            var repertoar = db.Repertoars.Find(izvedba.Repertoar.Datum);
            var predstava = db.Predstavas.Find(izvedba.Predstava_ID_Predstave);
            var sala = db.Salas.Find(izvedba.Sala_ID_sale);


            izvedba.Repertoar_Datum = izvedba.Repertoar.Datum;
                izvedba.Repertoar = repertoar;
            izvedba.Sala = sala;
            izvedba.Predstava = predstava;
            if (ModelState.IsValid)
            {
                db.Entry(izvedba).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult ObrisiPredstava(int id)
        {
            Predstava pred = db.Predstavas.Find(id);

            List<Glumac> glumci = db.Predstavas.FirstOrDefault(x => x.ID_Predstave == id).Glumacs.ToList();
            foreach (var item in glumci) {
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == id).Glumacs.Remove(item);
            }
            List<Reditelj> reditelji = db.Predstavas.FirstOrDefault(x => x.ID_Predstave == id).Rediteljs.ToList();
            foreach (var item in reditelji)
            {
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == id).Rediteljs.Remove(item);
            }
            db.Predstavas.Remove(pred);
            //mozda i recenzija
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ObrisiIzvedba(int id)
        {
            Izvedba izv = db.Izvedbas.Find(id);
            db.Izvedbas.Remove(izv);
            List<Rezervacija> rezervacije = db.Rezervacijas.Where(x => x.Izvedba_ID_izvedbe==id).ToList();
            foreach (var item in rezervacije)
            {
                db.Rezervacijas.Remove(item);
                }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CreateZona() {
            int idsale = (int)Session["sala"];
            int brojzona = db.Zonas.Where(x => x.Sala_ID_sale == idsale).Count();
            ViewBag.brojzona = brojzona;
            return View();        
        }
        [HttpPost]
        public ActionResult CreateZona(Zona zona, String redova,String sjedista) {
            //TODO: mozda odraditi preko procedure
            int idsale = (int)Session["sala"];
            Sala sala = db.Salas.Find(idsale);
            int maxr;
            int maxs;
            int kolona;
            int r = Int32.Parse(redova);
            maxr = db.Reds.Where(x => x.Zona_Sala_ID_sale == idsale).Count();
            
            if (sjedista==null) {
            maxs = db.Sjedistes.Where(x => x.Red_Zona_Sala_ID_sale == idsale).Max(x => x.ID_sjedista); }
            else { 
                maxs=Int32.Parse(sjedista); }
            if (maxr == 0) { kolona = maxs/r;  }
            else { kolona = maxs /maxr; }
            zona.Sala_ID_sale = idsale;
            zona.ID_zone = db.Zonas.Max(x => x.ID_zone)+1;
            zona.Sala = sala;
            db.Zonas.Add(zona);
            db.SaveChanges();
            for (int i = 0; i < r; i++)
            {
                Red red = new Red();
                red.ID_reda = maxr + i+1;
                red.Zona_ID_zone = zona.ID_zone;
                red.Zona_Sala_ID_sale = idsale;
                db.Reds.Add(red);
                db.SaveChanges();
                for (int j=0; j<kolona; j++)
                {
                Sjediste sjed= new Sjediste();
                if (maxr == 0) { sjed.ID_sjedista = i * kolona + j+1; }
                else { sjed.ID_sjedista = maxs + i * kolona + j+1; }
                sjed.Red_ID_reda = red.ID_reda;
                sjed.Red_Zona_ID_zone = zona.ID_zone;
                sjed.Red_Zona_Sala_ID_sale = idsale;
                db.Sjedistes.Add(sjed);
                db.SaveChanges();
                }
            }
              

            return RedirectToAction("DetaljiSala", new {id=idsale });
        }
    }
}


