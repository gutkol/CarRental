using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using CarRental.Database;
using CarRental.Obiekt;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            LoginViewModel loginmodel = CheckCookie();
            if (loginmodel == null)
                return View();
            else
            {
                Konta konto = null;
                Uprawnienia rola = null;
                using (var dc = new CarRentalEntities())
                {
                    konto = (from c in dc.Konta
                             where c.login == loginmodel.Login
                             select c).FirstOrDefault();

                    if (konto != null && konto.haslo == loginmodel.Password)
                    {
                        rola = (from c in dc.Uprawnienia
                                where c.id_konta == konto.id_konta
                                select c).FirstOrDefault();

                        Session["Login"] = loginmodel.Login.ToString();
                        Session["Privilege"] = rola.uprawnienie.ToString();

                        return RedirectToAction("Glowna");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Źle wprowadzono nazwę użytkownika lub hasło.");
                        return View();
                    }
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginmodel/*, string returnUrl*/)
        {
            Konta konto = null;
            Uprawnienia rola = null;

            using (var dc = new CarRentalEntities())
            {
                konto = (from c in dc.Konta
                           where c.login == loginmodel.Login
                           select c).FirstOrDefault();


                if (konto != null && konto.haslo == loginmodel.Password)
                {
                    rola = (from c in dc.Uprawnienia
                            where c.id_konta == konto.id_konta
                            select c).FirstOrDefault();

                    Session["Login"] = konto.login.ToString();
                    Session["Privilege"] = rola.uprawnienie.ToString();

                    if (loginmodel.Remember)
                    {
                        HttpCookie ckLogin = new HttpCookie("Login");
                        ckLogin.Expires = DateTime.Now.AddSeconds(3600);
                        ckLogin.Value = loginmodel.Login;
                        Response.Cookies.Add(ckLogin);

                        HttpCookie ckPassword = new HttpCookie("Password");
                        ckPassword.Expires = DateTime.Now.AddSeconds(3600);
                        ckPassword.Value = loginmodel.Password;
                        Response.Cookies.Add(ckPassword);

                        HttpCookie ckUprawnienie = new HttpCookie("Privilege");
                        ckUprawnienie.Expires = DateTime.Now.AddSeconds(3600);
                        ckUprawnienie.Value = rola.uprawnienie;
                        Response.Cookies.Add(ckUprawnienie);
                    }
                    else
                    {
                        HttpCookie ckLogin = new HttpCookie("Login");
                        ckLogin.Expires = DateTime.Now.AddSeconds(300);
                        ckLogin.Value = loginmodel.Login;
                        Response.Cookies.Add(ckLogin);

                        HttpCookie ckPassword = new HttpCookie("Password");
                        ckPassword.Expires = DateTime.Now.AddSeconds(300);
                        ckPassword.Value = loginmodel.Password;
                        Response.Cookies.Add(ckPassword);

                        HttpCookie ckUprawnienie = new HttpCookie("Privilege");
                        ckUprawnienie.Expires = DateTime.Now.AddSeconds(300);
                        ckUprawnienie.Value = rola.uprawnienie;
                        Response.Cookies.Add(ckUprawnienie);
                    }

                    return RedirectToAction("Glowna", "Home");
                    //return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Źle wprowadzono nazwę użytkownika lub hasło.");
                    return View(loginmodel);
                }
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.RemoveAll();
            if (Response.Cookies["Login"] != null)
            {
                HttpCookie ckLogin = new HttpCookie("Login");
                ckLogin.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckLogin);
            }    
            
            if(Response.Cookies["Password"] != null)
            {
                HttpCookie ckPassword = new HttpCookie("Password");
                ckPassword.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckPassword);
            }

            if(Response.Cookies["Privilege"] != null)
            {
                HttpCookie ckPrivilege = new HttpCookie("Privilege");
                ckPrivilege.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckPrivilege);
            }

            return RedirectToAction("Glowna");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registermodel)
        {
            if (!ModelState.IsValid)
            {
                return View(registermodel);
            }
            else
            {
                Konta konto = null;
                Kod_Pocztowy kod = null;
                Miejscowosc mc = null;
                Wojewodztwo wo = null;
                Klienci numer_pesel = null, adres_email = null;

                using (var dc = new CarRentalEntities())
                {
                    konto = (from c in dc.Konta
                             where c.login == registermodel.Login
                             select c).FirstOrDefault();
                    kod = (from d in dc.Kod_Pocztowy
                           where d.kod_pocztowy1 == registermodel.KodPocztowy
                           select d).FirstOrDefault();
                    mc = (from e in dc.Miejscowosc
                          where e.miejscowosc1 == registermodel.Miejscowosc
                          select e).FirstOrDefault();
                    wo = (from f in dc.Wojewodztwo
                          where f.wojewodztwo1 == registermodel.Wojewodztwo
                          select f).FirstOrDefault();
                    numer_pesel = (from g in dc.Klienci
                                   where g.PESEL == registermodel.Pesel
                                   select g).FirstOrDefault();
                    adres_email = (from h in dc.Klienci
                                   where h.email == registermodel.Email
                                   select h).FirstOrDefault();

                    if (konto == null && numer_pesel == null && adres_email == null &&
                        registermodel.Password == registermodel.ConfirmPassword)
                    {
                        dc.Konta.Add(new Konta()
                        {
                            login = registermodel.Login,
                            haslo = registermodel.Password
                        });
                        dc.Klienci.Add(new Klienci()
                        {
                            imie = registermodel.Imie,
                            nazwisko = registermodel.Nazwisko,
                            numer_tel = registermodel.NumerTelefonu,
                            PESEL = registermodel.Pesel,
                            email = registermodel.Email
                        });
                        dc.Uprawnienia.Add(new Uprawnienia()
                        {
                            uprawnienie = Uprawnienie.Użytkownik.ToString()
                        });

                        if (kod == null && mc == null && wo == null)
                        {
                            dc.Kod_Pocztowy.Add(new Kod_Pocztowy()
                            {
                                kod_pocztowy1 = registermodel.KodPocztowy
                            });
                            dc.Miejscowosc.Add(new Miejscowosc()
                            {
                                miejscowosc1 = registermodel.Miejscowosc
                            });
                            dc.Wojewodztwo.Add(new Wojewodztwo()
                            {
                                wojewodztwo1 = registermodel.Wojewodztwo
                            });
                            dc.Adres.Add(new Adres()
                            {
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        }

                        else if (kod != null && mc == null && wo == null)
                        {
                            dc.Miejscowosc.Add(new Miejscowosc()
                            {
                                miejscowosc1 = registermodel.Miejscowosc
                            });
                            dc.Wojewodztwo.Add(new Wojewodztwo()
                            {
                                wojewodztwo1 = registermodel.Wojewodztwo
                            });
                            dc.Adres.Add(new Adres()
                            {
                                id_kodu_pocztowego = kod.id_kodu_pocztowego,
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        } //kod jesli istnieje

                        else if (kod != null && mc != null && wo == null)
                        {
                            dc.Wojewodztwo.Add(new Wojewodztwo()
                            {
                                wojewodztwo1 = registermodel.Wojewodztwo
                            });
                            dc.Adres.Add(new Adres()
                            {
                                id_kodu_pocztowego = kod.id_kodu_pocztowego,
                                id_miejscowosci = mc.id_miejscowosci,
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        } //kod, mies

                        else if (kod == null && mc != null && wo != null)
                        {
                            dc.Kod_Pocztowy.Add(new Kod_Pocztowy()
                            {
                                kod_pocztowy1 = registermodel.KodPocztowy
                            });
                            dc.Adres.Add(new Adres()
                            {
                                id_miejscowosci = mc.id_miejscowosci,
                                id_wojewodztwa = wo.id_wojewodztwa,
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        } //mies, woj

                        else if (kod != null && mc == null && wo != null)
                        {
                            dc.Miejscowosc.Add(new Miejscowosc()
                            {
                                miejscowosc1 = registermodel.Miejscowosc
                            });
                            dc.Adres.Add(new Adres()
                            {
                                id_kodu_pocztowego = kod.id_kodu_pocztowego,
                                id_wojewodztwa = wo.id_wojewodztwa,
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        } //kod, woj

                        else if (kod == null && mc != null && wo == null)
                        {
                            dc.Kod_Pocztowy.Add(new Kod_Pocztowy()
                            {
                                kod_pocztowy1 = registermodel.KodPocztowy
                            });
                            dc.Wojewodztwo.Add(new Wojewodztwo()
                            {
                                wojewodztwo1 = registermodel.Wojewodztwo
                            });
                            dc.Adres.Add(new Adres()
                            {
                                id_miejscowosci = mc.id_miejscowosci,
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        } //mies

                        else if (kod == null && mc == null && wo != null)
                        {
                            dc.Kod_Pocztowy.Add(new Kod_Pocztowy()
                            {
                                kod_pocztowy1 = registermodel.KodPocztowy
                            });
                            dc.Miejscowosc.Add(new Miejscowosc()
                            {
                                miejscowosc1 = registermodel.Miejscowosc
                            });
                            dc.Adres.Add(new Adres()
                            {
                                id_wojewodztwa = wo.id_wojewodztwa,
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        } //woj

                        else
                        {
                            dc.Adres.Add(new Adres()
                            {
                                id_kodu_pocztowego = kod.id_kodu_pocztowego,
                                id_miejscowosci = mc.id_miejscowosci,
                                id_wojewodztwa = wo.id_wojewodztwa,
                                ulica = registermodel.Ulica,
                                numer_domu = registermodel.NumerDomu
                            });
                        }
                        dc.SaveChanges();
                        //to powyzej zapisuje mi do bazy danych przy rejestracji
                        
                    }

                    if(konto != null)
                    {
                        ModelState.AddModelError("", "Użytkownik: " + registermodel.Login + " już istnieje: ");
                        return View(registermodel);
                    }

                    if (numer_pesel != null)
                    {
                        ModelState.AddModelError("", "Proszę wpisać poprawny numer pesel ");
                        return View(registermodel);
                    }

                    if (adres_email != null)
                    {
                        ModelState.AddModelError("", "Podany adres email już istnieje ");
                        return View(registermodel);
                    }
                }
                return View("RegisteredIn", registermodel);
            }
        }

        [HttpGet]
        public ActionResult Glowna()
        {
            CarRentalEntities CRentities = new CarRentalEntities();
            var getListaMiejsc = CRentities.Miejsca.ToList();


            SelectList listaMiejsc = new SelectList(getListaMiejsc, "id_miejsca", "miejscowosc");
            ViewBag.listaMiejscPracy = listaMiejsc;
            return View();
        }

        [HttpPost]
        public ActionResult Glowna(SamochodyKontener rentalmodel)
        {
            var konto = CheckCookie();

            if (konto != null)
            {
                if (rentalmodel.rentalmodelkontener.ReturnPlace == false)
                {
                    rentalmodel.rentalmodelkontener.IDMiejsceOdd = rentalmodel.rentalmodelkontener.IDMiejsceWyp;
                }

                Session["rentalmodel"] = rentalmodel;
                return RedirectToAction("GetOrder", "rentals");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Oferta()
        {
            return View();
        }

        public ActionResult Oferta2()
        {
            return View();
        }

        public ActionResult Oferta3()
        {
            return View();
        }

        public ActionResult Ofirmie()
        {
            return View();
        }

        public ActionResult Regulamin()
        {
            return View();
        }

        public ActionResult Kontakt()
        {
            return View();
        }

        public ActionResult History()
        {
            LoginViewModel loginmodel = CheckCookie();
            if (Session["Privilege"].ToString() == "Użytkownik")
            {
                CarRentalEntities CRentities = new CarRentalEntities();

                using (var dc = new CarRentalEntities())
                {
                    var login = Session["Login"].ToString();

                    var konto = (from a in dc.Konta
                                 where a.login == login
                                 select a).FirstOrDefault();

                    var identyfikator = (from b in dc.Klienci
                                         where b.id_konta_kl == konto.id_konta
                                         select b).FirstOrDefault();

                    List<Wypozyczenia> historia = null;

                    historia = dc.Wypozyczenia.Where(x => x.id_klienta == identyfikator.id_klienta).ToList();

                    historia.ForEach(x =>
                    {
                        x.Samochody.Marka = dc.Marka.Where(z => z.id_marki == x.Samochody.id_marki).FirstOrDefault();
                        x.Samochody.Model = dc.Model.Where(z => z.id_modelu == x.Samochody.id_marki).FirstOrDefault();
                        x.Klienci = dc.Klienci.Where(z => z.id_klienta == x.Klienci.id_klienta).FirstOrDefault();
                        x.Miejsca = dc.Miejsca.Where(z => z.id_miejsca == x.id_miejsca_wyp).FirstOrDefault();
                        x.Miejsca1 = dc.Miejsca.Where(z => z.id_miejsca == x.id_miejsca_odd).FirstOrDefault();
                    });

                    return View(historia);
                }
            }
            return RedirectToAction("Login", "Home");
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