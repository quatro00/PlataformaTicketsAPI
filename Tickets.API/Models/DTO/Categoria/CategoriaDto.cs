using Tickets.API.Models.Domain;

namespace Tickets.API.Models.DTO.Categoria
{
    public class CategoriaDto
    {
        public Guid SucursalId { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }
    }
}
