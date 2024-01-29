using System;
using System.Collections.Generic;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class DbFirstDataContext : DbContext
{
    public DbFirstDataContext()
    {
    }

    public DbFirstDataContext(DbContextOptions<DbFirstDataContext> options) : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderRow> OrderRows { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0716F1EE20");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC0774C2DB9C");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC0768DC06CD");
        });

        modelBuilder.Entity<OrderRow>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__OrderRow__08D097A365F9F0B7");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderRows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRows__Order__56E8E7AB");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderRows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRows__Produ__57DD0BE4");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC074FFB3089");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__51300E55");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Manufa__5224328E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
