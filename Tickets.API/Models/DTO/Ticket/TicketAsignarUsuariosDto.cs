namespace Tickets.API.Models.DTO.Ticket
{
    public class TicketAsignarUsuariosDto
    {
        public List<Guid> agentes { get; set; } = new List<Guid>();
        public Guid ticketId { get; set; }
        public string observaciones { get; set; }
    }
}
