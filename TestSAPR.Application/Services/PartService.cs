using System;
using System.Collections.Generic;
using System.Text;
using TestSAPR.Application.DTOs.Part;
using TestSAPR.Application.DTOs.Part.Add;
using TestSAPR.Application.DTOs.Part.Delete;
using TestSAPR.Application.DTOs.Part.Rename;
using TestSAPR.Application.Interfaces;
using TestSAPR.Domain.Interfaces.Repositories;
using TestSAPR.Domain.Model;

namespace TestSAPR.Application.Services
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _repository;

        public PartService(IPartRepository repository)
        {
            _repository = repository;
        }

        public async Task<Part> AddNewPartAsync(AddPartDto dto, CancellationToken ct)
        {
            
            var part = await _repository.CreateNewPartAsync(dto.name, ct);
            return part;
        }       
        public async Task<Part> AddNestedPartAsync(AddNestedPartDto dto, CancellationToken ct)
        {
            var parentPart = await _repository.AddChildPart(dto.part_name, dto.parent_id, dto.quantity, ct);
            return parentPart;
        }


        public async Task<Part> RenamePartAsync(RenamePartDto dto, CancellationToken ct)
        {
            var updatedPart = await _repository.RenamePart(dto.part_id, dto.new_name, ct);
            return updatedPart;
        }

        public async Task<DeletePartResponse> DeletePartAsync(DeletePartDto dto, CancellationToken ct)
        {
            var deletedIds = await _repository.DeletePartAsync(dto.part_id, ct);
            return new DeletePartResponse(deletedIds);
        }


        public async Task<List<Part>> GetTreeAsync(CancellationToken ct)
        {
            var rootParts = await _repository.GetRootPartsAsync(ct);
            return rootParts;
        }

        
    }

}
