using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Proszę podać imię.")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwisko.")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę użytkownika.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Proszę podać adres e-mail.")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Proszę podać prawidłowy adres e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Proszę potwierdzić hasło.")]
        [Compare("Password", ErrorMessage = "Potwierdzone hasło nie pasuje do hasła.")]
        public string ConfirmPassword { get; set; }

        [Phone]
        [Required(ErrorMessage = "Proszę podać numer telefonu.")]
        [RegularExpression(@"^([0-9]{3})[ \s]\ ?([0-9]{3})[ \s]\ ?([0-9]{3})$",
            ErrorMessage = "Numer musi być w formacie: 123 123 123")]
        public string NumerTelefonu { get; set; }

        [Required(ErrorMessage = "Proszę podać PESEL.")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "PESEL musi składać się z 11 cyfr.")]
        public string Pesel { get; set; }

        [Required(ErrorMessage = "Proszę podać ulicę.")]
        public string Ulica { get; set; }

        [Required(ErrorMessage = "Proszę podać numer domu.")]
        [RegularExpression(@"^(\d{1,4})(\s{0,1})([a-zA-Z]{0,1})$", ErrorMessage = "Proszę podać poprawny numer domu.")]
        public string NumerDomu { get; set; }

        [Required(ErrorMessage = "Proszę podać kod pocztowy.")]
        [RegularExpression(@"^([0-9]{2})[-]([0-9]{3})$", ErrorMessage = "Kod pocztowy musi być w formacie: 00-000")]
        public string KodPocztowy { get; set; }

        [Required(ErrorMessage = "Proszę podać miejscowość.")]
        public string Miejscowosc { get; set; }

        [Required(ErrorMessage = "Proszę podać województwo.")]
        public string Wojewodztwo { get; set; }
    }
}