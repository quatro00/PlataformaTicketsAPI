namespace Tickets.API.Models.DTO.Ticket
{
    public class BorrarTicketMaterialDto
    {
        public Guid TicketId { get; set; }
        public Guid MaterialId { get; set; }
    }
}
