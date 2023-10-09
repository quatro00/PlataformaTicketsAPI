namespace Tickets.API.Models.DTO.Equipo
{
    public class EquipoListDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Guid SucursalId { get; set; }
        public string SucursalNombre { get; set; }
        public List<string> Categorias { get; set; }
        public List<EquipoListDetDto> Usuarios { get; set; }
        public List<EquipoListDetDto> Supervisores { get; set; }
        public bool Activo { get; set; }
    }

    public class EquipoListDetDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
    }
}
