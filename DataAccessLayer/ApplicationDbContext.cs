using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {                
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, string testing) : base(options)
        {
        }
    }
}
