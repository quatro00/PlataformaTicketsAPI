using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Usuario
{
    public Guid Id { get; set; }

    public string LoginId { get; set; } = null!;

    public int Matricula { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public Guid SucursalId { get; set; }

    public string CorreoElectronico { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual AspNetUser Login { get; set; } = null!;

    public virtual ICollection<RelUsuarioEquipo> RelUsuarioEquipos { get; set; } = new List<RelUsuarioEquipo>();

    public virtual Sucursal Sucursal { get; set; } = null!;

    public virtual ICollection<TicketArchivo> TicketArchivos { get; set; } = new List<TicketArchivo>();

    public virtual ICollection<TicketComentario> TicketComentarios { get; set; } = new List<TicketComentario>();

    public virtual ICollection<Ticket> TicketUsuarioCreacions { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketUsuarioUltimaModificacionNavigations { get; set; } = new List<Ticket>();

    public virtual ICollection<TicketUsuariosAsignado> TicketUsuariosAsignados { get; set; } = new List<TicketUsuariosAsignado>();
}
