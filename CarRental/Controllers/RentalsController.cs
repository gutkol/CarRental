using CarRental.Database;
using CarRental.Models;
using CarRental.Obiekt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class RentalsController : Controller
    {
        [HttpGet]
        public ActionResult GetOrder()
        {
            SamochodyKontener rentalmodel = (SamochodyKontener)Session["rentalmodel"];
            CarRentalEntities CRentities = new CarRentalEntities();

            var all = CRentities.Samochody.ToList();

            rentalmodel.ListAut = all;

            ViewBag.ListaAut = all;

            return View(rentalmodel);
        }

        [HttpPost]
        public ActionResult GetOrder(int id)
        {
            SamochodyKontener rentalmodel = (SamochodyKontener)Session["rentalmodel"];

            var login = Session["Login"].ToString();
            using (var dc = new CarRentalEntities())
            {
                if(login == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                var konto = (from a in dc.Konta
                             where a.login == login
                             select a).FirstOrDefault();

                var identyfikator = (from b in dc.Klienci
                                     where b.id_konta_kl == konto.id_konta
                                     select b).FirstOrDefault();

                dc.Wypozyczenia.Add(new Wypozyczenia()
                {
                    id_miejsca_wyp = rentalmodel.rentalmodelkontener.IDMiejsceWyp,
                    id_miejsca_odd = rentalmodel.rentalmodelkontener.IDMiejsceOdd,
                    id_samochodu = id,
                    id_klienta = identyfikator.id_klienta,
                    id_pracownika = 1,
                    data_wyp = rentalmodel.rentalmodelkontener.DataWyp,
                    data_odd = rentalmodel.rentalmodelkontener.DataOdd,
                    koszt = (rentalmodel.rentalmodelkontener.DataOdd - rentalmodel.rentalmodelkontener.DataWyp).Days * 290
                });
                dc.SaveChanges(); 
            }

            CarRentalEntities CREntities = new CarRentalEntities();

            Samochody samochod = CREntities.Samochody.Find(id);

            ViewBag.Message = samochod.Marka.marka1 + " " + samochod.Model.model1;

            return View("Ordered");
        }



    }
}