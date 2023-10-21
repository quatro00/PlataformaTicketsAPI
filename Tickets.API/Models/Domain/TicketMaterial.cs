using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class TicketMaterial
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public string Concepto { get; set; } = null!;

    public string? Tipo { get; set; }

    public string? Unidad { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public Guid? UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
