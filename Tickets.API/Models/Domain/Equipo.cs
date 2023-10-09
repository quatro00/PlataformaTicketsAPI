using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Equipo
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public Guid SucursalId { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual ICollection<RelCategoriaEquipo> RelCategoriaEquipos { get; set; } = new List<RelCategoriaEquipo>();

    public virtual ICollection<RelUsuarioEquipo> RelUsuarioEquipos { get; set; } = new List<RelUsuarioEquipo>();

    public virtual Sucursal Sucursal { get; set; } = null!;
}
