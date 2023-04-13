﻿using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;
using System.Windows;
using System.ComponentModel;
using Microsoft.Win32;

namespace HomeBudgetWPF
{
    public class Presenter : PresenterInterface
    {
        ViewInterface _view;
        HomeBudget _homeBudget;

        const string registrySubKey = @"SOFTWARE\BudgetApplication";
        const string previousDBKey = "Previous database";

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
        /// Adds a new category to the database. The description provided cannot be empty or the same as an already existing category. If
        /// the description is invalid, the category will not be added.
        /// </summary>
        /// <param name="description"> Description of the category. Generally this is the name of the category </param>
        /// <param name="categoryType"> The type of the category. For example, credit, expense, etc </param>
        public void AddCategory(string description, string categoryType)
        {
            bool canAdd = true;

            if (String.IsNullOrEmpty(description))
            {
                _view.ShowErrorMessage("There was a problem adding the category: \nThe category description cannot be empty.");
                return;
            }

            Category.CategoryType type = (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), categoryType);

            List<Category> categories = GetCategories();
            foreach (Category category in categories)
            {
                if (category.Description.ToLower() == description.ToLower())
                {
                    canAdd = false;
                    break;
                }
            }

            try
            {
                if (canAdd)
                {
                    _homeBudget.categories.Add(description, type);
                    _view.ShowSuccessMessage($"Successfully added a category with a description of '{description}' and a type of '{categoryType}'.");
                    _view.ResetValues();
                }
                else
                {
                    _view.ShowErrorMessage("There was a problem adding the category: \nThis category already exists.");
                    _view.ResetValues();
                }
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
            if (!Directory.Exists(Path.GetDirectoryName(database)))
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
            if (_homeBudget != null)
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

        /// <summary>
        /// Creates a home budget instance. Handles error validation for valid input.
        /// </summary>
        /// <param name="budgetFileName">The name of the budget file to be created.</param>
        /// <param name="budgetFolderPath">The folder of the budget to be created.</param>
        /// <param name="newDb">A boolean, true if the user wishes to create a new DB,
        ///  false if they wish to use an existing one.</param>
        /// <returns>True if the database is created properly, false otherwise.</returns>
        public bool EnterHomeBudget(string budgetFileName, string budgetFolderPath, bool newDb)
        {
            string fullDbPath;
            if (budgetFileName.Contains(" "))
            {
                _view.ShowErrorMessage("The file name cannot contain a string!");
                return false;
            }
            else
            {
                if (Directory.Exists(budgetFolderPath))
                {
                    fullDbPath = budgetFolderPath + "\\" + budgetFileName + ".db";
                }
                else
                {
                    fullDbPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\Budget\\{budgetFileName}.db";
                }
                //write to registry
                RegistryKey key = Registry.CurrentUser.CreateSubKey(registrySubKey);

                key.SetValue(previousDBKey, fullDbPath);
                key.Close();
                InitializeHomeBudget(fullDbPath, newDb);
                return true;
            }
        }

        /// <summary>
        /// Retrieves the previous database path from the registry and initilazes the homebudget using
        /// this path.
        /// </summary>
        /// <returns>True if there was a previous budget and it was initialized without error, false otherwise.</returns>
        public bool UsePreviousBudget()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registrySubKey);

            if (key != null && key.GetValue(previousDBKey) != null)
            {
                InitializeHomeBudget(key.GetValue(previousDBKey).ToString(), false);
                key.Close();
                return true;
            }
            else
            {
                _view.ShowErrorMessage("There was no previous budget in use.");
                return false;
            }
        }
    }
}