namespace Tickets.API.Models.DTO.Area
{
    public class AreaDto
    {
        public Guid Id { get; set; }
        public Guid? AreaPadreId { get; set; }
        public Guid? DepartamentoId { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public List<AreaDto> AreasHijas { get; set;}
    }
}
