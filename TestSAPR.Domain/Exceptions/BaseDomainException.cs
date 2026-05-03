using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Domain.Exceptions
{
    public class BaseDomainException:Exception
    {
        public object? ErrorData { get; }
        public int StatusCode { get; }

        protected BaseDomainException(string message, int statusCode, object? data = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorData = data;
        }
    }
}
