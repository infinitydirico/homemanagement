using System;
using System.Globalization;

namespace HomeManagement.Localization
{
    public class CultureChangeEventArgs : EventArgs
    {
        public CultureInfo Culture { get; set; }
    }
}
