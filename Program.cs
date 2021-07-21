using System;
using System.Data.SQLite;


namespace Machine_List_CSharp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Defines the SQLite database file
            string Database = "Data Source=database.sqlite";
            //SQL Code to check SQLite version
            string Version = "SELECT SQLITE_VERSION()";
            //Connects to the database
            using var con = new SQLiteConnection(Database);
            //Opens connection to database
            con.Open();

            //Runs SQL command
            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = Version;
            string version = cmd.ExecuteScalar().ToString();
            //Writes SQL result
            Console.WriteLine($"SQLite version: {version}");
            Console.WriteLine("Show, Insert or Alter? [s/i/a]:");
            string UserResponse = Console.ReadLine();

            //Checks User's response
            //Checks database table info
            if (UserResponse == "S" || UserResponse == "s")
            {
                //Requests table data from Computers Table
                cmd.CommandText = "SELECT * FROM Computers";
                cmd.ExecuteScalar().ToString();
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine($"ID: {rdr.GetInt32(0)}    PC: {rdr.GetString(1)}     Assigned: {rdr.GetString(2)}    Additional Info: {rdr.GetString(3)}");
                }
            }
            else if (UserResponse == "I" || UserResponse == "i")
            {
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

            }
            else if (UserResponse == "A" || UserResponse == "a")
            {
                
            }
            else
            {
                Console.WriteLine("\nPlease use either of the following letters: \n-> S for show table\n-> I for insert into table\n-> A for alter table");
            }
            con.Close();
        }
    }
}
