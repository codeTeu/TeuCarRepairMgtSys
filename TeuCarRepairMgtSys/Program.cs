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
        private static int id = -1;

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

            if (IsValid(temp))
            {
                switch (int.Parse(temp))
                {
                    case 1:
                        Menu("vehicle");
                        break;
                    case 2:
                        Menu("inventory");
                        break;
                    case 3:
                        Menu("repair");
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




        public static void Menu(string menuName, string eMsg = "")
        {
            char menuChar = menuName.ToCharArray()[0];

            PrintSubMenuHeader($"{menuName.ToUpper()} MENU", eMsg: eMsg, eMin: 1, eMax: 5);

            Console.WriteLine($"[1] View all {menuName}");
            Console.WriteLine("[2] Add new");
            Console.WriteLine("[3] Update");
            Console.WriteLine("[4] Delete");
            Console.WriteLine("[5] Return to main");
            Console.WriteLine("\nSelect a number: ");
            temp = Console.ReadLine().Trim();

            if (IsValid(temp))
            {
                switch (int.Parse(temp))
                {
                    case 1:
                        PrintAll(menuName.ToCharArray()[0]);
                        break;
                    case 2:
                        PrintSubMenuHeader($"ADD NEW {menuName}");
                        if (menuName.StartsWith('v'))
                        {
                            AskVehicleInfo(isUpdate: false);
                        }
                        else if (menuName.StartsWith('i'))
                        {
                            AskInvInfo(isUpdate: false);
                        }
                        else
                        {
                            AskRepInfo(isUpdate: false);
                        }
                        break;
                    case 3:
                        id = AskID(menuName, inInv: menuChar == 'i' ? true : false);
                        PrintAll(rec: menuChar, searchID: id, pause: false);

                        do
                        {
                            Console.WriteLine("\nUpdate this record? (y/n)");
                            temp = Console.ReadLine().ToLower();
                        } while (!temp.Equals("y") && !temp.Equals("n"));

                        if (temp.Equals("y"))
                        {
                            if (menuChar == 'v')
                            {
                                AskVehicleInfo(isUpdate: true, vIDToUpdate: id);
                            }
                            else if (menuChar == 'i')
                            {
                                id = GetID(FROM: menuName, WHERE: "id", id); //vid
                                AskInvInfo(isUpdate: true, vIDToUpdate: id);
                            }
                            else
                            {
                                AskRepInfo(isUpdate: true, whichID: id);
                            }

                        }
                        break;
                    case 4:
                        id = AskID(menuName, inInv: menuChar == 'i' ? true : false);
                        PrintAll(menuChar, searchID: id, pause: false);
                        AskDelete(dbTable: menuName, colname: menuChar == 'i' ? "vID" : "id", id);
                        break;
                    case 5:
                        MenuMain();
                        break;
                    default:
                        Menu(menuName, eMsg: "emRange"); //input not in range
                        break;
                }
            }
            else
            {
                Menu(menuName, eMsg: "emInt");
            }
            Menu(menuName);
        }


        public static void AskDelete(string dbTable = "", string colname = "", int id = -1)
        {
            do
            {
                Console.WriteLine("\n--------WARNING---------\nThe primary key of this record may be a foreign key in other database tables. \nIf you continue, data on those tables will also be deleted.");
                Console.WriteLine("\nDelete this record? (y/n)");
                temp = Console.ReadLine().ToLower();
            } while (!temp.Equals("y") && !temp.Equals("n"));

            if (temp.Equals("y"))
            {
                id = dbTable.StartsWith('i') ? GetID(FROM: "inventory", WHERE: "vID", id) : id;
                Delete(dbTable: dbTable, colname: colname, searchID: id);
            }
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
                if (temp.Length > 0 && IsValid(temp))
                {
                    int tempyr = int.Parse(temp);
                    temp = tempyr >= 1900 && tempyr <= 2021 ? temp : "";
                }
                else
                {
                    temp = "";
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
         * if vid exist, tell user to update record instead
         */
        public static void AskInvInfo(string eMsg = "", bool isUpdate = false, int vIDToUpdate = -1)
        {
            Inventory newInv = new Inventory();

            if (isUpdate == false)
            {
                do
                {
                    Console.WriteLine("Enter vehicle ID: ");
                    temp = Console.ReadLine().Trim();
                    newInv.VID = IsValid(temp) ? int.Parse(temp) : -1;
                    Console.WriteLine(newInv.VID < 0 ? "Error: Enter an existing vehicle id number.\n" : null);
                } while (newInv.VID < 0);

                //check if vehicle record exist
                //if not found, go back so user dont get stuck
                if (GetID(FROM: "vehicle", WHERE: "id", newInv.VID) < 0)
                {
                    Menu("inventory", "noV");
                }

                //check if vehicle already in inventory
                //if found tell user to go update instead
                if (GetID(FROM: "inventory", WHERE: "vID", newInv.VID) >= 0)
                {
                    Menu("inventory", eMsg: "idExist");
                }
            }
            else
            {
                newInv.VID = vIDToUpdate;
            }

            do
            {
                Console.WriteLine("Enter how many stock: ");
                temp = Console.ReadLine().Trim();
                newInv.Stock = IsValid(temp, type: 'i') ? int.Parse(temp) : -1;
                Console.WriteLine(newInv.Stock < 0 ? "Error: Enter a positive whole number only.\n" : "");
            } while (newInv.Stock < 0);

            do
            {
                Console.WriteLine("Enter price: ");
                temp = Console.ReadLine().Trim();
                newInv.Price = IsValid(temp, type: 'd') ? double.Parse(temp) : -1;
                Console.WriteLine(newInv.Price < 0 ? "Error: Enter a positive whole number or decimal only.\n" : "");
            } while (newInv.Price < 0);

            do
            {
                Console.WriteLine("Enter cost: ");
                temp = Console.ReadLine().Trim();
                newInv.Cost = IsValid(temp, type: 'd') ? double.Parse(temp) : -1;
                Console.WriteLine(newInv.Cost < 0 ? "Error: Enter a positive whole number or decimal only.\n" : "");
            } while (newInv.Cost < 0);

            InsertUpdate(tbChar: 'i', iIN: newInv, isUpdate: isUpdate);
        }

        /**
         * asks for the repair infos then insert or update accordingly 
         */
        public static void AskRepInfo(string eMsg = "", bool isUpdate = false, int whichID = -1)
        {
            Repair newR = new Repair();

            do
            {
                Console.WriteLine("Enter inventory ID: ");
                temp = Console.ReadLine().Trim();
                newR.IID = IsValid(temp) ? int.Parse(temp) : -1;
                Console.WriteLine(newR.IID < 0 ? "Error: Enter an existing inventory id number.\n" : null);
            } while (newR.IID < 0);

            //check if inventory record exist
            //if not found, go back so user dont get stuck
            if (GetID(FROM: "inventory", WHERE: "id", newR.IID) < 0)
            {
                Menu("repair", "noI");
            }

            do
            {
                Console.WriteLine("Part to be repaired: ");
                newR.WhatToRepair = Console.ReadLine().Trim().ToUpper();
                Console.WriteLine(temp.Length <= 0 ? "Error: Value can't be empty.\n" : "");
            } while (newR.WhatToRepair.Length <= 0);


            InsertUpdate(tbChar: 'r', rIN: newR, isUpdate: isUpdate);

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
                Menu("vehicle", eMsg: "dSucc");
            }
            else if (dbTable.StartsWith('i'))
            {
                Menu("inventory", eMsg: "dSucc");
            }
            else
            {
                Menu("repair", eMsg: "dSucc");
            }
        }


        /**
         * insert/updates the record in the db table
         */
        static void InsertUpdate(char tbChar, Vehicle vIN = null, Inventory iIN = null, Repair rIN = null, bool isUpdate = false)
        {
            string query = "";

            if (tbChar == 'v')
            {
                query = isUpdate == false ? $"INSERT INTO vehicle (vmake, vmodel, vyear, vcondition) VALUES (@make, @model, @year, @cond)" :
                                            $"UPDATE vehicle SET vmake=@make, vmodel=@model, vyear=@year,vcondition=@cond WHERE id=@id";
            }
            else if (tbChar == 'i')
            {
                query = isUpdate == false ? $"INSERT INTO Inventory(vID, stock, price, cost) VALUES (@vID,@stock,@price,@cost)" :
                                           $"UPDATE Inventory SET stock=@stock, price=@price, cost=@cost WHERE vID=@vID";
            }
            else if (tbChar == 'r')
            {
                query = isUpdate == false ? $"INSERT INTO Repair(iID, whatToRepair) VALUES (@iID, @whatToRepair)" :
                                           $"UPDATE Repair SET iID=@iID, whatToRepair=@whatToRepair WHERE id=@id ";
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                if (tbChar == 'v')
                {
                    if (isUpdate == true)
                    {
                        cmd.Parameters.AddWithValue("id", vIN.Id);
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
                    if (isUpdate == true)
                    {
                        cmd.Parameters.AddWithValue("id", rIN.Id);
                    }
                    cmd.Parameters.AddWithValue("iID", rIN.IID);
                    cmd.Parameters.AddWithValue("whatToRepair", rIN.WhatToRepair);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }


            if (tbChar == 'v')
            {
                Menu("vehicle", eMsg: isUpdate == false ? "aSucc" : "uSucc");
            }
            else if (tbChar == 'i')
            {
                Menu("inventory", eMsg: isUpdate == false ? "aSucc" : "uSucc");
            }
            else if (tbChar == 'r')
            {
                Menu("repair", eMsg: isUpdate == false ? "aSucc" : "uSucc");
            }
        }

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

            if (IsValid(temp))
            {
                id = GetID(FROM: tbname, WHERE: tbname.StartsWith('i') ? "vID" : "id", int.Parse(temp));

                if (id >= 0)
                {
                    return id;
                }
                else
                {
                    if (tbname.StartsWith('v'))
                    {
                        Menu("vehicle", eMsg: "noV");
                    }
                    else if (tbname.StartsWith('i'))
                    {
                        Menu("inventory", eMsg: "noI");
                    }
                    else if (tbname.StartsWith('r'))
                    {
                        Menu("repair", eMsg: "noR");
                    }
                }
            }
            else
            {
                AskID(tbname: tbname, "emInt", inInv: inInv);
            }
            return -1;
        }

        /**
         * retrieves ID from db
         * returns id if it exist, otherwise its -1
         */
        public static int GetID(string FROM, string WHERE, int searchID)
        {
            //storage for all id
            List<int> listID = new List<int>();

            string query = $"SELECT id FROM {FROM} WHERE {WHERE}= @searchID";

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
         * prints the closing messaage
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
         * checks value if its a valid int or double
         */
        public static bool IsValid(string inputValue, char type = 'i')
        {
            try
            {
                if (type == 'd')
                {
                    double value = double.Parse(inputValue);
                }
                else
                {
                    int value = int.Parse(inputValue);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /**
         * retrieves connetion string from json file
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
                query = searchID < 0 ? "SELECT id, vmake, vmodel, vyear, vcondition FROM Vehicle" :
                                         "SELECT id, vmake, vmodel, vyear, vcondition FROM Vehicle WHERE id=@searchID";
            }
            else if (rec == 'i')
            {
                query = searchID < 0 ? "SELECT id, vID, stock, price, cost FROM Inventory" :
                                         "SELECT id, vID, stock, price, cost FROM Inventory WHERE vID=@searchID";

            }
            else if (rec == 'r')
            {
                query = searchID < 0 ? "SELECT id, iID, whatToRepair FROM Repair" :
                                        "SELECT id, iID, whatToRepair FROM Repair WHERE id=@searchID";
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
