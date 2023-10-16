namespace Tickets.API.Models.DTO.Ticket
{
    public class TicketArchivoDto
    {
        public Guid Id { get; set; }
        public Guid? TicketId { get; set; }
        public Guid? UsuarioId { get; set; }
        public string Nombre { get;set; }
        public string Tipo { get; set; }
        public string Tamano { get; set; }
        public DateTime Fecha { get; set; }
    }
}
