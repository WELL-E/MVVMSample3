using System;

namespace MVVMSample.Common
{
    public class NumberChangedEventArgs : EventArgs
    {
        public int Number { get; set; }

        public NumberChangedEventArgs(int num)
        {
            Number = num;
        }
    }
}
