namespace Tickets.API.Models.DTO.Ticket
{
    public class ActualizarEstatusDto
    {
        public Guid TicketId { get; set; }
        public string Observaciones { get; set; }
    }
}
