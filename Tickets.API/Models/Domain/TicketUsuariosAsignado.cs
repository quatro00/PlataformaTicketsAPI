using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class TicketUsuariosAsignado
{
    public Guid TicketId { get; set; }

    public Guid UsuarioId { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
