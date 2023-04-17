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
        AddExpenseWindow _newExpenseWindow;
        AddCategory _newCategoryWindow;

        public MainWindow()
        {
            InitializeComponent();
            p = new Presenter(this);
        }

        public MainWindow(Presenter p)
        {
            InitializeComponent();
            this.p = p;
        }

        private void btn_browseBudgetFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result= folder.ShowDialog();
         
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                txb_budgetFolderPath.Text = folder.SelectedPath;
            }
        }

        private void btn_enterBudgetFolder_Click(object sender, RoutedEventArgs e)
        {
            if (p.EnterHomeBudget(txb_budgetFileName.Text,txb_budgetFolderPath.Text,(bool)chk_newBudget.IsChecked))
            {
                txblock_budgetInUse.Text = "There is a budget currently in use";

                _newExpenseWindow = new AddExpenseWindow(p, this);
                _newExpenseWindow.Visibility = Visibility.Hidden;
                _newCategoryWindow = new AddCategory(p, this, _newExpenseWindow);
                _newCategoryWindow.Visibility = Visibility.Hidden;
                _newExpenseWindow.AddCategoryPage = _newCategoryWindow;
            }

        }

        private void btn_addNewExpense_Click(object sender, RoutedEventArgs e)
        {
            if (p.VerifyHomeBudgetConnected())
            {
                Visibility = Visibility.Hidden;
                _newCategoryWindow.Visibility= Visibility.Hidden;
                _newExpenseWindow.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("You need to enter a budget to work with!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btn_addNewCategory_Click(object sender, RoutedEventArgs e)
        {
            if (p.VerifyHomeBudgetConnected())
            {
                Visibility = Visibility.Hidden;
                _newExpenseWindow.Visibility = Visibility.Hidden;
                _newCategoryWindow.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("You need to enter a budget to work with!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                p.CloseBudgetConnection();
                CloseOtherPages();
            }
        }

        private void CloseOtherPages()
        {
            if (_newExpenseWindow.Visibility != Visibility.Visible 
                && _newCategoryWindow.Visibility != Visibility.Visible)
            {
                _newExpenseWindow.Close();
                _newCategoryWindow.Close();
            }
        }

        public void ResetValues()
        {
            throw new NotImplementedException();
        }
    }
}
