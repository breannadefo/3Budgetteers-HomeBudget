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
    /// Interaction logic for DisplayExpenses.xaml
    /// </summary>
    public partial class DisplayExpenses : Window
    {
        MainWindow mainWindow;
        PresenterInterface presenterInterface;
        bool closeFromHomePageButton = false;

        public DisplayExpenses(MainWindow window, PresenterInterface p)
        {
            this.mainWindow = window;
            this.presenterInterface = p;
            InitializeComponent();
            //InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            cmb_categories.ItemsSource = Enum.GetValues(typeof(Category.CategoryType));
            cmb_categories.SelectedItem = Category.CategoryType.Expense;
        }

        private void btn_AddExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseWindow addExpenseWindow = new AddExpenseWindow(presenterInterface, this);
            Visibility = Visibility.Hidden;
            addExpenseWindow.Show();
        }

        private void btn_AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategoryWindow = new AddCategory(presenterInterface, this);
            Visibility = Visibility.Hidden;
            addCategoryWindow.Show();
        }

        private void btn_HomePage_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Visibility = Visibility.Visible;
            closeFromHomePageButton = true;
            this.Close();
        }

        private void ckb_GroupingAltered(object sender, RoutedEventArgs e)
        {

        }

        private void mi_Modify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //check if there are any unadded fields left
            //close the app as a whole
            if(!closeFromHomePageButton)
            mainWindow.Close();
        }
    }
}
