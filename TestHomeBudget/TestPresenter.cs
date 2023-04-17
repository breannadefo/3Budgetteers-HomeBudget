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
        /*
        [Fact]
        public void AddCategoryValidParameters()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);

            presenter.AddCategory("Shiny new sandals", "Expense");

            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }
        */

        [Fact]
        public void TestAddCategoryNullDescription()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);

            presenter.AddCategory(null, "Credit");

            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }


        #region Test Add Expense

        [Fact]
        public void TestAddValidExpense()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.InitializeHomeBudget("databases/database", true);

            presenter.AddExpense("Uber", "12/01/2023", "20.00", "Transport", false);

            Assert.False(view.CalledShowErrorMessages);
            Assert.True(view.CalledResetValues);
            Assert.True(view.CalledSuccessMessage);
        }

        [Fact]
        public void TestInvalidExpenseEmptyStringDescription()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);
            presenter.InitializeHomeBudget("databases/database", true);

            presenter.AddExpense(string.Empty, "12/01/2023", "20.00", "Expenses", false);

            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        public void TestInvalidExpenseNullDescription()
        {
            TestView view = new TestView();
            Presenter presenter = new Presenter(view);

            presenter.AddExpense(null, "12/01/2023", "20.00", "Expenses", false);

            Assert.True(view.CalledShowErrorMessages);
            Assert.False(view.CalledResetValues);
            Assert.False(view.CalledSuccessMessage);
        }

        #endregion
    }
}
