using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;

namespace TeuCarRepairMgtSys
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintVehicles();
        }

        static string GetConnectionString(string connectionStringName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("config.json");
            IConfiguration config = configurationBuilder.Build();
            return config["ConnectionStrings:"+ connectionStringName];
        }


        static void PrintVehicles()
        {
            string cs = GetConnectionString("DatabaseMdf");
            Console.WriteLine(cs);
            SqlConnection conn = new SqlConnection(cs);
            string query = "Select vID,vmake,vmodel,vyear,vcondition from Vehicle";

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                Console.WriteLine($"{id,5} {firstName,-15} {lastName,-15}");
            }
            conn.Close();

        }
    }

}

