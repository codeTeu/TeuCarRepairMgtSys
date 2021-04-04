using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;

namespace TeuCarRepairMgtSys
{

    class Program
    {
        private static string temp;
        private static string errorMsg;

        static void Main(string[] args)
        {
            MenuMain();
        }

        /**
         *  prints the main menu options of the system
         *  loops if input is wrong or not within range
         */
        public static void MenuMain(string eMsg = "", int eMin = 0, int eMax = 0)
        {
            PrintSubMenuHeader("MAIN MENU", eMsg: eMsg, eMin: 1, eMax: 4);

            Console.WriteLine("[1] Vehicle");
            Console.WriteLine("[2] Inventory");
            Console.WriteLine("[3] Repair");
            Console.WriteLine("[4] Exit Program");
            Console.WriteLine("\nSelect a number: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                switch (int.Parse(temp))
                {
                    case 1:
                        //MenuVehicle();
                        break;
                    case 2:
                        //MenuInventory();
                        break;
                    case 3:
                        //MenuRepair();
                        break;
                    case 4:
                        PrintExitMsg();
                        Environment.Exit(1);
                        break;
                    default:
                        MenuMain(eMsg: "emRange"); //input not in range
                        break;
                }
            }
            else
            {
                MenuMain(eMsg: "emInt");
            }

            MenuMain();
        }


        /**
         * prints the system header title
         */
        public static void PrintTitleHeader()
        {
            Console.Clear();
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("++++++++++ TEU CAR REPAIR MANAGEMENT SYSTEM ++++++++++");
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++\n");
        }

        /**
         * prints the clossing messaage
         */
        public static void PrintExitMsg()
        {
            PrintTitleHeader();
            Console.WriteLine("----------------- SYSTEM CLOSED ----------------");
        }

        /**
         * prints the header for each submenu
         */
        public static void PrintSubMenuHeader(string title, string eMsg = "", int eMin = 0, int eMax = 0)
        {
            AssignErrorMessage(eMsg, eMin: eMin, eMax: eMax);

            Console.Clear();
            PrintTitleHeader();
            Console.WriteLine($"{errorMsg}\n");

            Console.WriteLine($"---------------- {title} ----------------\n");
        }


        /**
         * assigns the specified error message 
         */
        public static void AssignErrorMessage(string msgType, int eMin = 0, int eMax = 0)
        {

            if (msgType.Equals("emEmpty"))
            {
                errorMsg = $"ERROR: Input can't be empty.";
            }
            else if (msgType.Equals("emInt"))
            {
                errorMsg = $"ERROR: Invalid input. Please enter a valid number.";
            }

            else if (msgType.Equals("emRange"))
            {
                errorMsg = $"ERROR: Invalid input. Please enter a number from {eMin} - {eMax}.";
            }
            else
            {
                errorMsg = "";
            }

        }


        /**
         * checks value if its a number
         * 
         * returns false if value is empty, has a non number 
         */
        public static bool IsValidInt(string inputValue)
        {
            bool result = false;

            //catch empty
            if (string.IsNullOrEmpty(inputValue))
            {
                return result;
            }

            //check each char
            char[] charArr = inputValue.ToCharArray();

            foreach (var value in charArr)
            {
                if (Char.IsNumber(value) == false)
                {
                    return false;
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }


        static string GetConnectionString(string connectionStringName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("config.json");
            IConfiguration config = configurationBuilder.Build();
            return config["ConnectionStrings:" + connectionStringName];
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

