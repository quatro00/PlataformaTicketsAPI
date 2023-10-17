namespace Tickets.API.Models.DTO.Ticket
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public int Folio { get; set; }
        public string Solicitante { get; set; }
        public string Subcategoria { get; set; }
        public string Categoria { get; set; }
        public string Sucursal { get; set; }
        public string Departamento { get; set; }
        public string Area { get; set; }
        public string Titulo { get; set; }
        public string Prioridad { get; set; }
        public decimal NivelDePrioridad { get; set; }
        public string Color { get; set; }
        public string Estatus { get; set; }
        public string EstatusColor { get; set; }
        public string FechaCreacion { get; set; }
    }
}
