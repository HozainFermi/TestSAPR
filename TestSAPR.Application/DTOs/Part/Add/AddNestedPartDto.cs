using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Application.DTOs.Part.Add
{
    public record AddNestedPartDto(Guid parent_id, string part_name, int quantity);
    
}
