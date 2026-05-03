using System;
using System.Collections.Generic;
using System.Text;
using TestSAPR.Domain.Model;

namespace TestSAPR.Domain.Interfaces.Repositories
{
    public interface IPartRepository
    {
        public Task<Part> CreateNewPartAsync(string name, CancellationToken ct);
        public Task<Part> AddChildPart(string partName, Guid parent_id, int quantity, CancellationToken ct);
        public Task<Part> RenamePart(Guid part_id, string new_name, CancellationToken ct);
        public Task<List<Guid>> DeletePartAsync(Guid partId, CancellationToken ct);
        Task<List<Part>> GetRootPartsAsync(CancellationToken ct);
    }
}
