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
                    Beverage b = _db.Beverages.FirstOrDefault(p => p.name == "blackCoffee");
                blackCoffee_price.InnerText = b.price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.name == "espresso");
                espresso_price.InnerText = b.price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.name == "cappuccino");
                cappuccino_price.InnerText = b.price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.name == "macchiato");
                macchiato_price.InnerText = b.price.ToString();

                b = _db.Beverages.FirstOrDefault(p => p.name == "coffeeWithCream");
                coffeeWithCream_price.InnerText = b.price.ToString();

            }
        }

        // Загрузка кол-ва напитков и картинок к ним
        public void CheckCountOfBeverages()
        {
            using (AppContext _db = new AppContext())
            {
                foreach (Beverage b in _db.Beverages)
                {
                    Debug.Print($"{b.id} --- {b.name}");
                    if (b.count != 0)
                    {
                        var myTextboxControl = (System.Web.UI.HtmlControls.HtmlImage)Page.FindControl(b.name);
                        string funcName = "SelectBeverage('" + b.name + "')";
                        myTextboxControl.Attributes.Add("onclick", funcName);
                        string attrStr = b.name + ".jpg";
                        myTextboxControl.Attributes.Add("src", attrStr);

                    }
                    // Если напитков не осталось
                    if (b.count == 0)
                    {
                        Debug.Print($"We have no {b.id}.{b.name}");
                        var myTextboxControl = (System.Web.UI.HtmlControls.HtmlImage)Page.FindControl(b.name);
                        myTextboxControl.Attributes.Add("onclick", "return false");
                        string attrStr = b.name + "_blocked.jpg";
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
                Beverage beverage = _db.Beverages.FirstOrDefault(p => p.name == nameOfBeverage);
                // Если внесено недостаточно монет
                if (beverage.price > coins) return -1;
                else
                {
                    // Уменьшаем кол-во напитков на один
                    foreach (Beverage b in _db.Beverages)
                    {
                        if (b.count>0) b.count--;                                                                        
                    }                   
                    //Рассчитываем сдачу
                    int change = coins - beverage.price;
                    Debug.Print("Change - " + change.ToString());

                    // Заносим в журнал покупку
                    int count = _db.Logs.Max(p => p.id);
                    //int count = countLog.id;

                    Log log = new Log();
                    log.id = count+1;
                    log.date = DateTime.Today.Date;
                    log.buying = nameOfBeverage;
                    log.revenue = beverage.price;
                    _db.Logs.Add(log);

                    _db.SaveChanges();

                    // Возвращаем сдачу
                    return change;
                }
            }

        }
    }
}