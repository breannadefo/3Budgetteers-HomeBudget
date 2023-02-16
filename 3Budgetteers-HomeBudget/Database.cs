using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Database
    {
        //backing fields
        private static SQLiteConnection _connection;

        //properties
        public static SQLiteConnection dbConnection
        {
            get { return _connection; }
            private set { _connection = value; }
        }

        //constructor
        public static void newDatabase(string filename)
        {            
            CloseDatabaseAndReleaseFile();
             
            SQLiteConnection connection = new SQLiteConnection($"Data Source={filename};Foreign Keys=1");
            dbConnection = connection;

            AddTables(connection);
        }

        //member methods

        public static void CloseDatabaseAndReleaseFile()
        {
            if (dbConnection != null)
            {
                //closing the connection to the database
                dbConnection.Close();

                //calls the garbage collector to make sure the connection is ended
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private static void AddTables(SQLiteConnection con)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);

            cmd = con.CreateCommand();

            cmd.CommandText = "CREATE TABLE categoryTypes(Id INT PRIMARY KEY, Description VARCHAR(20))";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE categories(Id INT PRIMARY KEY, Description VARCHAR(20), TypeId INT, FOREIGN KEY (TypeId) REFERENCES categoryTypes (Id))";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE expenses(Id INT PRIMARY KEY, Date VARCHAR(20), Description VARCHAR(50), Amount DOUBLE, CategoryId INT, FOREIGN KEY (CategoryId) REFERENCES categories (Id))";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        public static void existingDatabase(string filename)
        {

        }
    }
}
