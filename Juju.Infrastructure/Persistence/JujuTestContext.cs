using Juju.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Juju.Infrastructure.Persistence
{
    public partial class JujuTestContext : DbContext
    {

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Post> Post { get; set; }


        public JujuTestContext(DbContextOptions<JujuTestContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(500);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Body).HasMaxLength(500);

                entity.Property(e => e.Category).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(500);
            });
        }
    }
}