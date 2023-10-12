using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Prioridad
{
    public Guid Id { get; set; }

    public Guid SucursalId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal TiempoDeAtencion { get; set; }

    public decimal NivelDePrioridad { get; set; }

    public string Color { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Sucursal Sucursal { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
