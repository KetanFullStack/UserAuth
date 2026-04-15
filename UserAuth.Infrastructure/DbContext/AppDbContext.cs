using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Infrastructure.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using UserAuth.Domain.Entities;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Email).IsUnique();

                entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
                entity.Property(x => x.Email).IsRequired().HasMaxLength(150);
                entity.Property(x => x.Mobile).IsRequired().HasMaxLength(20);
            });
        }
    }
}
