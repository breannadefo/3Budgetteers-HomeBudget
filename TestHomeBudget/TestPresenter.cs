using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetWPF;
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
        public void AddValidExpense()
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
        public void InvalidExpenseEmptyStringDescription()
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
        public void InvalidExpenseNullDescription()
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
        public void InvalidExpenseNullDateTime()
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
        public void InvalidExpenseEmptyStringDateTime()
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
        public void InvalidExpenseEmptyAmount()
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
        public void InvalidExpenseNullAmount()
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
        public void InvalidExpenseStringAmount()
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
        public void InvalidExpenseNegativeAmount()
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
    }
}
