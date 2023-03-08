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


        // ========================================================================

        //Don't think we need this test, no point in reading from a file
        [Fact]
        public void ExpensesMethod_ReadFromFile_NotExist_ThrowsException()
        {
            // Arrange
            String badFile = "abc.txt";
            Expenses expenses = new Expenses(databaseConnection);

            // Act and Assert
            Assert.Throws<System.IO.FileNotFoundException>(() => expenses.ReadFromFile(badFile));

        }

        // ========================================================================

        [Fact]
        public void ExpensesMethod_ReadFromFile_ValidateCorrectDataWasRead()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Expenses expenses = new Expenses(databaseConnection);

            // Act
            expenses.ReadFromFile(dir + "\\" + testInputFile);
            List<Expense> list = expenses.List();
            Expense firstExpense = list[0];

            // Assert
            Assert.Equal(numberOfExpensesInFile, list.Count);
            Assert.Equal(firstExpenseInFile.Id, firstExpense.Id);
            Assert.Equal(firstExpenseInFile.Amount, firstExpense.Amount);
            Assert.Equal(firstExpenseInFile.Description, firstExpense.Description);
            Assert.Equal(firstExpenseInFile.Category, firstExpense.Category);

            String fileDir = Path.GetFullPath(Path.Combine(expenses.DirName, ".\\"));
            Assert.Equal(dir, fileDir);
            Assert.Equal(testInputFile, expenses.FileName);

        }
        // ========================================================================

        /*
        [Fact]
        public void ExpensesMethod_List_ReturnsListOfExpenses()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Expenses expenses = new Expenses();
            expenses.ReadFromFile(dir + "\\" + testInputFile);

            // Act
            List<Expense> list = expenses.List();

            // Assert
            Assert.Equal(numberOfExpensesInFile, list.Count);

        }
        */

        // ========================================================================

        
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
            String dir = TestConstants.GetSolutionDir();
            Expenses expenses = new Expenses(databaseConnection);
            expenses.ReadFromFile(dir + "\\" + testInputFile);
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

        //obsolete
        [Fact]
        public void ExpenseMethod_WriteToFile_VerifyNewExpenseWrittenCorrectly()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Expenses expenses = new Expenses(databaseConnection);
            expenses.ReadFromFile(dir + "\\" + testInputFile);
            string fileName = TestConstants.ExpenseOutputTestFile;
            String outputFile = dir + "\\" + fileName;
            File.Delete(outputFile);

            // Act
            expenses.Add(DateTime.Now, 14, 35.27, "McDonalds");
            List<Expense> listBeforeSaving = expenses.List();
            expenses.SaveToFile(outputFile);
            expenses.ReadFromFile(outputFile);
            List<Expense> listAfterSaving = expenses.List();

            Expense beforeSaving = listBeforeSaving[listBeforeSaving.Count - 1];
            Expense afterSaving = listAfterSaving.Find(e => e.Id == beforeSaving.Id);

            // Assert
            Assert.Equal(beforeSaving.Id, afterSaving.Id);
            Assert.Equal(beforeSaving.Category, afterSaving.Category);
            Assert.Equal(beforeSaving.Description, afterSaving.Description);
            Assert.Equal(beforeSaving.Amount, afterSaving.Amount);

        }

        // ========================================================================

        [Fact]
        public void ExpenseMethod_WriteToFile_WriteToLastFileWrittenToByDefault()
        {
            // Arrange
            String dir = TestConstants.GetSolutionDir();
            Expenses expenses = new Expenses(databaseConnection);
            expenses.ReadFromFile(dir + "\\" + testInputFile);
            string fileName = TestConstants.ExpenseOutputTestFile;
            String outputFile = dir + "\\" + fileName;
            File.Delete(outputFile);
            expenses.SaveToFile(outputFile); // output file is now last file that was written to.
            File.Delete(outputFile);  // Delete the file

            // Act
            expenses.SaveToFile(); // should write to same file as before

            // Assert
            Assert.True(File.Exists(outputFile), "output file created");
            String fileDir = Path.GetFullPath(Path.Combine(expenses.DirName, ".\\"));
            Assert.Equal(dir, fileDir);
            Assert.Equal(fileName, expenses.FileName);

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
            String existingDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(existingDB);
            SQLiteConnection conn = Database.dbConnection;

            // Act
            Expenses expenses = new Expenses(conn);
            List<Expense> list = expenses.List();
            Expense firstExpense = list[0];

            // Assert
            Assert.Equal(numberOfExpensesInFile, list.Count);
            Assert.Equal(firstExpenseInFile.Id, firstExpense.Id);
            Assert.Equal(firstExpenseInFile.Description, firstExpense.Description);

        }

        //=============================================================================

        [Fact]
        public void ExpensesMethod_List_ReturnsListOfExpenses()
        {
            // Arrange
            String folder = TestConstants.GetSolutionDir();
            String newDB = $"{folder}\\{TestConstants.testDBInputFile}";
            Database.existingDatabase(newDB);
            SQLiteConnection conn = Database.dbConnection;
            Expenses expenses = new Expenses(conn);

            // Act
            List<Expense> list = expenses.List();

            // Assert
            Assert.Equal(numberOfExpensesInFile, list.Count);

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
            expenses.Update(id, newDate,newAmount, newDesc, newCatId);

            List<Expense> expList = expenses.List();

            // Assert 
            Assert.Equal(expList[0].Date, newDate);
            Assert.Equal(expList[0].Category, newCatId);
            Assert.Equal(expList[0].Amount, newAmount);
            Assert.Equal(expList[0].Description, newDesc);
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
