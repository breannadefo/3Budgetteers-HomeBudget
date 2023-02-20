using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: Category
    //        - An individual category for budget program
    //        - Valid category types: Income, Expense, Credit, Saving
    // ====================================================================

    /// <summary>
    /// Represents a category that a budget item can belong to. It has an id, a description which serves at the category name,
    /// and a type.
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================

        /// <value>
        /// Gets the id that is linked to a specific category.
        /// </value>
        public int Id { get; }

        /// <value>
        /// Gets the name/description of the category.
        /// </value>
        public String Description { get; }

        /// <value>
        /// Gets the category's type from the CategoryType enum. Could be Income, Expense, Credit, or Savings.
        /// </value>
        public CategoryType Type { get; }

        /// <summary>
        /// Represents how a category gains, loses, uses, or stores money.
        /// </summary>
        public enum CategoryType
        {
            /// <summary>
            /// Means money is gained.
            /// </summary>
            Income,
            /// <summary>
            /// Means money is lost.
            /// </summary>
            Expense,
            /// <summary>
            /// Means a credit card was used for the expense.
            /// </summary>
            Credit,
            /// <summary>
            /// Means money is added to a savings account.
            /// </summary>
            Savings
        };

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Creates a new Category object with the provided id, description, and category type. If a category type isn't specified,
        /// the default is to make it an expense.
        /// </summary>
        /// <param name="id">This is the id that the category will have.</param>
        /// <param name="description">This is the name of the category.</param>
        /// <param name="type">This is what type of category it is. It can be income, expense, credit, or savings.</param>
        public Category(int id, String description, CategoryType type = CategoryType.Expense)
        {
            //Connecting to the database
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Database.dbConnection.CreateCommand();

            //Writing the insert command
            sqlite_cmd.CommandText = "INSERT INTO categories (Id, Description, TypeId) VALUES (@Id, @Description, @Type)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Id", id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Description", description));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Type", type));
            sqlite_cmd.ExecuteNonQuery();
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        /// <summary>
        /// This creates a new Category object that is an exact copy of the Category object that was passed in.
        /// </summary>
        /// <param name="category">The original Category object that is going to be copied.</param>
        public Category(Category category)
        {
            //Connecting to the database
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Database.dbConnection.CreateCommand();

            //Writing the insert command for ID
            sqlite_cmd.CommandText = "INSERT INTO categories (Id, Description, TypeId) VALUES (@Id, @Description, @Type)";
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Id", category.Id));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Description", category.Description));
            sqlite_cmd.Parameters.Add(new SQLiteParameter("@Type", category.Type));
            sqlite_cmd.ExecuteNonQuery();
        }
        // ====================================================================
        // String version of object
        // ====================================================================

        /// <summary>
        /// Overrides the method to return the description of the Cateogry object.
        /// </summary>
        /// <returns>The description of the category.</returns>
        public override string ToString()
        {
            return Description;
        }

    }
}

