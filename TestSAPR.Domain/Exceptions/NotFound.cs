using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Domain.Exceptions
{
    public class NotFound: BaseDomainException
    {
        public NotFound(string message, Guid part_id) : base(message, 302, part_id) { }
    }
}
