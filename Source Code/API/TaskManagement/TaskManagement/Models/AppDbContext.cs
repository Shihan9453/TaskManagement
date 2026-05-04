using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Models;

public partial class AppDbContext : DbContext
{

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TasksHistory> TasksHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tasks__3213E83F18C44ACF");

            entity.HasOne(d => d.AssignedPerson).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tasks__assigned___3B75D760");
        });

        modelBuilder.Entity<TasksHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TasksHis__3213E83F0AE30C72");

            entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Task).WithMany(p => p.TasksHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TasksHist__task___3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.TasksHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TasksHist__user___403A8C7D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F829AAC6D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
