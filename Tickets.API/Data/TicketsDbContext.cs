using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tickets.API.Models.Domain;

namespace Tickets.API.Data;

public partial class TicketsDbContext : DbContext
{
    public TicketsDbContext()
    {
    }

    public TicketsDbContext(DbContextOptions<TicketsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-540B3V8;Initial Catalog=TicketsDb;Persist Security Info=True;User ID=sa;Password=sql2;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Unidad");

            entity.ToTable("Sucursal");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Telefono2)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
