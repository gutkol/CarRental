using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class CarViewModel
    {
        public int IDWyposazenia { get; set; }
        public int IDMarki { get; set; }
        public int IDModelu { get; set; }
        public int IDTypu { get; set; }
        public int IDKoloru { get; set; }
        public int IDPaliwa { get; set; }

        public string Wyposazenie { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Typ { get; set; }
        public string Kolor { get; set; }
        public string Paliwo { get; set; }

        public string Rok_produkcji { get; set; }
        public string Pojemnosc_silnika { get; set; }
        public decimal Przebieg { get; set; }
    }
}