namespace Tickets.API.Models.DTO.Area
{
    public class CreateAreaBaseRequestDto
    {
        public Guid? AreaPadreId { get; set; }
        public Guid DepartamentoId { get; set; }
        public Guid? AreaId{ get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
