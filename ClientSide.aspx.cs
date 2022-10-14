using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VendingMachine
{
    public partial class ClientSide : System.Web.UI.Page
    {
        static VendingMachineRepository repos = new VendingMachineRepository();

        protected void Page_Load(object sender, EventArgs e)
        {

            CheckCountOfBeverages();
        }

        /*  [WebMethod]
          public static string ShowInsertedCoins()
          {
             // return "You inserted: " + coins.ToString();
          }*/


        /* [WebMethod]
         public static string BtnAddCoin(int coin)
         {
             coins = +coin;
             return coins.ToString();
             //ShowInsertedCoins();
         }*/

        /*  [WebMethod(enableSession: true)]
          public static void BtnAddCoin(int coin)
          {

              int MyCoins = (int)HttpContext.Current.Session["CoinCount"];
              MyCoins += coin;
              HttpContext.Current.Session["CoinCount"] = MyCoins;
              //Debug.Print("coins = " + MyCoins.ToString());

          }*/

        public void CheckCountOfBeverages()
        {
            using (AppContext _db = new AppContext())
            {
                // Уменьшаем кол-во напитков на один
                foreach (Beverage b in _db.Beverages)
                {
                    Debug.Print($"{b.id} --- {b.name}");
                    if (b.count != 0)
                    {
                        /*var myTextboxControl = (TextBox)Page.FindControl("b.name");
                        myTextboxControl.Attributes.Add("onclick", "window.open('someOtherLink');")*/

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

        public static void StartChecking()
        {
            CheckCountOfBeverages();
        }

        [WebMethod(enableSession: true)]
        public static int Sell(int coins, string nameOfBeverage)
        {

            //int MyCoins = (int)HttpContext.Current.Session["CoinCount"];
           // MyCoins += coin;
           // HttpContext.Current.Session["CoinCount"] = MyCoins;
            Debug.Print("coins = " + coins.ToString());
            Debug.Print("nameOfBeverage = " + nameOfBeverage);

            using (AppContext _db = new AppContext())
            {
                // Ищем в бд выбранный напиток
                Beverage beverage = _db.Beverages.FirstOrDefault(p => p.name == nameOfBeverage);
                Debug.Print("Founded - " + beverage.name);

                // Если внесено недостаточно монет
                if (beverage.price > coins) return -1;
                else
                {
                    // Уменьшаем кол-во напитков на один
                    foreach (Beverage b in _db.Beverages)
                    {
                        if (b.count != 0) 
                        { b.count--; 
                        
                        }                       
                    }

                    CheckCountOfBeverages();

                    //Рассчитываем сдачу
                    int change = coins - beverage.price;
                    Debug.Print("Change - " + change.ToString());

                    // Заносим в журнал покупку
                    int count = _db.Logs.Max(p => p.id);
                    //int count = countLog.id;

                    Log log = new Log();
                    log.id = count+1;
                    log.date = DateTime.Today.Date;
                    //log.date = "11.10.2022";
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