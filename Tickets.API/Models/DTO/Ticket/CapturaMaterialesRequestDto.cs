namespace Tickets.API.Models.DTO.Ticket
{
    public class CapturaMaterialesRequestDto
    {
        public Guid TicketId { get; set; }
        public string Concepto { get; set; }
        public string Tipo { get; set; }
        public string Unidad { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
