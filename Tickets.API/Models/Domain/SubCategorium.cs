using System;
using System.Collections.Generic;

namespace Tickets.API.Models.Domain;

public partial class SubCategorium
{
    public Guid Id { get; set; }

    public Guid CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Categorium Categoria { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
