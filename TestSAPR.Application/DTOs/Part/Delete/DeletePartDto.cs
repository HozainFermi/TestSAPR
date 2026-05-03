using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Application.DTOs.Part.Delete
{
    public record DeletePartDto(Guid part_id);
    public record DeletePartResponse(List<Guid> deleted_parts);
}
