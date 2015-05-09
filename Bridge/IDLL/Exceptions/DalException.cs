using System;

namespace Bridge.IDLL.Exceptions
{
    public class DalException : Exception
    {

        public DalException(string message)
            : base(message)
        {
        }

    }
}
