using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Budget;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for DisplayExpenses.xaml
    /// </summary>
    public partial class DisplayExpenses : Window, ViewInterface
    {
        MainWindow mainWindow;
        PresenterInterface presenterInterface;
        bool closeFromHomePageButton = false;

        public DisplayExpenses(MainWindow window, PresenterInterface p)
        {
            this.mainWindow = window;
            this.presenterInterface = p;
            InitializeComponent();
            //InitializeComboBox();
            DisplayExpensesInGrid();
        }

        private void InitializeComboBox()
        {
            cmb_categories.ItemsSource = Enum.GetValues(typeof(Category.CategoryType));
            cmb_categories.SelectedItem = Category.CategoryType.Expense;
        }

        private void btn_AddExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow addExpenseWindow = new AddExpenseWindow(presenterInterface, this);
            Visibility = Visibility.Hidden;
            presenterInterface.SetView(addExpenseWindow);
            addExpenseWindow.Show();
        }

        private void btn_AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategoryWindow = new AddCategory(presenterInterface, this);
            Visibility = Visibility.Hidden;
            presenterInterface.SetView(addCategoryWindow);
            addCategoryWindow.Show();
        }

        private void btn_HomePage_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Visibility = Visibility.Visible;
            closeFromHomePageButton = true;
            presenterInterface.SetView(mainWindow);
            this.Close();
        }

        private void ckb_GroupingAltered(object sender, RoutedEventArgs e)
        {

        }

        private void mi_Modify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_Delete_Click(object sender, RoutedEventArgs e)
        {
            if(!(dg_displayExpenses.SelectedItem == null || dg_displayExpenses.SelectedItem == string.Empty))
            {
                BudgetItem item = (BudgetItem)dg_displayExpenses.SelectedItem;
                presenterInterface.DeleteExpense(item.ExpenseID);
                this.DisplayExpensesInGrid();
            }
        }

        private void mi_Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //check if there are any unadded fields left
            //close the app as a whole
            if(!closeFromHomePageButton)
            mainWindow.Close();
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success!", MessageBoxButton.OK, MessageBoxImage.None);
        }

        public void ResetValues()
        {
            throw new NotImplementedException();
        }

        public void DisplayExpensesInGrid()
        {
            List<BudgetItem> expenses = presenterInterface.GetBudgetItems(null, null, false, 1);

            dg_displayExpenses.ItemsSource = expenses;
            dg_displayExpenses.Columns.Clear();

            DataGridTextColumn date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");
            DataGridTextColumn category = new DataGridTextColumn();
            category.Header = "Category";
            category.Binding = new Binding("Category");
            DataGridTextColumn descriptoin = new DataGridTextColumn();
            descriptoin.Header = "Description";
            descriptoin.Binding = new Binding("ShortDescription");
            DataGridTextColumn amount = new DataGridTextColumn();
            amount.Header = "Amount";
            amount.Binding = new Binding("Amount");
            DataGridTextColumn balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");

            dg_displayExpenses.Columns.Add(date);
            dg_displayExpenses.Columns.Add(category);
            dg_displayExpenses.Columns.Add(descriptoin);
            dg_displayExpenses.Columns.Add(amount);
            dg_displayExpenses.Columns.Add(balance);

        }
    }
}
