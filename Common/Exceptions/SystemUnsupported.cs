using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class SystemUnsupportedException : Exception
    {
        public SystemUnsupportedException()
        {
        }

        public SystemUnsupportedException(string message)
            : base(message)
        {
        }

        public SystemUnsupportedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
