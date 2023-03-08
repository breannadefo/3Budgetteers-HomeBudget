using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestHomeBudget_GetBudgetItems
    {
        string testInputFile = TestConstants.testExpensesInputFile;
        

        // ========================================================================
        // Get Expenses Method tests
        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItems_NoStartEnd_NoFilter()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string inFile = TestConstants.GetSolutionDir() + "\\" + testInputFile;
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB,inFile,false);
            List<Expense> listExpenses = homeBudget.expenses.List();
            List<Category> listCategories = homeBudget.categories.List();

            // Act
            List<BudgetItem> budgetItems =  homeBudget.GetBudgetItems(null,null,false,9);

            // Assert
            Assert.Equal(listExpenses.Count, budgetItems.Count);
            foreach (Expense expense in listExpenses)
            {
                BudgetItem budgetItem = budgetItems.Find(b => b.ExpenseID == expense.Id);
                Category category = listCategories.Find(c => c.Id == expense.Category);
                Assert.Equal(budgetItem.Category, category.Description);
                Assert.Equal(budgetItem.CategoryID, expense.Category);
                Assert.Equal(budgetItem.Amount, expense.Amount);
                Assert.Equal(budgetItem.ShortDescription, expense.Description);
            }
       }

        [Fact]
        public void HomeBudgetMethod_GetBudgetItems_NoStartEnd_NoFilter_VerifyBalanceProperty()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string inFile = TestConstants.GetSolutionDir() + "\\" + testInputFile;
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, inFile, false);

            //Act
            List<BudgetItem> budgetItems = homeBudget.GetBudgetItems(null, null, false, 9);

            

            // Assert
            double balance = 0;
            foreach (BudgetItem budgetItem in budgetItems)
            {
                balance = balance + budgetItem.Amount;
                Assert.Equal(balance, budgetItem.Balance);
            }

        }

        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItems_NoStartEnd_FilterbyCategory()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string inFile = TestConstants.GetSolutionDir() + "\\" + testInputFile;
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, inFile, false);
            Expenses expenses = new Expenses(Database.dbConnection);
            List<Expense> listAllExpenses = expenses.List();
            List<Category> listCategories = homeBudget.categories.List();
            int filterCategory = 9;

            // Act 
            List<BudgetItem> budgetItems = homeBudget.GetBudgetItems(null, null, true, filterCategory);
            List<Expense> listExpenses = new List<Expense>();
            int index = 0;
            foreach(Expense expense in listAllExpenses)
            {
                if(expense.Category == filterCategory)
                {
                    listExpenses.Add(expense);
                }
                index++;
            }

            // Assert
            Assert.Equal(listExpenses.Count, budgetItems.Count);
            foreach (Expense expense in listExpenses)
            {
                BudgetItem budgetItem = budgetItems.Find(b => b.ExpenseID == expense.Id);
                Category category = listCategories.Find(c => c.Id == expense.Category);
                Assert.Equal(budgetItem.Category, category.Description);
                Assert.Equal(budgetItem.CategoryID, expense.Category);
                Assert.Equal(budgetItem.Amount, expense.Amount);
                Assert.Equal(budgetItem.ShortDescription, expense.Description);
            }
        }

        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItems_2018_filterDate()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string inFile = TestConstants.GetSolutionDir() + "\\" + testInputFile;
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, inFile, false);
            Expenses expenses = new Expenses(Database.dbConnection);
            List<Expense> listAllExpenses = expenses.List();
            List<Category> listCategories = homeBudget.categories.List();
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime endDate = new DateTime(2018, 12, 31);

            // Act
            List<BudgetItem> budgetItems = homeBudget.GetBudgetItems(startDate, endDate, false, 0);
            List<Expense> listExpenses = new List<Expense>();
            foreach (Expense expense in listAllExpenses)
            {
                if(expense.Date > startDate && expense.Date < endDate)
                {
                    listExpenses.Add(expense);
                }
            }

            // Assert
            Assert.Equal(listExpenses.Count, budgetItems.Count);
            foreach (Expense expense in listExpenses)
            {
                BudgetItem budgetItem = budgetItems.Find(b => b.ExpenseID == expense.Id);
                Category category = listCategories.Find(c => c.Id == expense.Category);
                Assert.Equal(budgetItem.Category, category.Description);
                Assert.Equal(budgetItem.CategoryID, expense.Category);
                Assert.Equal(budgetItem.Amount, expense.Amount);
                Assert.Equal(budgetItem.ShortDescription, expense.Description);
            }
        }

        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItems_2018_filterDate_verifyBalance()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string inFile = TestConstants.GetSolutionDir() + "\\" + testInputFile;
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, inFile, false);
            Expenses expenses = new Expenses(Database.dbConnection);
            List<Expense> listAllExpenses = expenses.List();
            DateTime startDate = new DateTime(1900, 1, 1);
            DateTime endDate = new DateTime(2500, 1, 1);
            int filterCategory = 9;


            // Act
            List<BudgetItem> budgetItems = homeBudget.GetBudgetItems(null, null,  true, filterCategory);
            List<Expense> listExpenses = new List<Expense>();
            foreach (Expense expense in listAllExpenses)
            {
                if (expense.Category == filterCategory && expense.Date > startDate && expense.Date < endDate)
                {
                    listExpenses.Add(expense);
                }
            }

            Double controlTotal = 0;
            foreach (Expense expense in listExpenses)
            {
                controlTotal += expense.Amount;
            }

            double total = budgetItems[budgetItems.Count-1].Balance;
            

            // Assert
            Assert.Equal(controlTotal, total);
        }

        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItems_2018_filterDateAndCat10()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            string inFile = TestConstants.GetSolutionDir() + "\\" + testInputFile;
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, inFile, false);
            Expenses expenses = new Expenses(Database.dbConnection);
            List<Expense> listAllExpenses = expenses.List();
            List<Category> listCategories = homeBudget.categories.List();
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime endDate = new DateTime(2018, 12, 31);
            int filterCategory = 5;

            // Act
            List<BudgetItem> budgetItems = homeBudget.GetBudgetItems(startDate, endDate, true, filterCategory);
            List<Expense> listExpenses = new List<Expense>();
            foreach(Expense expense in listAllExpenses)
            {
                if(expense.Category == filterCategory && expense.Date > startDate && expense.Date < endDate)
                {
                    listExpenses.Add(expense);
                }
            }

            // Assert
            Assert.Equal(listExpenses.Count, budgetItems.Count);
            foreach (Expense expense in listExpenses)
            {
                BudgetItem budgetItem = budgetItems.Find(b => b.ExpenseID == expense.Id);
                Category category = listCategories.Find(c => c.Id == expense.Category);
                Assert.Equal(budgetItem.Category, category.Description);
                Assert.Equal(budgetItem.CategoryID, expense.Category);
                Assert.Equal(budgetItem.Amount, expense.Amount);
                Assert.Equal(budgetItem.ShortDescription, expense.Description);
            }
        }
    }
}

