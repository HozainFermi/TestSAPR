using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TestSAPR.Application.DTOs.Part;
using TestSAPR.Application.DTOs.Part.Add;
using TestSAPR.Application.DTOs.Part.Delete;
using TestSAPR.Application.DTOs.Part.Rename;
using TestSAPR.Domain.Model;

namespace TestSAPR.Application.Interfaces
{
    public interface IPartService
    {
        Task<Part> AddNewPartAsync(AddPartDto dto, CancellationToken ct);
        Task<Part> AddNestedPartAsync(AddNestedPartDto dto, CancellationToken ct);
        Task<Part> RenamePartAsync(RenamePartDto dto, CancellationToken ct);
        Task<DeletePartResponse> DeletePartAsync(DeletePartDto dto, CancellationToken ct);
        Task<List<Part>> GetTreeAsync(CancellationToken ct);


    }
}
