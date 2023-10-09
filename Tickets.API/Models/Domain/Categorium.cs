using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class Categorium
{
    public Guid Id { get; set; }

    public Guid SucursalId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<RelCategoriaEquipo> RelCategoriaEquipos { get; set; } = new List<RelCategoriaEquipo>();

    public virtual ICollection<SubCategorium> SubCategoria { get; set; } = new List<SubCategorium>();

    public virtual Sucursal Sucursal { get; set; } = null!;
}
