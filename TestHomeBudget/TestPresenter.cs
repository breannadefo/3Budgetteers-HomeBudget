using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetWPF;

namespace TestHomeBudget
{
    [Collection("Sequential")]
    public class TestPresenter
    {
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
        
        //Testing Amount
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
    }
}
