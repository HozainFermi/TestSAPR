using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TestSAPR.Infrastructure.Entity;

namespace TestSAPR.Infrastructure.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PartEntity> Parts { get; set; }
        public DbSet<PartStructureEntity> Structures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartEntity>(entity =>
            {
                entity.ToTable("parts");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired();

                entity.HasIndex(p => p.Name)
                .IsUnique(true);
                
                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<PartStructureEntity>(entity =>
            {
                entity.ToTable("part_structures");
                entity.HasKey(s => new {s.ParentId, s.ChildId });
               
                entity.Property(m => m.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(ps => ps.Quantity).IsRequired();

                entity
                    .HasOne(ps => ps.Parent)
                    .WithMany(p => p.ChildParts)
                    .HasForeignKey(ps => ps.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);


                entity
                    .HasOne(ps => ps.Child)
                    .WithMany(p => p.ParentParts)
                    .HasForeignKey(ps => ps.ChildId)
                    .OnDelete(DeleteBehavior.Restrict);


            });
        }

    }
}
