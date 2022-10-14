using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendingMachine
{
    public class VendingMachineRepository
    {

        //private static BeveragesRepository repository = new BeveragesRepository();
        // private List<Beverage> responses = new List<Beverage>();
        private  AppContext _db = new AppContext();
        public void Sell()
        {

        }
    }

    public class Constants
    {
        // Проверка подлинности Windows
        public static string SqlConnectionIntegratedSecurity
        {
            get
            {
                var sb = new SqlConnectionStringBuilder
                {
                    DataSource = @"OMOS-18\SQLEXPRESS",
                    // Подключение будет с проверкой подлинности пользователя Windows
                    IntegratedSecurity = true,
                    // Название целевой базы данных.
                    InitialCatalog = "CoffeeMachine"
                };
                return sb.ConnectionString;
            }
        }


        // Проверка подлинности SQL сервером
        /* public static string SqlConnectionSQLServer
         {
             get
             {
                 var sb = new SqlConnectionStringBuilder
                 {
                     DataSource = "Путь к серверу SQL",
                     IntegratedSecurity = false,
                     InitialCatalog = "DBMSSQL",
                     UserID = "Имя пользователя",
                     Password = "Пароль"
                 };

                 return sb.ConnectionString;
             }
         }*/
    }

    public class Log
    {      
        [Key] 
        public int id { get; set; }
        public DateTime date { get; set; }
        public string buying { get; set; }
        public int revenue { get; set; }
    }

    public class AppContext : DbContext
    {
        public DbSet<Beverage> Beverages { get; set; }
        public DbSet<Log> Logs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=srv-mts\sql2008;Initial Catalog=VM;Integrated Security=True");
            optionsBuilder.UseSqlServer(Constants.SqlConnectionIntegratedSecurity);
            // optionsBuilder.UseSqlServer(@"Server=.\sql2008;Database=VM;Trusted_Connection=True;");
        }
    }
}