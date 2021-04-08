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
        private static int vID = -1;
        private static int iID = -1;
        private static int rID = -1;
        private static List<string> list = new List<string>();
        private static int searchID = -1;
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
                        PrintSubMenuHeader("ADD NEW VEHICLE");
                        AskVehicleInfo(isUpdate: false);
                        break;
                    case 3:
                        vID = AskID("vehicle");
                        PrintAll('v', searchID: vID, pause: false);

                        do
                        {
                            Console.WriteLine("\nUpdate this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            AskVehicleInfo(isUpdate: true, vIDToUpdate: searchID);
                        }
                        break;
                    case 4:
                        searchID = AskID("vehicle");
                        PrintAll('v', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\n--------WARNING---------\nThe primary key of this record may be a foreign key in other database tables. \nIf you continue, data on those tables will also be deleted.");
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
         *  prints the Inventory menu options 
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
                        break;
                    case 2:
                        PrintSubMenuHeader("ADD NEW INVENTORY");
                        AskInvInfo(isUpdate: false);
                        break;
                    case 3:
                        searchID = AskID("inventory", inInv: true);
                        PrintAll('i', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\nUpdate this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            AskInvInfo(isUpdate: true, whichID: searchID);
                        }
                        break;
                    case 4:
                        searchID = AskID("inventory", inInv: true);
                        PrintAll('i', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\n--------WARNING---------\nThe primary key of this record may be a foreign key in other database tables. \nIf you continue, data on those tables will also be deleted.");
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



        /**
         *  prints the Repair menu options 
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
                        PrintSubMenuHeader("ADD NEW REPAIR");
                        AskRepInfo(isUpdate: false);
                        break;
                    case 3:
                        searchID = AskID("repair");
                        PrintAll('r', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\nUpdate this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            AskRepInfo(isUpdate: true, whichID: searchID);
                        }
                        break;
                    case 4:
                        searchID = AskID("repair", inInv: true);
                        PrintAll('r', searchID: searchID, pause: false);

                        do
                        {
                            Console.WriteLine("\n--------WARNING---------\nThe primary key of this record may be a foreign key in other database tables. \nIf you continue, data on those tables will also be deleted.");
                            Console.WriteLine("\nDelete this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            Delete(dbTable: "repair", colname: "rID", searchID);
                        }
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
         * asks for the vehicle infos then insert or update accordingly 
         */
        public static void AskVehicleInfo(string eMsg = "", bool isUpdate = false, int vIDToUpdate = -1)
        {
            Vehicle newV = new Vehicle();
            newV.Id = isUpdate == false ? -1 : vIDToUpdate;

            do
            {
                Console.WriteLine($"Enter Make: ");
                temp = Console.ReadLine().Trim().ToUpper();
                Console.WriteLine(temp.Length <= 0 ? $"Error:Value can't be empty.\n" : null);
            } while (temp.Length <= 0);

            newV.Make = temp;

            do
            {
                Console.WriteLine($"Model: ");
                temp = Console.ReadLine().Trim().ToUpper();
                Console.WriteLine(temp.Length <= 0 ? $"Error: Value can't be empty.\n" : null);
            } while (temp.Length <= 0);

            newV.Model = temp;

            do
            {
                Console.WriteLine($"Year(1900 - 2021): ");
                temp = Console.ReadLine().Trim().ToUpper();
                if (temp.Length > 0 && IsValidInt(temp))
                {
                    int tempyr = int.Parse(temp);
                    temp = tempyr >= 1900 && tempyr <= 2021 ? temp : "";
                }
                Console.WriteLine(temp.Length <= 0 ? $"Error: value must be between 19900 - 2021 only.\n" : null);
            } while (temp.Length <= 0);

            newV.Year = int.Parse(temp);

            do
            {
                Console.WriteLine($"Condition is new? (y/n): ");
                temp = Console.ReadLine().Trim().ToUpper();
                temp = temp.Equals("Y") ? "NEW" :
                                temp.Equals("N") ? "USED" : "";

                Console.WriteLine(temp.Length <= 0 ? $"Choose either y or n.\n" : null);
            } while (temp.Length <= 0);

            newV.Cond = temp;

            InsertUpdate(tbChar: 'v', vIN: newV, isUpdate: isUpdate);
        }

        /**
         * asks for the inventory infos then insert or update accordingly 
         */
        public static void AskInvInfo(string eMsg = "", bool isUpdate = false, int whichID = -1)
        {
            int stock;
            double price;
            double cost;

            if (isUpdate == false)
            {
                do
                {
                    Console.WriteLine("Enter vehicle ID: ");
                    temp = Console.ReadLine().Trim();
                    searchID = IsValidInt(temp) == true ? int.Parse(temp) : -1;
                    Console.WriteLine(searchID < 0 ? "Error: Enter a valid number.\n" : null);
                } while (searchID < 0);

                //check if vehicle record exist
                //if not found, go back so user dont get stuck
                if (GetID(SELECT: "vID", FROM: "vehicle", WHERE: "vID", searchID) < 0)
                {
                    MenuInventory("noV");
                }

                //check if vehicle already exit in inventory
                if (GetID(SELECT: "vID", FROM: "inventory", WHERE: "vID", searchID) >= 0)
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

            //InsertInventory(vID: searchID, stock, price, cost, isUpdate: isUpdate, whichID: whichID);
        }

        /**
         * asks for the repair infos then insert or update accordingly 
         */
        public static void AskRepInfo(string eMsg = "", bool isUpdate = false, int whichID = -1)
        {
            if (isUpdate == false)
            {
                do
                {
                    Console.WriteLine("Enter inventory ID: ");
                    temp = Console.ReadLine().Trim();
                    searchID = IsValidInt(temp) == true ? int.Parse(temp) : -1;
                    Console.WriteLine(searchID < 0 ? "Error: Enter a valid number.\n" : null);
                } while (searchID < 0);

                //check if inventory record exist
                //if not found, go back so user dont get stuck
                if (GetID(SELECT: "iID", FROM: "inventory", WHERE: "iID", searchID) < 0)
                {
                    MenuRepair("noI");
                }
            }

            do
            {
                Console.WriteLine("Part to be repaired: ");
                temp = Console.ReadLine().Trim().ToUpper();
                Console.WriteLine(temp.Length <= 0 ? "Error: Value can't be empty.\n" : "");
            } while (temp.Length <= 0);


            //InsertRepair(whatToRepair: temp, isUpdate: isUpdate, whichID: isUpdate == false ? searchID : whichID);

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

        /**
         * deletes record based on id then go back to menu
         */
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

            if (dbTable.StartsWith('v'))
            {
                MenuVehicle(eMsg: "dSucc");
            }
            else if (dbTable.StartsWith('i'))
            {
                MenuInventory(eMsg: "dSucc");
            }
            else
            {
                MenuRepair(eMsg: "dSucc");
            }
        }

        /**
         * insert/updates the repair record
         */
        /*
        static void InsertRepair(string whatToRepair, bool isUpdate = false, int whichID = -1)
        {
            string query = isUpdate == false ? "INSERT INTO Repair(iID, whatToRepair) VALUES (@whichID, @whatToRepair)" :
                                             "UPDATE Repair SET whatToRepair=@whatToRepair WHERE rID=@whichID ";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("whichID", whichID);
                cmd.Parameters.AddWithValue("whatToRepair", whatToRepair);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MenuRepair(eMsg: isUpdate == false ? "aSucc" : "uSucc");
        }
        */
        /**
         * insert/updates the record in the db table
         */
        static void InsertUpdate(char tbChar, Vehicle vIN = null, Inventory iIN = null, Repair rIN = null, bool isUpdate = false)
        {
            string query = "";

            if (tbChar == 'v')
            {
                query = isUpdate == false ? $"INSERT INTO vehicle (vmake, vmodel, vyear, vcondition) VALUES (@make, @model, @year, @cond)" :
                                            $"UPDATE vehicle SET vmake=@make, vmodel=@model, vyear=@year,vcondition=@cond WHERE vID=@vID";
            }
            else if (tbChar == 'i')
            {
                query = isUpdate == false ? $"INSERT INTO Inventory(vID, stock, price, cost) VALUES (@vID,@stock,@price,@cost)" :
                                           $"UPDATE Inventory SET stock=@stock, price=@price, cost=@cost WHERE vID=@vID";
            }
            else if (tbChar == 'r')
            {
                query = isUpdate == false ? $"INSERT INTO Repair(iID, whatToRepair) VALUES (@whichID, @whatToRepair)" :
                                           $"UPDATE Repair SET whatToRepair=@whatToRepair WHERE rID=@rID ";
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                if (tbChar == 'v')
                {
                    if (isUpdate == true)
                    {
                        cmd.Parameters.AddWithValue("vID", vIN.Id);
                    }
                    cmd.Parameters.AddWithValue("make", vIN.Make);
                    cmd.Parameters.AddWithValue("model", vIN.Model);
                    cmd.Parameters.AddWithValue("year", vIN.Year);
                    cmd.Parameters.AddWithValue("cond", vIN.Cond);
                }
                else if (tbChar == 'i')
                {
                    cmd.Parameters.AddWithValue("vID", iIN.VID);
                    cmd.Parameters.AddWithValue("stock", iIN.Stock);
                    cmd.Parameters.AddWithValue("price", iIN.Price);
                    cmd.Parameters.AddWithValue("cost", iIN.Cost);
                }
                else if (tbChar == 'r')
                {
                    cmd.Parameters.AddWithValue("rID", rIN.Id);
                    cmd.Parameters.AddWithValue("whatToRepair", rIN.WhatToRepair);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }


            if (tbChar == 'v')
            {
                MenuVehicle(eMsg: isUpdate == false ? "aSucc" : "uSucc");
            }
            else if (tbChar == 'i')
            {
                MenuInventory(eMsg: isUpdate == false ? "aSucc" : "uSucc");
            }
            else if (tbChar == 'r')
            {
                MenuRepair(eMsg: isUpdate == false ? "aSucc" : "uSucc");
            }
        }
        
        /**
         * insert/updates the inventory record
         */
        /*
        static void InsertInventory(int vID, int stock, double price, double cost, bool isUpdate = false, int whichID = -1)
        {
            string query = isUpdate == false ? "INSERT INTO Inventory(vID, stock, price, cost) VALUES (@vID,@stock,@price,@cost)" :
                                             "UPDATE Inventory SET stock=@stock, price=@price, cost=@cost WHERE vID=@whichID";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (isUpdate == false)
                {
                    cmd.Parameters.AddWithValue("vID", vID);
                }
                else
                {
                    cmd.Parameters.AddWithValue("whichID", whichID);
                }

                cmd.Parameters.AddWithValue("stock", stock);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("cost", cost);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            MenuInventory(eMsg: isUpdate == false ? "aSucc" : "uSucc");
        }
        */
        /**
         * asks for the tables id  and
         * checks if it exist in thhe db and
         * returns id if it exist, otherwise its -1
         */
        public static int AskID(string tbname, string eMsg = "", bool inInv = false)
        {
            string temptb = tbname.StartsWith('i') && inInv == true ? "vehicle" : tbname;

            PrintSubMenuHeader("Which record?", eMsg: eMsg);
            Console.WriteLine($"Enter {temptb} ID: ");
            temp = Console.ReadLine().Trim();

            if (IsValidInt(temp) == true)
            {
                searchID = int.Parse(temp);

                if (tbname.StartsWith('v') && GetID(SELECT: "vID", FROM: tbname, WHERE: "vID", searchID) >= 0 ||
                    tbname.StartsWith('i') && GetID(SELECT: "iID", FROM: tbname, WHERE: "vID", searchID) >= 0 ||
                    tbname.StartsWith('r') && GetID(SELECT: "rID", FROM: tbname, WHERE: "rID", searchID) >= 0)
                {
                    return searchID;
                }
                else
                {
                    if (tbname.StartsWith('v')) { MenuVehicle(eMsg: "noV"); }
                    else if (tbname.StartsWith('i')) { MenuInventory(eMsg: "noI"); }
                    else if (tbname.StartsWith('r')) { MenuRepair(eMsg: "noR"); }
                }
            }
            else
            {
                AskID(tbname: tbname, "emInt");
            }
            return -1;
        }

        /**
         * retrieves ID from db
         * checks if it exist in the db and
         * returns id if it exist, otherwise its -1
         */
        public static int GetID(string SELECT, string FROM, string WHERE, int searchID)
        {
            //storage for all id
            List<int> listID = new List<int>();

            string query = $"SELECT {SELECT} FROM {FROM} WHERE {WHERE}= @searchID";

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

            return listID.Count <= 0 ? -1 : listID[0];
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
                                         "SELECT iID, vID, stock, price, cost FROM Inventory WHERE vID=@searchID";

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
                Console.WriteLine("----------------------------------------------------------");
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
                Console.WriteLine("----------------------------------------------------------");
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int vid = reader.GetInt32(1);
                    int stock = reader.GetInt32(2);
                    double price = reader.GetDouble(3);
                    double cost = reader.GetDouble(4);

                    Console.WriteLine($"   {id,-5}  {vid,-10} {stock,-10}$ {price,-10}$ {cost,-10}");
                }
            }
            else if (rec == 'r')
            {
                Console.WriteLine($"   {"ID",-5} {"Inventory ID",-20} {"WHAT TO REPAIR",-20} ");
                Console.WriteLine("----------------------------------------------------------");
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
