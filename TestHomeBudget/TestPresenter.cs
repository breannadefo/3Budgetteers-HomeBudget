using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetWPF;
using Budget;
using Microsoft.Win32;

namespace TestHomeBudget
{
    [Collection("Sequential")]
    public class TestPresenter
    {
        //same keys as used in the presenter
        const string registrySubKey = @"SOFTWARE\BudgetApplication";
        const string previousDBKey = "Previous database";
     
        #region Test Add Category
        [Fact]
        public void AddCategoryValidParameters()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddCategory("Shiny new sandals", "Expense");

            presenter.CloseBudgetConnection();
            Assert.False(view.CalledShowErrorMessages);
            Assert.True(view.CalledResetValues);
            Assert.True(view.CalledSuccessMessage);
        }


        [Fact]
        public void AddCategoryNullDescription()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);

            presenter.AddCategory(null, "Credit");

            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }
        #endregion

        #region Test Add Expense
        //Testing Success
        [Fact]
        public void TestAddExpense_AddValidExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("Uber", "12/01/2023", "20.00", "Vacation", false);

            presenter.CloseBudgetConnection();
            Assert.False(view.CalledShowErrorMessages);
            Assert.True(view.CalledResetValues);
            Assert.True(view.CalledSuccessMessage);
        }

        //Testing Description
        [Fact]
        public void TestAddExpense_InvalidExpenseEmptyStringDescription()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense(string.Empty, "12/01/2023", "20.00", "Vacation", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestAddExpense_InvalidExpenseNullDescription()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense(null, "12/01/2023", "20.00", "Rent", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        //Testing DateTime
        [Fact]
        public void TestAddExpense_InvalidExpenseNullDateTime()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("Mcdonalds", null, "20.00", "Food", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestAddExpense_InvalidExpenseEmptyStringDateTime()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("Ferris Wheel", String.Empty, "20.00", "Food", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        //Testing Amount
        [Fact]
        public void TestAddExpense_InvalidExpenseEmptyAmount()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("Movie", "12/01/2023", String.Empty, "Entertainment", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestAddExpense_InvalidExpenseNullAmount()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("Movie", "12/01/2023", null, "Entertainment", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestAddExpense_InvalidExpenseStringAmount()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("Movie", "12/01/2023", "Amount", "Entertainment", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestAddExpense_InvalidExpenseNegativeAmount()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("Movie", "12/01/2023", "-19.99", "Entertainment", false);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }
        #endregion

        #region Test Update Expense
        //Testing Success Case
        [Fact]
        public void TestUpdateExpense_ValidUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/2022", "20.00", "Uber", "Vacation");

            presenter.CloseBudgetConnection();
            Assert.False(view.CalledShowErrorMessages);
            Assert.True(view.CalledResetValues);
            Assert.True(view.CalledSuccessMessage);
        }

        //Testing Datetime
        [Fact]
        public void TestUpdateExpense_InvalidDateUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", "20.00", "Uber", "Vacation");

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        //Testing Amount
        [Fact]
        public void TestUpdateExpense_InvalidAmountEmptyStringUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", string.Empty, "Uber", "Vacation");

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestUpdateExpense_InvalidAmountNullUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", null, "Uber", "Vacation");

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestUpdateExpense_InvalidAmountStringUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", "amount", "Uber", "Vacation");

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        //Testing Description
        [Fact]
        public void TestUpdateExpense_InvalidDescriptionNullUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", "20.00", null, "Vacation");

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestUpdateExpense_InvalidDescriptionStringEmptyUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", "20.00", string.Empty, "Vacation");

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        //Testing category
        [Fact]
        public void TestUpdateExpense_InvalidCategoryUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", "20.00", "Uber", "I really hope this isn't a caegory and if it is I will be really unhappy and may even go so far as to cry about it.");

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestUpdateExpense_InvalidCategoryNullUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", "20.00", "Uber", null);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestUpdateExpense_InvalidCategoryEmptyStringUpdateExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            presenter.AddExpense("test Expense", "12/01/2023", "20.01", "Utilities", false);
            view.MakeAllValuesFalse();
            presenter.UpdateExpense(1, "01/01/01/901/01/2143", "20.00", "Uber", string.Empty);

            presenter.CloseBudgetConnection();
            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        #endregion

        #region Test Home Page
        [Fact]
        public void TestEnterHomeBudget_ValidBudgetDirectoryExists()
        {
            //Arrange
            TestView view = new TestView();
            Presenter p = new Presenter(view);

            string budgetFileName = "testBudget";
            //store in user's home directory to garantee the directory exists on any machine
            string budgetFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            //Act
            p.EnterHomeBudget(budgetFileName, budgetFolderPath, true);

            //Assert
            Assert.True(p.VerifyHomeBudgetConnected());
            Assert.False(view.CalledShowErrorMessages);

            p.CloseBudgetConnection();
        }

        [Fact]
        public void TestEnterHomeBudget_ValidBudgetDirectoryDoesNotExist()
        {
            //Arrange
            TestView view = new TestView();
            Presenter p = new Presenter(view);

            string budgetFileName = "testBudget";
            //store in user's home directory to garantee the directory exists on any machine
            string budgetFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\this\\does\\not\\exist\\hopefully";

            //Act
            p.EnterHomeBudget(budgetFileName, budgetFolderPath, true);

            //Assert
            Assert.True(p.VerifyHomeBudgetConnected());
            Assert.False(view.CalledShowErrorMessages);
            
            p.CloseBudgetConnection();
        }

        [Fact]
        public void TestEnterHomeBudget_InvalidBudgetFileName()
        {
            //Arrange
            TestView view = new TestView();
            Presenter p = new Presenter(view);

            string budgetFileName = "test Budget";
            //store in user's home directory to garantee the directory exists on any machine
            string budgetFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\Budget\\";

            //Act
            p.EnterHomeBudget(budgetFileName, budgetFolderPath, true);

            //Assert
            Assert.False(p.VerifyHomeBudgetConnected());
            Assert.True(view.CalledShowErrorMessages);

            p.CloseBudgetConnection();
        }

        [Fact]
        public void TestUsePreviousBudget_PreviousBudgetUsed()
        {
            //Arrange
            TestView view = new TestView();
            Presenter p = new Presenter(view);

            string budgetFileName = "testBudget";
            //store in user's home directory to garantee the directory exists on any machine
            string budgetFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\Budget\\";

            //Act
            p.EnterHomeBudget(budgetFileName, budgetFolderPath, true);
            p.CloseBudgetConnection();

            //Assert
            Assert.True(p.UsePreviousBudget());
            Assert.True(p.VerifyHomeBudgetConnected());
            Assert.False(view.CalledShowErrorMessages);

            p.CloseBudgetConnection();
        }

        [Fact]
        public void TestUsePreviousBudget_PreviousBudgetNotUsed()
        {
            //Arrange
            TestView view = new TestView();
            Presenter p = new Presenter(view);

            string budgetFileName = "testBudget";
            //store in user's home directory to garantee the directory exists on any machine
            string budgetFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\Budget\\";

            //Act
            //Create a home budget and save the it to the registry
            p.EnterHomeBudget(budgetFileName, budgetFolderPath, true);
            p.CloseBudgetConnection();

            //delete the registry's key
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registrySubKey, true);
            key.DeleteValue(previousDBKey);
            key.Close();

            //Assert
            Assert.False(p.UsePreviousBudget());
            Assert.False(p.VerifyHomeBudgetConnected());
            Assert.True(view.CalledShowErrorMessages);

            p.CloseBudgetConnection();
        }
        #endregion

        #region Test Display Expenses

        [Fact]
        public void TestDisplayExpenses_ShowAllExpenses_NoGrouping()
        {
            //Arrange
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            bool month = false, category = false;

            Assert.False(view.CalledDisplayExpenses);
            Assert.False(view.CalledDisplayByMonth);
            Assert.False(view.CalledDisplayByCategory);
            Assert.False(view.CalledDisplayByMonthAndCategory);

            //Act
            presenter.GetBudgetItems(null, null, false, 1, month, category);

            //Assert
            Assert.True(view.CalledDisplayExpenses);
            Assert.False(view.CalledDisplayByMonth);
            Assert.False(view.CalledDisplayByCategory);
            Assert.False(view.CalledDisplayByMonthAndCategory);

            presenter.CloseBudgetConnection();
        }

        [Fact]
        public void TestDisplayExpenses_ShowAllExpenses_GroupByMonth()
        {
            //Arrange
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            bool month = true, category = false;

            Assert.False(view.CalledDisplayExpenses);
            Assert.False(view.CalledDisplayByMonth);
            Assert.False(view.CalledDisplayByCategory);
            Assert.False(view.CalledDisplayByMonthAndCategory);

            //Act
            presenter.GetBudgetItems(null, null, false, 1, month, category);

            //Assert
            Assert.False(view.CalledDisplayExpenses);
            Assert.True(view.CalledDisplayByMonth);
            Assert.False(view.CalledDisplayByCategory);
            Assert.False(view.CalledDisplayByMonthAndCategory);

            presenter.CloseBudgetConnection();
        }

        [Fact]
        public void TestDisplayExpenses_ShowAllExpenses_GroupByCategory()
        {
            //Arrange
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            bool month = false, category = true;

            Assert.False(view.CalledDisplayExpenses);
            Assert.False(view.CalledDisplayByMonth);
            Assert.False(view.CalledDisplayByCategory);
            Assert.False(view.CalledDisplayByMonthAndCategory);

            //Act
            presenter.GetBudgetItems(null, null, false, 1, month, category);

            //Assert
            Assert.False(view.CalledDisplayExpenses);
            Assert.False(view.CalledDisplayByMonth);
            Assert.True(view.CalledDisplayByCategory);
            Assert.False(view.CalledDisplayByMonthAndCategory);

            presenter.CloseBudgetConnection();
        }

        [Fact]
        public void TestDisplayExpenses_ShowAllExpenses_GroupByMonthAndCategory()
        {
            //Arrange
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.EnterHomeBudget("databases/testDatabase", "No folder specified", true);

            bool month = true, category = true;

            Assert.False(view.CalledDisplayExpenses);
            Assert.False(view.CalledDisplayByMonth);
            Assert.False(view.CalledDisplayByCategory);
            Assert.False(view.CalledDisplayByMonthAndCategory);

            //Act
            presenter.GetBudgetItems(null, null, false, 1, month, category);

            //Assert
            Assert.False(view.CalledDisplayExpenses);
            Assert.False(view.CalledDisplayByMonth);
            Assert.False(view.CalledDisplayByCategory);
            Assert.True(view.CalledDisplayByMonthAndCategory);

            presenter.CloseBudgetConnection();
        }

        #endregion
    }
}
