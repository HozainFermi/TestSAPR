using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Infrastructure.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

//        CREATE TABLE part(
//      id UUID PRIMARY KEY,
//      name VARCHAR(255) NOT NULL
//);

//        CREATE TABLE part_structure(
//            parent_id UUID NOT NULL,
//            child_id UUID NOT NULL,
//            quantity INT NOT NULL,
//            PRIMARY KEY (parent_id, child_id),
//            FOREIGN KEY (parent_id) REFERENCES part(id),
//            FOREIGN KEY (child_id) REFERENCES part(id)
//        );
    }
}
