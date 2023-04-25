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
    /// Interaction logic for UpdateExpenseWindow.xaml
    /// </summary>
    public partial class UpdateExpenseWindow : Window, ViewInterface
    {
        PresenterInterface _presenter;
        Expense _expense;
        DisplayExpenses _displayExpensesWindow;

        /// <summary>
        /// Opens a new update expense window. 
        /// Can only be opened by the display expenses window.
        /// </summary>
        /// <param name="Presenter">The presenter to be used to handle the backend logic.</param>
        /// <param name="Expense">The expense to modify.</param>
        /// <param name="display">The display expenses window the update window was called from.</param>
        public UpdateExpenseWindow(PresenterInterface Presenter, Expense Expense, DisplayExpenses display)
        {
            InitializeComponent();
            _presenter = Presenter;
            _expense = Expense;
            _displayExpensesWindow = display;
            IntializWithOldValues();
        }

        #region Startup Methods
        private void IntializWithOldValues()
        {
            categoryComboBox.Text = this._expense.Category.ToString();
            DateTextBox.Text = this._expense.Date.ToString();
            DescriptionTextBox.Text = this._expense.Description;
            AmountTextBox.Text = this._expense.Amount.ToString();
        }

        private void InitializeComboBox()
        {
            categoryComboBox.ItemsSource = _presenter.GetCategories();
        }

        private void InitializeComboBox(object sender, RoutedEventArgs e)
        {
            categoryComboBox.ItemsSource = _presenter.GetCategories();
        }

        private void InitializeComboBox(object sender, EventArgs e)
        {
            categoryComboBox.ItemsSource = _presenter.GetCategories();
        }
        #endregion

        //button to open add category window
        private void ModifyCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory cat = new AddCategory(_presenter, _displayExpensesWindow, this);
            this.Visibility = Visibility.Hidden;
            cat.Show();
        }

        private void CancelExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ResetValues();
        }

        private void UpdateExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.UpdateExpense(_expense.Id, DescriptionTextBox.Text, DateTextBox.Text, AmountTextBox.Text, categoryComboBox.Text);
        }

        private void DeleteExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            _presenter.DeleteExpense(this._expense.Id);
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Category> categories = _presenter.GetCategories();
            if (searchBox.Text == "")
                return;

            int index = categories.FindIndex((category) => category.Description.ToLower().StartsWith(searchBox.Text.ToLower()));
            if (index != -1)
                categoryComboBox.SelectedIndex = index;
        }


        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", 0, MessageBoxImage.Error);
        }

        public void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success", 0, MessageBoxImage.Information);
        }

        public void ResetValues()
        {
            DescriptionTextBox.Text = string.Empty;
            AmountTextBox.Text = string.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _displayExpensesWindow.Visibility = Visibility.Visible;
            _presenter.SetView(_displayExpensesWindow);
        }

        public void DisplayExpensesByMonthInGrid(List<BudgetItemsByMonth> items)
        {
            throw new NotImplementedException();
        }
    }


}
