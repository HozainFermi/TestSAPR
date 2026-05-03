using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Domain.Exceptions
{
    public class HierarchyCycle: BaseDomainException
    {
        public HierarchyCycle(string message, Guid id) : base(message, 409, id) { }
    }
}
