using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Models
{
    public class WorkerRegisterViewModel
    {
        [Required(ErrorMessage = "Proszę wybrać miejsce pracy.")]
        public int IDMiejscowoscPracy { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę użytkownika.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Proszę potwierdzić hasło.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Potwierdzone hasło nie pasuje do hasła.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Proszę podać imię.")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwisko.")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Proszę podać date zatrudnienia.")]
        public DateTime DataZatrudnienia{ get; set; }

        [Required(ErrorMessage = "Proszę podać stanowisko.")]
        public string Stanowisko { get; set; }

        //[NotMapped]
        //[Required(ErrorMessage = "Proszę wybrać miejsce pracy.")]
        //public SelectList MiejscowoscPracy { get; set; }

        [Required(ErrorMessage = "Proszę wybrać rodzaj uprawnienia.")]
        public Uprawnienie Uprawnienie { get; set; }
    }

    public enum Uprawnienie
    {
        Administrator,
        Pracownik,
        Użytkownik
    }

}