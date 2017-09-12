using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class WorkerPlaceModel
    {
        public int IDMiejsca { get; set; }

        [Required(ErrorMessage = "Proszę podać ulicę.")]
        public string Ulica { get; set; }

        [Required(ErrorMessage = "Proszę podać numer lokalu.")]
            [RegularExpression(@"^(\d{1,4})(\s{0,1})([a-zA-Z]{0,1})$", ErrorMessage = "Proszę podać poprawny numer lokalu.")]
        public string NumerLokalu { get; set; }

        [Required(ErrorMessage = "Proszę podać miejscowość, w której znajduje się firma.")]
        public string Miejscowosc { get; set; }

        [Required(ErrorMessage = "Proszę podać kod pocztowy.")]
            [RegularExpression(@"^([0-9]{2})[-]([0-9]{3})$", ErrorMessage = "Kod pocztowy musi być w formacie: 00-000")]
        public string KodPocztowy { get; set; }

        [Phone]
        [Required(ErrorMessage = "Proszę podać numer telefonu.")]
            [RegularExpression(@"^([0-9]{3})[ \s]\ ?([0-9]{3})[ \s]\ ?([0-9]{3})$",
                ErrorMessage = "Numer musi być w formacie: 123 123 123")]
        public string NumerTelefonu { get; set; }
    }
}