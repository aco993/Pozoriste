using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pozoriste.Models;

namespace Pozoriste.Controllers
{
    public class HomeController : Controller
    {
        private bazaPozoriste2Entities5 db = new bazaPozoriste2Entities5();
        public ActionResult Index()
        {
            var korisnik = db.Korisniks.Find(User.Identity.Name);
            if (korisnik != null)
            {
                if (korisnik.Administrator != null)
                {

                    return RedirectToAction("Index", "Admin", new { id = korisnik.ID_korisnika });
                }
                else if (korisnik.Biletar != null)
                {
                    return RedirectToAction("Index", "Biletar", new { id = korisnik.ID_korisnika });
                }
                else 
                    return RedirectToAction("Index", "Gledalac", new { id = korisnik.ID_korisnika });
            }
            else
            {

                Pozoriste.ViewModel.ProfilKorisnika profil = new ViewModel.ProfilKorisnika();
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
                var izvedbeSLista = db.Izvedbas.Where(x => x.Repertoar_Datum <= sedamDana).Select(x => x.ID_izvedbe).ToList();
                foreach (var izvedba in izvedbeSLista)
                {

                    profil.izvedbeS.Add(db.Izvedbas.Where(x => x.ID_izvedbe == izvedba).Single());
                }
                //izvedbe koje ce se odigrati u narednih mjesec dana
                profil.izvedbeM = new List<Izvedba>();
                var izvedbeMLista = db.Izvedbas.Where(x => x.Repertoar_Datum <= mjesecDana).Select(x => x.ID_izvedbe).ToList();
                foreach (var izvedba in izvedbeMLista)
                {

                    profil.izvedbeM.Add(db.Izvedbas.Where(x => x.ID_izvedbe == izvedba).Single());
                }
                return View(profil);
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}