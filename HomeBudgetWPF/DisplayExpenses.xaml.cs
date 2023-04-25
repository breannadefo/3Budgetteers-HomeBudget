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
            presenterInterface.SetView(this);
            InitializeComponent();
            InitializeComboBox();
            //DisplayExpensesInGrid();
            ShowExpenses();
        }

        private void InitializeComboBox()
        {
            cmb_categories.ItemsSource = presenterInterface.GetCategories();

            //cmb_categories.ItemsSource = Enum.GetValues(typeof(Category.CategoryType));
            //cmb_categories.SelectedItem = Category.CategoryType.Expense;
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
            ShowExpenses();
        }

        private void mi_Modify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_Delete_Click(object sender, RoutedEventArgs e)
        {

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

        public void DisplayExpensesInGrid(List<BudgetItem> items)
        {
            dg_displayExpenses.ItemsSource = items;
            dg_displayExpenses.Columns.Clear();

            DataGridTextColumn date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");

            DataGridTextColumn category = new DataGridTextColumn();
            category.Header = "Category";
            category.Binding = new Binding("Category");

            DataGridTextColumn description = new DataGridTextColumn();
            description.Header = "Description";
            description.Binding = new Binding("ShortDescription");

            DataGridTextColumn amount = new DataGridTextColumn();
            amount.Header = "Amount";
            amount.Binding = new Binding("Amount");

            DataGridTextColumn balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");

            dg_displayExpenses.Columns.Add(date);
            dg_displayExpenses.Columns.Add(category);
            dg_displayExpenses.Columns.Add(description);
            dg_displayExpenses.Columns.Add(amount);
            dg_displayExpenses.Columns.Add(balance);

        }

        public void DisplayExpensesByMonthInGrid(List<BudgetItemsByMonth> items)
        {
            dg_displayExpenses.ItemsSource = items;
            dg_displayExpenses.Columns.Clear();

            DataGridTextColumn month = new DataGridTextColumn();
            month.Header = "Month";
            month.Binding = new Binding("Month");

            DataGridTextColumn total = new DataGridTextColumn();
            total.Header = "Total";
            total.Binding = new Binding("Total");

            dg_displayExpenses.Columns.Add(month);
            dg_displayExpenses.Columns.Add(total);
        }

        public void DisplayExpensesByCategoryInGrid(List<BudgetItemsByCategory> items)
        {
            dg_displayExpenses.ItemsSource = items;
            dg_displayExpenses.Columns.Clear();

            DataGridTextColumn category = new DataGridTextColumn();
            category.Header = "Category";
            category.Binding = new Binding("Category");

            DataGridTextColumn total = new DataGridTextColumn();
            total.Header = "Total";
            total.Binding = new Binding("Total");

            dg_displayExpenses.Columns.Add(category);
            dg_displayExpenses.Columns.Add(total);
        }

        public void DisplayExpensesInGridDictionary(List<Dictionary<string, object>> items)
        {
            dg_displayExpenses.ItemsSource = items;
            dg_displayExpenses.Columns.Clear();

            //aually add the columns based on the dictionary
        }

        private void ShowExpenses()
        {
            bool month = false, cat = false;

            if (ckb_month.IsChecked == true)
            {
                month = true;
            }
            if (ckb_category.IsChecked == true)
            {
                cat = true;
            }

            presenterInterface.GetBudgetItems(null, null, false, 1, month, cat);
        }
    }
}
