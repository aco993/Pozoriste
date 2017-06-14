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

        //
        // GET: /Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Create
        public ActionResult CreateBiletar()
        {

            return View();
        }
        [HttpGet]
        public ActionResult DetaljiSala(int id)
        {
            var sala = db.Salas.Find(id);
            ViewBag.brojSjedista = db.Sjedistes.Where(x => x.Red_Zona_Sala_ID_sale == id).Count();
            ViewBag.brojRedova = db.Reds.Where(x => x.Zona_Sala_ID_sale == id).Count();
            List<Zona> listaZona = db.Zonas.Where(x => x.Sala_ID_sale == sala.ID_sale).ToList();
            return View(listaZona);
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



        //
        // GET: /Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Edit/5
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

        //
        // GET: /Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Delete/5
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

 /*           var id = TempData["idpredstave"];
            TempData["idpredstave1"] = TempData["idpredstave"];

            List<Glumac> glumciLista = db.Glumacs.ToList();

            var model = new DodavanjeGlumaca()
                        {
                            selektovaniGlumci = new[] { "1" },
                            glumci = glumciLista.Select(x => new SelectListItem
                            {
                                Value = x.ID_glumca.ToString(),
                                Text = x.Ime_glumca + " " + x.Prezime_glumca
                            })
                        };*/

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
            //srediti
     /*       List<Glumac> glumciLista = db.Glumacs.ToList();
            foreach (var item in lista.selektovaniGlumci)
            {
                var idpredstave = TempData["idpredstave1"];
                int i = Int32.Parse(item) - 1;
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == (int)idpredstave).Glumacs.Add(glumciLista[i]);
                db.SaveChanges();
            }
            return RedirectToAction("UbaciReditelje");*/
        }

        public ActionResult UbaciReditelje()
        {

            var id = TempData["idpredstave1"];
            TempData["idpredstave2"] = TempData["idpredstave1"];

            List<Reditelj> rediteljiLista = db.Rediteljs.ToList();

            var model = new DodavanjeReditelja()
            {
                selektovaniReditelji = new[] { "1" },
                reditelji = rediteljiLista.Select(x => new SelectListItem
                {
                    Value = x.ID_reditelja.ToString(),
                    Text = x.Ime_reditelja + " " + x.Prezime_reditelja
                })
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult UbaciReditelje(DodavanjeReditelja lista)
        {
            //srediti
            List<Reditelj> rediteljiLista = db.Rediteljs.ToList();
            foreach (var item in lista.selektovaniReditelji)
            {
                var idpredstave = TempData["idpredstave2"];
                int i = Int32.Parse(item) - 1;
                db.Predstavas.FirstOrDefault(x => x.ID_Predstave == (int)idpredstave).Rediteljs.Add(rediteljiLista[i]);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
            db.SaveChanges();
            TempData["cjenovnik"] = cjenovnik;
            TempData["idcjenovnika"] = cjenovnika;
            List<Cijena_zone> cjenZone = new List<Cijena_zone>();
            
            foreach (var item in zona)
            {
                Cijena_zone cjen_zona = new Cijena_zone(0, item.ID_zone, item.Sala_ID_sale, cjenovnik.id_cjenovnika);
                Zona zonica = db.Zonas.Where(x => x.ID_zone == item.ID_zone).Single();
                cjen_zona.Zona = zonica;
                cjenZone.Add(cjen_zona);
            }
            
            return View(cjenZone);
        }
        [HttpPost]
        public ActionResult DodajCjenovnik(List<Cijena_zone> cjenovnici)
        {
            foreach (Cijena_zone cjenovnik in cjenovnici)
            {
                Cijena_zone cjen = new Cijena_zone();
                cjen.Zona_ID_zone = db.Zonas.Where(x => x.Naziv_zone == cjenovnik.Zona.Naziv_zone).Select(x => x.ID_zone).Single();
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

        /*       [HttpGet]
       public ActionResult CreateGlumac()
       {
           return View();
       }
       [HttpPost]
       public ActionResult CreateGlumac(Glumac glumac)
       {

           ModelState.Remove("ID_glumca");
           if (ModelState.IsValid)
           {
               glumac.ID_glumca = db.Glumacs.Count() + 1; //TODO ovo treba srediti
               db.Glumacs.Add(glumac);
               db.SaveChanges();
               return RedirectToAction("Index");
           }
           return RedirectToAction("Index");
       }


       [HttpGet]
       public ActionResult CreateReditelj()
       {
           return View();
       }
       [HttpPost]
       public ActionResult CreateReditelj(Reditelj reditelj)
       {

           ModelState.Remove("ID_reditelja");
           if (ModelState.IsValid)
           {
               reditelj.ID_reditelja = db.Rediteljs.Count() + 1; //TODO ovo treba srediti
               db.Rediteljs.Add(reditelj);
               db.SaveChanges();
               return RedirectToAction("Index");
           }
           return RedirectToAction("Index");
       }
       [HttpGet]
       public ActionResult CreateOrganizator()
       {
           return View();
       }
       [HttpPost]
       public ActionResult CreateOrganizator(Organizator organizator)
       {

           ModelState.Remove("ID_organizatora");
           if (ModelState.IsValid)
           {
               organizator.ID_organizatora = db.Organizators.Count() + 1; //TODO ovo treba srediti
               db.Organizators.Add(organizator);
               db.SaveChanges();
               return RedirectToAction("Index");
           }
           return RedirectToAction("Index");
       }
      */

    }
}


