using System;

namespace Bridge.IBLL.Exceptions
{
    public class BllException : Exception
    {

        public BllException(string message)
            : base(message)
        {
        }

    }
}
