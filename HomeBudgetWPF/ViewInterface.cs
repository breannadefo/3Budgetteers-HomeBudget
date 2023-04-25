using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public interface ViewInterface
    {
        public void ShowErrorMessage(string message);

        public void ShowSuccessMessage(string message);

        public void ResetValues();

        public void DisplayExpensesInGrid(List<BudgetItem> items);

        public void DisplayExpensesByMonthInGrid(List<BudgetItemsByMonth> items);

        public void DisplayExpensesByCategoryInGrid(List<BudgetItemsByCategory> items);

        public void DisplayExpensesInGridDictionary(List<Dictionary<string, object>> items);
    }
}
