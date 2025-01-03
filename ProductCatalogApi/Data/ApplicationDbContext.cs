using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Data

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
