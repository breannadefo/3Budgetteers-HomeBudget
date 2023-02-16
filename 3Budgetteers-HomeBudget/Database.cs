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

    }
}
