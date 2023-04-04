using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public interface ViewInterface
    {
        public void ShowErrorMessage(string message);

        public void ShowSuccessMessage(string message);

    }
}
