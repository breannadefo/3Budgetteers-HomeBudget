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

        private void CreditCheckbox_Click(object sender, RoutedEventArgs e)
        {
            


        }

        public void ShowErrorMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowSuccessMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
