using System;

namespace Ya.Taxi
{
    public class FieldSizeNotSupportedException : Exception
    {
        public FieldSizeNotSupportedException() : base()
        {

        }

        public FieldSizeNotSupportedException(string message) : base(message)
        {

        }
    }
}