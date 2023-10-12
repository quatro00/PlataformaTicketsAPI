using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class TicketArchivo
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public Guid UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string Tamaño { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
