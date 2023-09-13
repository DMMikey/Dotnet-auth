using System;
using EntityLayer.Entites;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class ApplicationDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=dotnetAuth;User ID=SA;Password=reallyStrongPwd123;TrustServerCertificate=true;");
        }

        public DbSet<User> Users { get; set; }

    }
}

