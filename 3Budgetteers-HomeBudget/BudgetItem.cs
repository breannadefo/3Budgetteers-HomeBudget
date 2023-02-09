using System;
using System.Collections.Generic;
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
    // CLASS: BudgetItem
    //        A single budget item, includes Category and Expense
    // ====================================================================

    /// <summary>
    /// Holds the information about a singular budget item. Saves the data about the item's
    /// category, expense, date, a description, the amount the item is, and the balance after
    /// budget item expense has gone through.
    /// </summary>
    public class BudgetItem
    {
        /// <value>
        /// Gets or sets the id that is linked to the category the budget item falls under.
        /// </value>
        public int CategoryID { get; set; }

        /// <value>
        /// Gets or sets the id that is linked to this budget item's Expense object.
        /// </value>
        public int ExpenseID { get; set; }

        /// <value>
        /// Gets or sets the date of when this budget item took place.
        /// </value>
        public DateTime Date { get; set; }

        /// <value>
        /// Gets or sets the name of the category the budget item falls into.
        /// </value>
        public String Category { get; set; }

        /// <value>
        /// Gets or sets the description of the budget item.
        /// </value>
        public String ShortDescription { get; set; }

        /// <value>
        /// Gets or sets the cost of a budget item. If the amount is negative, it means that money was spent for this budget
        /// item, otherwise it means money was gained.
        /// </value>
        public Double Amount { get; set; }

        /// <value>
        /// Gets or sets the total balance of an account after being updated with any changes
        /// that the amount could create.
        /// </value>
        public Double Balance { get; set; }

    }

    /// <summary>
    /// Holds information about all the budget items for a month. Includes the month as a 
    /// string, a BudgetItem list that holds all the budget items from that month, and the
    /// total of all the budget item amounts as a double.
    /// </summary>
    public class BudgetItemsByMonth
    {
        /// <value>
        /// Gets or sets the month that all these budget items happened in.
        /// </value>
        public String Month { get; set; }

        /// <value>
        /// Gets or sets a list of all the budget items for the specified month.
        /// </value>
        public List<BudgetItem> Details { get; set; }

        /// <value>
        /// Gets or sets the total amount of all the budget items from this month.
        /// </value>
        public Double Total { get; set; }
    }

    /// <summary>
    /// Holds information about all the budget items of a category. Includes the category name
    /// as a string, a BudgetItem list that holds all the budget items that fall into the same
    /// category, and the total of all the budget item amounts as a double.
    /// </summary>
    public class BudgetItemsByCategory
    {
        /// <value>
        /// Gets or sets the category that all these budget items are a part of.
        /// </value>
        public String Category { get; set; }

        /// <value>
        /// Gets or sets a list of all the budget items that are a part of the same category.
        /// </value>
        public List<BudgetItem> Details { get; set; }

        /// <value>
        /// Gets or sets the total amount of all the budget items in this category.
        /// </value>
        public Double Total { get; set; }

    }


}
