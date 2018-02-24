using System;

namespace Ya.Taxi
{
    public class NotValidMovementException : Exception
    {
        public NotValidMovementException() : base()
        {
            //todo some logic
        }

        public NotValidMovementException(string message) : base(message)
        {
            //todo some logic
        }
    }
}