using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace TeuCarRepairMgtSys
{

    class Program
    {
        private static string cs = GetConnectionString("DatabaseMdf");
        private static string temp = "";
        private static string errorMsg = "";
        private static int searchID = -1;

        private static string model = "";
        private static string make = "";
        private static int year = -1;
        private static string cond = "";

        private static int vID, stock, iID;
        private static double price, cost;
        private static string whatToRepair = "";

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
         *  prints the Vehicle menu options 
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
                        PrintSubMenuHeader("ADD NEW VEHICLE", eMsg: eMsg);
                        AskVehicleInfo();
                        InsertVehicle(make, model, year, cond);
                        break;
                    case 3:
                        searchID = AskID("vehicle", 'v');
                        PrintAll('v', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\nUpdate this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            AskVehicleInfo();
                            InsertVehicle(make, model, year, cond, isUpdate: true, whichVID: searchID);
                        }
                        break;
                    case 4:
                        searchID = AskID("vehicle", 'v');
                        PrintAll('v', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\n--------WARNING---------\nThe primary key of this record is a foreign key in other database tables. \nIf you continue, data on those tables will also be deleted.");
                            Console.WriteLine("\nDelete this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            Delete(dbTable: "vehicle", colname: "vID", searchID);
                        }
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

            Console.WriteLine("[1] View all inventory");
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
                        PrintAll('i');
                        Console.ReadKey();
                        break;
                    case 2:
                        PrintSubMenuHeader("ADD NEW INVENTORY", eMsg: eMsg);
                        AskInvInfo(action: 'a');
                        InsertInventory(vID, stock, price, cost);
                        break;
                    case 3:
                        searchID = AskID("inventory", 'i');
                        PrintAll('i', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\nUpdate this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            AskInvInfo(action: 'u');
                            InsertInventory(searchID, stock, price, cost, isUpdate: true, whichIID: searchID);
                        }
                        break;
                    case 4:
                        searchID = AskID("inventory", 'i');
                        PrintAll('i', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\n--------WARNING---------\nThe primary key of this record is a foreign key in other database tables. \nIf you continue, data on those tables will also be deleted.");
                            Console.WriteLine("\nDelete this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            Delete(dbTable: "inventory", colname: "vID", searchID);
                        }
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



        public static void AskInvInfo(char action, string eMsg = "")
        {
            if (action == 'a')
            {
                do
                {
                    Console.WriteLine("Enter vehicle ID: ");
                    temp = Console.ReadLine().Trim();
                    vID = IsValidInt(temp) == true ? int.Parse(temp) : -1;
                    Console.WriteLine(vID < 0 ? "Error: Enter a valid number.\n" : null);
                } while (vID < 0);

                //go back so user dont get stuck
                if (IDInDB("vID", vID) == false)
                {
                    MenuInventory("noV");
                }

                //check if vehicle already exit in inventory
                if (IDExistInThisTable('i', "vID", vID) == true)
                {
                    MenuInventory(eMsg: "idExist");
                }
            }
            do
            {
                Console.WriteLine("Enter how many stock: ");
                temp = Console.ReadLine().Trim();

                stock = IsValidInt(temp) == true ? int.Parse(temp) : -1;
                Console.WriteLine(stock < 0 ? "Error: Stock must be a positive integer.\n" : "");
            } while (stock < 0);

            do
            {
                Console.WriteLine("Enter price: ");
                temp = Console.ReadLine().Trim();

                price = IsValidDouble(temp) == true ? double.Parse(temp) : -1;
                Console.WriteLine(price < 0 ? "Error: Price must be a positive whole number or decimal.\n" : "");
            } while (price < 0);

            do
            {
                Console.WriteLine("Enter cost: ");
                temp = Console.ReadLine().Trim();
                if (IsValidDouble(temp) == true)
                {
                    cost = double.Parse(temp);
                }
                else
                {
                    cost = -1;
                    Console.WriteLine(cost < 0 ? "Error: Cost must be a positive whole number or decimal.\n" : "");
                }

            } while (cost < 0);
        }


        private static bool IsValidDouble(string inputValue)
        {
            //catch empty
            if (inputValue.Trim().Length == 0)
            {
                return false;
            }

            int num = 0;
            int period = 0;

            char[] charArr = inputValue.ToCharArray();

            foreach (var value in charArr)
            {
                if (Char.IsNumber(value))
                {
                    num++;
                }
                else if (value == '.')
                {
                    period++;
                }
            }

            return num + period == charArr.Length && period <= 1 ? true : false;
        }

        public static void Delete(string dbTable, string colname, int searchID, string eMsg = "")
        {
            string query = $"DELETE FROM {dbTable} WHERE {colname}=@searchID";


            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("searchID", searchID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }


            MenuVehicle(eMsg: "dSucc");
        }

        public static void AskVehicleInfo(string eMsg = "")
        {


            do
            {
                Console.WriteLine("Make: ");
                model = Console.ReadLine().Trim();
                Console.WriteLine(model.Length == 0 ? "Error: Make can't be empty.\n" : "");
            } while (model.Length == 0);

            do
            {
                Console.WriteLine("Model: ");
                make = Console.ReadLine().Trim();
                Console.WriteLine(make.Length == 0 ? "Error: Model can't be empty.\n" : "");
            } while (make.Length == 0);

            do
            {
                Console.WriteLine("Year (1900-2021): ");
                temp = Console.ReadLine().Trim();
                if (IsValidInt(temp) == true)
                {
                    int tempyr = int.Parse(temp);
                    year = tempyr >= 1900 && tempyr <= 2021 ? tempyr : -1;
                }

                Console.WriteLine(year < 0 ? "Error: Enter a valid year between 1900-2021.\n" : "");

            } while (year < 0);

            do
            {
                Console.WriteLine("Condition is new? (y/n): ");
                temp = Console.ReadLine().ToLower();

                cond = temp.Equals("y") ? "new" :
                       temp.Equals("n") ? "used" : "";

                Console.WriteLine(cond.Length == 0 ? "Error: Choose either y or n.\n" : "");

            } while (cond.Length == 0);

        }

        static void InsertVehicle(string make, string model, int year, string cond, bool isUpdate = false, int whichVID = -1)
        {
            string query = isUpdate == false ? "INSERT INTO Vehicle(vmake, vmodel, vyear, vcondition) VALUES (@make,@model,@year,@cond)" :
                "UPDATE Vehicle SET vmake=@make, vmodel=@model, vyear=@year,vcondition=@cond WHERE vID=@whichVID";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("make", make);
                cmd.Parameters.AddWithValue("model", model);
                cmd.Parameters.AddWithValue("year", year);
                cmd.Parameters.AddWithValue("cond", cond);
                if (isUpdate == true)
                {
                    cmd.Parameters.AddWithValue("whichVID", whichVID);
                }
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MenuVehicle(eMsg: isUpdate == false ? "aSucc" : "uSucc");
        }

        static void InsertInventory(int vID, int stock, double price, double cost, bool isUpdate = false, int whichIID = -1)
        {
            string query = isUpdate == false ? "INSERT INTO Inventory(vID, stock, price, cost) VALUES (@vID,@stock,@price,@cost)" :
                 "UPDATE Inventory SET vID=@vID, stock=@stock, price=@price, cost=@cost WHERE iID=@whichIID";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("vID", vID);
                cmd.Parameters.AddWithValue("stock", stock);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("cost", cost);
                if (isUpdate == true)
                {
                    cmd.Parameters.AddWithValue("whichIID", whichIID);
                }
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MenuInventory(eMsg: "aSucc");
        }

        public static int AskID(string tbname, char dbTable, string eMsg = "")
        {
            PrintSubMenuHeader("Which record?", eMsg: eMsg);
            Console.WriteLine($"Enter {tbname} ID: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                int searchV = int.Parse(temp);

                if (dbTable == 'v' && IDInDB("vID", searchV) == true ||
                    dbTable == 'i' && IDInDB("iID", searchV) == true ||
                    dbTable == 'r' && IDInDB("rID", searchV) == true)
                {
                    return searchV;
                }
                else
                {
                    if (dbTable == 'v')
                    {
                        MenuVehicle(eMsg: "noV");
                    }
                    else if (dbTable == 'i')
                    {
                        MenuInventory(eMsg: "noI");
                    }
                    else
                    {
                        MenuRepair(eMsg: "noR");
                    }
                }
            }
            else
            {
                AskID(tbname: tbname, dbTable: dbTable, "emInt");
            }
            return -1;
        }

        public static bool IDExistInThisTable(char dbTable, string colID, int searchID)
        {
            //storage for all id
            List<int> listID = new List<int>();

            string tbname = "";

            if (dbTable == 'v')
            {
                tbname = "vID";
            }
            else if (dbTable == 'i')
            {
                tbname = "iID";
            }
            else
            {
                tbname = "rID";
            }

            string query = $"SELECT {tbname} FROM Inventory WHERE {colID}= @searchID";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("searchID", searchID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listID.Add(reader.GetInt32(0));
                }
            }

            return listID.Contains(searchID);
        }

        public static bool IDInDB(string vID_iID_rID, int searchID)
        {
            //storage for all vid
            List<int> listID = new List<int>();

            string query = "";
            string tbname = "";

            if (vID_iID_rID.Equals("vID"))
            {
                tbname = "vehicle";
            }
            else if (vID_iID_rID.Equals("iID"))
            {
                tbname = "inventory";
            }
            else
            {
                tbname = "repair";
            }
            query = $"SELECT {vID_iID_rID} FROM {tbname} WHERE {vID_iID_rID} = @searchID";


            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("searchID", searchID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listID.Add(reader.GetInt32(0));
                }
            }

            return listID.Contains(searchID);
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
            else if (msgType.Equals("noI"))
            {
                errorMsg = $"ERROR: Invalid input. No such record in the Inventory database.";
            }
            else if (msgType.Equals("noR"))
            {
                errorMsg = $"ERROR: Invalid input. No such record in the Repair database.";
            }
            else if (msgType.Equals("aSucc"))
            {
                errorMsg = $"System Message: Add record successful";
            }
            else if (msgType.Equals("uSucc"))
            {
                errorMsg = $"System Message: Update successful";
            }
            else if (msgType.Equals("dSucc"))
            {
                errorMsg = $"System Message: Delete successful.";
            }
            else if (msgType.Equals("idExist"))
            {
                errorMsg = $"System Message: Record exist for that ID, please use update instead.";
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

            if (!string.IsNullOrEmpty(inputValue))
            {
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
                        return true;
                    }
                }
            }

            return false;
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
        static void PrintAll(char rec, int searchID = -1, bool pause = true)
        {
            //assign title
            string title = "";

            if (rec == 'v') { title = searchID < 0 ? "VEHICLE LIST" : "RETRIEVED RECORD"; }
            else if (rec == 'i') { title = searchID < 0 ? "INVENTORY LIST" : "RETRIEVED RECORD"; }
            else if (rec == 'r') { title = searchID < 0 ? "REPAIR LIST" : "RETRIEVED RECORD"; }

            //print title
            PrintSubMenuHeader(title);

            SqlConnection conn = new SqlConnection(cs);

            //assign query
            string query = "";

            if (rec == 'v')
            {
                query = searchID < 0 ? "SELECT vID, vmake, vmodel, vyear, vcondition FROM Vehicle" :
                                         "SELECT vID, vmake, vmodel, vyear, vcondition FROM Vehicle WHERE vID=@searchID";
            }
            else if (rec == 'i')
            {
                query = searchID < 0 ? "SELECT iID, vID, stock, price, cost FROM Inventory" :
                                         "SELECT iID, vID, stock, price, cost FROM Inventory WHERE iID=@searchID";

            }
            else if (rec == 'r')
            {
                query = searchID < 0 ? "SELECT rID, iID, whatToRepair FROM Repair" :
                                        "SELECT rID, iID, whatToRepair FROM Repair WHERE rID=@searchID";
            }

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            if (searchID >= 0)
            {
                cmd.Parameters.AddWithValue("searchID", searchID);
            }
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
                    int iID = reader.GetInt32(1);
                    string whatToRapair = reader.GetString(2);

                    Console.WriteLine($"   {id,-5} {iID,-20} {whatToRapair,-20}");
                }
            }
            conn.Close();

            if (pause == true)
            {
                Console.ReadKey();
            }
        }




    }

}
