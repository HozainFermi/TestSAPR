using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Domain.Exceptions
{
    public class PartAlreadyExists : BaseDomainException
    {
        public PartAlreadyExists(string message, string name) : base(message, 409, name) {}
    }
}
