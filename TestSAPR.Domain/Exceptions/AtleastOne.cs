using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Domain.Exceptions
{
    public class AtLeastOne: BaseDomainException
    {
        public AtLeastOne(string message, Guid parent_id, Guid part_id) : base(message, 409, new {Parent=parent_id, Part=part_id}) { }
    }
}
