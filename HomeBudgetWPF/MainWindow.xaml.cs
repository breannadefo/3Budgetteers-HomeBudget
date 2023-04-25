using Budget;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeBudgetWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        PresenterInterface p;
        DisplayExpenses displayWindow;

        /// <summary>
        /// Is called only once, on app startup.
        /// Creates a main window and shows it.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            p = new Presenter(this);
        }

        private void btn_browseBudgetFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folder.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txb_budgetFolderPath.Text = folder.SelectedPath;
            }
        }

        private void btn_enterBudgetFolder_Click(object sender, RoutedEventArgs e)
        {
            if (p.EnterHomeBudget(txb_budgetFileName.Text, txb_budgetFolderPath.Text, (bool)chk_newBudget.IsChecked))
            {
                txblock_budgetInUse.Text = "There is a budget currently in use";
            }

        }
        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success!", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to close the app?", "App closing", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                CloseOtherPages();
                p.CloseBudgetConnection();
            }
        }

        private void CloseOtherPages()
        {
            if (p.VerifyHomeBudgetConnected())
            {
                if (displayWindow != null)
                {
                    displayWindow.Close();
                }
            }
        }

        public void ResetValues()
        {
            throw new NotImplementedException();
        }

        private void btn_previousBudget_Click(object sender, RoutedEventArgs e)
        {
            if (p.UsePreviousBudget())
            {
                txblock_budgetInUse.Text = "There is a budget currently in use";
            }
        }

        private void btn_viewExpenses_Click(object sender, RoutedEventArgs e)
        {
            DisplayExpenses display = new DisplayExpenses(this, p);
            Visibility= Visibility.Hidden;
            p.SetView(display);
            display.Show();
        }

        public void DisplayExpensesByMonthInGrid(List<BudgetItemsByMonth> items)
        {
            throw new NotImplementedException();
        }

        public void DisplayExpensesInGrid(List<BudgetItem> items)
        {
            throw new NotImplementedException();
        }

        public void DisplayExpensesByCategoryInGrid(List<BudgetItemsByCategory> items)
        {
            throw new NotImplementedException();
        }

        public void DisplayExpensesInGridDictionary(List<Dictionary<string, object>> items)
        {
            throw new NotImplementedException();
        }
    }
}
