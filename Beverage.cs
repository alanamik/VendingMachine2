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

        // Увеличить кол-во напитка в автомате
        public void AddCount(int value)
        {
            count += value;
        }

        // Уменьшить кол-во напитка в автомате
        public void ReduceCount(int value)
        {
            count -= value;
        }

        // Изменить цену напитка
        public void ChangePrice(int newPrice)
        {
            price = newPrice;
        }
    }
}