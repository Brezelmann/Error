using System;

namespace ErrorTest.Domain.Exceptions
{
    /// <summary>
    /// Gets thrown if the ValueObject receives a wrong Format.
    /// </summary>
    public class WrongNameFormatException : Exception
    {
        public WrongNameFormatException()
        {
        }

        public WrongNameFormatException(string message) : base(message)
        {
        }

        public WrongNameFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}