using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendingMachine
{
    // Класс напитка, продающегося в автомате
    public class Beverage
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public int price { get; set; }
    }
}