using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Budget;
using System.Data.SQLite;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestExpenses
    {
        int numberOfExpensesInFile = TestConstants.numberOfExpensesInFile;
        String testInputFile = TestConstants.testExpensesInputFile;
        int maxIDInExpenseFile = TestConstants.maxIDInExpenseFile;
        Expense firstExpenseInFile = new Expense(1, new DateTime(2021, 1, 10), 10, 12, "hat (on credit)");

        SQLiteConnection databaseConnection = Database.dbConnection;

        // ========================================================================

        [Fact]
        public void ExpensesObject_New()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String existingDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(existingDB);
            // Act
            Expenses expenses = new Expenses(databaseConnection);

            // Assert
            Assert.IsType<Expenses>(expenses);
        }

        [Fact]
        public void ExpensesMethod_List_ModifyListDoesNotModifyExpensesInstance()
        {
            // Arrange
            Expenses expenses = new Expenses(databaseConnection);
            List<Expense> list = expenses.List();

            // Assert
            Assert.NotEqual(list[0].Amount, expenses.List()[0].Amount + 25);

        }
        

        // ========================================================================

        [Fact]
        public void ExpensesMethod_Add()
        {
            // Arrange
            int categoryId = 1;
            double amount = 98.1;
            Categories categoeries = new Categories(Database.dbConnection, false);
            Expenses expenses = new Expenses(Database.dbConnection);
            categoeries.Add("Test Category", Category.CategoryType.Expense);

            // Act
            int initialSizeOfList = expenses.List().Count;
            expenses.Add(DateTime.Now, categoryId, amount,"new expense");
            int sizeOfList = expenses.List().Count;

            // Assert
            Assert.Equal(initialSizeOfList + 1, sizeOfList);
        }

        // ========================================================================

        [Fact]
        public void ExpensesMethod_Delete()
        {
            // Arrange
            Expenses expenses = new Expenses(databaseConnection);
            int IdToDelete = 1;
            expenses.Add(DateTime.Now, 1, 10, "new expense");

            // Act
            int initialSizeOfList = expenses.List().Count;
            expenses.Delete(IdToDelete);
            int sizeOfList = expenses.List().Count;

            // Assert
            Assert.Equal(initialSizeOfList - 1, sizeOfList);
        }

        // ========================================================================

        [Fact]
        public void ExpensesMethod_Delete_InvalidIDDoesntCrash()
        {
            // Arrange
            Expenses expenses = new Expenses(databaseConnection);
            int IdToDelete = 1006;
            int sizeOfList = expenses.List().Count;

            // Act
            try
            {
                expenses.Delete(IdToDelete);
                Assert.Equal(sizeOfList, expenses.List().Count);
            }

            // Assert
            catch
            {
                Assert.True(false, "Invalid ID causes Delete to break");
            }
        }


        // ========================================================================

        [Fact]
        public void ExpenseMethod_WriteToFile()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Expenses expenses = new Expenses(databaseConnection);
            string fileName = TestConstants.ExpenseOutputTestFile;
            String outputFile = dir + "\\" + fileName;
            File.Delete(outputFile);

            // Act
            expenses.WriteToFile(outputFile);

            // Assert
            Assert.True(File.Exists(outputFile), "output file created");

            // Cleanup
            if (FileEquals(dir + "\\" + testInputFile, outputFile))
            {
                File.Delete(outputFile);
            }

        }

        // ========================================================================

        [Fact]
        public void ExpensesMethod_ReadFromDatabase_ValidateCorrectDataWasRead()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories toSetUpDatabase = new Categories(conn, true);
            DateTime dates = DateTime.Now;
            int catId1 = 9, catId2 = 11, catId3 = 4;
            int numberOfExpenses = 3, firstId = 1;
            double amount1 = -9.78, amount2 = -14.95, amount3 = -43.67;
            string desc1 = "test obj 1", desc2 = "test obj 2", desc3 = "test obj 3";

            // Act
            Expenses expenses = new Expenses(conn);
            expenses.Add(dates, catId1, amount1, desc1);
            expenses.Add(dates, catId2, amount2, desc2);
            expenses.Add(dates, catId3, amount3, desc3);

            List<Expense> list = expenses.List();
            Expense firstExpense = list[0];

            // Assert
            Assert.Equal(numberOfExpenses, list.Count);
            Assert.Equal(firstId, firstExpense.Id);
            Assert.Equal(catId1, firstExpense.Category);
            Assert.Equal(amount1, firstExpense.Amount);
            Assert.Equal(desc1, firstExpense.Description);
            
        }

        //=============================================================================

        [Fact]
        public void ExpensesMethod_List_ReturnsListOfExpenses()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories toSetUpDatabase = new Categories(conn, true);
            DateTime dates = DateTime.Now;
            int catId1 = 9, catId2 = 11, catId3 = 4;
            int numberOfExpenses = 3, firstId = 1;
            double amount1 = -9.78, amount2 = -14.95, amount3 = -43.67;
            string desc1 = "test obj 1", desc2 = "test obj 2", desc3 = "test obj 3";

            Expenses expenses = new Expenses(conn);
            expenses.Add(dates, catId1, amount1, desc1);
            expenses.Add(dates, catId2, amount2, desc2);
            expenses.Add(dates, catId3, amount3, desc3);

            // Act
            List<Expense> list = expenses.List();

            // Assert
            Assert.Equal(numberOfExpenses, list.Count);

        }

        // ========================================================================

        [Fact]
        public void ExpensesMethod_UpdateExpense()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories cats = new Categories(conn, true);
            Expenses expenses = new Expenses(conn);
            DateTime originalDate = new DateTime(2019, 07, 14);
            DateTime newDate = new DateTime(2023, 03, 02);
            int originalCatId = 4, newCatId = 11;
            double originalAmount = -39.99, newAmount = -19.99;
            String originalDesc = "Video Projector", newDesc = "Tangled Movie";
            int id = 1;

            // Act
            expenses.Add(originalDate, originalCatId, originalAmount, originalDesc);
            expenses.Update(id, newDate, newAmount, newDesc, newCatId);

            List<Expense> expList = expenses.List();

            // Assert 
            Assert.Equal(expList[0].Date, newDate);
            Assert.Equal(expList[0].Category, newCatId);
            Assert.Equal(expList[0].Amount, newAmount);
            Assert.Equal(expList[0].Description, newDesc);
        }

        // ========================================================================

        [Fact]
        public void ExpensesMethod_UpdateExpense_ExpenseDoesNotExist()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories cats = new Categories(conn, true);
            List<Expense> expList = new List<Expense>();
            Expenses expenses = new Expenses(conn);
            DateTime originalDate = new DateTime(2019, 07, 14);
            DateTime newDate = new DateTime(2023, 03, 02);
            int originalCatId = 4, newCatId = 11;
            double originalAmount = -39.99, newAmount = -19.99;
            String originalDesc = "Video Projector", newDesc = "Tangled Movie";
            int id = 2;

            // Act
            expenses.Add(originalDate, originalCatId, originalAmount, originalDesc);

            try
            {
                expenses.Update(id, newDate, newAmount, newDesc, newCatId);
            }
            // Assert
            catch (Exception e)
            {
                expList = expenses.List();

                Assert.Equal(expList[0].Date, originalDate);
                Assert.Equal(expList[0].Category, originalCatId);
                Assert.Equal(expList[0].Amount, originalAmount);
                Assert.Equal(expList[0].Description, originalDesc);
            }

            //making sure the list was initialized in the catch
            Assert.True(expList.Count != 0);
        }

        // ========================================================================

        [Fact]
        public void ExpensesMethod_UpdateExpense_InvalidNewCatgegoryId()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\newDB.db";
            Database.newDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Categories cats = new Categories(conn, true);
            List<Expense> expList = new List<Expense>();
            Expenses expenses = new Expenses(conn);
            DateTime originalDate = new DateTime(2019, 07, 14);
            DateTime newDate = new DateTime(2023, 03, 02);
            int originalCatId = 4, newCatId = 111;
            double originalAmount = -39.99, newAmount = -19.99;
            String originalDesc = "Video Projector", newDesc = "Tangled Movie";
            int id = 1;

            // Act
            expenses.Add(originalDate, originalCatId, originalAmount, originalDesc);

            try
            {
                expenses.Update(id, newDate, newAmount, newDesc, newCatId);
            }
            // Assert
            catch (Exception e)
            {
                expList = expenses.List();

                Assert.Equal(expList[0].Date, originalDate);
                Assert.Equal(expList[0].Category, originalCatId);
                Assert.Equal(expList[0].Amount, originalAmount);
                Assert.Equal(expList[0].Description, originalDesc);
            }

            //making sure the list was initialized in the catch
            Assert.True(expList.Count != 0);
        }

        // ========================================================================

        // -------------------------------------------------------
        // helpful functions, ... they are not tests
        // -------------------------------------------------------

        // source taken from: https://www.dotnetperls.com/file-equals

        private bool FileEquals(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);
            if (file1.Length == file2.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
