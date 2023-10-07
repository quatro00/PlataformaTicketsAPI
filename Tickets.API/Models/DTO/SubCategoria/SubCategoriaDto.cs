using Tickets.API.Models.Domain;

namespace Tickets.API.Models.DTO.SubCategoria
{
    public class SubCategoriaDto
    {
        public Guid CategoriaId { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }

    }
}
