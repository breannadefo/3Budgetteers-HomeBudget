using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;

namespace HomeBudgetWPF
{
    public class Presenter : PresenterInterface
    {
        ViewInterface _view;
        HomeBudget _homeBudget;

        public Presenter(ViewInterface view) 
        {
            _view = view;
        }

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="description"> Description of the category. Generally this is the name of the category </param>
        /// <param name="categoryType"> The type of the category. For example, credit, expense, etc. </param>
        public void AddCategory(string description, string categoryType)
        {
            _homeBudget.categories.Add(description, (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), categoryType));
        }

        /// <summary>
        /// Adds an expense to the database
        /// </summary>
        /// <param name="description"> Description of the expense. </param>
        /// <param name="date"> The date on which the expense was inccured </param>
        /// <param name="amount"> The total amunt of the expense. This value should be postive </param>
        /// <param name="categoryId"> The id of the category </param>
        public void AddExpense(string description, DateTime date, double amount, int categoryId) 
        {
            _homeBudget.expenses.Add(date, categoryId, amount, description);
        }

        /// <summary>
        /// Gets a list of all the catgeories.
        /// </summary>
        /// <returns>A list of category objects.</returns>
        public List<Category> GetCategories()
        {
            return _homeBudget.categories.List();
        }

        /// <summary>
        /// Initializes a homebudget. Creates a directory path towards the new budget file if one doesn't exist.
        /// </summary>
        /// <param name="database">The path to the database file.</param>
        /// <param name="newDb">True if the user wants to create a new database, false otherwise.</param>
        public void InitializeHomeBudget(string database, bool newDb)
        {
            if(!Directory.Exists(Path.GetDirectoryName(database)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(database));
            }
            //crashes if user tries to create a new datase twice
            _homeBudget = new HomeBudget(database, newDb);
        }

        /// <summary>
        /// Called while trying to close the app. Closes the database connection if there is one active.
        /// </summary>
        public void CloseApp()
        {
            if(_homeBudget != null)
            {
                _homeBudget.CloseDB();
            }
        }

        /// <summary>
        /// Determines whether or not a home budget is in use.
        /// </summary>
        /// <returns>True if there is a home budget, false otherwise.</returns>
        public bool VerifyHomeBudgetConnected()
        {
            return _homeBudget != null;
        }
    }
}