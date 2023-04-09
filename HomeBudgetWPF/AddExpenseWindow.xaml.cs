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

        public AddExpenseWindow(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _presenter.SetView(this);
            InitializeComboBox();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            bool errorFound = false;

            //Validates the category
            Category.CategoryType categoryType = Category.CategoryType.Credit;
            if (categoryComboBox.Text == null || categoryComboBox.Text.ToString() == string.Empty)
            {
                ShowErrorMessage("Please select a category type from the drop down menu");
                errorFound = true;
            }
            else
            {
                if(!Enum.TryParse<Category.CategoryType>(categoryComboBox.SelectedValue.ToString(), out categoryType))
                {
                    errorFound = true;
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
                    amount = result;
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
                _presenter.AddExpense(description, date, amount, (int)categoryType);
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
        /// 
        /// </summary>
        public void ResetValues()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Occupies the categorytype combo box with the category types present in the
        /// category types enum.
        /// </summary>
        private void InitializeComboBox()
        {
            categoryComboBox.ItemsSource = Enum.GetValues(typeof(Category.CategoryType));
            categoryComboBox.SelectedItem = Category.CategoryType.Expense;
        }
    }
}
