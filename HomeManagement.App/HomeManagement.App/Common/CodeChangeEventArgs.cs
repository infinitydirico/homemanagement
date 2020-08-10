using System;
namespace HomeManagement.App.Common
{
    public class CodeChangeEventArgs : EventArgs
    {
        public bool HasChanged { get; set; }

        public static CodeChangeEventArgs Changed(bool changed) => new CodeChangeEventArgs { HasChanged = changed };
    }
}
