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
        public DisplayExpenses()
        {
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

        }

        private void btn_AddCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_HomePage_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
