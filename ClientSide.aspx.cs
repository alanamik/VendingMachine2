using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace VendingMachine
{
    public partial class ClientSide : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // При загрузке страницы загружаем цены и кол-во напитков
            CheckCountOfBeverages();
            CheckPrice();
        }

        // Загрузка цен
        public void CheckPrice()
        {
            using (AppContext _db = new AppContext())
            { 
                    Beverage b = _db.Beverages.FirstOrDefault(p => p.Name == "blackCoffee");
                blackCoffee_price.InnerText = b.Price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.Name == "espresso");
                espresso_price.InnerText = b.Price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.Name == "cappuccino");
                cappuccino_price.InnerText = b.Price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.Name == "macchiato");
                macchiato_price.InnerText = b.Price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.Name == "coffeeWithCream");
                coffeeWithCream_price.InnerText = b.Price.ToString();

            }
        }


        // Загрузка кол-ва напитков и картинок к ним
        public void CheckCountOfBeverages()
        {
            using (AppContext _db = new AppContext())
            {
                foreach (Beverage b in _db.Beverages)
                {
                    Debug.Print($"{b.Id} --- {b.Name}");
                    if (b.Count != 0)
                    {
                        var myTextboxControl = (System.Web.UI.HtmlControls.HtmlImage)Page.FindControl(b.Name);
                        string funcName = "SelectBeverage('" + b.Name + "')";
                        myTextboxControl.Attributes.Add("onclick", funcName);
                        string attrStr = b.Name + ".jpg";
                        myTextboxControl.Attributes.Add("src", attrStr);

                    }
                    // Если напитков не осталось
                    if (b.Count == 0)
                    {
                        Debug.Print($"We have no {b.Id}.{b.Name}");
                        var myTextboxControl = (System.Web.UI.HtmlControls.HtmlImage)Page.FindControl(b.Name);
                        myTextboxControl.Attributes.Add("onclick", "return false");
                        string attrStr = b.Name + "_blocked.jpg";
                        myTextboxControl.Attributes.Add("src", attrStr);
                    }
                }
            }
        }

        // Продажа напитка
        [WebMethod(enableSession: true)]
        public static int Sell(int coins, string nameOfBeverage)
        {
            Debug.Print("coins = " + coins.ToString());
            Debug.Print("nameOfBeverage = " + nameOfBeverage);
            using (AppContext _db = new AppContext())
            {
                // Ищем в бд выбранный напиток
                Beverage beverage = _db.Beverages.FirstOrDefault(p => p.Name == nameOfBeverage);
                // Если внесено недостаточно монет
                if (beverage.Price > coins) return -1;
                else
                {
                    // Уменьшаем кол-во напитков на один
                    foreach (Beverage b in _db.Beverages)
                    {
                        if (b.Count>0) b.Count--;                                                                        
                    }                   
                    //Рассчитываем сдачу
                    int change = coins - beverage.Price;
                    Debug.Print("Change - " + change.ToString());

                    // Заносим в журнал покупку
                    int count = _db.Logs.Max(p => p.Id);
                    //int count = countLog.id;

                    Log log = new Log();
                    log.Id = count+1;
                    log.Date = DateTime.Today.Date;
                    log.Buying = nameOfBeverage;
                    log.Revenue = beverage.Price;
                    _db.Logs.Add(log);

                    _db.SaveChanges();

                    // Возвращаем сдачу
                    return change;
                }
            }

        }
    }
}