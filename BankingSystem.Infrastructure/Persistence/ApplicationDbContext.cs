using BankingSystem.Domain.Entities;
using BankingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BankAccount>(entity =>
            {

                entity.HasKey(b => b.Id);

                entity.Property(b => b.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(b => b.Balance)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(b => b.CreatedAt)
                    .IsRequired();

                entity.Property(b => b.UserId)
                   .IsRequired();

                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.BankAccounts)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}
