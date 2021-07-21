using System;
using System.Data.SQLite;


namespace Machine_List_CSharp
{
    class sql
    {
        private string _Database = "Data Source=database.sqlite";
        public string Database
        {
            get
            {
                return _Database;
            }
            set
            {
                _Database = value;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            //Checks User's response
            //Checks database table info
            bool First_Choice_Loop = true;
            while (First_Choice_Loop == true)
            {
                Console.WriteLine("Show, Insert or Update? [s/i/u]:");
                Console.WriteLine("To quit. Type Quit or Q.");
                string UserResponse = Console.ReadLine();
                if (UserResponse == "S" || UserResponse == "s")
                {
                    First_Choice_Loop = false;
                    Show_Computers();
                }
                else if (UserResponse == "I" || UserResponse == "i")
                {
                    First_Choice_Loop = false;
                    Insert_Computers();
                }
                else if (UserResponse == "U" || UserResponse == "u")
                {
                    First_Choice_Loop = false;
                    Update_Computers();
                }
                else if (UserResponse == "quit" || UserResponse == "Quit" || UserResponse == "q" || UserResponse == "Q")
                {
                    First_Choice_Loop = false;
                }
                else
                {
                    Console.WriteLine("\nPlease use either of the following letters: \n-> S for show table\n-> I for insert into table\n-> U to update table info");
                }
            }
        }

        static void Show_Computers()
        {
            sql sql = new sql();
            using var con = new SQLiteConnection(sql.Database);
            //Opens connection to database
            con.Open();
            //Runs SQL command
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT * FROM Computers";
            cmd.ExecuteScalar().ToString();
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("| {0,-5} | {1,-24} | {2,-25} | {3,-45} |", "Row ID", "Computer Name", "Assigned To", "Additional Information"));
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            while (rdr.Read())
            {
                Console.WriteLine(string.Format("| {0,-6} | {1,-24} | {2,-25} | {3,-45} |", $"ID: {rdr.GetInt32(0)}", $"{rdr.GetString(1)}", $"{rdr.GetString(2)}", $"{rdr.GetString(3)}"));
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");

            con.Close();
        }

        static void Insert_Computers()
        {
            sql sql = new sql();
            using var con = new SQLiteConnection(sql.Database);
            //Opens connection to database
            con.Open();
            //Runs SQL command
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT * FROM Computers";
            cmd.ExecuteScalar().ToString();
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            bool LoopCheck = false;
            bool LoopPass = false;
            while (LoopCheck == false)
            {
                string Insert_Response = "y";
                if (LoopPass == true)
                {
                    Console.WriteLine("Do you want to insert another computer into the list? [Y/N]");
                    Insert_Response = Console.ReadLine();
                }

                if (Insert_Response == "Y" || Insert_Response == "y")
                {
                    Console.WriteLine("Input Computer name to be added to the list.");
                    string ComputerName = Console.ReadLine();
                    Console.WriteLine("Input Who will be assigned to the computer.");
                    string Assigned = Console.ReadLine();
                    Console.WriteLine("Input any additional information (can be blank).");
                    string Add_Info = Console.ReadLine();

                    cmd.CommandText =  "INSERT INTO Computers(Computer_Name, Assigned_To, Additional_Info) VALUES(@Computer, @Assign, @Info)";
                    
                    //IMPORTANT: This will prevent SQL Injection.
                    cmd.Parameters.AddWithValue("@Computer", ComputerName);
                    cmd.Parameters.AddWithValue("@Assign", Assigned);
                    cmd.Parameters.AddWithValue("@Info", Add_Info);

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Inserted into the list");

                    //Most likely not the efficient way to do this but best I can do on short notice
                    LoopPass = true;
                }
                else
                {
                    LoopCheck = true;
                    Console.WriteLine("Closing...");
                }
            }
            con.Close();
        }

        static void Update_Computers()
        {
            sql sql = new sql();
            using var con = new SQLiteConnection(sql.Database);
            //Opens connection to database
            con.Open();
            
            //Runs SQL command
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT * FROM Computers";
            cmd.ExecuteScalar().ToString();
            using SQLiteDataReader rdr = cmd.ExecuteReader();


            //SQL Output
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("| {0,-5} | {1,-24} | {2,-25} | {3,-45} |", "Row ID", "Computer Name", "Assigned To", "Additional Information"));
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            while (rdr.Read())
            {
                Console.WriteLine(string.Format("| {0,-6} | {1,-24} | {2,-25} | {3,-45} |", $"ID: {rdr.GetInt32(0)}", $"{rdr.GetString(1)}", $"{rdr.GetString(2)}", $"{rdr.GetString(3)}"));
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------\n");
            rdr.Close();
            ////////////////////

            Console.WriteLine("Which row ID would you like to update? (Only type the row ID number)");
            String Update_Id_Response = Console.ReadLine();
            
            cmd.CommandText = "SELECT * FROM Computers WHERE Table_ID = @ID";
            cmd.Parameters.AddWithValue("@ID", Update_Id_Response);
            cmd.ExecuteScalar().ToString();
            //I could have useds MARS but people suggested against this. So I am just closing and opening a new reader.
            using SQLiteDataReader rdr2 = cmd.ExecuteReader();

            //SQL Output
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("| {0,-5} | {1,-24} | {2,-25} | {3,-45} |", "Row ID", "Computer Name", "Assigned To", "Additional Information"));
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            while (rdr2.Read())
            {
                Console.WriteLine(string.Format("| {0,-6} | {1,-24} | {2,-25} | {3,-45} |", $"ID: {rdr2.GetInt32(0)}", $"{rdr2.GetString(1)}", $"{rdr2.GetString(2)}", $"{rdr2.GetString(3)}"));
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------\n");
            rdr2.Close();
            ////////////////////

            Console.WriteLine("Which column do you wish to update? [1-3]");
            Console.WriteLine("1 - Computer Name");
            Console.WriteLine("2 - Assigned To");
            Console.WriteLine("3 - Additional Information");
            bool Update_Loop_Check = true;
            while (Update_Loop_Check == true)
            {
                String Update_Table_Choice = Console.ReadLine();

                if (Update_Table_Choice == "1")
                {
                    Update_Loop_Check = false;
                }            
                else if (Update_Table_Choice == "2")
                {
                    Update_Loop_Check = false;
                }
                else if (Update_Table_Choice == "3")
                {
                    Update_Loop_Check = false;
                }
                else
                {
                    Console.WriteLine("You can only use one of the following below.");
                    Console.WriteLine("1 - Computer Name");
                    Console.WriteLine("2 - Assigned To");
                    Console.WriteLine("3 - Additional Information");
                    Console.WriteLine("Which column do you wish to update? [1-3]");
                }
            }
            
            //cmd.CommandText =  "UPDATE Computers SET (Computer_Name, Assigned_To, Additional_Info) VALUES(@Computer, @Assign, @Info) WHERE Table_ID = @ID";
            //cmd.Parameters.AddWithValue("@ID", Alter_Id_Response)

            con.Close();
        }
    }
}
