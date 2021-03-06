﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Example
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<ModelA> ModelAs { get; set; }
        public DbSet<ModelB> ModelBs { get; set; }
        public DbSet<ModelC> ModelCs { get; set; }
    }
}
