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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            bool errorFound = false;
            DateTime date;
            if(DateTextBox.Text == null || DateTextBox.Text == string.Empty)
            {
                ShowErrorMessage("The date " + DateTextBox.Text + " is not valid");
                errorFound = true;
            }
            else
            {
                date = DateTime.Parse(DateTextBox.Text);
            }



            string description = DescriptionTextBox.Text;



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
    }
}
