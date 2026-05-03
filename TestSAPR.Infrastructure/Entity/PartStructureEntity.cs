using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Infrastructure.Entity
{
    public class PartStructureEntity
    {
        public DateTime CreatedAt { get; set; }
        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }
        public int Quantity { get; set; }

        public PartEntity Parent { get; set; }
        public PartEntity Child { get; set; }
    }
}
