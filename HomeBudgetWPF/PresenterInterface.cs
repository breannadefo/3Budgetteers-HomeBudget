using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public interface PresenterInterface
    {
        public void AddCategory(string description, string categoryType);

        public void AddExpense(string description, string date, string amount, string categpory, bool credit);

        public void UpdateExpense(int id, string date, string amount, string description, string category);

        public void DeleteExpense(int expenseId);

        public List<Category> GetCategories();

        public void InitializeHomeBudget(string database, bool newDb);

        public bool EnterHomeBudget(string budgetFileName, string budgetFolderPath, bool newDb);

        public bool VerifyHomeBudgetConnected();

        public void CloseBudgetConnection();

        public void SetView(ViewInterface view);

        public bool UsePreviousBudget();
    }
}
