using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;
using System.Windows;

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
            if (String.IsNullOrEmpty(description))
            {
                _view.ShowErrorMessage("There was a problem adding the cateory: \nThe category description cannot be empty.");
            }

            Category.CategoryType type = (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), categoryType);
            try
            {
                _homeBudget.categories.Add(description, type);
                _view.ShowSuccessMessage($"Successfully added a category with a description of '{description}' and a type of '{categoryType}'.");
            }
            catch (Exception ex)
            {
                _view.ShowErrorMessage(ex.Message);
            }
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
        /// <param name="database">The path to the database file.</param>
        /// <param name="newDb">True if the user wants to create a new database, false otherwise.</param>
        public void InitializeHomeBudget(string database, bool newDb)
        {
            CloseBudgetConnection();
            if(!Directory.Exists(Path.GetDirectoryName(database)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(database));
            }
            //crashes if user tries to create a new datase twice
            _homeBudget = new HomeBudget(database, newDb);
        }

        /// <summary>
        /// Closes the database connection if there is one active.
        /// </summary>
        public void CloseBudgetConnection()
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

        /// <summary>
        /// Retrieves a list of all the categories from the database
        /// </summary>
        /// <returns> List of category objects </returns>
        public List<Category> GetAllCategories()
        {
            return _homeBudget.categories.List();
        }

        public bool EnterHomeBudget(string budgetFileName, string budgetFolderPath, bool newDb)
        {
            if (budgetFileName.Contains(" "))
            {
                _view.ShowErrorMessage("The file name cannot contain a string!");
                return false;
            }
            else
            {
                if (Directory.Exists(budgetFolderPath))
                {
                    InitializeHomeBudget(budgetFolderPath+ "\\" + budgetFileName+ ".db", newDb);
                }
                else
                {
                    InitializeHomeBudget($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\Budget\\{budgetFileName}.db", newDb);
                }
                return true;
            }
        }
    }
}