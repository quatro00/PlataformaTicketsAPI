using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class RelCategoriaEquipo
{
    public Guid CategoriaId { get; set; }

    public Guid EquipoId { get; set; }

    public bool Activo { get; set; }

    public virtual Categorium Categoria { get; set; } = null!;

    public virtual Equipo Equipo { get; set; } = null!;
}
