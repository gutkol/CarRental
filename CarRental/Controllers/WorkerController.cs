using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using CarRental.Database;
using System.Net;
using System.Data.Entity;

namespace CarRental.Controllers
{
    public class WorkerController : Controller
    {
        private CarRentalEntities CRentities = new CarRentalEntities();

        [HttpGet]
        public ActionResult WorkerRegister()
        {
            LoginViewModel loginmodel = CheckCookie();
            if(loginmodel != null)
            {
                if (Session["Privilege"].ToString() != "Administrator")
                    return RedirectToAction("Login", "Home");
                else
                {
                    //WorkerRegisterViewModel WRVM = new WorkerRegisterViewModel();

                    var getListaMiejsc = CRentities.Miejsca.ToList();

                    //SelectList listaMiejsc = new SelectList(getListaMiejsc, "id_miejsca", "miejscowosc");
                    //WRVM.MiejscowoscMiejscaPracy = listaMiejsc;

                    SelectList listaMiejsc = new SelectList(getListaMiejsc, "id_miejsca", "miejscowosc");
                    ViewBag.listaMiejscPracy = listaMiejsc;
                    return View();
                }
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WorkerRegister(WorkerRegisterViewModel registermodel)
        {
            LoginViewModel loginmodel = CheckCookie();
            if (loginmodel != null)
            {
                if (Session["Privilege"].ToString() != "Administrator")
                    return RedirectToAction("Login", "Home");
                else
                {
                    if (!ModelState.IsValid)
                    {
                        return View(registermodel);
                    }
                    else
                    {
                        var getListaMiejsc = CRentities.Miejsca.ToList();

                        SelectList listaMiejsc = new SelectList(getListaMiejsc, "id_miejsca", "miejscowosc");
                        ViewBag.listaMiejscPracy = listaMiejsc;

                        Konta konto = null;
                        Miejsca miejscePracy = null;

                        using (var dc = new CarRentalEntities())
                        {
                            konto = (from c in dc.Konta
                                     where c.login == registermodel.Login
                                     select c).FirstOrDefault();

                            miejscePracy = (from d in dc.Miejsca
                                            where d.id_miejsca == registermodel.IDMiejscowoscPracy
                                            select d).FirstOrDefault();

                            if (konto == null && registermodel.Password == registermodel.ConfirmPassword)
                            {
                                dc.Konta.Add(new Konta()
                                {
                                    login = registermodel.Login,
                                    haslo = registermodel.Password
                                });

                                dc.Pracownicy.Add(new Pracownicy()
                                {
                                    id_miejsca_pracy = miejscePracy.id_miejsca,
                                    imie = registermodel.Imie,
                                    nazwisko = registermodel.Nazwisko,
                                    data_zatrudnienia = registermodel.DataZatrudnienia,
                                    stanowisko = registermodel.Stanowisko
                                });

                                dc.Uprawnienia.Add(new Uprawnienia()
                                {
                                    uprawnienie = registermodel.Uprawnienie.ToString()
                                });

                                dc.SaveChanges();

                                ModelState.Clear();
                                ViewBag.Account = registermodel.Imie + " " + registermodel.Nazwisko;
                            }

                            if (konto != null)
                            {
                                ModelState.AddModelError("", "Użytkownik: " + registermodel.Login + " już istnieje: ");
                                return View(registermodel);
                            }
                        }
                        return View();
                    }
                }
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult GetWorker()
        {

            List<Pracownicy> pracownik = null;

            using(var dc = new CarRentalEntities())
            {
                pracownik = dc.Pracownicy.ToList();
                pracownik.ForEach(x =>
                {
                    x.Miejsca = dc.Miejsca.Where(z => z.id_miejsca == x.id_miejsca_pracy).FirstOrDefault();
                    x.Konta = dc.Konta.Where(z => z.id_konta == x.id_konta_pr).FirstOrDefault();
                });
            }
            return View(pracownik);
        }

        [HttpGet]
        public ActionResult EditWorker(int? id)
        {
            LoginViewModel loginmodel = CheckCookie();
            if (loginmodel != null)
            {
                if (Session["Privilege"].ToString() != "Administrator")
                    return RedirectToAction("Login", "Home");
                else
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    Pracownicy pracownik = CRentities.Pracownicy.Find(id);

                    if (pracownik == null)
                    {
                        return HttpNotFound();
                    }
                    return View(pracownik);
                }
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWorker(Pracownicy pracownik)
        {
            if (ModelState.IsValid)
            {
                CRentities.Entry(pracownik).State = EntityState.Modified;
                CRentities.SaveChanges();
                TempData["Edit"] = pracownik.id_pracownika;

                return RedirectToAction("GetWorker");
            }
            return View(pracownik);
        }

        public ActionResult DeleteWorker(int? id)
        {
            LoginViewModel loginmodel = CheckCookie();
            if (loginmodel != null)
            {
                if (Session["Privilege"].ToString() != "Administrator")
                    return RedirectToAction("Login", "Home");
                else
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    Pracownicy pracownik = CRentities.Pracownicy.Find(id);

                    if (pracownik == null)
                    {
                        return HttpNotFound();
                    }
                    return View(pracownik);
                }
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost, ActionName("DeleteWorker")]
        public ActionResult ConfirmedDeleteWorker(int id)
        {
            Pracownicy pracownik = CRentities.Pracownicy.Find(id);

            Konta konto = null;
            Uprawnienia uprawnienie = null;

            konto = (from a in CRentities.Konta
                     where a.id_konta == pracownik.id_konta_pr
                     select a).FirstOrDefault();

            if (konto != null)
            {
                uprawnienie = (from b in CRentities.Uprawnienia
                               where b.id_konta == konto.id_konta
                               select b).FirstOrDefault();

                CRentities.Uprawnienia.Remove(uprawnienie);
                CRentities.Konta.Remove(konto);
            }

            string osoba = pracownik.imie + " " + pracownik.nazwisko;

            CRentities.Pracownicy.Remove(pracownik);

            CRentities.SaveChanges();
            TempData["Delete"] = id + " " + osoba;

            return RedirectToAction("GetWorker");
        }

        public ActionResult DetailWorker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pracownicy pracownik = CRentities.Pracownicy.Find(id);

            if (pracownik == null)
            {
                return HttpNotFound();
            }
            return View(pracownik);
        }

        [HttpGet]
        public ActionResult RentalsHistory()
        {
            LoginViewModel loginmodel = CheckCookie();
            if (loginmodel != null)
            {
                if (Session["Privilege"].ToString() != "Administrator" || Session["Privilege"].ToString() != "Pracownik")
                    return RedirectToAction("Login", "Home");
                else
                {
                    List<Wypozyczenia> historia = null;

                    using (var dc = new CarRentalEntities())
                    {
                        historia = dc.Wypozyczenia.ToList();

                        historia.ForEach(x =>
                        {
                            x.Samochody = dc.Samochody.Where(z => z.id_samochodu == x.id_samochodu).FirstOrDefault();
                            x.Samochody.Marka = dc.Marka.Where(z => z.id_marki == x.Samochody.id_marki).FirstOrDefault();
                            x.Samochody.Model = dc.Model.Where(z => z.id_modelu == x.Samochody.id_modelu).FirstOrDefault();
                            x.Klienci = dc.Klienci.Where(z => z.id_klienta == x.id_klienta).FirstOrDefault();
                            x.Miejsca = dc.Miejsca.Where(z => z.id_miejsca == x.id_miejsca_wyp).FirstOrDefault();
                            x.Miejsca1 = dc.Miejsca.Where(z => z.id_miejsca == x.id_miejsca_odd).FirstOrDefault();
                            x.Pracownicy = dc.Pracownicy.Where(z => z.id_pracownika == z.id_pracownika).FirstOrDefault();
                        });
                    }

                    return View(historia);
                }
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult DetailsRentalsHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Wypozyczenia historia = CRentities.Wypozyczenia.Find(id);

            if (historia == null)
            {
                return HttpNotFound();
            }
            return View(historia);
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