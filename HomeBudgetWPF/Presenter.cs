using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Sets the view to a new view
        /// </summary>
        /// <param name="view"> The new class that will be used as the view </param>
        public void SetView(ViewInterface view)
        {
            _view = view;
        }

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="description"> Description of the category. Generally this is the name of the category </param>
        /// <param name="categoryType"> The type of the category. For example, credit, expense, etc </param>
        public void AddCategory(string description, string categoryType)
        {
            _homeBudget.categories.Add(description, (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), categoryType));
        }

        /// <summary>
        /// Adds an expense to the database.
        /// </summary>
        /// <param name="description"> Description of the expense </param>
        /// <param name="date"> The date on which the expense was inccured </param>
        /// <param name="amount"> The total amunt of the expense. This value should be postive </param>
        /// <param name="categoryId"> The id of the category </param>
        public void AddExpense(string description, DateTime date, double amount, int categoryId) 
        {
            _homeBudget.expenses.Add(date, categoryId, amount, description);
        }

        /// <summary>
        /// Retrieves a list of all the categories
        /// </summary>
        /// <returns> A list of all categories in the database </returns>
        public List<Category> GetCategories()
        {
            return _homeBudget.categories.List();
        }

        /// <summary>
        /// Initializes a homa budget instance and saves it to the backing field. Intializes the database
        /// with the file provided in the parameters. If the newDb bool is false it will assume the database
        /// already exists. If it is true, it will asume it needs to create one.
        /// </summary>
        /// <param name="database"> The filepath to the database file </param>
        /// <param name="newDb"> If false, reads from existing database </param>
        public void InitializeHomeBudget(string database, bool newDb)
        {
            _homeBudget = new HomeBudget(database, newDb);
        }
    }
}
