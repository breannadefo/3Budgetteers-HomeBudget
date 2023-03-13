using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestHomeBudget
    {
        string testInputFile = TestConstants.testDBInputFile;

        // ========================================================================

        [Fact]
        public void HomeBudgetObject_New_WithFilename()
        {
            // Arrange
            string folder = TestConstants.GetSolutionDir();
            String file = $"{folder}\\unedited.db";

            int numExpenses = TestConstants.numberOfExpensesInFile;
            int numCategories = TestConstants.numberOfCategoriesInFile;

            // Act
            HomeBudget homeBudget = new HomeBudget(file, false);

            // Assert 
            Assert.IsType<HomeBudget>(homeBudget);
            Assert.Equal(numExpenses, homeBudget.expenses.List().Count);
            Assert.Equal(numCategories, homeBudget.categories.List().Count);

        }
        
        // -------------------------------------------------------
        // helpful functions, ... they are not tests
        // -------------------------------------------------------
        private bool FileSameSize(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);
            return (file1.Length == file2.Length);
        }

    }
}

