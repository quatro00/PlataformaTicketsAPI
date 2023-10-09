using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class RelUsuarioEquipo
{
    public Guid UsuarioId { get; set; }

    public Guid EquipoId { get; set; }

    public bool EsSupervisor { get; set; }

    public bool Activo { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
