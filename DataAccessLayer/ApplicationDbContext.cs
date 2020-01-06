using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<CurrencyFrom> CurrencyFroms { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
