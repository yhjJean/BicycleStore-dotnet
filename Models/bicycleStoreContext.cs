using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BicycleStore.Models
{
    public partial class bicycleStoreContext : DbContext
    {
        public bicycleStoreContext()
        {
        }

        public bicycleStoreContext(DbContextOptions<bicycleStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<admin> admins { get; set; } = null!;
        public virtual DbSet<bicycle> bicycles { get; set; } = null!;
        public virtual DbSet<employee> employees { get; set; } = null!;
        public virtual DbSet<rental> rentals { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<admin>(entity =>
            {
                entity.Property(e => e.Username).IsFixedLength();
            });

            modelBuilder.Entity<employee>(entity =>
            {
                entity.Property(e => e.Username).IsFixedLength();
            });

            modelBuilder.Entity<rental>(entity =>
            {
                entity.Property(e => e.MatricNo).IsFixedLength();

                entity.HasOne(d => d.Bicycle)
                    .WithMany(p => p.rentals)
                    .HasForeignKey(d => d.BicycleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_rental_bicycle");

                entity.HasOne(d => d.CreatedByAdminNavigation)
                    .WithMany(p => p.rentals)
                    .HasForeignKey(d => d.CreatedByAdmin)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_rental_admin");

                entity.HasOne(d => d.CreatedByEmployeeNavigation)
                    .WithMany(p => p.rentals)
                    .HasForeignKey(d => d.CreatedByEmployee)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_rental_employee");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
