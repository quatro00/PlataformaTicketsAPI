using Tickets.API.Models.Domain;

namespace Tickets.API.Models.DTO.Ticket
{
    public class CrearTicketRequestDto
    {
        public List<Guid> AreaId { get; set; }
        public Guid PrioridadId { get; set; }
        public string Titulo { get; set; }
        public Guid SubCategoriaId { get; set; }
        public string Descripcion { get; set; }
        public List<CrearTicketRequestArchivoDto> Archivos { get; set; }
        
    }

    public class CrearTicketRequestArchivoDto
    {
        public string NombreFisico { get; set; }
        public string Nombre { get; set; }
    }
}
