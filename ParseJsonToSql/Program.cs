using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using Newtonsoft.Json;

//КОНСОЛЬНОЕ ПРИЛОЖЕНИЕ,КОТОРОЕ ПОЗВОЛЯЕТ ПРЕОБРАЗОВАТЬ ДАННЫЕ В ФАЙЛАХ JSON В SQL БАЗУ ДАННЫХ
namespace ParseJsonToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Item> items = new List<Item>();
            string jsonstring = File.ReadAllText(@"E:\Программирование\ТЗ Jun C#\day30.json");
            var data = JsonConvert.DeserializeObject<List<Item>>(jsonstring);

            string myConnectionString = @"Data Source=DESKTOP-0RUJG5Q\SQLEXPRESS;Initial Catalog=AnalysisCountSteps;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(myConnectionString))
            {
                con.Open();
                foreach (var item in data)
                {
                    if (SaveToDatabase(con, item)) Console.WriteLine("Успешно : " + item.User + " Сохранен в базу данных!");
                    else Console.WriteLine("Ошибка : " + item.User + " Невозможно сохранить в базу данных!");
                }
            }
            Console.Read();
        }
        static bool SaveToDatabase(SqlConnection con, Item obj)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO [Users] (Rank_, User_, Status_, Steps) Values(@Rank_, @User_, @Status_, @Steps)", con))
                {
                    cmd.Parameters.Add(new SqlParameter("@Rank_", obj.Rank));
                    cmd.Parameters.Add(new SqlParameter("@User_", obj.User));
                    cmd.Parameters.Add(new SqlParameter("@Status_", obj.Status));
                    cmd.Parameters.Add(new SqlParameter("@Steps", obj.Steps));
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception objEx)
            {
                return false;
            }
        }
    }

    public class Item
    {
        public int Rank { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public int Steps { get; set; }
    }

}

