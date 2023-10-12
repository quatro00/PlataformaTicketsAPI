using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Area
{
    public Guid Id { get; set; }

    public Guid? AreaPadreId { get; set; }

    public Guid? DepartamentoId { get; set; }

    public string? Clave { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual Area? AreaPadre { get; set; }

    public virtual Departamento? Departamento { get; set; }

    public virtual ICollection<Area> InverseAreaPadre { get; set; } = new List<Area>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
