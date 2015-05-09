using System;

namespace IBLL.Exceptions
{
    public class BllException : Exception
    {

        public BllException(string message)
            : base(message)
        {
        }

    }
}
