using CarRental.Database;
using CarRental.Models;
using CarRental.Obiekt;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class CarsController : Controller
    {
        private CarRentalEntities CRentities = new CarRentalEntities();

        [HttpGet]
        public ActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCar(CarViewModel addmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(addmodel);
            }
            else
            {
                Marka marka = null;
                Model model = null;
                Wyposazenie wyposazenie = null;
                Typ typ = null;
                Kolor kolor = null;
                Paliwo paliwo = null;

                using (var dc = new CarRentalEntities())
                {
                    marka = (from a in dc.Marka
                             where a.marka1 == addmodel.Marka
                             select a).FirstOrDefault();

                    model = (from b in dc.Model
                             where b.model1 == addmodel.Model
                             select b).FirstOrDefault();

                    wyposazenie = (from c in dc.Wyposazenie
                                   where c.rodzaj_wyp == addmodel.Wyposazenie
                                   select c).FirstOrDefault();

                    typ = (from d in dc.Typ
                           where d.typ1 == addmodel.Typ
                           select d).FirstOrDefault();

                    kolor = (from e in dc.Kolor
                             where e.kolor1 == addmodel.Kolor
                             select e).FirstOrDefault();

                    paliwo = (from f in dc.Paliwo
                              where f.rodzaj_paliwa == addmodel.Paliwo
                              select f).FirstOrDefault();

                    if (marka == null)
                    {
                        dc.Marka.Add(new Marka()
                        {
                            marka1 = addmodel.Marka
                        });
                        dc.SaveChanges();
                    }

                    if (model == null)
                    {
                        dc.Model.Add(new Model()
                        {
                            model1 = addmodel.Model
                        });
                        dc.SaveChanges();
                    }

                    if (wyposazenie == null)
                    {
                        dc.Wyposazenie.Add(new Wyposazenie()
                        {
                            rodzaj_wyp = addmodel.Wyposazenie
                        });
                        dc.SaveChanges();

                    }

                    if (typ == null)
                    {
                        dc.Typ.Add(new Typ()
                        {
                            typ1 = addmodel.Typ
                        });
                        dc.SaveChanges();
                    }

                    if (kolor == null)
                    {
                        dc.Kolor.Add(new Kolor()
                        {
                            kolor1 = addmodel.Kolor
                        });
                        dc.SaveChanges();
                    }

                    if (paliwo == null)
                    {
                        dc.Paliwo.Add(new Paliwo()
                        {
                            rodzaj_paliwa = addmodel.Paliwo
                        });
                        dc.SaveChanges();
                    }

                    marka = (from a in dc.Marka
                             where a.marka1 == addmodel.Marka
                             select a).FirstOrDefault();

                    model = (from b in dc.Model
                             where b.model1 == addmodel.Model
                             select b).FirstOrDefault();

                    wyposazenie = (from c in dc.Wyposazenie
                                   where c.rodzaj_wyp == addmodel.Wyposazenie
                                   select c).FirstOrDefault();

                    typ = (from d in dc.Typ
                           where d.typ1 == addmodel.Typ
                           select d).FirstOrDefault();

                    kolor = (from e in dc.Kolor
                             where e.kolor1 == addmodel.Kolor
                             select e).FirstOrDefault();

                    paliwo = (from f in dc.Paliwo
                              where f.rodzaj_paliwa == addmodel.Paliwo
                              select f).FirstOrDefault();

                    if (marka != null && model != null && wyposazenie != null && typ != null && kolor != null && typ != null)
                    {
                        dc.Samochody.Add(new Samochody()
                        {
                            id_marki = marka.id_marki,
                            id_modelu = model.id_modelu,
                            id_wyposazenia = wyposazenie.id_wyposazenia,
                            id_typu = typ.id_typu,
                            id_koloru = kolor.id_koloru,
                            id_paliwa = paliwo.id_paliwa,
                            rok_prod = addmodel.Rok_produkcji,
                            poj_silnika = addmodel.Pojemnosc_silnika,
                            przebieg = addmodel.Przebieg
                        });
                        dc.SaveChanges();
                    }
                }
                ViewBag.Car = addmodel.Marka + " " + addmodel.Model;
                return View();
            }
        }

        public ActionResult GetCar()
        {
            //SamochodyKontener listaAut = new SamochodyKontener();



            //var all = CRentities.Samochody.ToList();
            //listaAut.ListAut = all;

            //ViewBag.ListaAut = all;

            List<Samochody> samochod = null;

            using (var dc = new CarRentalEntities())
            {
                samochod = dc.Samochody.ToList();
                samochod.ForEach(x => {
                    x.Marka = dc.Marka.Where(z => z.id_marki == x.id_marki).FirstOrDefault();
                    x.Model = dc.Model.Where(z => z.id_modelu == x.id_modelu).FirstOrDefault();
                    x.Typ = dc.Typ.Where(z => z.id_typu == x.id_typu).FirstOrDefault();
                    x.Paliwo = dc.Paliwo.Where(z => z.id_paliwa == x.id_paliwa).FirstOrDefault();
                    x.Wyposazenie = dc.Wyposazenie.Where(z => z.id_wyposazenia == x.id_wyposazenia).FirstOrDefault();
                    x.Kolor = dc.Kolor.Where(z => z.id_koloru == x.id_koloru).FirstOrDefault();
                });
            }
            var result = samochod;
                return View(result);
        }

        [HttpGet]
        public ActionResult EditCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Samochody samochod = CRentities.Samochody.Find(id);

            if (samochod == null)
            {
                return HttpNotFound();
            }
            return View(samochod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCar(Samochody samochod)
        {
            if (ModelState.IsValid)
            {
                CRentities.Entry(samochod).State = EntityState.Modified;
                CRentities.SaveChanges();
                TempData["Edit"] = samochod.id_samochodu;

                return RedirectToAction("GetCar");
            }
            return View(samochod);
        }

        public ActionResult DeleteCar(int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Samochody samochod = CRentities.Samochody.Find(id);

            if (samochod == null)
            {
                return HttpNotFound();
            }
            return View(samochod);
        }

        [HttpPost, ActionName("DeleteCar")]
        public ActionResult ConfirmedDeleteCar (int id)
        {
            Samochody samochod = CRentities.Samochody.Find(id);
            CRentities.Samochody.Remove(samochod);
            CRentities.SaveChanges();
            TempData["Delete"] = id;

            return RedirectToAction("GetCars");
        }

        public ActionResult DetailCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Samochody samochod = CRentities.Samochody.Find(id);

            if (samochod == null)
            {
                return HttpNotFound();
            }
            return View(samochod);
        }
    }
}