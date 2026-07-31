using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.Exceptions
{
    public class NotFoundAppException : Exception
    {
        public NotFoundAppException(string message) : base(message)  { }
    }
}
