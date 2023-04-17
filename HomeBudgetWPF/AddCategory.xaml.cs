using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Budget;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window, ViewInterface
    {
        #region backing fields

        PresenterInterface _presenter;
        MainWindow _homePage;
        AddExpenseWindow _expensePage;
        bool _cameFromAddExpensePage = false;

        #endregion

        #region constructor

        /// <summary>
        /// Creates a new window that allows the user to add a new category to their database.
        /// </summary>
        /// <param name="presenter">The presenter object that contains logic methods.</param>
        /// <param name="homePage">The main window.</param>
        /// <param name="expensePage">The window where the user can add expenses. It is set as null if no value is provided.</param>
        public AddCategory(PresenterInterface presenter, MainWindow homePage, AddExpenseWindow expensePage = null)
        {
            InitializeComponent();
            InitializeComboBox();
            _presenter = presenter;
            _homePage = homePage;
            _expensePage = expensePage;
        }

        #endregion

        #region properties

        public bool FromAddExpense
        {
            get { return _cameFromAddExpensePage; }
            set { _cameFromAddExpensePage = value; }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Displays a pop up that shows an error message.
        /// </summary>
        /// <param name="message">The error message to be displayed.</param>
        public void ShowErrorMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays a pop up that confirms that the category was added.
        /// </summary>
        /// <param name="message">The success message to be displayed.</param>
        public void ShowSuccessMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Success!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Resets the description and category type values to their default values.
        /// </summary>
        public void ResetValues()
        {
            tbx_description.Text = null;
            cmb_types.SelectedItem = Category.CategoryType.Expense;
        }

        #endregion

        #region private methods

        private void InitializeComboBox()
        {
            cmb_types.ItemsSource = Enum.GetValues(typeof(Category.CategoryType));
            cmb_types.SelectedItem = Category.CategoryType.Expense;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            string description = tbx_description.Text;
            string categoryType = cmb_types.SelectedItem.ToString();

            _presenter.AddCategory(description, categoryType);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string description = tbx_description.Text;
            string message = "A description was entered but no category was added for it. Are you sure you wish to exit the screen?";
            
            if (CheckForWantToLeaveWithUnsavedChanges(message))
            {
                if (FromAddExpense == false)
                {
                    CloseOtherPages();
                }
                else
                {
                    e.Cancel = true;
                    this.Visibility = Visibility.Hidden;
                    ResetValues();
                    FromAddExpense = false;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void CloseOtherPages()
        {
            if (_expensePage.Visibility != Visibility.Visible
                && _homePage.Visibility != Visibility.Visible)
            {
                _expensePage.Close();
                _homePage.Close();
            }
        }

        private void btn_homeScreen_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForWantToLeaveWithUnsavedChanges("There are unsaved changes. Do you still want to leave?"))
            {
                this.Visibility = Visibility.Hidden;
                _expensePage.Visibility = Visibility.Hidden;
                _homePage.Visibility = Visibility.Visible;
                ResetValues();
                _presenter.SetView(_homePage);

                FromAddExpense = false;
            }
        }

        private void btn_AddExpense_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForWantToLeaveWithUnsavedChanges("There are unsaved changes. Do you still want to leave?"))
            {
                _expensePage.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
                _homePage.Visibility = Visibility.Hidden;
                ResetValues();
                _presenter.SetView(_expensePage);

                FromAddExpense = false;
            }
        }

        private bool CheckForWantToLeaveWithUnsavedChanges(string message)
        {
            if (!String.IsNullOrEmpty(tbx_description.Text))
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(message, "Unsaved changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

    }
}
