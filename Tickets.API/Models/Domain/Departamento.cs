using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Departamento
{
    public Guid Id { get; set; }

    public Guid SucursalId { get; set; }

    public string Clave { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual Sucursal Sucursal { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
