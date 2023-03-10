using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;
using Xunit.Abstractions;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestHomeBudget_GetBudgetItemsByCategory
    {
        string testInputFile = TestConstants.testExpensesInputFile;
        
        // ========================================================================
        // Get Expenses By Month Method tests
        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByCategory_NoStartEnd_NoFilter()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, false);
            List<Expense> listAllExpenses = homeBudget.expenses.List();
            //BudgetItemsByCategory firstRecord = TestConstants.budgetItemsByCategory_FirstRecord;
            int testCateogory = 1;

            // Act
            List<BudgetItemsByCategory> budgetItemsByCategory = homeBudget.GeBudgetItemsByCategory(null, null, false, 9);
            List<Expense> listExpenses = new List<Expense>();

            //Getting all expenses that fall under the testCategoryId
            foreach (Expense expense in listAllExpenses)
            {
                if (expense.Category == testCateogory)
                {
                    listExpenses.Add(expense);
                }
            }

            //Getting all expenses that fall under the testCategoryId
            BudgetItemsByCategory testCategoryItem = new BudgetItemsByCategory();
            foreach (BudgetItemsByCategory testCategoryBudgetItem in budgetItemsByCategory)
            {
                if (testCategoryBudgetItem.Details[0].CategoryID == testCateogory)
                {
                    testCategoryItem = testCategoryBudgetItem;
                }
            }

            //Asert
            Assert.Equal(listExpenses.Count, testCategoryItem.Details.Count);

            for(int record = 0; record < listExpenses.Count; record++)
            {
                Assert.Equal(testCategoryItem.Details[record].CategoryID, testCateogory);
            }
        }

        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByCategory_NoStartEnd_FilterbyCategory()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, false);
            int maxRecords14 = TestConstants.budgetItemsByCategory14;
            int maxRecords20 = TestConstants.budgetItemsByCategory20;

            // Act
            //why calling getbudgetitems by month?
            List<BudgetItemsByMonth> budgetItemsByCategory = homeBudget.GetBudgetItemsByMonth(null, null, true, 14);

            // Assert
            Assert.Equal(maxRecords14, budgetItemsByCategory.Count);


            // Act
            budgetItemsByCategory = homeBudget.GetBudgetItemsByMonth(null, null, true, 20);

            // Assert
            Assert.Equal(maxRecords20, budgetItemsByCategory.Count);

        }
        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByCategory_2018_filterDateAndCat9()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, false);
            Expenses expenses = new Expenses(Database.dbConnection);
            //List<BudgetItemsByCategory> validBudgetItemsByCategory = TestConstants.getBudgetItemsByCategory2018_Cat9();
            List<Expense> listAllExpenses = homeBudget.expenses.List();
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime endDate = new DateTime(2018, 12, 31);
            int filterCategoryId = 9;

            // Act
            List<BudgetItemsByCategory> budgetItemsByCategory = homeBudget.GeBudgetItemsByCategory(startDate, endDate, true, filterCategoryId);
            List<Expense> listExpenses = new List<Expense>();

            //Getting all expenses that fall under the filterCateforyId and are between the two dates
            foreach (Expense expense in listAllExpenses)
            {
                if (expense.Category == filterCategoryId && expense.Date > startDate && expense.Date < endDate)
                {
                    listExpenses.Add(expense);
                }
            }

            //Getting the corresponding budgetItemsByCategory
            BudgetItemsByCategory testCategoryItem = new BudgetItemsByCategory();
            foreach (BudgetItemsByCategory testCategoryBudgetItem in budgetItemsByCategory)
            {
                if (testCategoryBudgetItem.Details[0].CategoryID == filterCategoryId)
                {
                    testCategoryItem = testCategoryBudgetItem;
                }
            }


            // Assert
            Assert.Equal(listExpenses.Count, testCategoryItem.Details.Count);

            for (int record = 0; record < testCategoryItem.Details.Count; record++)
            {
                Assert.Equal(testCategoryItem.Details[record].CategoryID, filterCategoryId);
                Assert.True(testCategoryItem.Details[record].Date > startDate);
                Assert.True(testCategoryItem.Details[record].Date < endDate);
            }
        }


        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByCategory_2018_filterDate()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, false);
            List<Expense> listAllExpenses = homeBudget.expenses.List();
            //List<BudgetItemsByCategory> validBudgetItemsByCategory = TestConstants.getBudgetItemsByCategory2018();
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime endDate = new DateTime(2018, 12, 31);
            int filterCategoryId = 9;

            // Act
            List<BudgetItemsByCategory> budgetItemsByCategory = homeBudget.GeBudgetItemsByCategory(startDate, endDate, false, filterCategoryId);
            List<Expense> listExpenses = new List<Expense>();

            //Getting all expenses that fall under the filterCateforyId and are between the two dates
            foreach (Expense expense in listAllExpenses)
            {
                if(expense.Category == filterCategoryId && expense.Date > startDate && expense.Date < endDate)
                {
                    listExpenses.Add(expense);
                }
            }

            //Getting the corresponding budgetItemsByCategory
            BudgetItemsByCategory testCategoryItem = new BudgetItemsByCategory();
            foreach (BudgetItemsByCategory testCategoryBudgetItem in budgetItemsByCategory)
            {
                if (testCategoryBudgetItem.Details[0].CategoryID == filterCategoryId)
                {
                    testCategoryItem = testCategoryBudgetItem;
                }
            }

            // Assert
            Assert.Equal(listExpenses.Count, testCategoryItem.Details.Count);

            for (int record = 0; record < testCategoryItem.Details.Count; record++)
            {
                Assert.Equal(testCategoryItem.Details[record].CategoryID, filterCategoryId);
                Assert.True(testCategoryItem.Details[record].Date > startDate);
                Assert.True(testCategoryItem.Details[record].Date < endDate);
            }
        }
    }
}

