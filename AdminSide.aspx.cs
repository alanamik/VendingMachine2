using Newtonsoft.Json;
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
    public partial class AdminSide : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadListOfBeverages();
            //Для теста показываем информацию о макиато
            ShowInfo("macchiato");
            // Переменная для выбранного напитка, информацию о котором нужно изменить
            Session["infoAbout"] = (string)"";
        }

        // Загрузка листа напитков в select element
        public void LoadListOfBeverages()
        {
            using (AppContext _db = new AppContext())
            {
                foreach (Beverage b in _db.Beverages)
                {
                    SelectBeverage.Items.Add(b.Name);
                }
            }
        }

        // Показать информацию о напитке
        public void ShowInfo(string name)
        {
            using (AppContext _db = new AppContext())
            {
                Beverage b = _db.Beverages.FirstOrDefault(p => p.Name == name);
                showName.InnerText = "Название: " + b.Name;
                showPrice.InnerText = "Цена: " + b.Price.ToString();
                showCount.InnerText = "Количество: " + b.Count.ToString();
            }
        }

        // Метод для возвращения информации о напитке в форме json
        // (В процессе разработки)
        [WebMethod(enableSession: true)]
        public static string ShowBeverageInfo(string name)
        {
            Debug.Print("ShowBeverageInfo = " + name);

            Beverage b = new Beverage();
            using (AppContext _db = new AppContext())
            {
                b = _db.Beverages.FirstOrDefault(p => p.Name == name);
            }
            return JsonConvert.SerializeObject(b);
        }

        // Добавить новый напиток в БД
        [WebMethod(enableSession: true)]
        public static int AddNewBeverage(string name, int count, int price)
        {
            using (AppContext _db = new AppContext())
            {
                Beverage bev = new Beverage();
                bev.Id = (_db.Beverages.Max(b => b.Id)) + 1;
                bev.Name = name;
                bev.Count = count;
                bev.Price = price;

                _db.Beverages.Add(bev);
                _db.SaveChanges();
                return 1;
            }
        }

        // Изменить данные о напитке в БД
        [WebMethod(enableSession: true)]
        public static int ChangeBeverageInfo(string name, int count, int price)
        {
            using (AppContext _db = new AppContext())
            {
                var bev = _db.Beverages.Where(b => b.Name == name).FirstOrDefault();
                bev.Price = price;
                bev.Count = count;
                _db.SaveChanges();
                return 1;
            }
        }
    }
}