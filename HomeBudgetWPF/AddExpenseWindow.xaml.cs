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
        PresenterInterface _presenter;
        DisplayExpenses displayWindow;

        /// <summary>
        /// Creates a new window where the user can add expenses to their budget.
        /// Called from the display expenses window.
        /// </summary>
        /// <param name="presenter">The presenter object that contains logic methods.</param>
        /// <param name="display">The display expenses window.</param>
        public AddExpenseWindow(PresenterInterface presenter, DisplayExpenses display)
        {
            InitializeComponent();
            _presenter = presenter;
            displayWindow = display;
            InitializeComboBox();
            setDatePickerToToday();
        }

        #region Public Methods

        /// <summary>
        /// Adds a new expense based on user inputs. All user inputs are validated. If any of the
        /// user inputs are invalid the method shows the user an error messages and does not add
        /// the expense. All user inputs remain uncahnged
        /// </summary>
        /// <param name="sender"> The button that triggered the method </param>
        /// <param name="e"> Contains information pertaining to the button click event </param>
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
            MessageBox.Show(message, "Success", 0, MessageBoxImage.Information);
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
                    //stops if the user does not wish to close the window
                    return;
                }
            }
            _presenter.SetView(displayWindow);
            displayWindow.Visibility = Visibility.Visible;
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

        private void ModifyCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategoryPage = new AddCategory(_presenter, displayWindow, this);
            Visibility = Visibility.Hidden;
            _presenter.SetView(addCategoryPage);
            addCategoryPage.Show();
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

        #endregion

        private void btn_viewDisplayExpenses_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
