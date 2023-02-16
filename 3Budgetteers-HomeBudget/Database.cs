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

        /// <summary>
        /// Gets or sets the connection to a database.
        /// </summary>
        public static SQLiteConnection dbConnection
        {
            get { return _connection; }
            private set { _connection = value; }
        }

        //constructor

        /// <summary>
        /// Creates a new database, populates it with the budget tables, and opens the conneciton.
        /// </summary>
        /// <param name="filename">The file that the database resides in.</param>
        public static void newDatabase(string filename)
        {            
            CloseDatabaseAndReleaseFile();
             
            SQLiteConnection connection = new SQLiteConnection($"Data Source={filename};Foreign Keys=1");
            dbConnection = connection;

            AddTables(connection);
        }

        //member methods

        /// <summary>
        /// Closes the database and severs the connection. It also calls the garbage collector to
        /// make sure any all databases are completely closed before .
        /// </summary>
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

        /// <summary>
        /// Creates an SQLite command to add the three budget tables to the database.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        private static void AddTables(SQLiteConnection connection)
        {
            SQLiteCommand cmd = new SQLiteCommand(connection);

            cmd = connection.CreateCommand();

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
