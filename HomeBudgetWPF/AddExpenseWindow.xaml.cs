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
    /// Interaction logic for AddExpenseWindow.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window, ViewInterface
    {
        Presenter _presenter;
        Window _homePage;

        public AddExpenseWindow(Presenter presenter, Window homePage)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.SetView(this);
            _homePage = homePage;
            InitializeComboBox();
            setDatePickerToToday();
        }

        public AddExpenseWindow()
        {
            Presenter presenter = new Presenter(this);
            InitializeComponent();
            _presenter = presenter;
            _presenter.SetView(this);
            InitializeComboBox();
            setDatePickerToToday();
        }

        #region Methods
        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            bool checkbox = (bool)CreditCheckbox.IsChecked;
            _presenter.AddExpense(DescriptionTextBox.Text, DateTextBox.Text, AmountTextBox.Text, categoryComboBox.Text, checkbox);
        }

        private void CancelExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure you want to cancel the current expense? This will remove the inputs you have made", "Cancel Expense", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                this.ResetValues();
            }
        }
        
        /// <summary>
        /// Creates an error message pop up and displays it to the user
        /// </summary>
        /// <param name="message"> Message contained in the pop up </param>
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", 0,  MessageBoxImage.Error);
        }

        /// <summary>
        /// Creates a success message pop up and displays it to the user
        /// </summary>
        /// <param name="message"> Message contained in the pop up </param>
        public void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success", 0);
        }

        /// <summary>
        /// Resets the amount, description and checkbox to empty/unchecked
        /// </summary>
        public void ResetValues()
        {
            DescriptionTextBox.Text = string.Empty;
            AmountTextBox.Text = string.Empty;
            CreditCheckbox.IsChecked = false;
        }

        private void AddExpenseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(DescriptionTextBox.Text.ToString() != string.Empty || AmountTextBox.Text != string.Empty)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes, do you still want to close the expenses window?", "App closing", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    _presenter.CloseBudgetConnection();
                }
            }
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

        private void CurrentDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_homePage is MainWindow)
            {
                Close();
                _homePage.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("We have a big problem");
            }
        }

        private void ModifyCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategoryWindow = new AddCategory(this._presenter, _homePage);
            addCategoryWindow.Show();
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

        private void setDatePickerToToday()
        {
            DateTime today = DateTime.Now;
            DateTextBox.Text = today.ToString();
        }

        private void ShowExpenseAddedMessage(string description)
        {
            MessageBox.Show("Expense " + description + " has been added succesfully!", "Expense Added Succesfully", 0, MessageBoxImage.Information);
        }
        #endregion

    }
}
