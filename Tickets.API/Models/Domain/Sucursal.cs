using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Sucursal
{
    public Guid Id { get; set; }

    public string Clave { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Telefono2 { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();

    public virtual ICollection<Prioridad> Prioridads { get; set; } = new List<Prioridad>();
}
