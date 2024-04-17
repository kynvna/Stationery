using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StationeryAPI.Models;

public partial class DemoCoreContext : DbContext
{
    public DemoCoreContext()
    {
    }

    public DemoCoreContext(DbContextOptions<DemoCoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-NSN1HKQ;Initial Catalog=ShoppingWeb;Persist Security Info=True;User ID=sa;Password=IAmAdmin123;Encrypt=True;Trust Server Certificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__departme__3213E83FECF14A90");

            entity.ToTable("department");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DepName)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("dep_name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmp).HasName("PK__employee__D52A94EFBD2D8F7A");

            entity.ToTable("employee");

            entity.Property(e => e.IdEmp).HasColumnName("id_emp");
            entity.Property(e => e.DepId).HasColumnName("dep_id");
            entity.Property(e => e.EmpName)
                .HasMaxLength(100)
                .HasDefaultValue("no name")
                .HasColumnName("emp_name");

            entity.HasOne(d => d.Dep).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK__employee__dep_id__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
