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
        AddExpenseWindow _addExpensePage;
        UpdateExpenseWindow _updateExpensePage;
        DisplayExpenses _displayExpensesWindow;

        #endregion

        #region constructor

        /// <summary>
        /// Creates a new window that allows the user to add a new category to the database.
        /// The expense and update windows stored in this page are set to null.
        /// Is called when opened from the display epenses window.
        /// </summary>
        /// <param name="presenter">The presenter to be used.</param>
        /// <param name="display">The display expense window.</param>
        public AddCategory(PresenterInterface presenter, DisplayExpenses display)
        {
            InitializeComponent();
            InitializeComboBox();
            _presenter = presenter;
            _displayExpensesWindow = display;
            _addExpensePage = null;
            _updateExpensePage = null;
        }

        /// <summary>
        /// Creates a new window that allows the user to add a new category to their database.
        /// The update expense window is set to null.
        /// Is called from the add expense window.
        /// </summary>
        /// <param name="presenter">The presenter object that contains logic methods.</param>
        /// <param name="homePage">The main window.</param>
        /// <param name="expensePage">The window where the user can add expenses. It is set as null if no value is provided.</param>
        public AddCategory(PresenterInterface presenter, DisplayExpenses displayWindow, AddExpenseWindow expensePage)
        {
            InitializeComponent();
            InitializeComboBox();
            _presenter = presenter;
            _displayExpensesWindow = displayWindow;
            _addExpensePage = expensePage;
            _updateExpensePage = null;
        }

        /// <summary>
        /// Creates a new add category window. 
        /// Is opened by the update expense window.
        /// The add expense window is set to null.
        /// </summary>
        /// <param name="presenter">The presenter that contains logic methods.</param>
        /// <param name="displayWindow">The display window.</param>
        /// <param name="updateWindow">The update window.</param>
        public AddCategory(PresenterInterface presenter, DisplayExpenses displayWindow, UpdateExpenseWindow updateWindow)
        {
            InitializeComponent();
            InitializeComboBox();
            _presenter = presenter;
            _displayExpensesWindow = displayWindow;
            _updateExpensePage = updateWindow;
            _addExpensePage = null;
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
                //if it was opened by the update expense page
                if(_updateExpensePage != null)
                {

                }
                //if it was opened by the add expense page
                else if(_addExpensePage != null)
                {

                }
                //if it was opened by the view expense page
                else
                {

                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btn_homeScreen_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForWantToLeaveWithUnsavedChanges("There are unsaved changes. Do you still want to leave?"))
            {
                this.Visibility = Visibility.Hidden;
                _addExpensePage.Visibility = Visibility.Hidden;
                //_homePage.Visibility = Visibility.Visible;
                ResetValues();
                //_presenter.SetView(_homePage);

                FromAddExpense = false;
            }
        }

        private void btn_AddExpense_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForWantToLeaveWithUnsavedChanges("There are unsaved changes. Do you still want to leave?"))
            {
                _addExpensePage.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
                //_homePage.Visibility = Visibility.Hidden;
                ResetValues();
                _presenter.SetView(_addExpensePage);

                FromAddExpense = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns>True if the window can close, false otherwise.</returns>
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
