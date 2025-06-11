using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativoChallenge.Application.Common.Exceptions
{
    public class ForbiddenAccessException : ApplicationException
    {
        public ForbiddenAccessException() : base("You do not have permission to perform this action.")
        {
        }
        public ForbiddenAccessException(string message) : base(message)
        {
        }
        public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


}