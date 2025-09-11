using DeliveryMotorcycle.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Infrastructure.Data
{
    public class DeliveryMotorcycleDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DeliveryMotorcycleDbContext(DbContextOptions<DeliveryMotorcycleDbContext> options) : base(options) { }

        public DbSet<Motorcycle> Motorcycle { get; set; }
        public DbSet<MotorcycleNotification> Notifications { get; set; }
        public DbSet<DeliveryMan> DeliveryMans { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Plan> Plans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Motorcycle>().HasIndex(m => m.Plate).IsUnique();

            modelBuilder.Entity<DeliveryMan>().HasIndex(d => d.Cnpj).IsUnique();

            modelBuilder.Entity<DeliveryMan>().HasIndex(d => d.CnhNumber).IsUnique();
        }
    }
}
