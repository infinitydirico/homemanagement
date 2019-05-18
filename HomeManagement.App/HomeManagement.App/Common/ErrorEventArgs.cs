using System;

namespace HomeManagement.App.Common
{
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}
