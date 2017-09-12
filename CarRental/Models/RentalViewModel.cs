using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class RentalViewModel
    {
        public int IDMiejsceWyp { get; set; }
        public int IDMiejsceOdd { get; set; }
        public DateTime DataWyp { get; set; }
        public DateTime DataOdd { get; set; }
        public bool ReturnPlace { get; set; }
    }
}