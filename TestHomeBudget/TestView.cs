using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetWPF;


namespace TestHomeBudget
{
    internal class TestView : ViewInterface
    {
        bool _calledResetValues;
        bool _calledShowErrorMessages;
        bool _calledShowSuccessMessages;

        public TestView()
        {
            _calledResetValues = false;
            _calledShowErrorMessages = false;
            _calledShowSuccessMessages = false;
        }

        public bool CalledResetValues
        {
            get { return _calledResetValues; }
        }

        public bool CalledShowErrorMessages
        {
            get { return _calledShowErrorMessages; }
        }

        public bool CalledSuccessMessage
        {
            get { return _calledShowSuccessMessages; }
        }

        public void ResetValues()
        {
            _calledResetValues = true;
        }

        public void ShowErrorMessage(string message)
        {
            _calledShowErrorMessages = true;
        }

        public void ShowSuccessMessage(string message)
        {
            _calledShowSuccessMessages = true;
        }
    }
}
