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
            if (UserResponse == "S" || UserResponse == "s")
            {
                cmd.CommandText = "SELECT * FROM Computers";
                using SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine($"ID: {rdr.GetInt32(0)}    PC: {rdr.GetString(1)}     Assigned: {rdr.GetString(2)}    Additional Info: {rdr.GetString(3)}");
                }
            }
            else if (UserResponse == "I" || UserResponse == "i")
            {

            }
            else if (UserResponse == "A" || UserResponse == "a")
            {

            }
            else
            {
                Console.WriteLine("Please use either of the following letters: \nS for show table\nI for insert into table\nA for alter table");
            }
            con.Close();
        }
    }
}
