using System;

namespace Sorted_Box
{
    /// <summary>
    /// Exception when the amount received from the customer is out of range
    /// </summary>
    public class AmountOutOfRangeException : Exception
    {
        public AmountOutOfRangeException() { }
        public AmountOutOfRangeException(string message) : base(message) { }
    }
}
