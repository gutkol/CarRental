using CarRental.Database;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class PlaceController : Controller
    {
        private CarRentalEntities CRentities = new CarRentalEntities();

        public ActionResult GetWorkerPlace()
        {
            using (var dc = new CarRentalEntities())
            {
                var all = dc.Miejsca.ToList();

                return View(all);
            }
        }

        [HttpGet]
        public ActionResult EditWorkerPlace(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Miejsca miejsce = CRentities.Miejsca.Find(id);

            if (miejsce == null)
            {
                return HttpNotFound();
            }
            return View(miejsce);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWorkerPlace([Bind(Include = "id_miejsca,ulica,numer,kod_pocztowy,miejscowosc,numer_tel")] Miejsca miejsce)
        {
            if (ModelState.IsValid)
            {
                CRentities.Entry(miejsce).State = EntityState.Modified;
                CRentities.SaveChanges();
                TempData["Edit"] = miejsce.id_miejsca;
                
                return RedirectToAction("GetWorkerPlace");
            }
            return View(miejsce);
        }


        public ActionResult DeleteWorkerPlace(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Miejsca miejsce = CRentities.Miejsca.Find(id);

            if (miejsce == null)
            {
                return HttpNotFound();
            }
            return View(miejsce);
        }

        [HttpPost, ActionName("DeleteWorkerPlace")]
        public ActionResult DeleteConfirmedWorkerPlace(int id)
        {
            Miejsca miejsce = CRentities.Miejsca.Find(id);
            CRentities.Miejsca.Remove(miejsce);
            CRentities.SaveChanges();
            TempData["Delete"] = id;
            return RedirectToAction("GetWorkerPlace");
        }

        //public List<SelectListItem> GetMiejscaList()
        //{
        //    List<SelectListItem> listMiejsca = new List<SelectListItem>();

        //    using (var dc = new CarRentalEntities())
        //    {
        //        foreach (var item in dc.Miejsca)
        //        {
        //            listMiejsca.Add(new SelectListItem
        //            {
        //                Value = item.id_miejsca.ToString(),
        //                Text = String.Format(item.miejscowosc)
        //            });
        //        }
        //    }
        //    return listMiejsca;
        //}

        [HttpGet]
        public ActionResult WorkerPlace()
        {
            LoginViewModel loginmodel = CheckCookie();
            if (loginmodel != null)
            {
                if (Session["Privilege"].ToString() != "Administrator")
                    return RedirectToAction("Login", "Home");
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkerPlace(WorkerPlaceModel placemodel)
        {
            if (!ModelState.IsValid)
            {
                return View(placemodel);
            }
            else
            {
                using (var dc = new CarRentalEntities())
                {

                    dc.Miejsca.Add(new Miejsca()
                    {
                        ulica = placemodel.Ulica,
                        numer = placemodel.NumerLokalu,
                        miejscowosc = placemodel.Miejscowosc,
                        kod_pocztowy = placemodel.KodPocztowy,
                        numer_tel = placemodel.NumerTelefonu
                    });

                    dc.SaveChanges();
                }

                ModelState.Clear();
                ViewBag.Message = placemodel.Miejscowosc;

                return View();
            }
        }

        //Sprawdzenie ciasteczek
        public LoginViewModel CheckCookie()
        {
            LoginViewModel account = null;

            string loginUser = string.Empty, passwordUser = string.Empty, privilegeUser = string.Empty;
            if (Response.Cookies["Login"] != null)
                loginUser = Request.Cookies["Login"].Value;

            if (Response.Cookies["Password"] != null)
                passwordUser = Request.Cookies["Password"].Value;

            if (Response.Cookies["Privilege"] != null)
                privilegeUser = Request.Cookies["Privilege"].Value;

            if (!String.IsNullOrEmpty(loginUser) && !String.IsNullOrEmpty(passwordUser) && !String.IsNullOrEmpty(privilegeUser))
            {
                account = new LoginViewModel { Login = loginUser, Password = passwordUser };
                Session["Login"] = loginUser.ToString();
                Session["Privelege"] = privilegeUser.ToString();
            }

            return account;
        }
    }
}