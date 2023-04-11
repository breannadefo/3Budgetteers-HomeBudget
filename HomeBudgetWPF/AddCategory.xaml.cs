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
        Presenter _presenter;

        /// <summary>
        /// Creates a new window that allows the user to add a new category to their database.
        /// </summary>
        /// <param name="presenter">The presenter object that contains logic methods.</param>
        public AddCategory(Presenter presenter)
        {
            InitializeComponent();
            InitializeComboBox();
            _presenter = presenter;
            _presenter.SetView(this);
        }

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

        /// <summary>
        /// Displays a pop up that shows an error message.
        /// </summary>
        /// <param name="message">The error message to be displayed.</param>
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Displays a pop up that confirms that the category was added.
        /// </summary>
        /// <param name="message">The success message to be displayed.</param>
        public void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Resets the description and category type values to their default values.
        /// </summary>
        public void ResetValues()
        {
            tbx_description.Text = null;
            cmb_types.SelectedItem = Category.CategoryType.Expense;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string description = txb_description.Text;
            string message = "A description was entered but no category was added for it. Are you sure you wish to exit the screen?";
            
            if (!String.IsNullOrEmpty(description))
            {
                MessageBoxResult result = MessageBox.Show(message, "Unsaved changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel();
                }
                else
                {
                    _presenter.CloseApp();
                }
            }
        }
    }
}
