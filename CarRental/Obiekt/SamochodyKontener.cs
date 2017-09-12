using CarRental.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarRental.Obiekt
{
    public class SamochodyKontener
    {
        public IList<Samochody> ListAut { get; set; }

        public CarRental.Models.RentalViewModel rentalmodelkontener { get; set; }
    }
}