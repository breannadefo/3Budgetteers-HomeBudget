using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using System.Globalization;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: expenses
    //        - A collection of expense items,
    //        - Read / write to file
    //        - etc
    // ====================================================================

    /// <summary>
    /// Used to hold collections of Expense objects and to read/write from files that pertain to
    /// expense information. It contains a default file name, a list that holds all of the Expense
    /// objects, a file name, and a directory name. It has methods to manage the list of expenses,
    /// and some to read/write to files that have or will soon hold information about the expenses.
    /// Only the default file name is static, so to access anything else in the class, an instance
    /// must be created first.
    /// </summary>
    public class Expenses
    {
        /// <summary>
        /// Creates an instance of expenses object passing in a valid database connection.
        /// The DB connection is passed in to ensure there is a connection to a valid database.
        /// </summary>
        /// <param name="conn">The connection to the database.</param>
        public Expenses(System.Data.SQLite.SQLiteConnection conn)
        {

        }

        //=====================================================================
        /// <summary>
        /// This method prints the expense information to a file for easy access and understanding.
        /// We are assuming the file exists.
        /// </summary>
        /// <param name="filepath">The path of the file to be written to.</param>
        public void WriteToFile(string filepath)
        {
            try
            {
                List<Expense> exps = List();

                string[] expenses = new string[exps.Count];

                for(int i = 0;i < expenses.Length; i++)
                {
                    expenses[i] = $"Description: {exps[i].Description} | Date: {exps[i].Date} | Amount: ${String.Format("{0:0.00}",exps[i].Amount)}";
                }

                File.WriteAllLines(filepath, expenses);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error while writing to file: {e.Message}");
            }
        }


        // ====================================================================
        // Add expense
        // ====================================================================
        private void Add(Expense expense)
        {
            //Connecting to the database
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Database.dbConnection.CreateCommand();

            //Writing the insert command
            sqlite_cmd.CommandText = "INSERT INTO expenses (Date, Description, Amount, CategoryId) VALUES (@Date, @Description, @Amount, @CategoryId);";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Date", expense.Date.ToString("yyyy-MM-dd")));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Description", expense.Description));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Amount", expense.Amount));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@CategoryId", (int)expense.Category));
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates or adds a new Expense objects to the expenses list. To create a new expense, it 
        /// needs an id, a date, a category, an amount, and a description. the date, category, amount,
        /// and description are all passed into the method in parameters, but the id has to be
        /// calculated based on the expense ids that already are in the database. It then adds the 
        /// expense to the database.
        /// 
        /// <example>
        /// Here's an example of how to use the method:
        /// 
        /// <code>
        /// 
        /// Expenses exp = new Expenses();
        /// exp.Add(DateTime.Now, 9, 66.96, "Dining room chair");
        /// exp.Add(DateTime.Now, 9, 34.99, "2 gallon of pink paint");
        /// exp.Add(DateTime.Now, 14, 12.50, "Poutine");
        /// 
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <param name="date">Represents the date the expense happened on.</param>
        /// <param name="category">Represents the category the expense belongs to.</param>
        /// <param name="amount">Represents the monetary value of the expense.</param>
        /// <param name="description">Represents a description of what the expense is.</param>
        public void Add(DateTime date, int category, Double amount, String description)
        {
            //Connecting to the database
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Database.dbConnection.CreateCommand();

            //Writing the insert command
            try
            {
                sqlite_cmd.CommandText = "INSERT INTO expenses (Date, Description, Amount, CategoryId) VALUES (@Date, @Description, @Amount, @CategoryId);";
                sqlite_cmd.Parameters.Add(new SQLiteParameter("@Date", date.ToString("yyyy-MM-dd")));
                sqlite_cmd.Parameters.Add(new SQLiteParameter("@Description", description));
                sqlite_cmd.Parameters.Add(new SQLiteParameter("@Amount", amount));
                sqlite_cmd.Parameters.Add(new SQLiteParameter("@CategoryId", category));
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new SQLiteException(e.Message);
            }
            finally
            {
                sqlite_cmd.Dispose();
            }
        }

        // ====================================================================
        // Delete expense
        // ====================================================================
        /// <summary>
        /// Removes an expense from the database. It uses the id that is passed as a parameter to find the
        /// index of the expense with the same id. Once the index is found, the expense is removed from the Database.
        /// 
        /// <example>
        /// Here's an example of how to use the method:
        /// 
        /// <code>
        /// 
        /// Expenses exp = new Expenses();
        /// exp.Add(DateTime.Now, 10, 14.87, "T-Shirt");
        /// exp.Add(DateTime.Now, 13, 60.00, "Monthly bus pass");
        /// 
        /// exp.Delete(1);
        /// 
        /// </code>
        /// 
        /// </example>
        /// 
        /// </summary>
        /// <param name="Id">Represents the expense id of the expense that will be deleted.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the id passed into the method
        /// doesn't match with any expense ids in the list.</exception>
        public void Delete(int Id)
        {
            try
            {
                //Connecting to the database
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = Database.dbConnection.CreateCommand();

                //Writing the Delete command
                sqlite_cmd.CommandText = "DELETE FROM expenses WHERE Id=@Id";
                sqlite_cmd.Parameters.Add(new SQLiteParameter("@Id", Id));
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Error is only thrown if it an SQLiteException because code should only throw an error if
                //the program did not have thep permissions to delete from the database
                if (ex is SQLiteException)
                {
                    throw new SQLiteException(ex.Message);
                }
                else if (ex is KeyNotFoundException)
                {
                    //The user story states nothing should happen if the Expense could not be found. Therefore
                    //nothing is done here
                }
                else
                {
                    Console.WriteLine("Error, " + ex.Message);
                }
            }
        }

        // ====================================================================
        // Return list of expenses
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Creates a list of the expenses to return to the caller of the method.
        /// 
        /// <example>
        /// Here's an example of how to use this method:
        /// 
        /// <code>
        /// 
        /// Expenses exp = new Expenses();
        /// exp.Add(DateTime.Now, 3, 9.87, "Eggs");
        /// exp.Add(DateTime.Now, 3, 8.31, "Goldfish");
        /// <![CDATA[
        /// List<Expense> expenses = exp.List();
        /// 
        /// foreach(Expense e in expenses){
        /// 
        /// Console.WriteLine(e.ToString);
        /// 
        /// }
        /// ]]>
        /// </code>
        /// 
        /// This example gets all the expenses and outputs them to the console.
        /// </example>
        /// 
        /// </summary>
        /// <returns>A list of all the expenses in the database.</returns>
        public List<Expense> List()
        {
            const int idColumn = 0, dateColumn = 1, descriptionColumn = 2, amountColumn = 3, categoryIdColumn = 4;
            List<Expense> list = new List<Expense>();

            SQLiteDataReader reader;
            SQLiteCommand cmd;
            cmd = Database.dbConnection.CreateCommand();

            cmd.CommandText = "SELECT Id, Date, Description, Amount, CategoryId FROM expenses ORDER BY Id ASC;";

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(idColumn);

                    string date = reader.GetString(dateColumn);
                    DateTime realDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    string description = reader.GetString(descriptionColumn);

                    double amount = reader.GetDouble(amountColumn);

                    int catId = reader.GetInt32(categoryIdColumn);

                    list.Add(new Expense(id, realDate, catId, amount, description));
                }
            }

            reader.Close();

            return list;
        }
        

        /// <summary>
        /// Updates the data of an expense with the values that are passed in. It finds the expense in the database that will be updated,
        /// and replaces the old values with the new ones. An exception is thrown if the expense can't be found or if it can't be updated.
        /// 
        /// <example>
        /// Here's an example of how to use this method:
        /// 
        /// <code>
        /// Expenses expenses = new Expenses(connection, true);
        /// expenses.Add(DateTime.Now(), 4, -14.99, "Movie Tickets");        
        /// </code>
        /// Now in the expenses table, there is an expense that has an id of 1, a date of the current date and time, the categoryId that
        /// matches the 'Entertainment' category, an amount of $14.99, and a description of 'Movie Tickets'.
        /// 
        /// <code>
        /// DateTime newDate = new DateTime(2008, 11, 27);
        /// expenses.Update(1, newDate, 11, -14.99, "Movie Tickets Gift Card");
        /// </code>
        /// After this line is run, that same expense from above still has an id of 1, and amount of $14.99, but the date has changed
        /// to November 27, 2008, the categoryId has changed to match the 'Gifts' category, and the description has been updated to be
        /// 'Movie Tickets Gift Card'.
        /// 
        /// </example>
        /// </summary>
        /// <param name="expenseId">The id of the expense that will be updated.</param>
        /// <param name="date">The new date that will replace the old one.</param>
        /// <param name="amount">The new amount that will replace the old one.</param>
        /// <param name="description">The new description that will replace the old one.</param>
        /// <param name="categoryId">The new category id that will replace the old one.</param>
        /// <exception cref="SQLiteException">Thrown if there are multiple expenses with the same id or if there are no matches for
        /// the id.</exception>
        public void Update(int expenseId, DateTime date, double amount, string description, int categoryId)
        {
            SQLiteConnection connection = Database.dbConnection;
            SQLiteCommand command = connection.CreateCommand();
            SQLiteDataReader reader;

            command.CommandText = "SELECT COUNT(*) FROM expenses WHERE Id=@id;";
            command.Parameters.Add(new SQLiteParameter("@id", expenseId));
            
            reader = command.ExecuteReader();

            if (reader.Read())
            {
                int count = reader.GetInt32(0);
                if (count > 1)
                {
                    throw new SQLiteException("Cannot update multiple expenses with the same id.");
                }
                else if (count == 0)
                { 
                    throw new SQLiteException("There were no expenses that matched the provided id.");
                }
            }
            reader.Close();

            string formattedDate = date.ToString("yyyy-MM-dd");

            command.CommandText = "UPDATE expenses SET Date = @Date, Description = @Description, Amount = @Amount, CategoryId = @CategoryId WHERE Id = @ExpenseId;";
            command.Parameters.Add(new SQLiteParameter("@Date", formattedDate));
            command.Parameters.Add(new SQLiteParameter("@Description", description));
            command.Parameters.Add(new SQLiteParameter("@Amount", amount));
            command.Parameters.Add(new SQLiteParameter("@CategoryId", categoryId));
            command.Parameters.Add(new SQLiteParameter("@ExpenseId", expenseId));

            command.ExecuteNonQuery();
        }

    }
}

