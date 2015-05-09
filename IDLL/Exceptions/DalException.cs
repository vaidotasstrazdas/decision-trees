using System;

namespace IDLL.Exceptions
{
    public class DalException : Exception
    {

        public DalException(string message)
            : base(message)
        {
        }

    }
}
