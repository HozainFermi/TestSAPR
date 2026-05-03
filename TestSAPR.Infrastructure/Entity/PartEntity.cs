using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestSAPR.Infrastructure.Entity
{
    public class PartEntity
    {        
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }

        public List<PartStructureEntity> ChildParts { get; set; } 
        public List<PartStructureEntity> ParentParts { get; set; }
    }
}
