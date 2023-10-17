using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class TicketEstatus
{
    public int EstatusId { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
