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

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<Prioridad> Prioridads { get; set; }

    public virtual DbSet<RelCategoriaEquipo> RelCategoriaEquipos { get; set; }

    public virtual DbSet<RelUsuarioEquipo> RelUsuarioEquipos { get; set; }

    public virtual DbSet<SubCategorium> SubCategoria { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketArchivo> TicketArchivos { get; set; }

    public virtual DbSet<TicketComentario> TicketComentarios { get; set; }

    public virtual DbSet<TicketEstatus> TicketEstatuses { get; set; }

    public virtual DbSet<TicketMaterial> TicketMaterials { get; set; }

    public virtual DbSet<TicketUsuariosAsignado> TicketUsuariosAsignados { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SQL8006.site4now.net;Initial Catalog=db_aa0913_ticketsdb;User Id=db_aa0913_ticketsdb_admin;Password=Suikoden2.");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("Area");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AreaPadre).WithMany(p => p.InverseAreaPadre)
                .HasForeignKey(d => d.AreaPadreId)
                .HasConstraintName("FK_Area_Area");

            entity.HasOne(d => d.Departamento).WithMany(p => p.Areas)
                .HasForeignKey(d => d.DepartamentoId)
                .HasConstraintName("FK_Area_Departamento");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Categoria)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categoria_Sucursal");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.ToTable("Departamento");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Departamentos)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departamento_Sucursal");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.ToTable("Equipo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Equipos)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipo_Sucursal");
        });

        modelBuilder.Entity<Prioridad>(entity =>
        {
            entity.ToTable("Prioridad");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.NivelDePrioridad).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TiempoDeAtencion).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Prioridads)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prioridad_Sucursal");
        });

        modelBuilder.Entity<RelCategoriaEquipo>(entity =>
        {
            entity.HasKey(e => new { e.CategoriaId, e.EquipoId });

            entity.ToTable("RelCategoriaEquipo");

            entity.HasOne(d => d.Categoria).WithMany(p => p.RelCategoriaEquipos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelCategoriaEquipo_Categoria");

            entity.HasOne(d => d.Equipo).WithMany(p => p.RelCategoriaEquipos)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelCategoriaEquipo_Equipo");
        });

        modelBuilder.Entity<RelUsuarioEquipo>(entity =>
        {
            entity.HasKey(e => new { e.UsuarioId, e.EquipoId });

            entity.ToTable("RelUsuarioEquipo");

            entity.HasOne(d => d.Equipo).WithMany(p => p.RelUsuarioEquipos)
                .HasForeignKey(d => d.EquipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelUsuarioEquipo_Equipo");

            entity.HasOne(d => d.Usuario).WithMany(p => p.RelUsuarioEquipos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelUsuarioEquipo_Usuario");
        });

        modelBuilder.Entity<SubCategorium>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Categoria).WithMany(p => p.SubCategoria)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubCategoria_Categoria");
        });

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

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AreaString).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(500);

            entity.HasOne(d => d.Area).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Ticket_Area");

            entity.HasOne(d => d.Departamento).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.DepartamentoId)
                .HasConstraintName("FK_Ticket_Departamento");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_TicketEstatus");

            entity.HasOne(d => d.Prioridad).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.PrioridadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Prioridad");

            entity.HasOne(d => d.SubCategoria).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SubCategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_SubCategoria");

            entity.HasOne(d => d.UsuarioCreacion).WithMany(p => p.TicketUsuarioCreacions)
                .HasForeignKey(d => d.UsuarioCreacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Usuario");

            entity.HasOne(d => d.UsuarioUltimaModificacionNavigation).WithMany(p => p.TicketUsuarioUltimaModificacionNavigations)
                .HasForeignKey(d => d.UsuarioUltimaModificacion)
                .HasConstraintName("FK_Ticket_Usuario1");
        });

        modelBuilder.Entity<TicketArchivo>(entity =>
        {
            entity.ToTable("TicketArchivo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.NombreFisico).HasMaxLength(500);
            entity.Property(e => e.Tamaño).HasMaxLength(50);
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketArchivos)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketArchivo_Ticket");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TicketArchivos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketArchivo_Usuario");
        });

        modelBuilder.Entity<TicketComentario>(entity =>
        {
            entity.ToTable("TicketComentario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Texto).HasColumnName("texto");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketComentarios)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketComentario_Ticket");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TicketComentarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketComentario_Usuario");
        });

        modelBuilder.Entity<TicketEstatus>(entity =>
        {
            entity.HasKey(e => e.EstatusId);

            entity.ToTable("TicketEstatus");

            entity.Property(e => e.EstatusId).ValueGeneratedNever();
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<TicketMaterial>(entity =>
        {
            entity.ToTable("TicketMaterial");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cantidad).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Concepto)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unidad)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketMaterials)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketMaterial_Ticket");
        });

        modelBuilder.Entity<TicketUsuariosAsignado>(entity =>
        {
            entity.HasKey(e => new { e.TicketId, e.UsuarioId });

            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketUsuariosAsignados)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketUsuariosAsignados_Ticket");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TicketUsuariosAsignados)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketUsuariosAsignados_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Perfil");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Apellidos)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.LoginId).HasMaxLength(450);
            entity.Property(e => e.Nombre)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Login).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.LoginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_AspNetUsers");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Sucursal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
