using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestHomeBudget_GetBudgetItemsByMonth
    {
        string testInputFile = TestConstants.testExpensesInputFile;
        

        // ========================================================================
        // Get Expenses By Month Method tests
        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByMonth_NoStartEnd_NoFilter()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, true);

            homeBudget.expenses.Add(new DateTime(2018, 1, 3), 9, 10, "First test expense");
            homeBudget.expenses.Add(new DateTime(2018, 1, 25), 10, -5, "Second test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 3), 9, -8, "third test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 3), 9, 14, "Fourth test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 13), 10, 15, "Fifth test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 9), 10, -4, "Sixth test expense");

            string[] expectedMonths = { "2018-01", "2019-01", "2020-01" };
            double[] expectedTotals = { 5, 7, 10 };


            // Act
            List<BudgetItemsByMonth> budgetItemsByMonth = homeBudget.GetBudgetItemsByMonth(null, null, false, 9);


            // Assert
            Assert.Equal(3, budgetItemsByMonth.Count);

            // verify all records
            for (int i = 0; i < budgetItemsByMonth.Count; i++)
            {
                Assert.Equal(2, budgetItemsByMonth[i].Details.Count);
                Assert.Equal(expectedMonths[i], budgetItemsByMonth[i].Month);
                Assert.Equal(expectedTotals[i], budgetItemsByMonth[i].Total);
            }
        }

        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByMonth_NoStartEnd_FilterbyCategory()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, true);

            homeBudget.expenses.Add(new DateTime(2018, 1, 3), 9, 10, "First test expense");
            homeBudget.expenses.Add(new DateTime(2018, 1, 25), 10, -5, "Second test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 3), 9, -8, "third test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 3), 9, 14, "Fourth test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 13), 10, 15, "Fifth test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 9), 10, -4, "Sixth test expense");

            string[] expectedMonths = {"2018-01", "2019-01", "2020-01" };
            double[] expectedTotals = { 10 , -8, 14};

            
            // Act
            List<BudgetItemsByMonth> budgetItemsByMonth = homeBudget.GetBudgetItemsByMonth(null, null, true, 9);
            

            // Assert
            Assert.Equal(3, budgetItemsByMonth.Count);

            // verify all records
            for (int i = 0; i < budgetItemsByMonth.Count; i++){
                Assert.Equal(1, budgetItemsByMonth[i].Details.Count);
                Assert.Equal(expectedMonths[i], budgetItemsByMonth[i].Month);
                Assert.Equal(expectedTotals[i], budgetItemsByMonth[i].Total);
                Assert.Equal(9, budgetItemsByMonth[i].Details[0].CategoryID);
            }
        }
        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByMonth_2018_filterDateAndCat9()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, true);

            homeBudget.expenses.Add(new DateTime(2018, 1, 3), 9, 10, "First test expense");
            homeBudget.expenses.Add(new DateTime(2018, 1, 25), 10, -5, "Second test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 3), 9, -8, "third test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 3), 9, 14, "Fourth test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 13), 10, 15, "Fifth test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 9), 10, -4, "Sixth test expense");

            const string expectedMonth = "2018-01";
            const double expectedTotal = 10;

            // Act
            List<BudgetItemsByMonth> budgetItemsByMonth = homeBudget.GetBudgetItemsByMonth(new DateTime(2018, 1, 1), new DateTime(2018, 12, 31), true, 9);
            BudgetItemsByMonth firstRecordTest = budgetItemsByMonth[0];

            // Assert
            Assert.Equal(1, budgetItemsByMonth.Count);

            // verify 1st record
            Assert.Equal(expectedMonth, firstRecordTest.Month);
            Assert.Equal(expectedTotal, firstRecordTest.Total);
            Assert.Equal(1, firstRecordTest.Details.Count);
            Assert.Equal(9, firstRecordTest.Details[0].CategoryID);
        }

        // ========================================================================

        [Fact]
        public void HomeBudgetMethod_GetBudgetItemsByMonth_2018_filterDate()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String goodDB = $"{folder}\\{TestConstants.testDBInputFile}";
            String messyDB = $"{folder}\\messy.db";
            System.IO.File.Copy(goodDB, messyDB, true);
            HomeBudget homeBudget = new HomeBudget(messyDB, true);

            homeBudget.expenses.Add(new DateTime(2018, 1, 3), 9, 10, "First test expense");
            homeBudget.expenses.Add(new DateTime(2018, 1, 25), 10, -5, "Second test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 3), 9, -8, "third test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 3), 9, 14, "Fourth test expense");
            homeBudget.expenses.Add(new DateTime(2019, 1, 13), 10, 15, "Fifth test expense");
            homeBudget.expenses.Add(new DateTime(2020, 1, 9), 10, -4, "Sixth test expense");

            const string expectedMonth = "2018-01";
            const double expectedTotal = 5;

            // Act
            List<BudgetItemsByMonth> budgetItemsByMonth = homeBudget.GetBudgetItemsByMonth(new DateTime(2018, 1, 1), new DateTime(2018, 12, 31), false, 9);
            BudgetItemsByMonth firstRecordTest = budgetItemsByMonth[0];

            // Assert
            Assert.Equal(1, budgetItemsByMonth.Count);

            // verify 1st record
            Assert.Equal(expectedMonth, firstRecordTest.Month);
            Assert.Equal(expectedTotal, firstRecordTest.Total);
            Assert.Equal(2, firstRecordTest.Details.Count);
        }
    }
}

