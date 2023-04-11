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
        Presenter p;

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

        //OPTIONAL FOR LATER
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
            }
            
        }

        private void btn_addNewExpense_Click(object sender, RoutedEventArgs e)
        {
            if (p.VerifyHomeBudgetConnected())
            {
                AddExpenseWindow newWindow = new AddExpenseWindow(p);
                Visibility = Visibility.Hidden;
                newWindow.Show();
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
                AddCategory newWindow = new AddCategory(p);
                this.Close();
                newWindow.Show();
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
            }
        }

        public void ResetValues()
        {
            throw new NotImplementedException();
        }
    }
}
