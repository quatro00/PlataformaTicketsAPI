namespace Tickets.API.Models.DTO.Categoria
{
    public class CategoriaListDto
    {
        public Guid Id { get; set; }
        public Guid SucursalId { get; set; }
        public string SucursalClave { get; set; }
        public string SucursalNombre { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public List<CategoriaEqupoListDto> Equipos { get; set; }

        public bool Activo { get; set; }
    }

    public class CategoriaEqupoListDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }
}
