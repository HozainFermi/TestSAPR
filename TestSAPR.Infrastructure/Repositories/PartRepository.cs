using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TestSAPR.Domain.Exceptions;
using TestSAPR.Domain.Interfaces.Repositories;
using TestSAPR.Domain.Model;
using TestSAPR.Infrastructure.ApplicationDbContext;
using TestSAPR.Infrastructure.Entity;

namespace TestSAPR.Infrastructure.Repositories
{
    public class PartRepository : IPartRepository
    {
        private AppDbContext _context;
        public PartRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Part> AddChildPart(string partName, Guid parentId, int quantity, CancellationToken ct)
        {
            Guid partId;
            var foundPart= await _context.Parts.Where(p=> p.Name==partName).FirstOrDefaultAsync(ct);
            if (foundPart != null)
            {
                partId = foundPart.Id;
            }
            else
            {
                var createdPart = await _context.Parts.AddAsync(new PartEntity {Name=partName});
                await _context.SaveChangesAsync(ct);
                partId = createdPart.Entity.Id;
            }


            if (partId == parentId) {throw new HierarchyCycle("Part cant contain itself!", partId);}
            if (quantity <= 0) {throw new AtLeastOne("Needs at least one part to add nested item", parentId, partId);}
            if (await IsCycleExists(parentId, partId, ct)) {throw new HierarchyCycle("Part cant contain itself!", partId);}

            var structure = new PartStructureEntity
            {
                ParentId = parentId,
                ChildId = partId,
                Quantity = quantity
            };

            await _context.Structures.AddAsync(structure, ct);
            await _context.SaveChangesAsync(ct);
       
            return await GetPartByIdAsync(parentId, ct);
        }

        private async Task<bool> IsCycleExists(Guid parentId, Guid partId, CancellationToken ct)
        {
            var query = @"
            WITH ParentHierarchy AS (
            SELECT ParentId  FROM part_structures WHERE ChildId = {0}
            UNION ALL
            SELECT s.ParentId  FROM part_structures s
            INNER JOIN ParentHierarchy ph ON s.ChildId  = ph.ParentId 
            )

            SELECT CAST(CASE WHEN EXISTS (
            SELECT 1 FROM ParentHierarchy WHERE ParentId = {1}
            )THEN 1 ELSE 0 END AS BIT)";
        
            var result = await _context.Database
                .SqlQueryRaw<bool>(query, parentId, partId)
                .ToListAsync(ct);

            return result.FirstOrDefault();

        }

        public async Task<Part> GetPartByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Parts
                .Include(p => p.ChildParts)
                    .ThenInclude(cp => cp.Child)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (entity == null) { throw new NotFound("Part is not found", id); }

            return new Part
            {
                Id = entity.Id,
                Name = entity.Name,
                Children = entity.ChildParts.Select(cp => new Part
                {
                    Id = cp.Child.Id,
                    Name = cp.Child.Name,
                    Quantity = cp.Quantity
                }).ToList()
            };
        }



        public async Task<Part> CreateNewPartAsync(string name, CancellationToken ct)
        {
            if(await _context.Parts.AnyAsync(p =>p.Name==name, cancellationToken: ct))
            {
                throw new PartAlreadyExists("Part with that name already exists",name);
            }

            var added = await _context.Parts.AddAsync(new PartEntity {Name = name});
            await _context.SaveChangesAsync(ct);

            return new Part {Name=added.Entity.Name, Id=added.Entity.Id };
        }

        public async Task<List<Guid>> DeletePartAsync(Guid partId, CancellationToken ct)
        {
            var part = await _context.Parts
                .Include(p => p.ChildParts)
                .Include(p => p.ParentParts)
                .FirstOrDefaultAsync(p => p.Id == partId, ct);

            if (part == null) { throw new NotFound("Part not found",partId); }

            List<Guid> deletedChildren = new List<Guid>();
            var childrenIds = part.ChildParts.Select(cp => cp.ChildId).ToList();

            _context.Structures.RemoveRange(part.ChildParts);
            _context.Structures.RemoveRange(part.ParentParts);            
            _context.Parts.Remove(part);
            
            await _context.SaveChangesAsync(ct);

            foreach (Guid childId in childrenIds)
            {
                bool isUsed = await _context.Structures.AnyAsync(s => s.ChildId == childId, ct);
                if (!isUsed)
                {
                    var part_to_delete = await _context.Parts.FindAsync(childId, ct);
                    if (part_to_delete != null)
                    {
                        _context.Parts.Remove(part_to_delete);
                        deletedChildren.Add(childId);
                    }
                }
            }

            await _context.SaveChangesAsync(ct);
            deletedChildren.Add(partId);
            return deletedChildren;
        }


        public async Task<Part> RenamePart(Guid partId, string newName, CancellationToken ct)
        {
            var part = await _context.Parts.FindAsync (partId, ct);
            if (part == null) { throw new NotFound("Part not found", partId); }

            part.Name = newName;
            await _context.SaveChangesAsync();

            return new Part { Id = part.Id, Name = part.Name };
        }

        public async Task<List<Part>> GetRootPartsAsync(CancellationToken ct)
        {
            var allParts = await _context.Parts.ToListAsync(ct);
            var allLinks = await _context.Structures.ToListAsync(ct);

            var partMap = allParts.ToDictionary(p => p.Id, p => new Part
            {
                Id = p.Id,
                Name = p.Name,
                Children = new List<Part>()
            });
            foreach (var link in allLinks)
            {
                if (partMap.ContainsKey(link.ParentId) && partMap.ContainsKey(link.ChildId))
                {
                    var childClone = new Part
                    {
                        Id = partMap[link.ChildId].Id,
                        Name = partMap[link.ChildId].Name,
                        Quantity = link.Quantity,
                        Children = partMap[link.ChildId].Children 
                    };
                    partMap[link.ParentId].Children.Add(childClone);
                }
            }

            var childIds = allLinks.Select(l => l.ChildId).ToHashSet();
            return partMap.Values.Where(p => !childIds.Contains(p.Id)).ToList();
        }




    }
}
