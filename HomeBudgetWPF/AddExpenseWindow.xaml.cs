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
            bool errorFound = false;

            //Validates the category
            Category category = null;
            if (categoryComboBox.Text == null || categoryComboBox.Text.ToString() == string.Empty)
            {
                ShowErrorMessage("Please select a category type from the drop down menu");
                errorFound = true;
            }
            else
            {
                List<Category> categories = _presenter.GetCategories();
                foreach (Category categoryFromList in categories)
                {
                    if (categoryFromList.ToString() == categoryComboBox.Text.ToString())
                    {
                        category = categoryFromList;
                        break;
                    }
                }
            }

            //Validates the date
            DateTime date = DateTime.Now;
            if(DateTextBox.Text == null || DateTextBox.Text == string.Empty)
            {
                ShowErrorMessage("The date " + DateTextBox.Text + " is not valid");
                errorFound = true;
            }
            else
            {
                date = DateTime.Parse(DateTextBox.Text);
            }

            //Validates the description
            string description = string.Empty;
            if(DescriptionTextBox.Text == null || DescriptionTextBox.Text == string.Empty)
            {
                ShowErrorMessage("The description cannot be empty");
                errorFound = true;
            }
            else
            {
                description = DescriptionTextBox.Text;
            }

            //Validates amount
            int amount = 0;
            if(AmountTextBox.Text == null || AmountTextBox.Text == string.Empty)
            {
                ShowErrorMessage("Amount cannot be none. Please input an amount for the expense");
                errorFound = true;
            }
            else
            {
                if(int.TryParse(AmountTextBox.Text, out int result))
                {
                    if(result < 0)
                    {
                        ShowErrorMessage("Amount cannot be negative");
                    }
                    else
                    {
                        amount = result;
                    }
                }
                else
                {
                    ShowErrorMessage("Amount cannt be a word or contain letters. It must be a number represting the cost of the expense");
                    errorFound = true;
                }
            }

            //If no error has been encountered the values are added
            if(!errorFound)
            {
                if (category.Type == Category.CategoryType.Expense || category.Type == Category.CategoryType.Savings)
                {
                    _presenter.AddExpense(description, date, amount * -1, category.Id);
                }
                else
                {
                    _presenter.AddExpense(description, date, amount, category.Id);
                }
                

                if(CreditCheckbox.IsChecked == true)
                {
                    _presenter.AddExpense("credit", date, amount, 8);
                }

                ShowExpenseAddedMessage(description);
                ResetValues();
            }
        }

        /// <summary>
        /// Resets the values in the user input boxes to their default values if the user clicks
        /// yes on the pop up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            //Unhide the home page
        }

        private void ModifyCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategoryWindow = new AddCategory(this._presenter);
            addCategoryWindow.Show();
        }
        #endregion

        #region Private Methods
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
