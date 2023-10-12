using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Ticket
{
    public Guid Id { get; set; }

    public Guid AreaId { get; set; }

    public int Folio { get; set; }

    public int EstatusId { get; set; }

    public Guid PrioridadId { get; set; }

    public string Titulo { get; set; } = null!;

    public Guid SubCategoriaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioUltimaModificacion { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual TicketEstatus Estatus { get; set; } = null!;

    public virtual Prioridad Prioridad { get; set; } = null!;

    public virtual SubCategorium SubCategoria { get; set; } = null!;

    public virtual ICollection<TicketArchivo> TicketArchivos { get; set; } = new List<TicketArchivo>();

    public virtual ICollection<TicketComentario> TicketComentarios { get; set; } = new List<TicketComentario>();

    public virtual Usuario UsuarioCreacion { get; set; } = null!;

    public virtual Usuario? UsuarioUltimaModificacionNavigation { get; set; }
}
