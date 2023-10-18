namespace Tickets.API.Models.DTO.TicketComentario
{
    public class TicketComentarioRequestDto
    {
        public Guid TicketId { get; set; }
        public string Mensaje { get; set; }
    }
}
