using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Budget;
using HomeBudgetWPF;


namespace TestHomeBudget
{
    internal class TestView : ViewInterface
    {
        bool _calledResetValues;
        bool _calledShowErrorMessages;
        bool _calledShowSuccessMessages;
        bool _calledDisplayExpenses;
        bool _calledDisplayByMonth;
        bool _calledDisplayByCategory;
        bool _calledDisplayByMonthAndCategory;

        public TestView()
        {
            _calledResetValues = false;
            _calledShowErrorMessages = false;
            _calledShowSuccessMessages = false;
            _calledDisplayExpenses = false;
            _calledDisplayByMonth = false;
            _calledDisplayByCategory = false;
            _calledDisplayByMonthAndCategory = false;
        }

        #region properties

        public bool CalledResetValues
        {
            get { return _calledResetValues; }
        }

        public bool CalledShowErrorMessages
        {
            get { return _calledShowErrorMessages; }
        }

        public bool CalledSuccessMessage
        {
            get { return _calledShowSuccessMessages; }
        }

        public bool CalledDisplayExpenses
        {
            get { return _calledDisplayExpenses; }
        }

        public bool CalledDisplayByMonth
        {
            get { return _calledDisplayByMonth; }
        }

        public bool CalledDisplayByCategory
        {
            get { return _calledDisplayByCategory; }
        }

        public bool CalledDisplayByMonthAndCategory
        {
            get { return _calledDisplayByMonthAndCategory; }
        }

        #endregion

        #region methods

        public void MakeAllValuesFalse()
        {
            _calledResetValues = false;
            _calledShowErrorMessages = false;
            _calledShowSuccessMessages = false;
            _calledDisplayExpenses = false;
            _calledDisplayByMonth = false;
            _calledDisplayByCategory = false;
            _calledDisplayByMonthAndCategory = false;
        }

        public void DisplayExpensesByCategoryInGrid(List<BudgetItemsByCategory> items)
        {
            _calledDisplayByCategory = true;
        }

        public void DisplayExpensesByMonthInGrid(List<BudgetItemsByMonth> items)
        {
            _calledDisplayByMonth = true;
        }

        public void DisplayExpensesInGrid(List<BudgetItem> items)
        {
            _calledDisplayExpenses = true;
        }

        public void DisplayExpensesInGridDictionary(List<Dictionary<string, object>> items)
        {
            _calledDisplayByMonthAndCategory = true;
        }

        public void ResetValues()
        {
            _calledResetValues = true;
        }

        public void ShowErrorMessage(string message)
        {
            _calledShowErrorMessages = true;
        }

        public void ShowSuccessMessage(string message)
        {
            _calledShowSuccessMessages = true;
        }

        #endregion
    }
}
