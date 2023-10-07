namespace Tickets.API.Models.DTO.SubCategoria
{
    public class SubCategoriaListDto
    {
        public Guid Id { get; set; }
        public Guid SucursalId { get; set; }
        public string SucursalNombre { get; set; }
        public string SucursalClave { get; set; }
        public Guid CategoriaId { get; set; }
        public string CategoriaName { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
