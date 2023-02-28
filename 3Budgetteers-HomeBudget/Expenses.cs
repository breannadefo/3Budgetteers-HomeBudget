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

        // ====================================================================
        // Add expense
        // ====================================================================
        private void Add(Expense exp)
        {
            
        }

        /// <summary>
        /// Creates or adds a new Expense objects to the expenses list. To create a new expense, it 
        /// needs an id, a date, a category, an amount, and a description. the date, category, amount,
        /// and description are all passed into the method in parameters, but the id has to be
        /// calculated based on the expense ids that already are in the list, so it finds the largest
        /// id and increses the value by one. With all the information gathered, the new expense object 
        /// is created and immediately added to the list.
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

        }

        // ====================================================================
        // Delete expense
        // ====================================================================

        /// <summary>
        /// Removes an expense from the list. It uses the id that is passed as a parameter to find the
        /// index of the expense with the same id. Once the index is found, the expense at its place
        /// gets removed from the list.
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

        }

        // ====================================================================
        // Return list of expenses
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================

        /// <summary>
        /// Creates a copy of the expenses list to return to the caller of the method. It creates
        /// a new list and copies each expense to the new list. This is done because lists are 
        /// passed by reference and the list should not be editable outside of the methods from
        /// this class.
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
        /// List<Expense> copyOfList = exp.List();
        /// 
        /// copyOfList.RemoveAt(1);
        /// ]]>
        /// </code>
        /// 
        /// Since its a copy, the RemoveAt call won't change anything in the actual expenses instance.
        /// </example>
        /// 
        /// </summary>
        /// <returns>A copy of the expenses list.</returns>
        public List<Expense> List()
        {
            const int idColumn = 0, dateColumn = 1, descColumn = 2, amountColumn = 3, catIdColumn = 4;
            List<Expense> exps = new List<Expense>();

            SQLiteDataReader reader;
            SQLiteCommand cmd = Database.dbConnection.CreateCommand();

            cmd.CommandText = "SELECT Id, Date, Description, Amount, CategoryId FROM expenses ORDER BY Id ASC;";

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int expenseId = reader.GetInt32(idColumn);

                    string date = reader.GetString(dateColumn);
                    DateTime convertedDate = ConvertStringToDate(date);

                    string description = reader.GetString(descColumn);

                    double amount = reader.GetDouble(amountColumn);

                    int catID = reader.GetInt32(catIdColumn);

                    exps.Add(new Expense(expenseId, convertedDate, catID, amount, description));
                }
            }

            reader.Close();

            return exps;
        }

        private DateTime ConvertStringToDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

    }
}

