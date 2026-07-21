using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualSenseSharp
{
    public class UnknownDualsenseException : Exception
    {
        public UnknownDualsenseException(string? message = null, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}
