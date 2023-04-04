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
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="description"> Description of the category. Generally this is the name of the category </param>
        /// <param name="categoryType"> The type of the category. For example, credit, expense, etc. </param>
        public void AddCategory(string description, string categoryType)
        {
            if (String.IsNullOrEmpty(description))
            {
                _view.ShowErrorMessage("ERROR! The category description cannot be empty.");
            }

            Category.CategoryType type = (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), categoryType);
            try
            {
                _homeBudget.categories.Add(description, type);
            }
            catch (Exception ex)
            {
                _view.ShowErrorMessage(ex.Message);
            }
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
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategories()
        {
            return _homeBudget.categories.List();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="newDb"></param>
        public void InitializeHomeBudget(string database, bool newDb)
        {
            _homeBudget = new HomeBudget(database, newDb);
        }
    }
}
