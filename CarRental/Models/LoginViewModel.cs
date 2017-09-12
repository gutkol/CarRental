using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Proszę podać nazwę użytkownika")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło")]
        public string Password { get; set; }

        public bool Remember { get; set; }
    }
}