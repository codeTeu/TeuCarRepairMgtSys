using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace TeuCarRepairMgtSys
{

    class Program
    {
        private static string temp;
        private static string errorMsg;

        /*
        string[] mainMenuArr = { "Vehicle", "Inventory", "Repair", "Exit Program" };
        string[] vehicleMenuArr = { "List all vehicles", "Add new", "Update ", "Delete ", "Return to main" };
        string[] invMenuArr = { "View inventory", "Add new", "Update ", "Delete", "Return to main" };
        string[] repMenuArr = { "View all repairs", "Add new", "Update ", "Delete", "Return to main" };
        */

        static void Main(string[] args)
        {
            MenuMain(eMin: 1, eMax: 4);
        }

        /**
         *  prints the main menu options of the system
         *  loops if input is wrong or not within range
         */
        public static void MenuMain(string eMsg = "", int eMin = 0, int eMax = 0)
        {

            PrintSubMenuHeader("MAIN MENU", eMsg: eMsg, eMin: 1, eMax: 4);

            Console.WriteLine("[1] Vehicles");
            Console.WriteLine("[2] Inventory");
            Console.WriteLine("[3] Repair");
            Console.WriteLine("[4] Exit");
            Console.WriteLine("\nSelect a number: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                switch (int.Parse(temp))
                {
                    case 1:
                        MenuVehicle();
                        break;
                    case 2:
                        MenuInventory();
                        break;
                    case 3:
                        MenuRepair();
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
         *  prints the Vehihcle menu options 
         *  loops if input is wrong or not within range
         */
        public static void MenuVehicle(string eMsg = "", int eMin = 0, int eMax = 0)
        {
            PrintSubMenuHeader("VEHICLE MENU", eMsg: eMsg, eMin: 1, eMax: 5);

            Console.WriteLine("[1] List all vehicles");
            Console.WriteLine("[2] Add new");
            Console.WriteLine("[3] Update");
            Console.WriteLine("[4] Delete");
            Console.WriteLine("[5] Return to main");
            Console.WriteLine("\nSelect a number: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                switch (int.Parse(temp))
                {
                    case 1:
                        PrintAll('v');
                        break;
                    case 2:
                        AskVehicleInfo(out string model, out string make, out int year, out string cond);
                        InsertVehicle(make, model, year, cond);
                        break;
                    case 3:
                        //ask vehicle id
                        //print vehicle record

                        //ask vehicle model, make, year, condition
                        //put each to var
                        //update to db
                        break;
                    case 4:
                        int vID = AskVID();
                        DeleteVehicle();
                        //ask vehicle id
                        //print vehicle record

                        //Ask if sure delete
                        //delete from db
                        break;
                    case 5:
                        MenuMain();
                        break;
                    default:
                        MenuVehicle(eMsg: "emRange"); //input not in range
                        break;
                }
            }
            else
            {
                MenuVehicle(eMsg: "emInt");
            }
            MenuVehicle();
        }
        public static void DeleteVehicle(string eMsg = "")
        { 
        
        }
        public static void AskVehicleInfo(out string model, out string make, out int year, out string cond, string eMsg = "")
        {
            year = 0;

            PrintSubMenuHeader("ADD NEW VEHICLE", eMsg: eMsg);
            do
            {
                Console.WriteLine("Model: ");
                model = Console.ReadLine().Trim();
                Console.WriteLine(model.Length == 0 ? "Error: Model can't be empty.\n" : "");
            } while (model.Length == 0);

            do
            {
                Console.WriteLine("Make: ");
                make = Console.ReadLine().Trim();
                Console.WriteLine(make.Length == 0 ? "Error: Make can't be empty.\n" : "");
            } while (make.Length == 0);

            do
            {
                Console.WriteLine("year (1990-2021): ");
                temp = Console.ReadLine().Trim();
                if (IsValidInt(temp) == true)
                {
                    int tempyr = int.Parse(temp);
                    year = tempyr >= 1990 && tempyr <= 2021 ? int.Parse(temp) : 0;
                }

                Console.WriteLine(year == 0 ? "Error: Enter a valid year between 1990-2021.\n" : "");

            } while (year == 0);

            do
            {
                Console.WriteLine("Condition is new? (y/n): ");
                temp = Console.ReadLine().ToLower();

                cond = temp.Equals("y") ? "new" :
                       temp.Equals("n") ? "used" : "";

                Console.WriteLine(cond.Length == 0 ? "Error: Choose either y or n.\n" : "");

            } while (cond.Length == 0);

        }

        static void InsertVehicle(string make, string model, int year, string cond)
        {
            string cs = GetConnectionString("DatabaseMdf");
            string query = "INSERT INTO Vehicle(vmake, vmodel, vyear, vcondition) VALUES (@make,@model,@year,@cond)";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("make", make);
                cmd.Parameters.AddWithValue("model", model);
                cmd.Parameters.AddWithValue("year", year);
                cmd.Parameters.AddWithValue("cond", cond);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MenuVehicle(eMsg: "vSucc");
        }


        /**
         *  prints the Vehicle menu options 
         *  loops if input is wrong or not within range
         */
        public static void MenuInventory(string eMsg = "", int eMin = 0, int eMax = 0)
        {
            PrintSubMenuHeader("INVENTORY MENU", eMsg: eMsg, eMin: 1, eMax: 5);

            Console.WriteLine("[1] View all inventory");
            Console.WriteLine("[2] Get specific inventory");
            Console.WriteLine("[3] Add new");
            Console.WriteLine("[4] Update");
            Console.WriteLine("[5] Delete");
            Console.WriteLine("[6] Return to main");
            Console.WriteLine("\nSelect a number: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                switch (int.Parse(temp))
                {
                    case 1:
                        PrintAll('i');
                        break;
                    case 2:
                        //ask vehicleID
                        //asign var 
                        //print list
                        int validVID = AskVID();
                        if (validVID >= 0)
                        {
                            PrintAll('i', vID: validVID);
                        }

                        break;
                    case 3:
                        //ask vehicleID
                        //check if vID exist in vehicle db - loop

                        //numberOnHand, price, cost
                        //put each to var
                        //insert to db
                        break;
                    case 4:
                        //ask vehicle id

                        //ask vehicle model, make, year, condition
                        //put each to var
                        //update to db
                        break;
                    case 5:
                        //ask inv id
                        //delete from db
                        break;
                    case 6:
                        MenuMain();
                        break;
                    default:
                        MenuInventory(eMsg: "emRange"); //input not in range
                        break;
                }
            }
            else
            {
                MenuInventory(eMsg: "emInt");
            }
            MenuInventory();
        }

        public static int AskVID(string eMsg = "")
        {
            PrintSubMenuHeader("Retrieve Record", eMsg: eMsg);
            Console.WriteLine("Enter vehicle ID: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                int searchV = int.Parse(temp);
                if (VehicleInDB(searchV) == true)
                {
                    return searchV;
                }
                else
                {
                    MenuVehicle(eMsg: "noV");
                }
            }
            else
            {
                AskVID("emInt");
            }
            return -1;
        }
        public static bool VehicleInDB(int vID)
        {
            //storage for all vid
            List<int> listID = new List<int>();

            //connect db
            string cs = GetConnectionString("DatabaseMdf");
            string query = "SELECT vID, vmake, vmodel, vyear, vcondition FROM Vehicle WHERE vID = @vID";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("vID", vID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listID.Add(reader.GetInt32(0));
                }
            }

            return listID.Contains(vID);
        }
        /**
         *  prints the Vehihcle menu options 
         *  loops if input is wrong or not within range
         */
        public static void MenuRepair(string eMsg = "", int eMin = 0, int eMax = 0)
        {
            PrintSubMenuHeader("REPAIR MENU", eMsg: eMsg, eMin: 1, eMax: 5);

            Console.WriteLine("[1] View all");
            Console.WriteLine("[2] Add new");
            Console.WriteLine("[3] Update");
            Console.WriteLine("[4] Delete");
            Console.WriteLine("[5] Return to main");
            Console.WriteLine("\nSelect a number: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                switch (int.Parse(temp))
                {
                    case 1:
                        PrintAll('r');
                        break;
                    case 2:
                        //ask vehicleID, numberOnHand, price, cost
                        //put each to var
                        //insert to db
                        break;
                    case 3:
                        //ask vehicle id
                        //print inv record

                        //ask vehicle model, make, year, condition
                        //put each to var
                        //update to db
                        break;
                    case 4:
                        //ask inv id
                        //print vehicle record

                        //Ask if sure delete
                        //delete from db
                        break;
                    case 5:
                        MenuMain();
                        break;
                    default:
                        MenuRepair(eMsg: "emRange"); //input not in range
                        break;
                }
            }
            else
            {
                MenuRepair(eMsg: "emInt");
            }
            MenuRepair();
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

            Console.WriteLine($"---------------------- {title} ----------------------\n");
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
            else if (msgType.Equals("noV"))
            {
                errorMsg = $"ERROR: Invalid input. No such record in the Vehicle database.";
            }
            else if (msgType.Equals("vSucc"))
            {
                errorMsg = $"System Message: Vehicle successfully added.";
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

        /**
         * rertrieves conncetion string from json file
         */
        static string GetConnectionString(string connectionStringName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("config.json");
            IConfiguration config = configurationBuilder.Build();
            return config["ConnectionStrings:" + connectionStringName];
        }

        /**
         * prints all records in the db
         */
        static void PrintAll(char rec, int vID = -1)
        {
            //assign title
            string title = "";

            if (rec == 'v') { title = "VEHICLE LIST"; }
            else if (rec == 'i') { title = "INVENTORY"; }
            else if (rec == 'r') { title = "REPAIR LIST"; }

            //print title
            PrintSubMenuHeader(title);

            //connect db
            string cs = GetConnectionString("DatabaseMdf");
            SqlConnection conn = new SqlConnection(cs);

            //assign query
            string query = "";

            if (rec == 'v')
            {
                query = "SELECT vID, vmake, vmodel, vyear, vcondition FROM Vehicle";
            }
            else if (rec == 'i')
            {
                query = vID < 0 ? "SELECT iID, vID, stock, price, cost FROM Inventory" :
                    "SELECT iID, vID, stock, price, cost FROM Inventory WHERE vID=@vID";

            }
            else if (rec == 'r')
            {
                query = "SELECT rID, iID, whatToRepair FROM Repair";
            }
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("vID", vID);
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (rec == 'v')
            {
                Console.WriteLine($"   {"ID",-5} {"MAKE",-10} {"MODEL",-10} {"YEAR",-10} { "CONDITION",-10}");

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string make = reader.GetString(1);
                    string model = reader.GetString(2);
                    int year = reader.GetInt32(3);
                    string cond = reader.GetString(4);

                    Console.WriteLine($"   {id,-5} {make,-10} {model,-10} {year,-10} {cond,-10}");
                }
            }
            else if (rec == 'i')
            {
                Console.WriteLine($"   {"ID",-5} {"VID",-10} {"STOCK",-10} {"PRICE",-10} { "COST",-10}");

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int vid = reader.GetInt32(1);
                    int stock = reader.GetInt32(2);
                    double price = reader.GetDouble(3);
                    double cost = reader.GetDouble(4);

                    Console.WriteLine($"   {id,-5} {vid,-10} {stock,-10} {price,-10} {cost,-10}");
                }
            }
            else if (rec == 'r')
            {
                Console.WriteLine($"   {"ID",-5} {"Inventory ID",-20} {"WHAT TO REPAIR",-20} ");

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string iID = reader.GetString(1);
                    string whatToRapair = reader.GetString(2);

                    Console.WriteLine($"   {id,-5} {iID,-20} {whatToRapair,-20}");
                }
            }
            conn.Close();
            Console.ReadKey();
        }



        static void PrintInventory()
        {
            PrintSubMenuHeader("INVENTORY OF VEHICLE");
            string cs = GetConnectionString("DatabaseMdf");
            SqlConnection conn = new SqlConnection(cs);
            string query = "Select vID, vmake, vmodel,  vyear, vcondition from Vehicle";

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            Console.WriteLine($"   {"ID",-5} {"MAKE",-10} {"MODEL",-10} {"YEAR",-10} { "CONDITION",-10}");
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string make = reader.GetString(1);
                string model = reader.GetString(2);
                int year = reader.GetInt32(3);
                string cond = reader.GetString(4);

                Console.WriteLine($"   {id,-5} {make,-10} {model,-10} {year,-10} {cond,-10}");
            }
            conn.Close();
            Console.ReadKey();
        }


    }

}
