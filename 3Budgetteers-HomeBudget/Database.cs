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
        #region backing fields

        private static SQLiteConnection _connection;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the connection to a database.
        /// </summary>
        public static SQLiteConnection dbConnection
        {
            get { return _connection; }
            private set { _connection = value; }
        }

        #endregion

        #region constructor

        /// <summary>
        /// Creates a new database, populates it with the budget tables, and opens the conneciton.
        /// It also adds the category types to the categoryTypes table and adds default categories to
        /// the categories table.
        /// </summary>
        /// <param name="filename">The file that the database resides in.</param>
        public static void newDatabase(string filename)
        {            
            CloseDatabaseAndReleaseFile();
             
            SQLiteConnection connection = new SQLiteConnection($"Data Source={filename};Foreign Keys=1");
            dbConnection = connection;

            dbConnection.Open();

            AddTables(connection);
            AddCategoryTypes(connection);
            AddDefaultCategories(connection);
        }

        #endregion

        #region member methods

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

            //dropping the tables if they already exist
            cmd.CommandText = "DROP TABLE IF EXISTS expenses";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS categories";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DROP TABLE IF EXISTS categoryTypes";
            cmd.ExecuteNonQuery();

            //creating the tables
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
            CloseDatabaseAndReleaseFile();

            // your code
            Database._connection = new SQLiteConnection($"Data Source={filename};Foreign Keys=1");
            Database.dbConnection.Open();
        }

        /// <summary>
        /// Adds the category types to the categoryTypes table in the database.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        private static void AddCategoryTypes(SQLiteConnection connection)
        {
            SQLiteCommand cmd = new SQLiteCommand(connection);

            cmd = connection.CreateCommand();

            cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (1, 'Income');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (2, 'Expense');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (3, 'Credit');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes (Id, Description) VALUES (4, 'Savings');";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        /// <summary>
        /// Adds all the default categories to the categories table in the database.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        private static void AddDefaultCategories(SQLiteConnection connection)
        {
            SQLiteCommand cmd = new SQLiteCommand(connection);

            cmd = connection.CreateCommand();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Utilities', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Rent', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Food', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Entertainment', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Education', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Micellaneous', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Medical Expenses', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Vacation', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Credit Card', 3);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Clothes', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Gifts', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Insurance', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Transportation', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Eating Out', 2);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Savings', 4);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES ('Income', 1);";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        #endregion
    }
}
