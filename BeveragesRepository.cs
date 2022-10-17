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
    // Класс для создания строки подключения к БД
    public class Constants
    {
        // Проверка подлинности Windows (Для разработки и тестирования)
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
        public static string appPlace = AppDomain.CurrentDomain.BaseDirectory;
        public static string connectString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='%CONTENTROOTPATH%App_Data\\CoffeeMachine.mdf';Integrated Security=True";
    }


    // Класс записи в журнал учета
    public class Log
    {
        [Key]
        public int id { get; set; }
        public DateTime date { get; set; }
        public string buying { get; set; }
        public int revenue { get; set; }
    }

    // Класс для доступа к БД
    public class AppContext : DbContext
    {
        public DbSet<Beverage> Beverages { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string n = Constants.appPlace;
            Constants.connectString = Constants.connectString.Replace("%CONTENTROOTPATH%", n);
            optionsBuilder.UseSqlServer(Constants.connectString);
        }
    }
}