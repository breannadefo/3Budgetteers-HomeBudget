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
    // CLASS: Expense
    //        - An individual expense for budget program
    // ====================================================================

    /// <summary>
    /// Creates a record for everytime money is gained or spent. The object contains an id, the date from when the expense occured,
    /// the amount of the expense, a short description of what the expense was, and which category the expense falls under.
    /// </summary>
    public class Expense
    {
        // ====================================================================
        // Properties
        // ====================================================================

        /// <value>
        /// Gets the id of the expense.
        /// </value>
        public int Id { get; private set; }

        /// <value>
        /// Gets the date of when the expense happened.
        /// </value>
        public DateTime Date { get; private set; }

        /// <value>
        /// Gets or sets the cost of the expense. If the amount is negative, it means the expense was made using a credit card.
        /// </value>
        public Double Amount { get; private set; }

        /// <value>
        /// Gets or sets a short description of what the expense was for.
        /// </value>
        public String Description { get; private set; }

        /// <value>
        /// Gets or sets the category id of the category that the expense belongs to.
        /// </value>
        public int Category { get; private set; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the expense category exists in the
        //        categories object
        // ====================================================================

        /// <summary>
        /// Creates a new Expense object with the provided id, date, category, amount, and description.
        /// </summary>
        /// <param name="id">The id linked to the expense.</param>
        /// <param name="date">The date the expense happened.</param>
        /// <param name="category">The category the expense belongs to.</param>
        /// <param name="amount">How much the expense was for.</param>
        /// <param name="description">A couple words indicating what the expense was for.</param>
        public Expense(int id, DateTime date, int category, Double amount, String description)
        {
            this.Id = id;
            this.Date = date;
            this.Category = category;
            this.Amount = amount;
            this.Description = description;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        // ====================================================================

        /// <summary>
        /// Creates a new Expense object that is an exact copy of the Expense object that was passed into the constructor.
        /// </summary>
        /// <param name="obj">The Expense object that will be copied.</param>
        public Expense (Expense obj)
        {
            this.Id = obj.Id;
            this.Date = obj.Date;
            this.Category = obj.Category;
            this.Amount = obj.Amount;
            this.Description = obj.Description;
           
        }
    }
}
