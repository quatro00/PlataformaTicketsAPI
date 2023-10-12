using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class TicketComentario
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public Guid UsuarioId { get; set; }

    public string Texto { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
