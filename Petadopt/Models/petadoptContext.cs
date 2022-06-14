using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Petadopt.Models
{
    public partial class petadoptContext : DbContext
    {
        public petadoptContext()
        {
        }

        public petadoptContext(DbContextOptions<petadoptContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Pet> Pets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:petadopt.database.windows.net,1433;Initial Catalog=petadopt;Persist Security Info=False;User ID=adminjg;Password=Petadopt0987;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("pets");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Coat)
                    .HasMaxLength(20)
                    .HasColumnName("coat");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .HasColumnName("description");

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .HasColumnName("gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Picture)
                    .HasMaxLength(20)
                    .HasColumnName("picture");

                entity.Property(e => e.Size)
                    .HasMaxLength(20)
                    .HasColumnName("size");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .HasColumnName("type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
