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

        string[] mainMenuArr = { "Vehicle", "Inventory", "Repair", "Exit Program" };
        string[] vehicleMenuArr = { "List all vehicles", "Add new", "Update ", "Delete ", "Return to main" };
        string[] invMenuArr = { "View inventory", "Add new", "Update ", "Delete", "Return to main" };
        string[] repMenuArr = { "View all repairs", "Add new", "Update ", "Delete", "Return to main" };

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
                        PrintVehicles();
                        break;
                    case 2:
                        //ask vehicle model, make, year, condition
                        //put each to var
                        //insert to db
                        break;
                    case 3:
                        //ask vehicle id
                        //print vehicle record

                        //ask vehicle model, make, year, condition
                        //put each to var
                        //update to db
                        break;
                    case 4:
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

        /**
         *  prints the Vehicle menu options 
         *  loops if input is wrong or not within range
         */
        public static void MenuInventory(string eMsg = "", int eMin = 0, int eMax = 0)
        {
            PrintSubMenuHeader("INVENTORY MENU", eMsg: eMsg, eMin: 1, eMax: 5);

            Console.WriteLine("[1] View an inventory");
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
                        //ask vehicle id
                        //print inv record
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
                        //ask inv id
                        //print inv record
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
            PrintSubMenuHeader("CAR LIST");
            string cs = GetConnectionString("DatabaseMdf");
            SqlConnection conn = new SqlConnection(cs);
            string query = "Select vID,vmake,vmodel,vyear,vcondition from Vehicle";

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            Console.WriteLine($"{"ID",5} {"MAKE",-10} {"MODEL",-10} {"YEAR",-10} { "CONDITION",-10}");

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string make = reader.GetString(1);
                string model = reader.GetString(2);
                int year = reader.GetInt32(3);
                string cond = reader.GetString(4);

                Console.WriteLine($"{id,5} {make,-10} {model,-10} {year,-10} {cond,-10}");
            }
            conn.Close();
            WaitForAnyInput();
        }

        /**
         * basically pauses on a section.
         * waits for users input before continuing the code
         * used for debugging and for any section where we want user to finish reading whats on screen
         * 
         */
        public static void WaitForAnyInput()
        {
            Console.WriteLine("\nPress any key to go back: ");
            Console.ReadKey();
        }
    }

}
